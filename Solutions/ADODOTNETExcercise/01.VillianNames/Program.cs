using Microsoft.Data.SqlClient;

string connectionString = "Server=.;Database=MinionsDB;User Id=sa;Password=monkeyFlip930!;TrustServerCertificate=True";

using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    await sqlConnection.OpenAsync();

    string query = "SELECT v.[Name],COUNT(mv.MinionId) AS [Count] FROM Villains AS v LEFT JOIN MinionsVillains AS mv ON v.Id = mv.VillainId GROUP BY v.[Name] HAVING COUNT(mv.MinionId) > @minionsCountParam ORDER BY COUNT(mv.MinionId) DESC";
    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
    {
        sqlCommand.Parameters.AddWithValue("@minionsCountParam", 3);
        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            string name = reader["Name"].ToString();
            string count = reader["Count"].ToString();
            Console.WriteLine($"{name} - {count}");
        }
    }
}