using System;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Windows.Media;

namespace AdvanceHelperWEB
{
    /// <summary>
    /// Логика взаимодействия для Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }


        private void SurBtn_Click(object sender, RoutedEventArgs e)
        {   
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirPath.Text = folderBrowser.SelectedPath;
            }
        }

        private void ChBtn_Click(object sender, RoutedEventArgs e)
        {
            FilesAddtoListBox();
            DirectoriesAddtoListBox();
        }

        string[] ListBoxFiles; //Массив с расположением всех файлов
        string[] ListBoxFolders; //Массив с расположением всех папок

        private void FilesAddtoListBox() //Добавление файлов в FilesList
        {
            try
            {
                FilesList.Items.Clear();
                string[] AllFiles = Directory.GetFiles(DirPath.Text, "*.docx", SearchOption.TopDirectoryOnly);
                Array.Copy(AllFiles, ListBoxFiles = new string[AllFiles.Length], AllFiles.Length);
                foreach (string filename in AllFiles)
                {
                    FilesList.Items.Add(System.IO.Path.GetFileName(filename));
                }
            }
            catch (DirectoryNotFoundException) { MessageBox.Show("Выбранная директория не найдена."); }
            catch (ArgumentException)
            {
                /*MessageBox.Show("Путь не выбран.");
                FilesList.Items.Clear();*/
            }
        }
        private void DirectoriesAddtoListBox() //Добавление папок в CatalogsList
        {
            try
            {
                CatalogsList.Items.Clear();
                string[] AllFolders = Directory.GetDirectories(DirPath.Text);
                Array.Copy(AllFolders, ListBoxFolders = new string[AllFolders.Length], AllFolders.Length);
                foreach (string foldername in AllFolders)
                {
                    string directory = new DirectoryInfo(foldername).Name;
                    CatalogsList.Items.Add(directory);
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Путь не выбран.");
                FilesList.Items.Clear();
            }
        }

        private void EnterBtn_Copy2_Click(object sender, RoutedEventArgs e)
        {
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            var colorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF56E5F3"));
            var BlueStyle = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0463A4"));
            var TopPanel = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF02F4FF"));
            gradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF1B77D3"), 0));
            gradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF2291BB"), 1));
            this.Resources.Add("ScreenGradientBrush", gradientBrush);
            this.Resources.Add("ButtonsBrush", colorBrush);
            this.Resources.Add("PanelBrush", BlueStyle);
            this.Resources.Add("TopPanelBrush", TopPanel);
            EnterBtn.Background = (Brush)this.TryFindResource("ButtonsBrush");
            CheckBtn.Background = (Brush)this.TryFindResource("ButtonsBrush");
            RenameBtn.Background = (Brush)this.TryFindResource("ButtonsBrush");
            CreateBtn.Background = (Brush)this.TryFindResource("ButtonsBrush");
            DeleteBtn.Background = (Brush)this.TryFindResource("ButtonsBrush");
            MakeAcheckBtn.Background = (Brush)this.TryFindResource("ButtonsBrush");
            OpenDirBtn.Background = (Brush)this.TryFindResource("ButtonsBrush");
            grid.Background = (Brush)this.TryFindResource("ScreenGradientBrush");
            panel.Fill = (Brush)this.TryFindResource("PanelBrush");
            topPanel.Fill = (Brush)this.TryFindResource("TopPanelBrush");
        }

        private void EnterBtn_Copy7_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
