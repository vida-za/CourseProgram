using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using static CourseProgram.Models.Constants;
using System;
using System.Collections.Generic;
using CourseProgram.Services.DataServices.ExtDataService;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddDriverViewModel : BaseAddViewModel
    {
        private readonly CategoryDataService _dataService;
        private readonly List<Category> _categories;

        public AddDriverViewModel(ServicesStore servicesStore, INavigationService driverViewNavigationService)
        {
            _categories = new List<Category>();

            SubmitCommand = new AddDriverCommand(this, servicesStore, driverViewNavigationService);
            CancelCommand = new NavigateCommand(driverViewNavigationService);

            _dataService = servicesStore._categoryService;

            UpdateData();
        }

        private async void UpdateData()
        {
            _categories.Clear();

            IEnumerable<Category> temp = await _dataService.GetTemplateList();

            foreach (Category category in temp)
            {
                _categories.Add(category);
            }
        }

        public List<Category> Categories => _categories;

        private string _driverName;
        public string DriverName
        {
            get => _driverName;
            set
            {
                if (value.Length < 101)
                {
                    _driverName = value;
                    OnPropertyChanged(nameof(DriverName));
                }
            }
        }

        private string _passport;
        public string Passport
        {
            get => _passport;
            set
            {
                if (value.Length < 31)
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
    }
}