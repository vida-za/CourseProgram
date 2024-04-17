namespace CourseProgram.Models
{
    public class Empty : IModel
    {
        public int ID => 0;

        public string GetProcedureDelete() => string.Empty;

        public string GetSelectorID() => string.Empty;

        public string GetSelectors() => string.Empty;

        public string GetTable() => string.Empty;
    }
}