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
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        private string _house;
        public string House
        {
            get => _house;
            set
            {
                _house = value;
                OnPropertyChanged(nameof(House));
            }
        }

        private string _structure;
        public string Structure
        {
            get => _structure;
            set
            {
                _structure = value;
                OnPropertyChanged(nameof(Structure));
            }
        }

        private string _frame;
        public string Frame
        {
            get => _frame;
            set
            {
                _frame = value;
                OnPropertyChanged(nameof(Frame));
            }
        }
    }
}