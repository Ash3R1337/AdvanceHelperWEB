using System.Windows;
using AHlibrary;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Diagnostics;
using System;
using System.Windows.Media.Imaging;
using AdvanceHelperWPF;

namespace AdvanceHelperWEB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileHandler fileHandler = new FileHandler();
        public MainWindow()
        {
            InitializeComponent();
            labelForgetPassInstruct.Content = $"Обратитесь к администратору базы данных,\nлибо сообщите о проблеме на почту: \n{fileHandler.GetPath("config.txt", "Почта для связи: ")}";
        }

        Regex regex = new Regex(@"[^a-zA-Z0-9]");

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MatchCollection matches = regex.Matches(LoginBox.Text);
            if (matches.Count > 0)
            {
                System.Windows.MessageBox.Show("Логин содержит недопустимые символы");
                LoginBox.Text = LoginBox.Text.Replace(matches[0].ToString(), "");
                LoginBox.BorderBrush = Brushes.Red;
            }
            else LoginBox.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuOpenCheck();
        }

        private void LoginBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MenuOpenCheck();
            }
        }

        private void PasswordHidden_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MenuOpenCheck();
            }
        }

        private void MenuOpenCheck()
        {
            DBconnect dbconnect = new DBconnect();
            bool Auth = dbconnect.AuthCheck(LoginBox, PasswordHidden);
            if (Auth == true)
            {
                string UserStatus = dbconnect.GetValueByString("Статус", "пользователи", "Логин", LoginBox.Text);
                if (UserStatus == "администратор бд")
                {
                    Tables tables = new Tables(LoginBox.Text, UserStatus);
                    tables.Show();
                    this.Close();
                }
                else
                {
                    Menu menu = new Menu(LoginBox.Text, UserStatus);
                    menu.Show();
                    this.Close();
                }
            }
        }

        //Скрытие/Показ пароля
        private void ShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword.Source = new BitmapImage(new Uri("eye-icon.png", UriKind.Relative));
            ShowPasswordFunction();
        }
        private void ShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowPassword.Source = new BitmapImage(new Uri("eye-blind-icon.png", UriKind.Relative));
            HidePasswordFunction();
        }
        private void ShowPassword_MouseLeave(object sender, MouseEventArgs e) 
        {
            ShowPassword.Source = new BitmapImage(new Uri("eye-blind-icon.png", UriKind.Relative));
            HidePasswordFunction();
        }
        private void ShowPasswordFunction()
        {
            PasswordUnmask.Visibility = Visibility.Visible;
            PasswordHidden.Visibility = Visibility.Hidden;
            PasswordUnmask.Text = PasswordHidden.Password;
        }

        private void HidePasswordFunction()
        {
            PasswordUnmask.Visibility = Visibility.Hidden;
            PasswordHidden.Visibility = Visibility.Visible;
        }

        private void labelForgetPass_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            labelForgetPass.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 174));
            labelForgetPassInstruct.Visibility = Visibility.Visible;
        }
        private void labelForgetPass_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            labelForgetPass.Foreground = new SolidColorBrush(Color.FromRgb(3, 172, 118));
            labelForgetPassInstruct.Visibility = Visibility.Hidden;
        }
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InstructionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("help.chm");
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK); }
        }
    }
}
