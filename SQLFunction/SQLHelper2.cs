
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace groverale
{
    public static class SQLHelper2
    {

        public static List<LeaderboardUser> GetStoreLeaderBoard(string employeeId, Settings settings) 
        {
            // Best practice is to scope the SqlConnection to a "using" block
            using (SqlConnection conn = new SqlConnection(settings.ConnectionString))
            {
                // Connect to the database
                conn.Open();

                // Leaderbaord
                var leaderBoard = new List<LeaderboardUser>();
                var storeId = 0;

                var storeQuery = $"SELECT storeId FROM {settings.TableName} WHERE {settings.EmployeeIdentifierField} = '{employeeId}';";

                using (SqlCommand command = new SqlCommand(storeQuery, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"storeId: {reader[0]}");
                            storeId = (int)reader[0];
                        }
                    }
                };

                var leaderBoardQuery = $"SELECT commissionDaily, commissionWeekly, employeeId, employeeEmail FROM {settings.TableName} WHERE storeId = '{storeId}';"; 
                        
                if (storeId > 0)
                {
                    using (SqlCommand command = new SqlCommand(leaderBoardQuery, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var user = new LeaderboardUser
                                {
                                    Daily = (int)reader[0],
                                    Weekly = (int)reader[1],
                                    UserId = reader[2].ToString(),
                                    UserEmail = reader[3].ToString()
                                };

                                leaderBoard.Add(user);
                            }
                        }
                    }

                    // Populated board (order by descending - highest at the top)
                    return leaderBoard.OrderByDescending(u => u.Weekly).ToList();
                }
                
                // empty leaderboard
                return leaderBoard;
            }
        }
    }
}