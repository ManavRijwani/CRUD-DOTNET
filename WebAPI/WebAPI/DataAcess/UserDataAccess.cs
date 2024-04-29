using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using WebAPI.Models;

namespace WebAPI.DataAcess
{
    public class UserDataAccess
    {
        private readonly string _connectionString;

        public UserDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //    public User Authenticate(string username, string password)
        //    {
        //        using (var connection = new NpgsqlConnection(_connectionString))
        //        {
        //            connection.Open();

        //            var query = "SELECT * FROM users WHERE username = @Username AND password = @Password";
        //            using (var command = new NpgsqlCommand(query, connection))
        //            {
        //                command.Parameters.AddWithValue("@Username", username);
        //                command.Parameters.AddWithValue("@Password", password);

        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        return new User
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("id")),
        //                            Username = reader.GetString(reader.GetOrdinal("username")),
        //                            Password = reader.GetString(reader.GetOrdinal("password"))
        //                        };
        //                    }
        //                }
        //            }
        //        }

        //        return null;
        //    }
        //}

        public void RegisterUser(RegisterRequest registerRequest)
        {
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("insert into users(username,email,password,contact,role) values(@username,@email,@password,@contact,'user')", connection))
                    {
                        command.Parameters.AddWithValue("@username", registerRequest.Username);
                        command.Parameters.AddWithValue("@email", registerRequest.Email);
                        command.Parameters.AddWithValue("@password", registerRequest.Password);
                        command.Parameters.AddWithValue("@contact", registerRequest.Contact);

                        command.ExecuteNonQuery();
                    }
                }

            }
        }

        public IEnumerable<RegisterRequest> GetUser()
        {
            List<RegisterRequest> users = new List<RegisterRequest>();


            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand("Select username, email, password, contact from users", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var std = new RegisterRequest
                        {
                            Username = reader["username"].ToString(),
                            Email = reader["email"].ToString(),
                            Password = reader["password"].ToString(),
                            Contact = Convert.ToInt32(reader["contact"]),
                        };
                        users.Add(std);
                    }
                }
            }
            return users;
        }

        //public bool IsValidUser(string username, string password)
        //{
        //    using (var connection = new NpgsqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        var query = "SELECT COUNT(*) FROM users WHERE username = @Username AND password = @Password";
        //        using (var command = new NpgsqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Username", username);
        //            command.Parameters.AddWithValue("@Password", password);

        //            var result = (long)command.ExecuteScalar();
        //            return result == 1;
        //        }
        //    }
        //}

        public Temp IsValidUser(string username, string password)
        {
            Temp temp;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT username, password, role FROM users WHERE username = @Username AND password = @Password";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            temp=new Temp
                            {
                                Username = row["username"].ToString(),
                                Password = row["password"].ToString(),
                                Role = row["role"].ToString()
                            };
                        return temp;
                        }
                    }
                    return null;
                }
            }

        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    var result = (long)await command.ExecuteScalarAsync();
                    return result > 0;
                }
            }
        }


        public RegisterRequest GetUserbyID(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand("Select username from users where id = @id;", connection);
                command.Parameters.AddWithValue("id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        RegisterRequest obj = new RegisterRequest
                        {
                            Username = reader["username"].ToString(),
                        };
                        return obj;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
    }
}
