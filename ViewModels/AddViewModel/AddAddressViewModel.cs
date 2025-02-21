using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Services;
using CourseProgram.Stores;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddAddressViewModel : BaseAddViewModel
    {
        public AddAddressViewModel(ServicesStore servicesStore, INavigationService addressViewNavigationService)
        {
            SubmitCommand = new AddAddressCommand(this, servicesStore, addressViewNavigationService);
            CancelCommand = new NavigateCommand(addressViewNavigationService);
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value.Length < 101 && LettersOnlyRegex.IsMatch(value))
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                if (value.Length < 101 && LettersOnlyRegex.IsMatch(value))
                {
                    _street = value;
                    OnPropertyChanged(nameof(Street));
                }
            }
        }

        private string _house;
        public string House
        {
            get => _house;
            set
            {
                if (value.Length < 11 && DigitsOnlyRegex.IsMatch(value))
                {
                    _house = value;
                    OnPropertyChanged(nameof(House));
                }
            }
        }

        private string _structure;
        public string Structure
        {
            get => _structure;
            set
            {
                if (value.Length < 6 && DigitsOnlyRegex.IsMatch(value))
                {
                    _structure = value;
                    OnPropertyChanged(nameof(Structure));
                }
            }
        }

        private string _frame;
        public string Frame
        {
            get => _frame;
            set
            {
                if (value.Length < 6 && DigitsOnlyRegex.IsMatch(value))
                {
                    _frame = value;
                    OnPropertyChanged(nameof(Frame));
                }
            }
        }
    }
}