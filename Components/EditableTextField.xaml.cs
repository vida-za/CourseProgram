using System.Windows;
using System.Windows.Controls;

namespace CourseProgram.Components
{
    /// <summary>
    /// Логика взаимодействия для EditableTextField.xaml
    /// </summary>
    public partial class EditableTextField : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(EditableTextField)
                , new PropertyMetadata(string.Empty));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public EditableTextField()
        {
            InitializeComponent();
        }
    }
}