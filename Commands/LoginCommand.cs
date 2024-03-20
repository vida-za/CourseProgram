using CourseProgram.Services;
using CourseProgram.ViewModels;
using System.Threading;

namespace CourseProgram.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly NavigationService _navigationService;
        private DBConnection db;

        public LoginCommand(LoginViewModel loginViewModel, NavigationService navigationService)
        {
            _loginViewModel = loginViewModel;
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            db = new DBConnection(5, null, null, "Login cnn");
            User.Username = _loginViewModel.Username;
            User.Password = _loginViewModel.Password;

            db.Login = User.Username;
            db.Password = User.Password;
            db.AutoConnect();
            _loginViewModel.Message = "Waiting";

            Thread.Sleep(1000);
            if (db.ConnectionState == 1)
            {
                db.CancelConnect();
                _navigationService.Navigate();
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