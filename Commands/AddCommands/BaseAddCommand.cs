using CourseProgram.Services;
using CourseProgram.Stores;
using System;
using System.Threading.Tasks;

namespace CourseProgram.Commands.AddCommands
{
    public abstract class BaseAddCommand : CommandBaseAsync
    {
        protected ServicesStore _servicesStore;
        protected INavigationService _navigationService;

        protected abstract void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e);

        public override Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
