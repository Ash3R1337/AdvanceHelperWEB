using System;
using System.Windows;
using System.IO;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Forms;
using AHlibrary;
using AdvanceHelperWPF;
using System.Linq;

namespace AdvanceHelperWEB
{
    /// <summary>
    /// Логика взаимодействия для Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        string userLogin;
        string userStatus;
        string DirPathStr; //Переменная, хранящая путь к текущей директории
        FileHandler fileHandler = new FileHandler();
        public Start(string UserLogin, string UserStatus)
        {
            InitializeComponent();
            if (File.Exists("config.txt"))
            {
                DirPathStr = fileHandler.GetPath("config.txt", "Путь к рабочей директории = "); //Получение пути из файла config.txt
                DirPath.Text = DirPathStr;
                if (DirPathStr != "")
                {
                    FilesAddtoListBox();
                    DirectoriesAddtoListBox();
                }
            }
            labelLogin.Content = UserLogin;
            userLogin = UserLogin;
            userStatus = UserStatus;
        }

        private void ChBtn_Click(object sender, RoutedEventArgs e) //Выбор рабочей директории
        {
            DirPathStr = DirPath.Text;
            FilesAddtoListBox();
            DirectoriesAddtoListBox();
        }

        private void SurBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirPath.Text = folderBrowser.SelectedPath;
                DirPathStr = DirPath.Text;
            }
        }

        private void OpenWfolderBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DirPath.Text != "")
                    Process.Start(DirPath.Text);
                else MessageBox.Show("Введите путь к директории.");
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void SortBtn_Click(object sender, RoutedEventArgs e) //Распределение файлов по директориям
        {
            SortFiles(DirPathStr);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e) //Удаление выбранного файла
        {
            FileDelete();
        }

        private void CheckBtn_Click(object sender, RoutedEventArgs e) //Просмотр файла
        {
            OpenFile();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e) //Сохранение выбранной директории в файл
        {
            fileHandler.FileSave("config.txt", DirPath.Text, "Путь к рабочей директории = ");
            MessageBox.Show("Путь был успешно сохранен.");
            FilesAddtoListBox();
            DirectoriesAddtoListBox();
        }

        private void MainBtn_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(userLogin, userStatus);
            menu.Show();
            this.Close();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e) //Создание нового каталога
        {
            CreateCatalog(DirPathStr);
        }

        private void RenameBtn_Click(object sender, RoutedEventArgs e)
        {
            FileRename(DirPathStr);
        }

        private void OpenDirBtn_Click(object sender, RoutedEventArgs e)
        {
            DirOpen();
        }

        private void DeleteFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderDelete();
        }

        private void SheetGenerate_Click(object sender, RoutedEventArgs e)
        {
            Excel excel = new Excel();
            excel.ExcelCreateDocument("Ведомость", userLogin);
        }

        private void SaveVedBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string destinationFolderPath = folderBrowser.SelectedPath;
                    string destinationFilePath = Path.Combine(destinationFolderPath, "Ведомость.xlsx");
                    File.Copy("Ведомость.xlsx", destinationFilePath);
                    MessageBox.Show("Файл был успешно сохранен в выбранную папку.");
                    Process.Start("explorer.exe", destinationFolderPath);
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void DirOpen()
        {
            try
            {
                if (DirPath.Text != "")
                {
                    int index = CatalogsList.SelectedIndex;
                    Process.Start(ListBoxFolders[index]);
                }
                else MessageBox.Show("Введите путь к директории.");
            }
            catch (Win32Exception) { MessageBox.Show("Выбранная папка не найдена."); DirectoriesAddtoListBox(); }
            catch (IndexOutOfRangeException) { MessageBox.Show("Выберите папку, которую нужно просмотреть."); }
            catch (NullReferenceException) { MessageBox.Show("Ошибка. Проверьте правильность рабочей директории"); }
        }

        public void OpenFile() //Просмотр файла
        {
            try
            {
                if (DirPath.Text != "")
                {
                    int index = FilesList.SelectedIndex;
                    Process.Start(ListBoxFiles[index]);
                }
                else MessageBox.Show("Введите путь к директории.");
            }
            catch (Win32Exception) { MessageBox.Show("Выбранный файл не найден."); FilesAddtoListBox(); }
            catch (IndexOutOfRangeException) { MessageBox.Show("Выберите файл, который нужно просмотреть."); }
            catch (NullReferenceException) { MessageBox.Show("Ошибка. Проверьте правильность рабочей директории"); }
        }

        public void FileRename(string DirPath) //Переименование файла
        {
            try
            {
                FileRenameWindow fileRenameWindow = new FileRenameWindow(FilesList.SelectedItem.ToString());
                if (fileRenameWindow.ShowDialog() == true)
                {
                    try
                    {
                        int index = FilesList.SelectedIndex;
                        string sourceFileName = ListBoxFiles[index];
                        string destFileName = DirPath + "\\" + fileRenameWindow.FileNameStr;
                        File.Move(sourceFileName, destFileName);
                        MessageBox.Show("Файл был успешно переименован");
                    }
                    catch (FileNotFoundException) { MessageBox.Show("Выбранного файла не существует"); }
                    catch (IndexOutOfRangeException) { MessageBox.Show("Выберите файл, который нужно переименовать"); }
                    FilesAddtoListBox();
                }
            }
            catch (NullReferenceException) { MessageBox.Show("Ошибка. Проверьте правильность рабочей директории"); }
        }

        public void FileDelete() //Удаление файла
        {
            try
            {
                MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Вы действительно хотите удалить файл?", "Подтверждение", (MessageBoxButtons)MessageBoxButton.YesNo, (MessageBoxIcon)MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int index = FilesList.SelectedIndex;
                    string path = ListBoxFiles[index];
                    File.Delete(path);
                    MessageBox.Show("Файл был успешно удален");
                    FilesAddtoListBox();
                }
            }
            catch (FileNotFoundException) { MessageBox.Show("Выбранного файла не существует"); }
            catch (IndexOutOfRangeException) { MessageBox.Show("Выберите файл, который нужно удалить"); }
            catch (NullReferenceException) { MessageBox.Show("Ошибка. Проверьте правильность рабочей директории"); }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void FolderDelete() //Удаление папки
        {
            try
            {
                MessageBoxResult result = (MessageBoxResult)MessageBox.Show("Вы действительно хотите удалить папку?", "Подтверждение", (MessageBoxButtons)MessageBoxButton.YesNo, (MessageBoxIcon)MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int index = CatalogsList.SelectedIndex;
                    string path = ListBoxFolders[index];
                    Directory.Delete(path);
                    MessageBox.Show("Папка была успешно удалена");
                    DirectoriesAddtoListBox();
                }
            }
            catch (FileNotFoundException) { MessageBox.Show("Выбранной папки не существует"); }
            catch (IndexOutOfRangeException) { MessageBox.Show("Выберите папку, которую нужно удалить"); }
            catch (NullReferenceException) { MessageBox.Show("Ошибка. Проверьте правильность рабочей директории"); }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public bool CreateCatalog(string DirPath) //Создание папки
        {
            CatalogWindow catalogWindow = new CatalogWindow();
            if (DirPath != "")
            {
                if (catalogWindow.ShowDialog() == true)
                {
                    if (Directory.Exists(DirPath + "\\" + catalogWindow.DirNameStr))
                    {
                        MessageBox.Show($"Каталог {catalogWindow.DirNameStr} уже существует");
                        return false;
                    }
                    else
                    {
                        Directory.CreateDirectory(DirPath + "\\" + catalogWindow.DirNameStr);
                        MessageBox.Show($"Каталог {catalogWindow.DirNameStr} успешно создан");
                        DirectoriesAddtoListBox();
                        return true;
                    }
                }
            }
            else
                MessageBox.Show("Путь не выбран");
            return false;
        }

        string[] ListBoxFiles; //Массив с расположением всех файлов
        string[] ListBoxFolders; //Массив с расположением всех папок

        public void FilesAddtoListBox() //Добавление файлов в FilesList
        {
            try
            {
                string[] formats = fileHandler.GetPath("config.txt", "Доступные форматы файлов (через запятую): ").Split(',');
                string pattern = "";
                foreach (string format in formats)
                {
                    pattern += "." + format;
                }
                FilesList.Items.Clear();
                string[] AllFiles = Directory.EnumerateFiles(DirPathStr).Where(file => pattern.Contains(Path.GetExtension(file).ToLower())).ToArray();
                Array.Copy(AllFiles, ListBoxFiles = new string[AllFiles.Length], AllFiles.Length);
                foreach (string filename in AllFiles)
                {
                    FilesList.Items.Add(System.IO.Path.GetFileName(filename));
                }
            }
            catch (DirectoryNotFoundException) {/*MessageBox.Show("Выбранная директория не найдена.");*/}
            catch (ArgumentException)
            {
                /*MessageBox.Show("Путь не выбран.");
                FilesList.Items.Clear();*/
            }
        }
        public void DirectoriesAddtoListBox() //Добавление папок в CatalogsList
        {
            try
            {
                CatalogsList.Items.Clear();
                string[] AllFolders = Directory.GetDirectories(DirPathStr);
                Array.Copy(AllFolders, ListBoxFolders = new string[AllFolders.Length], AllFolders.Length);
                foreach (string foldername in AllFolders)
                {
                    string directory = new DirectoryInfo(foldername).Name;
                    CatalogsList.Items.Add(directory);
                }
            }
            catch (DirectoryNotFoundException) { MessageBox.Show("Выбранная директория не найдена."); }
            catch (ArgumentException)
            {
                MessageBox.Show("Путь не выбран.");
                FilesList.Items.Clear();
            }
        }

        public void SortFiles(string DirPath) //Распределение файлов по папкам
        {
            try
            {
                string[] AllFolders = Directory.GetDirectories(DirPath);
                string[] AllFiles = Directory.GetFiles(DirPath, "*.docx", SearchOption.TopDirectoryOnly);
                int count = 0;
                foreach (string folder in AllFolders)
                    foreach (string filename in AllFiles)
                    {
                        try
                        {
                            string Name = new DirectoryInfo(folder).Name;
                            Regex FolderName = new Regex(Name);
                            MatchCollection match = FolderName.Matches(Path.GetFileName(filename));
                            if (match.Count > 0)
                            {
                                string file = Path.GetFileName(filename);
                                string NewPath = Path.Combine(folder, file);
                                File.Move(filename, NewPath);
                                count++;
                                FilesAddtoListBox();
                            }
                        }
                        catch (IOException) { MessageBox.Show($"Файл {Path.GetFileName(filename)} уже есть в одной из папок"); }
                    }
                if (count > 0) MessageBox.Show($"Было распределено {count} файлов.");
                else MessageBox.Show("Файлы для распределения не были найдены");
            }
            catch (DirectoryNotFoundException) { MessageBox.Show("Путь не выбран."); }
            catch (ArgumentException) { MessageBox.Show("Путь не выбран."); FilesList.Items.Clear(); }
        }

        private void TeacherPortfolioBtn_Click(object sender, RoutedEventArgs e)
        {
            TeachersPortfolio teachersPortfolio = new TeachersPortfolio(userLogin, userStatus);
            teachersPortfolio.Show();
            this.Close();
        }
    }
}
