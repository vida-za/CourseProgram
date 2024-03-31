namespace CourseProgram.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel() 
        {
            Title = "Добро пожаловать!";
            Description = "Это приложение, разработанно для оптимизации управления компанией логистических перевозок. " +
                "Сочетая в себе простоту использования и высокую функциональность, включает в себя автоматизацию и " +
                "упрощение работы с базой данных компании, улучшения процессов мониторинга и управления перевозками.";
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}