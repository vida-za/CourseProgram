using System.Windows;

namespace CourseProgram.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение закрытия", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes) 
            {
                App.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}