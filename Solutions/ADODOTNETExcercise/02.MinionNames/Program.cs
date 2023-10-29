using Microsoft.Data.SqlClient;

string connectionString = "Server=.;Database=MinionsDB;User Id=sa;Password=monkeyFlip930!;TrustServerCertificate=True";

using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    await sqlConnection.OpenAsync();

    string query = "SELECT v.[Name] AS VillainName,m.[Name] AS MinionName,m.Age AS MinionAge FROM Villains AS v LEFT JOIN MinionsVillains AS mv ON v.Id = mv.VillainId LEFT JOIN Minions AS m ON m.Id = mv.MinionId WHERE v.Id = @Id ORDER BY v.Id, m.[Name]";

    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
    {
        string id = Console.ReadLine();
        sqlCommand.Parameters.AddWithValue("@Id", id);
        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        if(!reader.HasRows)
        {
            Console.WriteLine($"No villain with ID {id} exists in the database.");
        }

        bool checkedOnce = false;
        while (await reader.ReadAsync())
        {
            if (!checkedOnce)
            {
                checkedOnce = true;
                Console.WriteLine($"Villain: {reader["VillainName"]}");
            }
            
            Console.WriteLine(reader["MinionName"] == null || reader["MinionName"].ToString() == string.Empty ? "(no minions)" : $"1. {reader["MinionName"]} {reader["MinionAge"]}");
        }
    }
}