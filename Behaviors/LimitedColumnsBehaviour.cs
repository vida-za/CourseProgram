using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace CourseProgram.Behaviors
{
    public class LimitedColumnsBehaviour : Behavior<DataGrid>
    {
        public static readonly DependencyProperty MaxColumnsProperty = DependencyProperty.Register(nameof(MaxColumns), typeof(int), typeof(LimitedColumnsBehaviour), new PropertyMetadata(int.MaxValue));

        public int MaxColumns
        {
            get => (int)GetValue(MaxColumnsProperty);
            set => SetValue(MaxColumnsProperty, value);
        }

        private int _currentColumnCount;

        protected override void OnAttached()
        {
            AssociatedObject.AutoGeneratingColumn += OnAutoGeneratingColumn;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AutoGeneratingColumn -= OnAutoGeneratingColumn;
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (_currentColumnCount >= MaxColumns)
                e.Cancel = true;
            else
                _currentColumnCount++;
        }
    }
}