using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows.Controls;

namespace CourseProgram.Behaviors
{
    public class ColumnHeaderBehaviour : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.AutoGeneratingColumn += OnGeneratingColumn;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AutoGeneratingColumn -= OnGeneratingColumn;
        }

        private static void OnGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            else
                e.Cancel = true;
        }
    }
}