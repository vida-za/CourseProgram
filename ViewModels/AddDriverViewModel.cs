using CourseProgram.Commands;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using System;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class AddDriverViewModel : BaseViewModel
    {
        public AddDriverViewModel(DriverDataService driverDataService, NavigationService driverViewNavigationService)
        {
            SubmitCommand = new AddDriverCommand(this, driverDataService, driverViewNavigationService);
            CancelCommand = new NavigateCommand(driverViewNavigationService);
        }

        private string _driverName;
        public string DriverName
        {
            get => _driverName;
            set
            {
                _driverName = value;
                OnPropertyChanged(nameof(DriverName));
            }
        }

        private string _passport;
        public string Passport
        {
            get => _passport;
            set
            {
                _passport = value;
                OnPropertyChanged(nameof(Passport));
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

        private DateTime _birthDay = new(2000, 1, 1);
        public DateTime BirthDay
        {
            get => _birthDay;
            set 
            {
                _birthDay = value;
                OnPropertyChanged(nameof(BirthDay));
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
    }
}