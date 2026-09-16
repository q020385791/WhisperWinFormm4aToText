import os
import sys
from pathlib import Path

import torch
import whisper

DEFAULT_MODEL = "large-v3"  # Default model to use if none is specified
DEFAULT_DEVICE = "cuda"
SUPPORTED_MEDIA_EXTENSIONS = {".mp4"}

def ensure_srt_path(output_path):
    output_path = Path(output_path)
    if output_path.suffix.lower() != ".srt":
        output_path = Path(f"{output_path}.srt")
    return output_path


def output_srt_for_file(media_file, output_path):
    output_path = Path(output_path)
    if output_path.exists() and output_path.is_dir():
        return output_path / f"{media_file.stem}.srt"
    return ensure_srt_path(output_path)


def format_srt_time(seconds):
    return (
        f"{int(seconds // 3600):02}:"
        f"{int((seconds % 3600) // 60):02}:"
        f"{int(seconds % 60):02},"
        f"{int((seconds % 1) * 1000):03}"
    )


def write_srt(result, output_srt):
    output_srt.parent.mkdir(parents=True, exist_ok=True)
    with open(output_srt, "w", encoding="utf-8") as srt_file:
        for i, segment in enumerate(result["segments"]):
            start = segment["start"]
            end = segment["end"]
            text = segment["text"]

            srt_file.write(
                f"{i + 1}\n"
                f"{format_srt_time(start)} --> {format_srt_time(end)}\n"
                f"{text.strip()}\n\n"
            )


def resolve_model_name(requested_model):
    available_models = set(whisper.available_models())
    if requested_model not in available_models:
        fallback_model = DEFAULT_MODEL if DEFAULT_MODEL in available_models else sorted(available_models)[0]
        print(f"Whisper model '{requested_model}' is not available; using '{fallback_model}' instead.")
        return fallback_model
    return requested_model


def resolve_device(requested_device):
    requested_device = requested_device.lower()
    if requested_device.startswith("cuda") and not torch.cuda.is_available():
        print(
            "CUDA GPU is not available in this Python environment. "
            "Install a CUDA-enabled PyTorch build or set WHISPER_DEVICE=cpu to run on CPU.",
            file=sys.stderr,
        )
        return None
    return requested_device


def transcribe_media(model, media_file, output_srt):
    result = model.transcribe(str(media_file), task="transcribe")
    write_srt(result, output_srt)
    print(f"Subtitle saved to {output_srt}")


def find_media_files(folder):
    return sorted(
        (
            path
            for path in folder.iterdir()
            if path.is_file() and path.suffix.lower() in SUPPORTED_MEDIA_EXTENSIONS
        ),
        key=lambda path: path.name.lower(),
    )


def folder_output_name(folder):
    return folder.name or "batch_output"


def batch_output_paths(media_files, output_folder):
    used_names = {}
    for media_file in media_files:
        base_name = media_file.stem
        used_names[base_name.lower()] = used_names.get(base_name.lower(), 0) + 1
        suffix = "" if used_names[base_name.lower()] == 1 else f"_{used_names[base_name.lower()]}"
        yield media_file, output_folder / f"{base_name}{suffix}.srt"


def transcribe_folder(model, media_folder, output_root):
    media_files = find_media_files(media_folder)
    if not media_files:
        supported = ", ".join(sorted(SUPPORTED_MEDIA_EXTENSIONS))
        print(f"No supported media files were found in {media_folder}. Supported extensions: {supported}", file=sys.stderr)
        return 1

    batch_folder = Path(output_root) / folder_output_name(media_folder)
    batch_folder.mkdir(parents=True, exist_ok=True)
    print(f"Batch output folder: {batch_folder}")

    failures = []
    for index, (media_file, output_srt) in enumerate(batch_output_paths(media_files, batch_folder), start=1):
        print(f"[{index}/{len(media_files)}] Transcribing {media_file.name}")
        try:
            transcribe_media(model, media_file, output_srt)
        except Exception as exc:
            failures.append((media_file, exc))
            print(f"Failed to transcribe {media_file}: {exc}", file=sys.stderr)

    if failures:
        print(f"Batch finished with {len(failures)} failed file(s).", file=sys.stderr)
        return 1

    print(f"Batch complete. {len(media_files)} subtitle file(s) saved under {batch_folder}")
    return 0


def main():
    if len(sys.argv) < 3:
        print("Usage: python whisper_transcription.py <media_file_or_folder> <output_srt_or_folder> [model]", file=sys.stderr)
        return 2

    media_path = Path(sys.argv[1])
    output_path = Path(sys.argv[2])
    model_name = sys.argv[3] if len(sys.argv) > 3 else os.getenv("WHISPER_MODEL", DEFAULT_MODEL)
    device_name = os.getenv("WHISPER_DEVICE", DEFAULT_DEVICE)

    if not media_path.exists():
        print(f"Input path does not exist: {media_path}", file=sys.stderr)
        return 1

    if media_path.is_file() and media_path.suffix.lower() not in SUPPORTED_MEDIA_EXTENSIONS:
        print(f"Only .mp4 files are supported: {media_path}", file=sys.stderr)
        return 1

    model_name = resolve_model_name(model_name)
    device_name = resolve_device(device_name)
    if device_name is None:
        return 1

    print(f"Using Whisper model '{model_name}' on device '{device_name}'.")
    if device_name == "cuda":
        print(f"GPU: {torch.cuda.get_device_name(0)}")

    model = whisper.load_model(model_name, device=device_name)

    if media_path.is_dir():
        if output_path.suffix.lower() == ".srt":
            print("When the input path is a folder, the output path must be a folder instead of an .srt file.", file=sys.stderr)
            return 1
        return transcribe_folder(model, media_path, output_path)

    output_srt = output_srt_for_file(media_path, output_path)
    transcribe_media(model, media_path, output_srt)
    return 0


if __name__ == "__main__":
    sys.exit(main())
