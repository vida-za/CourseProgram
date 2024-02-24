using System;
using System.Windows;
using System.Windows.Controls;

namespace CourseProgram.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            if (login == "admin" && password == "password")
            {
                this.NavigationService.Navigate(new Uri("DriverPage.xaml", UriKind.Relative));
            }
            else
            {
                txtError.Text = "Неверный логин или пароль";
            }
        }
    }
}