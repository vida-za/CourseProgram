namespace CourseProgram.ViewModels.DetailViewModel
{
    public class BaseDetailViewModel : BaseViewModel
    {
        private bool _isDirty = false;
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                _isDirty = value;
                OnPropertyChanged(nameof(IsDirty));
            }
        }
    }
}