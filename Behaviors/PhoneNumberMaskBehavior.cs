using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace CourseProgram.Behaviors
{
    public class PhoneNumberMaskBehavior : Behavior<TextBox>, INotifyPropertyChanged
    {
        private const string DefaultText = "+7 ";

        private string _maskedText = DefaultText;
        public string MaskedText
        {
            get => _maskedText;
            set
            {
                _maskedText = value;
                OnPropertyChanged(nameof(MaskedText));
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Text = MaskedText;
            AssociatedObject.CaretIndex = MaskedText.Length;
            AssociatedObject.PreviewTextInput += OnTextInput;
            AssociatedObject.PreviewKeyDown += OnKeyDown;
            AssociatedObject.GotFocus += OnGotFocus;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AssociatedObject.Text) || AssociatedObject.Text == DefaultText)
            {
                AssociatedObject.Text = DefaultText;
                AssociatedObject.CaretIndex = DefaultText.Length;
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string))) 
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));
                var digits = new string(pastedText.Where(char.IsDigit).ToArray());

                MaskedText = FormatPhoneNumber(digits);
                AssociatedObject.Text = MaskedText;
                AssociatedObject.CaretIndex = MaskedText.Length;
            }
            e.CancelCommand();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && AssociatedObject.CaretIndex > 4)
            {
                var digits = new string(AssociatedObject.Text.Where(char.IsDigit).ToArray());
                if (digits.Length > 1)
                    digits = digits.Substring(0, digits.Length - 1);

                MaskedText = FormatPhoneNumber(digits);
                AssociatedObject.Text = MaskedText;
                AssociatedObject.CaretIndex = MaskedText.Length;
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                e.Handled = true;
            }
        }

        private void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
                return;
            }

            var digits = new string(AssociatedObject.Text.Where(char.IsDigit).ToArray()) + e.Text;
            MaskedText = FormatPhoneNumber(digits);
            AssociatedObject.Text = MaskedText;
            AssociatedObject.CaretIndex = MaskedText.Length;
            e.Handled = true;
        }

        private string FormatPhoneNumber(string digits)
        {
            if (digits.Length > 11) digits = digits.Substring(0, 11);

            if (digits.Length > 10) return $"+7 ({digits.Substring(1, 3)}) {digits.Substring(4, 3)}-{digits.Substring(7, 2)}-{digits.Substring(9, 2)}";
            if (digits.Length > 7) return $"+7 ({digits.Substring(1, 3)}) {digits.Substring(4, 3)}-{digits.Substring(7)}";
            if (digits.Length > 4) return $"+7 ({digits.Substring(1, 3)}) {digits.Substring(4)}";
            if (digits.Length > 1) return $"+7 ({digits.Substring(1)})";

            return "+7 ";
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnTextInput;
            AssociatedObject.PreviewKeyDown -= OnKeyDown;
            AssociatedObject.GotFocus -= OnGotFocus;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}