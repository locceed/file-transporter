using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
using System.Threading;

namespace file_transporter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            drive();
            textBox1.Text = Environment.CurrentDirectory;
        }
        string path = "";
        string doubleclicktemp = "";
        bool doubleclick = false;
        bool simplifyd = false;
        ArrayList drives = new ArrayList();
        ArrayList files = new ArrayList();
        ArrayList orgfilelocations = new ArrayList();
        ArrayList desfilelocations = new ArrayList();
        Thread copy_thread;
        bool added = false;
        bool sused = false;
        private void drive()
        {
            try
            {
                listBox.Items.Clear();
                foreach (DriveInfo each in DriveInfo.GetDrives())
                {
                    drives.Add(each.ToString());
                    listBox.Items.Add(each);
                }
                textBox.Text = path;
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }
        private void back()
        {
            if (path.IndexOf(@"\") != -1) 
            {
                path = path.Substring(0, path.LastIndexOf(@"\")) + "\\";
                listBox1.Items.Clear();
                foreach (string each in Directory.GetFileSystemEntries(path))
                {
                    files.Add(each.Substring(each.LastIndexOf("\\") + 1));
                    listBox1.Items.Add(each.Substring(each.LastIndexOf("\\") + 1));
                }
                textBox.Text = path;
            }
        }
        private void pre_dir()
        {
            try
            {
                path = listBox.SelectedItems[0].ToString();
                listBox1.Items.Clear();
                foreach (string each in Directory.GetFileSystemEntries(path))
                {
                    files.Add(each.Substring(each.LastIndexOf("\\") + 1));
                    listBox1.Items.Add(each.Substring(each.LastIndexOf("\\") + 1));
                }
                path = path.Replace(@"\", "");
                textBox.Text = path;
            }
            catch(IOException)
            {
                MessageBox.Show("这个磁盘无法访问");
            }
        }
        private void pre_doubleclick()
        {
            if (listBox1.SelectedItems.Count > 1)
            {
                doubleclick = false;
            }
            else if (listBox1.SelectedItems.Count == 1)
            {
                doubleclick = true;
                doubleclicktemp = listBox1.SelectedItems[0].ToString();
            }
            else
            {

            }
        }
        private void doubleclickdir()
        {
            try
            {
                if (listBox1.SelectedItems.Count == 0)
                {

                    path += @"\" + doubleclicktemp;
                    listBox1.Items.Clear();
                    foreach (string each in Directory.GetFileSystemEntries(path))
                    {
                        files.Add(each.Substring(each.LastIndexOf("\\") + 1));
                        listBox1.Items.Add(each.Substring(each.LastIndexOf("\\") + 1));
                    }
                    textBox.Text = path;
                }
                else
                {
                    listBox1.UnselectAll();
                }
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("程序没有足够的权限访问这个文件");
            }
        }
        private void add()
        {
            simplifyd = false;
            foreach (object each in listBox1.SelectedItems)
            {
                listBox2.Items.Add(path + @"\" + each.ToString());
            }
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            drive();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            back();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            add();
            added = false;
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            listBox1.SelectAll();
        }
        private void button10_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                textBox1.Text = path + @"\" + listBox1.SelectedItems[0].ToString();
            }
            else
            {
                MessageBox.Show("你选择的文件数有问题哦");
            }
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pre_dir();
        }
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pre_doubleclick();
        }
        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            doubleclickdir();
        }
        private void textBox_MouseEnter(object sender, MouseEventArgs e)
        {
            textBox.Text = path;
        }
        ///////////////////////////////////////////////////////
        private void delete()
        {
            if(listBox2.SelectedItems.Count>0)
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            }
            else
            {

            }
        }
        private void simplify()
        {
            simplifyd = true;
            bool temp1 = false;
            ArrayList temp = new ArrayList();
            foreach(object each in listBox2.Items)
            {
                foreach(object each1 in temp)
                {
                    if (each.ToString() == each1.ToString()) 
                    {
                        temp1 = true;
                        break;
                    }
                    else
                    {

                    }
                }
                if (temp1 == true)
                {
                    temp1 = false;
                }
                else
                {
                    temp.Add(each.ToString());
                }
            }
            listBox2.Items.Clear();
            foreach (object each in temp)
            {
                listBox2.Items.Add(each.ToString());
            }
        }
        private void save()
        {
            string path1 = Environment.CurrentDirectory;
            FileStream file1 = new FileStream(path1 + "\\saves.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer1 = new StreamWriter(file1);
            foreach(object each in listBox2.Items)
            {
                writer1.WriteLine(each.ToString());
            }
            writer1.WriteLine(textBox1.Text);
            writer1.Flush();
            writer1.Close();
            file1.Close();
        }
        private void readsave()
        {
            string path1 = Environment.CurrentDirectory;
            FileStream file1 = new FileStream(path1 + "\\saves.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader reader1 = new StreamReader(file1);
            string line = "";
            ArrayList temp = new ArrayList();
            while((line =reader1.ReadLine()) != null)
            {
                temp.Add(line);
            }
            int count = 0;
            foreach(object each in temp)
            {
                if (count == temp.Count)
                {
                    textBox1.Text = each.ToString();
                }
                else
                {
                    listBox2.Items.Add(each);
                }
                count++;
            }
            simplifyd = false;
            reader1.Close();
            file1.Close();
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            delete();
        }
        private void TabItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (simplifyd == false)
            {
                simplify();
            }
            else
            {

            }
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            listBox2.SelectAll();
        }
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            save();
            textBlock5.Text = "已存储";
        }
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            readsave();
            textBlock5.Text = "已读取";
        }
        ///////////////////////////////////////////////////////
        public void start()
        {
            string target = "" ;
            this.Dispatcher.Invoke(new Action(delegate
                {
                    target = textBox1.Text;
                }));
            Directory.CreateDirectory(target);
            this.Dispatcher.Invoke(new Action(delegate
            {
                textBlock1.Text = "准备中:";
            }));
            foreach (object each in listBox3.Items)
            {
               locatefile(each.ToString(),target);
            }
            this.Dispatcher.Invoke(new Action(delegate
            {
                textBlock1.Text = "正在复制:";
            }));
            int count = 0;
            this.Dispatcher.Invoke(new Action(delegate
            {
                progessbar.Maximum = orgfilelocations.Count;
            }));
            foreach(object each in orgfilelocations)
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    textBlock4.Text = each.ToString() ;
                }));
                File.Copy(each.ToString(), desfilelocations[count].ToString(),true);
                this.Dispatcher.Invoke(new Action(delegate
                {
                    progessbar.Value++;
                }));
                count++;
            }
            this.Dispatcher.Invoke(new Action(delegate
            {
                textBlock4.Text = "无";
                textBlock1.Text = "就绪";
                button4.IsEnabled = true;
            }));
        }
        public void locatefile(string orgpath,string despath)
        {
            try
            {
                string filename = orgpath.Substring(orgpath.LastIndexOf("\\") + 1);
                despath += "\\" + filename;
                foreach (string each in Directory.GetFileSystemEntries(orgpath))
                {
                    if (Directory.Exists(each))
                    {
                        if (!Directory.Exists(despath + "\\" + each.Substring(each.LastIndexOf("\\") + 1)))
                        {
                            Directory.CreateDirectory(despath + "\\" + each.Substring(each.LastIndexOf("\\") + 1));
                        }
                        locatefile(each, despath);
                    }
                    else
                    {
                        if (!Directory.Exists(despath))
                        {
                            Directory.CreateDirectory(despath);
                        }
                        orgfilelocations.Add(each);
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            textBlock4.Text = each.ToString();
                        }));
                        desfilelocations.Add(despath + "\\" + each.Substring(each.LastIndexOf("\\") + 1));
                    }
                }
            }
            catch
            {
                orgfilelocations.Add(orgpath);
                desfilelocations.Add(despath);
            }
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            progessbar.Value = 0;
            button4.IsEnabled = false;
            copy_thread = new Thread(new ThreadStart(start));
            copy_thread.Start();
        }
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sused == false)
                {
                    copy_thread.Suspend();
                    sused = true;
                }
                else
                {
                    copy_thread.Resume();
                    sused = false;
                }
            }
            catch
            {

            }
        }
        private void TabItem_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (added == false)
            {
                foreach (object each in listBox2.Items)
                {
                    listBox3.Items.Add(each);
                }
                added = true;
            }
        }
    }
}
