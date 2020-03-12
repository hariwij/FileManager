using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileManager;

namespace TestApp
{
    public partial class Form1 : Form
    {
        FileBrowser fb = new FileBrowser(@"E:\Artists");
        public Form1()
        {
            InitializeComponent();
            fb.FileOnClick += Fb_FileOnClick;
            fb.OnDirChanged += Fb_OnDirChanged;
            Ref();
        }

        private void Fb_OnDirChanged(string Path)
        {
            Ref();
        }

        private void Fb_FileOnClick(string FilePath)
        {
            Process.Start(FilePath);
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            fb.NavigateTo(listBox2.SelectedValue.ToString());
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            fb.NavigateTo(listBox1.SelectedValue.ToString());
        }
        void Ref()
        {
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox1.DataSource = fb.FilesAndDirs.Dirs;
            listBox2.DataSource = fb.FilesAndDirs.Files;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fb.GoBack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var res = fb.CreateFolder(textBox1.Text);
            MessageBox.Show($"{res.Item1}\n{res.Item2}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var res = fb.CreateFile(textBox2.Text);
            MessageBox.Show($"{res.Item1}\n{res.Item2}");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
