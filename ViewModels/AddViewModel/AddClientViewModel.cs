using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Services;
using CourseProgram.Stores;
using System.Collections.ObjectModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddClientViewModel : BaseAddViewModel
    {
        public AddClientViewModel(ServicesStore servicesStore, INavigationService navigationService) 
        {
            SubmitCommand = new AddClientCommand(this, servicesStore, navigationService);
            CancelCommand = new NavigateCommand(navigationService);
        }

        public ObservableCollection<string> TypeArray => GetFullEnumDescription(typeof(ClientTypeValues));

        #region don`t must have
        private string _bik;
        public string BIK
        {
            get => _bik;
            set
            {
                if (value.Length < 10 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _bik = value;
                    OnPropertyChanged(nameof(BIK));
                }
            }
        }

        private string _correspondent;
        public string Correspondent
        {
            get => _correspondent;
            set
            {
                if (value.Length < 21 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _correspondent = value;
                    OnPropertyChanged(nameof(Correspondent));
                }
            }
        }

        private string _bank;
        public string Bank
        {
            get => _bank;
            set
            {
                if (value.Length < 51 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _bank = value;
                    OnPropertyChanged(nameof(Bank));
                }
            }
        }
        #endregion

        #region must have
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value.Length < 101 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                if (value.Length < 8 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        private string _inn;
        public string INN
        {
            get => _inn;
            set
            {
                if (value.Length < 13 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _inn = value;
                    OnPropertyChanged(nameof(INN));
                }
            }
        }

        private string _kpp;
        public string KPP
        {
            get => _kpp;
            set
            {
                if (value.Length < 10 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _kpp = value;
                    OnPropertyChanged(nameof(KPP));
                }
            }
        }

        private string _ogrn;
        public string OGRN
        {
            get => _ogrn;
            set
            {
                if (value.Length < 16 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _ogrn = value;
                    OnPropertyChanged(nameof(OGRN));
                }
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private string _checking;
        public string Checking
        {
            get => _checking; 
            set
            {
                if (value.Length < 21 && LettersAndDigitsRegex.IsMatch(value))
                {
                    _checking = value;
                    OnPropertyChanged(nameof(Checking));
                }
            }
        }

        private string _phoneLoad;
        public string PhoneLoad
        {
            get => _phoneLoad;
            set
            {
                _phoneLoad = value;
                OnPropertyChanged(nameof(PhoneLoad));
            }
        }

        private string _phoneOnLoad;
        public string PhoneOnLoad
        {
            get => _phoneOnLoad;
            set
            {
                _phoneOnLoad = value;
                OnPropertyChanged(nameof(PhoneLoad));
            }
        }
        #endregion
    }
}