using Microsoft.Data.SqlClient;

string connectionString = "Server=.;Database=SoftUni;User Id=sa;Password=monkeyFlip930!;TrustServerCertificate=True";

using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    await sqlConnection.OpenAsync();

    string query = "SELECT * FROM Employees WHERE Salary > @salaryParam";

    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
    {
        //sqlCommand.ExecuteScalar()
        //sqlCommand.ExecuteReader()
        //sqlCommand.ExecuteNonQuery() 
        sqlCommand.Parameters.AddWithValue("@salaryParam", 50000);
        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            string firstName = reader["FirstName"].ToString();
            string lastName = reader["LastName"].ToString();
            decimal salary = decimal.Parse(reader["Salary"].ToString());
            Console.WriteLine($"{firstName} {lastName} - {salary:c}");
        }

            
    }
}