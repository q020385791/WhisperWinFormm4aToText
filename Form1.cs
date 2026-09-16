using System.Diagnostics;

namespace m4aToText
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            string mediaPath = txtM4APath.Text.Trim();
            string outputPath = txtOutputPath.Text.Trim();

            if (!string.IsNullOrWhiteSpace(mediaPath) && !string.IsNullOrWhiteSpace(outputPath))
            {
                if (Directory.Exists(mediaPath)
                    && string.Equals(Path.GetExtension(outputPath), ".srt", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("資料夾批次轉換時，請選擇輸出資料夾，而不是輸出 .srt 檔案。");
                    return;
                }
                if (File.Exists(mediaPath)
                    && !string.Equals(Path.GetExtension(mediaPath), ".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("目前只支援輸入 .mp4 檔案。");
                    return;
                }

                // Run Python script
                string scriptPath = Path.Combine(AppContext.BaseDirectory, "whisper_transcription.py");
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "python";
                start.ArgumentList.Add(scriptPath);
                start.ArgumentList.Add(mediaPath);
                start.ArgumentList.Add(outputPath);
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.CreateNoWindow = true;

                btnConvert.Enabled = false;
                Cursor? previousCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    using (Process? process = Process.Start(start))
                    {
                        if (process == null)
                        {
                            MessageBox.Show("Failed to start Python process.");
                            return;
                        }

                        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                        Task<string> errorTask = process.StandardError.ReadToEndAsync();

                        await process.WaitForExitAsync();

                        string result = await outputTask;
                        string error = await errorTask;

                        if (process.ExitCode != 0)
                        {
                            List<string> details = new List<string>();
                            if (!string.IsNullOrWhiteSpace(error))
                            {
                                details.Add(error.Trim());
                            }
                            if (!string.IsNullOrWhiteSpace(result))
                            {
                                details.Add(result.Trim());
                            }

                            string errorMessage = details.Count == 0
                                ? $"Python exited with code {process.ExitCode}."
                                : string.Join($"{Environment.NewLine}{Environment.NewLine}", details);
                            MessageBox.Show($"Error: {errorMessage}");
                        }
                        else
                        {
                            string successMessage = string.IsNullOrWhiteSpace(result)
                                ? $"Conversion complete! Subtitle saved to {outputPath}"
                                : $"Conversion complete!{Environment.NewLine}{Environment.NewLine}{result.Trim()}";
                            MessageBox.Show(successMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    if (previousCursor is not null)
                    {
                        Cursor.Current = previousCursor;
                    }
                    btnConvert.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please select both MP4 file or source folder and output path.");
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "MP4 Files (*.mp4)|*.mp4";
                openFileDialog.Title = "Select MP4 file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtM4APath.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select source folder";
                folderBrowserDialog.UseDescriptionForTitle = true;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtM4APath.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void btnSelectOutput_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Subtitle Files (*.srt)|*.srt";
                saveFileDialog.Title = "Select Output SRT file location";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = saveFileDialog.FileName; // Write the selected output path to the TextBox
                }
            }
        }

        private void btnSelectOutputFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select output folder";
                folderBrowserDialog.UseDescriptionForTitle = true;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }
    }
}
