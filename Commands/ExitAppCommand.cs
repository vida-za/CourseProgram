namespace CourseProgram.Commands
{
    public class ExitAppCommand : CommandBase
    {
        public ExitAppCommand() { }

        public override void Execute(object? parameter)
        {
            App.Current.Shutdown();
        }
    }
}