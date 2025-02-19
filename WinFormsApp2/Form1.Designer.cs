namespace ModerProgram
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            startButton = new Button();
            stopButton = new Button();
            AddBanWord = new Button();
            AddBanProgram = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.Location = new Point(12, 12);
            startButton.Name = "startButton";
            startButton.Size = new Size(115, 52);
            startButton.TabIndex = 0;
            startButton.Text = "Start";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += StartButton_Click;
            // 
            // stopButton
            // 
            stopButton.Location = new Point(12, 86);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(115, 51);
            stopButton.TabIndex = 1;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += StopButton_Click;
            // 
            // AddBanWord
            // 
            AddBanWord.Location = new Point(175, 66);
            AddBanWord.Name = "AddBanWord";
            AddBanWord.Size = new Size(114, 23);
            AddBanWord.TabIndex = 2;
            AddBanWord.Text = "Add Ban Word";
            AddBanWord.UseVisualStyleBackColor = true;
            AddBanWord.Click += AddBanWord_Click;
            // 
            // AddBanProgram
            // 
            AddBanProgram.Location = new Point(175, 100);
            AddBanProgram.Name = "AddBanProgram";
            AddBanProgram.Size = new Size(114, 23);
            AddBanProgram.TabIndex = 3;
            AddBanProgram.Text = "Add Ban Program";
            AddBanProgram.UseVisualStyleBackColor = true;
            AddBanProgram.Click += AddBanProgram_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(156, 28);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(156, 23);
            textBox1.TabIndex = 4;
            // 
            // MainForm
            // 
            ClientSize = new Size(345, 149);
            Controls.Add(textBox1);
            Controls.Add(AddBanWord);
            Controls.Add(AddBanProgram);
            Controls.Add(startButton);
            Controls.Add(stopButton);
            Name = "MainForm";
            Text = "Activity Tracker";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;

        private Button AddBanWord;
        private Button AddBanProgram;
        private TextBox textBox1;

        private void StartButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => TrackKeys(cancellationTokenSource.Token));
            Task.Run(() => MonitorProcesses(cancellationTokenSource.Token));
            startButton.Enabled = false;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
            startButton.Enabled = true;
        }


    }
}
