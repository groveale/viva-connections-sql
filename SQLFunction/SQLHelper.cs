
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace groverale
{
    public static class SQLHelper
    {

        public static double GetEmployeeCommissionDaily(string employeeId, Settings settings) 
        {
            // Best practice is to scope the SqlConnection to a "using" block
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString))
            {
                // Connect to the database
                conn.Open();

                // Perhaps overcomplicated by using the settings variables
                // SELECT commissionDaily FROM EmployeeCommission WHERE employeeEmail = 'alex@groverale.onmicrosoft.com';
                SqlCommand selectCommand = new SqlCommand(
                    $"SELECT {settings.ValueField} FROM {settings.TableName} WHERE {settings.EmployeeIdentifierField} = '{employeeId}';", 
                    conn);
                SqlDataReader results = selectCommand.ExecuteReader();
                
                // Enumerate over the rows
                while(results.Read())
                {
                    Console.WriteLine(results.FieldCount);
                    Console.WriteLine($"{settings.ValueField}: {results[0]}");
                    
                    // We know that this type is an Int
                    return (int)results[0];
                }

                return 0;
            }
        }

        // We could have used one function but I thought more examples the better for this sample
        // This uses a hard coded value for the field we want
        public static double GetEmployeeCommissionWeekly(string employeeId, Settings settings) 
        {
            // Best practice is to scope the SqlConnection to a "using" block
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString))
            {
                // Connect to the database
                conn.Open();

                // Perhaps overcomplicated by using the settings variables
                // SELECT commissionDaily FROM EmployeeCommission WHERE employeeEmail = 'alex@groverale.onmicrosoft.com';
                SqlCommand selectCommand = new SqlCommand(
                    $"SELECT commissionWeekly FROM {settings.TableName} WHERE {settings.EmployeeIdentifierField} = '{employeeId}';", 
                    conn);
                SqlDataReader results = selectCommand.ExecuteReader();
                
                // Enumerate over the rows
                while(results.Read())
                {
                    Console.WriteLine(results.FieldCount);
                    Console.WriteLine($"commissionWeekly: {results[0]}");
                    
                    // We know that this type is an Int
                    return (int)results[0];
                }

                return 0;
            }
        }

        // Unused but demonstrating how we can get both values in one call to the db
        // Reusing our response object
        public static SQLCommissionResponse GetEmployeeCommission(string employeeId, Settings settings) 
        {
            // Best practice is to scope the SqlConnection to a "using" block
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString))
            {
                // Connect to the database
                conn.Open();

                // Perhaps overcomplicated by using the settings variables
                // SELECT commissionDaily, commissionWeekly FROM EmployeeCommission WHERE employeeEmail = 'alex@groverale.onmicrosoft.com';
                SqlCommand selectCommand = new SqlCommand(
                    $"SELECT {settings.ValueField}, commissionWeekly FROM {settings.TableName} WHERE {settings.EmployeeIdentifierField} = '{employeeId}';", 
                    conn);
                SqlDataReader results = selectCommand.ExecuteReader();

                
                
                // Enumerate over the rows
                while(results.Read())
                {
                    Console.WriteLine(results.FieldCount);
                    Console.WriteLine($"{settings.ValueField}: {results[0]}");
                    Console.WriteLine($"commissionWeekly: {results[1]}");
                    
                    // We know that this type is an Int
                    // divide by 100 to get pounds (db is stored in pence)
                    return new SQLCommissionResponse 
                    {
                        Daily = (int)results[0],
                        Weekly = (int)results[1]
                    };
                }

                // return 0s if user not found
                return new SQLCommissionResponse { Daily = 0, Weekly = 0 };
            }
        }

        public static List<LeaderboardUser> GetStoreLeaderBoard(string employeeId, Settings settings) 
        {
            // Best practice is to scope the SqlConnection to a "using" block
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString))
            {
                // Connect to the database
                conn.Open();

                // Perhaps overcomplicated by using the settings variables
                // SELECT commissionDaily, commissionWeekly FROM EmployeeCommission WHERE employeeEmail = 'alex@groverale.onmicrosoft.com';
                SqlCommand selectCommand = new SqlCommand(
                    $"SELECT storeId FROM {settings.TableName} WHERE {settings.EmployeeIdentifierField} = '{employeeId}';", 
                    conn);
                SqlDataReader results = selectCommand.ExecuteReader();

                // Leaderbaord
                var leaderBoard = new List<LeaderboardUser>();
                var storeId = 0;
                
                // Enumerate over the rows
                while(results.Read())
                {
                    Console.WriteLine(results.FieldCount);
                    Console.WriteLine($"storeId: {results[0]}");
                    
                    storeId = (int)results[0];
                    results.Close();
                    break;
                }

                if (storeId > 0)
                {
                    selectCommand = new SqlCommand(
                        $"SELECT commissionDaily, commissionWeekly, employeeId, employeeEmail FROM {settings.TableName} WHERE storeId = '{storeId}';", 
                        conn);

                    SqlDataReader leaderBoardResults = selectCommand.ExecuteReader();

                    while(leaderBoardResults.Read())
                    {
                        var user = new LeaderboardUser
                        {
                            Daily = (int)leaderBoardResults[0],
                            Weekly = (int)leaderBoardResults[1],
                            UserId = leaderBoardResults[2].ToString(),
                            UserEmail = leaderBoardResults[3].ToString()
                        };

                        leaderBoard.Add(user);
                    }

                    // Populated board (order by descending - highest at the top)
                    return leaderBoard.OrderByDescending(u => u.Weekly).ToList();
                }
                
                // empty leaderboard
                return leaderBoard;
            }
        }


    }

    public class LeaderboardUser 
    {
        public string UserEmail {get;set;}
        public string UserId {get;set;}
        public double Daily {get;set;}
        public double Weekly {get;set;}

    }
}