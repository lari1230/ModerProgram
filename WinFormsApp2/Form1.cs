using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ModerProgram
{
    public partial class MainForm : Form
    {
        private static string logFilePath = "activity_log.txt";
        private static HashSet<string> blockedWords = new HashSet<string> { "password", "secret" };
        private static HashSet<string> blockedPrograms = new HashSet<string> { "notepad.exe", "calc.exe" };
        private CancellationTokenSource cancellationTokenSource;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => TrackKeys(cancellationTokenSource.Token));
            Task.Run(() => MonitorProcesses(cancellationTokenSource.Token));
        }
        private void AddBanWord_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < textBox1.Text.Count(); i++) {
                if (!textBox1.Text.Contains("."))
                {
                    blockedWords.Add(textBox1.Text);
                    MessageBox.Show($"Слово \"{textBox1.Text}\" добавлено \nв список запрещённых слов.", "Notification");
                    textBox1?.Clear();
                }
                else
                {
                    MessageBox.Show("Слово не может содержать знаков препинания!","WARNING!");
                    textBox1?.Clear();
                }
            }
        }

        private void AddBanProgram_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < textBox1?.Text.Count(); i++)
            {
                if (textBox1.Text.Contains("."))
                {
                    blockedWords.Add(textBox1.Text);
                    MessageBox.Show($"Программа \"{textBox1.Text}\" добавлена \nв список запрещённых программ.", "Notification");
                    textBox1?.Clear();
                }
                else
                {
                    MessageBox.Show("Программа должна иметь разширение!","WARNING!");
                    textBox1?.Clear();
                }
            }
        }
        private async Task TrackKeys(CancellationToken token)
        {
            StringBuilder typedText = new StringBuilder();
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(50);
                for (int i = 0; i < 255; i++)
                {
                    if (GetAsyncKeyState((Keys)i) == -32767)
                    {
                        char key = (char)i;
                        typedText.Append(key);
                        File.AppendAllText(logFilePath, key.ToString());
                        foreach (var word in blockedWords)
                        {
                            if (typedText.ToString().Contains(word))
                            {
                                File.AppendAllText("moderation_log.txt", $"Запрещенное слово обнаружено: {word}\n");
                                typedText.Clear();
                            }
                        }
                    }
                }
            }
        }

        private async Task MonitorProcesses(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(2000);
                var processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    string processName = process.ProcessName + ".exe";
                    File.AppendAllText(logFilePath, $"Запущено: {processName} в {DateTime.Now}\n");
                    if (blockedPrograms.Contains(processName))
                    {
                        File.AppendAllText("moderation_log.txt", $"Блокировка программы: {processName}\n");
                        process.Kill();
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

    }
}
