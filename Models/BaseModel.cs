namespace CourseProgram.Models
{
    public abstract class BaseModel : IModel
    {
        public int ID { get; protected set; }

        public virtual string GetProcedureDelete() => "\"\""; 
        public virtual string GetSelectorID() => "\"\""; 
        public virtual string GetSelectors() => "\"\""; 
        public virtual string GetTable() => "\"\"";
    }
}