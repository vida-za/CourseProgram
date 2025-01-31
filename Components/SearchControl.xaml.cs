using System.Windows;
using System.Windows.Controls;

namespace CourseProgram.Components
{
    /// <summary>
    /// Логика взаимодействия для SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsSearchEnabledProperty =
            DependencyProperty.Register("IsSearchEnabled", typeof(bool), typeof(SearchControl),
                new PropertyMetadata(false));

        public static readonly DependencyProperty CheckBoxTextProperty =
            DependencyProperty.Register("CheckBoxText", typeof(string), typeof(SearchControl), 
                new PropertyMetadata("Поиск"));

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        public bool IsSearchEnabled
        {
            get => (bool)GetValue(IsSearchEnabledProperty);
            set => SetValue(IsSearchEnabledProperty, value);
        }

        public string CheckBoxText
        {
            get => (string)GetValue(CheckBoxTextProperty); 
            set => SetValue(CheckBoxTextProperty, value);
        }

        public SearchControl()
        {
            InitializeComponent();
        }
    }
}