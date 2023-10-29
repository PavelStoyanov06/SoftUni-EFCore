using Microsoft.Data.SqlClient;

string connectionString = "Server=.;Database=MinionsDB;User Id=sa;Password=monkeyFlip930!;TrustServerCertificate=True";

using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    await sqlConnection.OpenAsync();
	
    string country = Console.ReadLine();

    int countryCode = 0;

    string fetchCountryIdQuery = "SELECT Id FROM Countries WHERE [Name] = @CountryName";

    using (SqlCommand fetchCountryIdCommand = new SqlCommand(fetchCountryIdQuery, sqlConnection))
    {
        fetchCountryIdCommand.Parameters.AddWithValue("@CountryName", country);
        using (SqlDataReader reader = await fetchCountryIdCommand.ExecuteReaderAsync())
        {
            while (reader.Read())
            {
                countryCode = int.Parse(reader["Id"].ToString());
            }
        }
    }

    string mainQuery = "UPDATE Towns SET [Name] = UPPER([Name]) WHERE CountryCode = @CountryCode";

    using (SqlCommand mainCommand = new SqlCommand(mainQuery, sqlConnection))
    {
        mainCommand.Parameters.AddWithValue("@CountryCode", countryCode);
        int rowsAffected = mainCommand.ExecuteNonQuery();
    }

    List<string> affectedRows = new List<string>();

    string sideSelectQuery = "SELECT [Name] FROM TOWNS WHERE CountryCode = @CountryCode";
    using (SqlCommand sideSelectCommand = new SqlCommand(sideSelectQuery, sqlConnection))
    {
        sideSelectCommand.Parameters.AddWithValue("@CountryCode", countryCode);
        using(SqlDataReader reader = await sideSelectCommand.ExecuteReaderAsync())
        {
            while (reader.Read())
            {
                affectedRows.Add(reader["Name"].ToString());
            }
        }
    }

    Console.WriteLine($"{affectedRows} towns were affected.");
    Console.WriteLine($"[{string.Join(", ", affectedRows)}]");
}