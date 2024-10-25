import whisper
import sys

m4a_file = sys.argv[1]
output_srt = sys.argv[2]

if not output_srt.endswith(".srt"):
    output_srt += ".srt"

model = whisper.load_model("turbo")
result = model.transcribe(m4a_file, task="transcribe")
with open(output_srt, "w", encoding="utf-8") as srt_file:
    for i, segment in enumerate(result['segments']):
        start = segment['start']
        end = segment['end']
        text = segment['text']

        # Format time in SRT style
        start_time = f"{int(start // 3600):02}:{int((start % 3600) // 60):02}:{int(start % 60):02},{int((start % 1) * 1000):03}"
        end_time = f"{int(end // 3600):02}:{int((end % 3600) // 60):02}:{int(end % 60):02},{int((end % 1) * 1000):03}"

        # Write the segment in SRT format
        srt_file.write(f"{i + 1}\n{start_time} --> {end_time}\n{text.strip()}\n\n")