using System;
using System.Windows.Controls;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Views
{
    /// <summary>
    /// Логика взаимодействия для AddDriverView.xaml
    /// </summary>
    public partial class AddDriverView : UserControl
    {
        public AddDriverView()
        {
            InitializeComponent();
            cbx.ItemsSource = Enum.GetValues(typeof(DriverCategory));
        }
    }
}