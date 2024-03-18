using CourseProgram.Commands;
using CourseProgram.DataClasses;
using CourseProgram.Services;
using CourseProgram.Stores;
using System;
using System.Windows.Input;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels
{
    public class AddDriverViewModel : BaseViewModel
    {
        public AddDriverViewModel(DriverData driverData, NavigationService driverViewNavigationService)
        {
            SubmitCommand = new AddDriverCommand(this, driverData, driverViewNavigationService);
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

        private DriverCategory _category;
        public DriverCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
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