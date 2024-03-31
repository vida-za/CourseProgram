using CourseProgram.Services;
using CourseProgram.ViewModels;
using System.Threading;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands
{
    public class LoginCommand : CommandBaseAsync
    {
        private readonly LoginViewModel _loginViewModel;
        //private readonly INavigationService _navigationService;
        private DBConnection db;

        public LoginCommand(LoginViewModel loginViewModel/*, INavigationService navigationService*/)
        {
            _loginViewModel = loginViewModel;
            //_navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _loginViewModel.Message = "Waiting";

            User.Username = _loginViewModel.Username;
            User.Password = _loginViewModel.Password;

            db = new DBConnection(Server, Database, User.Username, User.Password, "Login");

            await db.OpenAsync();

            Thread.Sleep(500);
            if (db.ConnectionState == 1)
            {
                db.Close();
                App.Current.MainWindow.DialogResult = true;
                //_navigationService.Navigate();
            }
            else
            {
                _loginViewModel.Username = string.Empty;
                _loginViewModel.Password = string.Empty;
                _loginViewModel.Message = "Error";
            }
        }
    }
}