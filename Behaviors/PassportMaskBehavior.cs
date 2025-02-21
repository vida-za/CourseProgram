using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace CourseProgram.Behaviors
{
    public class PassportMaskBehavior : Behavior<TextBox>, INotifyPropertyChanged
    {
        private const string DefaultText = "____ ______";

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
            AssociatedObject.CaretIndex = 0;
            AssociatedObject.PreviewKeyDown += OnKeyDown;
            AssociatedObject.PreviewTextInput += OnTextInput;
            AssociatedObject.GotFocus += OnGotFocus;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AssociatedObject.Text) || AssociatedObject.Text == DefaultText)
            {
                AssociatedObject.Text = DefaultText;
                AssociatedObject.CaretIndex = 0;
            }
        }

        private void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]) || (MaskedText.Replace(" ", "").Length >= 10 && MaskedText != DefaultText))
            {
                e.Handled = true;
                return;
            }

            var digits = new string(AssociatedObject.Text.Where(char.IsDigit).ToArray()) + e.Text;
            MaskedText = FormatPassportNumber(digits);
            AssociatedObject.Text = MaskedText;
            AssociatedObject.CaretIndex = MaskedText.Length;
            e.Handled = true;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && AssociatedObject.CaretIndex > 0)
            {
                var digits = new string(AssociatedObject.Text.Where(char.IsDigit).ToArray());
                if (digits.Length > 0)
                    digits = digits.Substring(0, digits.Length - 1);

                MaskedText = FormatPassportNumber(digits);
                AssociatedObject.Text = MaskedText;
                AssociatedObject.CaretIndex = MaskedText.Length;
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                e.Handled = true;
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));
                var digits = new string(pastedText.Where(char.IsDigit).ToArray());

                MaskedText = FormatPassportNumber(digits);
                AssociatedObject.Text = MaskedText;
                AssociatedObject.CaretIndex = MaskedText.Length;
            }
            e.CancelCommand();
        }

        private string FormatPassportNumber(string digits)
        {
            if (digits.Length > 10) digits = digits.Substring(0, 10);

            if (digits.Length > 4) return $"{digits.Substring(0, 4)} {digits.Substring(4)}";
            return digits;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= OnKeyDown;
            AssociatedObject.PreviewTextInput -= OnTextInput;
            AssociatedObject.GotFocus -= OnGotFocus;
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}