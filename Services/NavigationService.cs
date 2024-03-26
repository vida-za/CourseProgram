using CourseProgram.Stores;
using CourseProgram.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CourseProgram.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<BaseViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<BaseViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            if (_navigationStore.CurrentViewModel is DriverListingViewModel)
            {
                DriverListingViewModel temp = _navigationStore.CurrentViewModel as DriverListingViewModel;
                if (temp.SelectedDriver is not null) _navigationStore.CurrentViewModel = _createViewModel();
            }
            else
                _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}