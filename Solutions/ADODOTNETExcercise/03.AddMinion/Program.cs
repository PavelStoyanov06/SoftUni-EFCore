using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

string connectionString = "Server=.;Database=MinionsDB;User Id=sa;Password=monkeyFlip930!;TrustServerCertificate=True;";

using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    await sqlConnection.OpenAsync();

    string[] minionInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
    string minionName = minionInfo[1];
    int minionAge = int.Parse(minionInfo[2]);
    string minionTown = minionInfo[3];

    string query = "SELECT * FROM Towns WHERE [Name] = @TownName";

    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
    {
        sqlCommand.Parameters.AddWithValue("@TownName", minionTown);
        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        if (!reader.HasRows) 
        {
            reader.Close();
            string addTownQuery = "INSERT INTO Towns([Name], CountryCode) VALUES (@NewTownName, @NewTownCountryCode)";

            using (SqlCommand addTownCommand = new SqlCommand(addTownQuery, sqlConnection))
            {
                addTownCommand.Parameters.AddWithValue("@NewTownName", minionTown);
                addTownCommand.Parameters.AddWithValue("@NewTownCountryCode", 1);
                addTownCommand.ExecuteNonQuery();
            }
            Console.WriteLine($"Town {minionTown} was added to the database.");
        }
    }

    string[] villainInfo = Console.ReadLine().Split(" ").ToArray();
    string villainName = villainInfo[1];

    string query1 = "SELECT * FROM Villains WHERE [Name] = @VillainName";

    using (SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection))
    {
        sqlCommand1.Parameters.AddWithValue("@VillainName", villainName);
        SqlDataReader reader = await sqlCommand1.ExecuteReaderAsync();

        
        if (!reader.HasRows)
        {
            reader.Close();
            string addVillainQuery = "INSERT INTO Villains([Name], EvilnessFactorId) VALUES (@NewVillainName, @NewVillainFactor)";

            using (SqlCommand addVillainCommand = new SqlCommand(addVillainQuery, sqlConnection))
            {
                addVillainCommand.Parameters.AddWithValue("@NewVillainName", villainName);
                addVillainCommand.Parameters.AddWithValue("@NewVillainFactor", 4);
                addVillainCommand.ExecuteNonQuery();
            }
            Console.WriteLine($"Villain {villainName} was added to the database.");
        }
    }

    int townId = 0;
    string fetchIdQuery = "SELECT Id FROM Towns WHERE [Name] = @TownName";
    using (SqlCommand fetchIdCommand = new SqlCommand(fetchIdQuery, sqlConnection))
    {
        fetchIdCommand.Parameters.AddWithValue("@TownName", minionTown);
        SqlDataReader reader = await fetchIdCommand.ExecuteReaderAsync();

        while (reader.Read())
        {
            townId = int.Parse(reader["Id"].ToString());
        }

        reader.Close();
    }

    string mainQuery = "INSERT INTO Minions ([Name], Age, TownId) VALUES (@MinionName, @MinionAge, @MinionTownId)";

    using (SqlCommand mainCommand = new SqlCommand(mainQuery, sqlConnection))
    {
        mainCommand.Parameters.AddWithValue("@MinionName", minionName);
        mainCommand.Parameters.AddWithValue("@MinionAge", minionAge);
        mainCommand.Parameters.AddWithValue("@MinionTownId", townId);
        mainCommand.ExecuteNonQuery();
    }

    int villainId = 0;
    string fetchVillainIdQuery = "SELECT Id FROM Villains WHERE [Name] = @VillainName";
    using (SqlCommand fetchVillainIdCommand = new SqlCommand(fetchVillainIdQuery, sqlConnection))
    {
        fetchVillainIdCommand.Parameters.AddWithValue("@VillainName", villainName);
        SqlDataReader reader = await fetchVillainIdCommand.ExecuteReaderAsync();

        while (reader.Read())
        {
            villainId = int.Parse(reader["Id"].ToString());
        }

        reader.Close();
    }
    int minionId = 0;
    string fetchMinionIdQuery = "SELECT Id FROM Minions WHERE [Name] = @MinionName";
    using (SqlCommand fetchVillainIdCommand = new SqlCommand(fetchMinionIdQuery, sqlConnection))
    {
        fetchVillainIdCommand.Parameters.AddWithValue("@MinionName", minionName);
        SqlDataReader reader = await fetchVillainIdCommand.ExecuteReaderAsync();

        while (reader.Read())
        {
            minionId = int.Parse(reader["Id"].ToString());
        }
        reader.Close();
    }
    string joinedTableQuery = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@MinionId, @VillainId);";
    using (SqlCommand joinedTableCommand = new SqlCommand(joinedTableQuery, sqlConnection))
    {
        joinedTableCommand.Parameters.AddWithValue("@MinionId", minionId);
        joinedTableCommand.Parameters.AddWithValue("@VillainId", villainId);
        await joinedTableCommand.ExecuteNonQueryAsync();
    }
    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
}