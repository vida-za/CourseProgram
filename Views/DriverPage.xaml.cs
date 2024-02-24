using System.Windows.Controls;
using CourseProgram.ViewModels;

namespace CourseProgram.Views
{
    /// <summary>
    /// Логика взаимодействия для DriverPage.xaml
    /// </summary>
    public partial class DriverPage : Page
    {
        public DriverPage()
        {
            InitializeComponent();
            DataContext = new DriverViewModel();
        }
    }
}