using System;

namespace groverale
{
    public class Settings
    {
        public string? ConnectionString { get; set; }
        public string? TableName { get; set; }
        public string? EmployeeIdentifierField { get; set; }
        public string? ValueField { get; set; }

        public static Settings LoadSettings()
        {
            return new Settings 
            {
                ConnectionString = Environment.GetEnvironmentVariable("SQLConString"),
                TableName = Environment.GetEnvironmentVariable("SQLTableName"),
                EmployeeIdentifierField = Environment.GetEnvironmentVariable("SQLEmployeeIdentifier"),
                ValueField = Environment.GetEnvironmentVariable("SQLValueField"),
            };
        }
    }
}