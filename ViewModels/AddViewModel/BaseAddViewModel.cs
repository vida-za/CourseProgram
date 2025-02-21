using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class BaseAddViewModel : BaseViewModel
    {
        public ICommand SubmitCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }

        protected static readonly Regex LettersAndDigitsRegex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9\s-,.'""+]*$", RegexOptions.Compiled);
        protected static readonly Regex LettersOnlyRegex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ\s-,.'""+]*$", RegexOptions.Compiled);
        protected static readonly Regex DigitsOnlyRegex = new Regex(@"^[0-9\s,.]*$", RegexOptions.Compiled);
    }
}