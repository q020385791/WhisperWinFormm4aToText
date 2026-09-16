# 安裝whisper
pip install -U openai-whisper

# GPU 加速
本程式預設使用 NVIDIA GPU。請在執行環境安裝 CUDA 版 PyTorch，例如：

```powershell
python -m pip install torch==2.7.0 torchvision==0.22.0 --index-url https://download.pytorch.org/whl/cu118
```

若要暫時改回 CPU，可在執行前設定：

```powershell
$env:WHISPER_DEVICE="cpu"
```


# 可選：指定 Whisper 模型
預設使用 large-v3。若要改用其他模型，可在執行前設定 WHISPER_MODEL，例如 turbo。

# 批次轉換資料夾
來源路徑可選單一 MP4 檔，也可選資料夾。若來源路徑是資料夾，程式只會轉換該資料夾內的 MP4 檔，其他格式會略過不處理，並在指定輸出資料夾下建立一個與來源資料夾同名的子資料夾，將字幕檔輸出到該子資料夾。

# 根據系統安裝ffmpeg
on Ubuntu or Debian
sudo apt update && sudo apt install ffmpeg

on Arch Linux
sudo pacman -S ffmpeg

on MacOS using Homebrew (https://brew.sh/)
brew install ffmpeg

on Windows using Chocolatey (https://chocolatey.org/)
choco install ffmpeg

on Windows using Scoop (https://scoop.sh/)
scoop install ffmpeg

# 此專案為轉換 mp4 檔案至 srt 字幕檔案用
