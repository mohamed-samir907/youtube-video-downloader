using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExtractor;

namespace Sales_Project
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool b;

        private void button4_Click(object sender, EventArgs e)
        {
            if(!b)
            {
                this.WindowState = FormWindowState.Minimized;
            }   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select Your Path..." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = fbd.SelectedPath;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please Select Your Folder", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(textBox1.Text);
                VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == Convert.ToInt32(comboBox1.Text));
                if (video.RequiresDecryption)
                    DownloadUrlResolver.DecryptDownloadUrl(video);
                VideoDownloader downloader = new VideoDownloader(video, Path.Combine(textBox2.Text + "\\", video.Title + video.VideoExtension));
                downloader.DownloadProgressChanged += downloader_DownloadProgressChanged;
                Thread t = new Thread(() => { downloader.Execute(); }) { IsBackground = true };
                t.Start();
            }
            catch { }
        }

        private void Downloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void downloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    progressBar1.Value = (int)e.ProgressPercentage;
                    label3.Text = string.Format("{0:0.##}", e.ProgressPercentage) + "%";
                    progressBar1.Update();
                }));
            }
            catch { }
        }
    }
}
