namespace CourseProgram.ViewModels
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

        protected abstract void Find();

        public abstract void UpdateData();
    }
}