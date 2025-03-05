using CourseProgram.Stores;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CourseProgram.ViewModels.ListingViewModel
{
    public abstract class BaseListingViewModel : BaseViewModel
    {
        protected ServicesStore _servicesStore;
        protected SelectedStore _selectedStore;
        protected ControllersStore _controllersStore;

        protected DispatcherTimer updateTimer;

        public ICommand SelectionChangedCommand { get; protected set; }

        private string _textFilter;
        public string TextFilter
        {
            get => _textFilter;
            set
            {
                _textFilter = value;
                OnPropertyChanged(nameof(TextFilter));
                Find();
            }
        }

        private bool _stateFilter;
        public bool StateFilter
        {
            get => _stateFilter;
            set
            {
                _stateFilter = value;
                if (_stateFilter == false)
                    TextFilter = string.Empty;
                OnPropertyChanged(nameof(StateFilter));
            }
        }

        protected static void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        protected abstract void Find();

        public abstract void UpdateData();

        public abstract Task UpdateDataAsync();

        public virtual void SwitchHandler() { }
    }
}