using System.ComponentModel;

namespace CourseProgram.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Wid { get; set; }
        public int Heig { get; set; }

        protected void OnPropertyChanged(string propertyName)
        {
            Wid = 1200;
            Heig = 500;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}