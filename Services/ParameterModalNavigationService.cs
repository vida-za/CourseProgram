using CourseProgram.Stores;
using CourseProgram.ViewModels;
using System;

namespace CourseProgram.Services
{
    public class ParameterModalNavigationService<TParameter, TViewModel> 
        where TViewModel : BaseViewModel
    {
        private readonly ModalNavigationStore _navigationStore;
        private readonly Func<TParameter, TViewModel> _createViewModel;

        public ParameterModalNavigationService(
            ModalNavigationStore navigationStore,
            Func<TParameter, TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(TParameter parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel(parameter);
        }
    }
}