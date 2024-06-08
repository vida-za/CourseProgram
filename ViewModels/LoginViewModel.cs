using CourseProgram.Commands;
using System.Configuration;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand ExitApp { get; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            Server = ConfigurationManager.AppSettings["Server"];
            Database = ConfigurationManager.AppSettings["DatabaseName"];
            Username = "postgres"; //debug
            Password = "Gnbxrf2004"; //debug

            ExitApp = new ExitAppCommand();
            LoginCommand = new LoginCommand(this);
        }

        private string _server;
        public string Server
        {
            get => _server; 
            set 
            { 
                _server = value;
                OnPropertyChanged(nameof(Server));
                UpdateConfig("Server", value);
            }
        }

        private string _database;
        public string Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged(nameof(Database));
                UpdateConfig("DatabaseName", value);
            }
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

        static void UpdateConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}