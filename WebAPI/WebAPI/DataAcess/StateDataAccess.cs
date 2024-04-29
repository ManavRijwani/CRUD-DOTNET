using Npgsql;
using WebAPI.Models;

namespace WebAPI.DataAcess
{
    public class StateDataAccess
    {
        private readonly string _connectionstring;
        public StateDataAccess(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<State> GetState()
        {
            List<State> states = new List<State>();


            using (var connection = new NpgsqlConnection(_connectionstring))
            {
                connection.Open();

                var command = new NpgsqlCommand("Select * from \"tblStateMaster\";", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var std = new State
                        {
                            intStateID = Convert.ToInt32(reader["intStateID"]),
                            StrStateName = reader["StrStateName"].ToString(),                   
                            intCountryID = Convert.ToInt32(reader["intCountryID"]),
                        };
                        states.Add(std);
                    }
                }
            }
            return states;
        }
    }
}
