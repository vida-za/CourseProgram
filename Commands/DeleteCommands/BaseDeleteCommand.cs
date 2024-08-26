using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.DeleteCommands
{
    public abstract class BaseDeleteCommand : CommandBaseAsync
    {
        public override async void Execute(object? parameter)
        {
            MessageBoxResult message = MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                IsExecuting = true;

                try
                {
                    await ExecuteAsync(parameter);
                }
                finally { IsExecuting = false; }
            }
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await ExecuteDeleteAsync(parameter);
        }

        protected abstract Task ExecuteDeleteAsync(object? parameter);
    }
}