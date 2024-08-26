using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Services;
using CourseProgram.Stores;
using System;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddWorkerViewModel : BaseAddViewModel
    {
        public AddWorkerViewModel(ServicesStore servicesStore, INavigationService navigationService) 
        {
            SubmitCommand = new AddWorkerCommand(this, servicesStore, navigationService);
            CancelCommand = new NavigateCommand(navigationService);
        }

        #region properties
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value.Length < 101)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private DateOnly _birthDay = new(2000, 1, 1);
        public DateOnly BirthDay
        {
            get => _birthDay;
            set
            {
                if (value < DateOnly.FromDateTime(DateTime.Now).AddYears(-18)
                    && value > DateOnly.FromDateTime(DateTime.Now).AddYears(-150))
                {
                    _birthDay = value;
                    OnPropertyChanged(nameof(BirthDay));
                }
            }
        }

        private string _passport;
        public string Passport
        {
            get => _passport;
            set
            {
                if (value.Length < 101)
                {
                    _passport = value;
                    OnPropertyChanged(nameof(Passport));
                }
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone; 
            set
            {
                if (value.Length < 21)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }
        #endregion
    }
}