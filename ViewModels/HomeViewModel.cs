using CourseProgram.Stores;

namespace CourseProgram.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;

        public HomeViewModel(ServicesStore servicesStore) 
        {
            _servicesStore = servicesStore;

            Title = "Добро пожаловать!";
            Description = "Это приложение, разработанно для оптимизации управления компанией логистических перевозок. " +
                "Сочетая в себе простоту использования и высокую функциональность, включает в себя автоматизацию и " +
                "упрощение работы с базой данных компании, улучшения процессов мониторинга и управления перевозками.";

            InitData();
        }

        private async void InitData() 
        {
            foreach (var serv in _servicesStore.GetAllServices())
            {
                await ((dynamic)serv).GetItemsAsync();
            }
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}