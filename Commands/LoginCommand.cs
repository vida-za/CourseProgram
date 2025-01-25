using CourseProgram.Services.DBServices;
using CourseProgram.ViewModels;
using System.Threading;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands
{
    public class LoginCommand : CommandBaseAsync
    {
        private readonly LoginViewModel _viewModel;
        private Connection loginConnection;

        public LoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _viewModel.Message = "Waiting";

            User.Username = _viewModel.Username;
            User.Password = _viewModel.Password;

            loginConnection = new Connection(_viewModel.Server, _viewModel.Database, User.Username, User.Password);

            await loginConnection.OpenAsync();

            Thread.Sleep(500);
            if (loginConnection.ConnectionState == 1)
            {
                await loginConnection.CloseAsync();
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