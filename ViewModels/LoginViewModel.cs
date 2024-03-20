using CourseProgram.Commands;
using CourseProgram.Services;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;

        public LoginViewModel(NavigationService navigationService)
        {
            Username = "postgres"; //debug
            Password = "Gnbxrf2004"; //debug

            _navigationService = navigationService;

            LoginCommand = new LoginCommand(this, _navigationService);
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

        public ICommand LoginCommand { get; set; }
    }
}