using CourseProgram.Services;
using CourseProgram.ViewModels;
using System.Threading;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands
{
    public class LoginCommand : CommandBaseAsync
    {
        private readonly LoginViewModel _viewModel;
        private DBConnection db;

        public LoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _viewModel.Message = "Waiting";

            User.Username = _viewModel.Username;
            User.Password = _viewModel.Password;

            db = new DBConnection(_viewModel.Server, _viewModel.Database, User.Username, User.Password, "Login");

            await db.OpenAsync();

            Thread.Sleep(500);
            if (db.ConnectionState == 1)
            {
                db.Close();
                App.Current.MainWindow.DialogResult = true;
            }
            else
            {
                _viewModel.Username = string.Empty;
                _viewModel.Password = string.Empty;
                _viewModel.Message = "Error";
            }
        }
    }
}