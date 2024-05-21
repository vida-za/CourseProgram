namespace CourseProgram.ViewModels.ListingViewModel
{
    public abstract class BaseListingViewModel : BaseViewModel
    {
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
                OnPropertyChanged(nameof(StateFilter));
            }
        }

        protected abstract void Find();

        public abstract void UpdateData();
    }
}