using CourseProgram.Commands;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand ExitApp { get; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            Username = "postgres"; //debug
            Password = "Gnbxrf2004"; //debug

            ExitApp = new ExitAppCommand();
            LoginCommand = new LoginCommand(this);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
    }
}