using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddDriverViewModel : BaseViewModel
    {
        public AddDriverViewModel(ServicesStore servicesStore, INavigationService driverViewNavigationService)
        {
            _categories = new List<Category>();

            SubmitCommand = new AddDriverCommand(this, servicesStore, driverViewNavigationService);
            CancelCommand = new NavigateCommand(driverViewNavigationService);

            _categoryDataService = servicesStore._categoryService;

            UpdateData();
        }

        private async void UpdateData()
        {
            _categories.Clear();

            IEnumerable<Category> temp = await _categoryDataService.GetItemsAsync();

            foreach (Category category in temp)
            {
                _categories.Add(category);
            }
        }

        private readonly CategoryDataService _categoryDataService;
        private readonly List<Category> _categories;
        public List<Category> Categories => _categories;

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

        private DateOnly _birthDay = new(2000, 1, 1);
        public DateOnly BirthDay
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