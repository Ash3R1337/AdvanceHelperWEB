using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

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
            try
            {
                InitializeComponent();
                foreach (Certificate certificate in certificates)
                {
                    imagePaths.Add(certificate.FilePath);
                    certificateName.Add(certificate.Name);
                }
                certificateImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePaths[0])));
                certificateImage.ToolTip = certificateName[0];
                currentIndex = 0;
                totalCertificates.Content = $"Количество документов: {imagePaths.Count.ToString()}";
                currentCertificate.Content = $"{(currentIndex + 1).ToString()}/{imagePaths.Count.ToString()}";
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error); }
        }

        private void PreviousCertificate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    certificateImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePaths[currentIndex])));
                    certificateImage.ToolTip = certificateName[currentIndex];
                    currentCertificate.Content = $"{(currentIndex + 1).ToString()}/{imagePaths.Count.ToString()}";
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error); }
        }

        private void NextCertificate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentIndex < int.Parse(imagePaths.Count.ToString()) - 1)
                {
                    currentIndex++;
                    certificateImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePaths[currentIndex])));
                    certificateImage.ToolTip = certificateName[currentIndex];
                    currentCertificate.Content = $"{(currentIndex + 1).ToString()}/{imagePaths.Count.ToString()}";
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error); }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
