using System.Windows;

namespace AdvanceHelperWEB
{
    /// <summary>
    /// Логика взаимодействия для FileRenameWindow.xaml
    /// </summary>
    public partial class FileRenameWindow : Window
    {
        public FileRenameWindow(string FileName)
        {
            InitializeComponent();
            FileNameTextBox.Text = FileName;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string FileNameStr
        {
            get { return FileNameTextBox.Text; }
        }
    }
}
