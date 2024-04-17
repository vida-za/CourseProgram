namespace CourseProgram.Models
{
    public interface IModel
    {
        public int ID { get; }

        public string GetSelectors();
        public string GetTable();
        public string GetSelectorID();
        public string GetProcedureDelete();
    }
}