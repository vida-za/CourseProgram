using System.Windows.Input;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class BaseAddViewModel : BaseViewModel
    {
        public ICommand SubmitCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }
    }
}