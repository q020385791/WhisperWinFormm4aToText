namespace m4aToText
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConvert = new Button();
            btnSelectFile = new Button();
            btnSelectFolder = new Button();
            txtM4APath = new TextBox();
            btnSelectOutput = new Button();
            btnSelectOutputFolder = new Button();
            txtOutputPath = new TextBox();
            SuspendLayout();
            // 
            // btnConvert
            // 
            btnConvert.Location = new Point(461, 101);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(75, 23);
            btnConvert.TabIndex = 0;
            btnConvert.Text = "轉換";
            btnConvert.UseVisualStyleBackColor = true;
            btnConvert.Click += btnConvert_Click;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new Point(461, 27);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(105, 23);
            btnSelectFile.TabIndex = 1;
            btnSelectFile.Text = "選擇檔案";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Location = new Point(572, 27);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(125, 23);
            btnSelectFolder.TabIndex = 2;
            btnSelectFolder.Text = "選擇資料夾";
            btnSelectFolder.UseVisualStyleBackColor = true;
            btnSelectFolder.Click += btnSelectFolder_Click;
            // 
            // txtM4APath
            // 
            txtM4APath.Location = new Point(17, 28);
            txtM4APath.Name = "txtM4APath";
            txtM4APath.Size = new Size(438, 23);
            txtM4APath.TabIndex = 3;
            // 
            // btnSelectOutput
            // 
            btnSelectOutput.Location = new Point(461, 61);
            btnSelectOutput.Name = "btnSelectOutput";
            btnSelectOutput.Size = new Size(105, 23);
            btnSelectOutput.TabIndex = 4;
            btnSelectOutput.Text = "選擇輸出檔案";
            btnSelectOutput.UseVisualStyleBackColor = true;
            btnSelectOutput.Click += btnSelectOutput_Click;
            // 
            // btnSelectOutputFolder
            // 
            btnSelectOutputFolder.Location = new Point(572, 61);
            btnSelectOutputFolder.Name = "btnSelectOutputFolder";
            btnSelectOutputFolder.Size = new Size(125, 23);
            btnSelectOutputFolder.TabIndex = 5;
            btnSelectOutputFolder.Text = "選擇輸出資料夾";
            btnSelectOutputFolder.UseVisualStyleBackColor = true;
            btnSelectOutputFolder.Click += btnSelectOutputFolder_Click;
            // 
            // txtOutputPath
            // 
            txtOutputPath.Location = new Point(17, 59);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.Size = new Size(438, 23);
            txtOutputPath.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(714, 148);
            Controls.Add(txtOutputPath);
            Controls.Add(btnSelectOutputFolder);
            Controls.Add(btnSelectOutput);
            Controls.Add(txtM4APath);
            Controls.Add(btnSelectFolder);
            Controls.Add(btnSelectFile);
            Controls.Add(btnConvert);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConvert;
        private Button btnSelectFile;
        private Button btnSelectFolder;
        private TextBox txtM4APath;
        private Button btnSelectOutput;
        private Button btnSelectOutputFolder;
        private TextBox txtOutputPath;
    }
}
