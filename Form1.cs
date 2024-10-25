using System.Diagnostics;

namespace m4aToText
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string m4aPath = txtM4APath.Text;
            string outputPath = txtOutputPath.Text;

            if (!string.IsNullOrWhiteSpace(m4aPath) && !string.IsNullOrWhiteSpace(outputPath))
            {
                // Run Python script
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "python";
                start.Arguments = $"whisper_transcription.py \"{m4aPath}\" \"{outputPath}\"";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.CreateNoWindow = true;

                using (Process process = Process.Start(start))
                {
                    string result = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        MessageBox.Show($"Error: {error}");
                    }
                    else
                    {
                        MessageBox.Show($"Conversion complete! Subtitle saved to {outputPath}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select both M4A file and output path.");
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Audio Files (*.m4a)|*.m4a";
                openFileDialog.Title = "Select M4A file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtM4APath.Text = openFileDialog.FileName;
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
    }
}
