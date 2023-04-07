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
using System.Windows.Shapes;

namespace AdvanceHelperWPF
{
    /// <summary>
    /// Логика взаимодействия для CertificatesWindow.xaml
    /// </summary>
    public partial class CertificatesWindow : Window
    {
        private int currentIndex = 0;
        List<string> imagePaths = new List<string>();
        List<string> certificateName = new List<string>();

        public CertificatesWindow(List<Certificate> certificates)
        {
            InitializeComponent();
            foreach (Certificate certificate in certificates)
            {
                imagePaths.Add(certificate.FilePath);
                certificateName.Add(certificate.Name);
            }
            certificateImage.Source = new BitmapImage(new Uri(imagePaths[0], UriKind.Relative));
            certificateImage.ToolTip = certificateName[0];
            currentIndex = 0;
            totalCertificates.Content = $"Количество документов: {imagePaths.Count.ToString()}";
            currentCertificate.Content = $"{(currentIndex + 1).ToString()}/{imagePaths.Count.ToString()}";
        }

        private void PreviousCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                certificateImage.Source = new BitmapImage(new Uri(imagePaths[currentIndex], UriKind.Relative));
                certificateImage.ToolTip = certificateName[currentIndex];
                currentCertificate.Content = $"{(currentIndex + 1).ToString()}/{imagePaths.Count.ToString()}";
            }
        }

        private void NextCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < int.Parse(imagePaths.Count.ToString()) - 1)
            {
                currentIndex++;
                certificateImage.Source = new BitmapImage(new Uri(imagePaths[currentIndex], UriKind.Relative));
                certificateImage.ToolTip = certificateName[currentIndex];
                currentCertificate.Content = $"{(currentIndex + 1).ToString()}/{imagePaths.Count.ToString()}";
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
