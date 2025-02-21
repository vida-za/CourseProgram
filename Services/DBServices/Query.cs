using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DBServices
{
    public class Query : IDisposable
    {
        private string CommandText = string.Empty;
        private CommandTypes CommandType { get; set; }

        private string ObjectName = string.Empty;
        private readonly Dictionary<string, string> Parameters = new Dictionary<string, string>(); 
        private readonly List<string> Fields = new List<string>();

        public Where WhereClause { get; } = new Where();

        public Query(CommandTypes type, string objectName)
        {
            CommandType = type;
            ObjectName = $"\"{objectName}\"";
        }

        public string GetObjectName()
        {
            return ObjectName;
        }

        public void AddParameter(string name, object? value)
        {
            string parameterValue;

            if (value is null or (object)"Null")
            {
                parameterValue = "Null";
            }
            else if (value is bool boolValue)
            {
                parameterValue = boolValue ? "true" : "false";
            }
            else if (value is DateOnly dateOnlyValue)
            {
                parameterValue = dateOnlyValue == DateOnly.MinValue ? "Null" : $"'{dateOnlyValue:yyyy-MM-dd}'";
            }
            else if (value is DateTime dateTimeValue)
            {
                parameterValue = dateTimeValue == DateTime.MinValue ? "Null" : $"'{dateTimeValue:yyyy-MM-dd HH:mm:ss}'";
            }
            else if (value is string stringValue)
            {
                parameterValue = string.IsNullOrWhiteSpace(stringValue) ? "Null" : $"'{stringValue}'";
            }
            else if (value is float floatValue)
            {
                parameterValue = value.ToString().Replace(',', '.');
            }
            else
            {
                parameterValue = value.ToString()!;
            }

            Parameters[name] = parameterValue;
        }

        public int GetCountParametres()
        {
            return Parameters.Count;
        }

        public void AddFields(params string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.IsNullOrEmpty())
                    continue;
                    
                var field = arg.Trim();
                if (!field.Contains("Count")) field = $"\"{arg}\"";

                if (!Fields.Contains(field))
                    Fields.Add(field);
                
            }
        }

        public override string ToString()
        {
            return CommandText;
        }

        public new CommandTypes GetType()
        {
            return CommandType;
        }

        public class Where : IDisposable
        {
            private readonly List<Condition> Conditions = new();

            public void AddCondition(string field, string op, string value)
            {
                Conditions.Add(new Condition(field, op, value));
            }

            // Равенство
            public void Equals(string field, string value)
            {
                AddCondition(field, "=", value);
            }

            // Неравенство
            public void NotEquals(string field, string value)
            {
                AddCondition(field, "!=", value);
            }

            // Больше
            public void GreaterThan(string field, string value)
            {
                AddCondition(field, ">", value);
            }

            // Меньше
            public void LessThan(string field, string value)
            {
                AddCondition(field, "<", value);
            }

            // Больше или равно
            public void GreaterThanOrEquals(string field, string value)
            {
                AddCondition(field, ">=", value);
            }

            // Меньше или равно
            public void LessThanOrEquals(string field, string value)
            {
                AddCondition(field, "<=", value);
            }

            // LIKE
            public void Like(string field, string pattern)
            {
                AddCondition(field, "LIKE", pattern);
            }

            // IS NULL
            public void IsNull(string field)
            {
                AddCondition(field, "is", "null");
            }

            // IS NOT NULL
            public void IsNotNull(string field)
            {
                AddCondition(field, "is not", "null");
            }

            // BETWEEN
            public void Between(string field, string value1, string value2)
            {
                var betweenCondition = $"{field} BETWEEN '{value1}' AND '{value2}'";
                Conditions.Add(new Condition(betweenCondition, string.Empty, string.Empty)); // Используем пустые оператор и значение
            }

            public override string ToString()
            {
                if (Conditions.Count == 0) 
                    return string.Empty;
                else
                    return " Where " + string.Join(" AND ", Conditions);
            }

            public void Dispose()
            {
                Conditions.Clear();
            }

            public class Condition
            {
                public string Field { get; set; }
                public string Operator { get; set; }
                public string Value { get; set; }

                public Condition(string field, string op, string value)
                {
                    Field = field;
                    Operator = op;
                    Value = value;
                }

                public override string ToString()
                {
                    if (Operator == string.Empty) return Field;
                    return $"\"{Field}\" {Operator} {Value}";
                }
            }

        }

        public void CollectQuery()
        {
            switch (CommandType)
            {
                case CommandTypes.SelectQuery: CommandText = $"Select {string.Join(", ", Fields)} From {ObjectName}{WhereClause};"; break;
                case CommandTypes.InsertQuery: BuildInsertQuery(); break;
                case CommandTypes.DeleteQuery: CommandText = $"Delete From {ObjectName} {WhereClause};"; break;
                case CommandTypes.UpdateQuery: BuildUpdateQuery(); break;
                case CommandTypes.Procedure: CommandText = $"Call({string.Join(", ", Parameters.Values)});"; break;
                case CommandTypes.ScalarFunction: CommandText = $"Select {ObjectName}({string.Join(", ", Parameters.Values)});"; break;
                case CommandTypes.TableFunction: CommandText = $"Select {string.Join(", ", Fields)} From {ObjectName}({string.Join(", ", Parameters.Values)});"; break;
                default: break;
            }
        }

        private void BuildInsertQuery()
        {
            if (Parameters == null || Parameters.Count == 0)
                throw new InvalidOperationException("Zero parametres for Insert");

            var fields = string.Join(", ", Parameters.Keys.Select(k => $"\"{k}\""));
            var values = string.Join(", ", Parameters.Values);

            CommandText = $"Insert Into {ObjectName}({fields}) Values({values});";
        }

        private void BuildUpdateQuery()
        {
            if (Parameters == null || Parameters.Count == 0)
                throw new InvalidOperationException("Zero parametres for Update");

            List<string> formattedParametres = new List<string>();
            foreach (var parameter in Parameters)
            {
                formattedParametres.Add($"\"{parameter.Key}\" = {parameter.Value}");
            }

            CommandText = $"Update {ObjectName} Set {string.Join(" ,", formattedParametres)} {WhereClause};";
        }

        public void Dispose()
        {
            CommandText = string.Empty;
            ObjectName = string.Empty;

            WhereClause.Dispose();

            Parameters?.Clear();
            Fields?.Clear();
        }
    }
}
