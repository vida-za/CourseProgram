using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class EditableTextFieldViewModel : BaseViewModel
    {
        public ICommand EditCommand { get; }

        public EditableTextFieldViewModel()
        {
            IsEditing = false;
            EditCommand = new RelayCommand(() => IsEditing = !IsEditing);
        }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }
    }
}