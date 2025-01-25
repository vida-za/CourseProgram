using CourseProgram.Stores;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CourseProgram.ViewModels
{
    public class OperationalViewModel : BaseViewModel
    {
        #region fields
        private readonly ServicesStore _servicesStore;
        private readonly SelectedStore _selectedStore;

        private readonly DispatcherTimer updateTimer;

        //private readonly ObservableCollection<> _hauls;
        //private readonly ObservableCollection<> _bids;

        public ICommand SelectionChangedCommand { get; }
        #endregion

        public OperationalViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore) 
        {
            _servicesStore = servicesStore;
            _selectedStore = selectedStore;

            //_hauls = new ObservableCollection<>();
            //_bids = new ObservableCollection<>();

            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);

            UpdateData();

            updateTimer = new()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        #region methods
        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateData();
        }

        public async void UpdateData()
        {
            //
        }

        //protected override void Find()
        //{
        //    if (!string.IsNullOrEmpty(TextFilter))
        //        SelectedItem = Items.FirstOrDefault(obj => obj.Street.ToLower().Contains(TextFilter.ToLower()), SelectedItem);
        //}

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }
        #endregion

        #region properties

        #endregion
    }
}