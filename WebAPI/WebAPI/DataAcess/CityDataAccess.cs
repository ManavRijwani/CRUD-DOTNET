using Npgsql;
using System.Data;
using System.Diagnostics.Metrics;
using WebAPI.Models;

namespace WebAPI.DataAcess
{
   
        public class CityDataAccess
        {
            private readonly string _connectionstring;
            public CityDataAccess(IConfiguration configuration)
            {
                _connectionstring = configuration.GetConnectionString("DefaultConnection");
            }


            //function
            public IEnumerable<City> GetCity()
            {
                List<City> cities = new List<City>();


                using (var connection = new NpgsqlConnection(_connectionstring))
                {
                    connection.Open();

                    var command = new NpgsqlCommand("Select * from Get_city()", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var std = new City
                            {
                                intCityID = Convert.ToInt32(reader["intCityID"]),
                                StrCityName = reader["StrCityName"].ToString(),
                                intStateID = Convert.ToInt32(reader["intStateID"]),
                                intCountryID = Convert.ToInt32(reader["intCountryID"]),
                            };
                            cities.Add(std);
                        }
                    }
                }
                return cities;
            }

            public City GetCitybyID(int intCityID)
            {
                using (var connection = new NpgsqlConnection(_connectionstring))
                {
                    connection.Open();

                    var command = new NpgsqlCommand("Select * from Get_citybyid(@id)", connection);
                    command.Parameters.AddWithValue("id", intCityID);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new City
                            {
                                StrCityName = reader["StrCityName"].ToString(),
                                intStateID = Convert.ToInt32(reader["intStateID"]),
                                intCountryID = Convert.ToInt32(reader["intCountryID"]),
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }


            public void IncertCity(City city)
            {
                {
                    using (var connection = new NpgsqlConnection(_connectionstring))
                    {
                        connection.Open();
                        using (var command = new NpgsqlCommand("select * from insert_city(@CityName,@StateID,@CountryID)", connection))
                        {
                            command.Parameters.AddWithValue("CityName", city.StrCityName);
                            command.Parameters.AddWithValue("StateID", city.intStateID);
                            command.Parameters.AddWithValue("CountryID", city.intCountryID);

                            command.ExecuteNonQuery();
                        }
                    }

                }
            }


            public void UpdateCity(City city)
            {
                using (var connection = new NpgsqlConnection(_connectionstring))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("select * from update_city(@city_id,@CityName,@StateID,@CountryID)", connection))
                    {
                        command.Parameters.AddWithValue("@city_id", city.intCityID);
                        command.Parameters.AddWithValue("@CityName", city.StrCityName);
                        command.Parameters.AddWithValue("@StateID", city.intStateID);
                        command.Parameters.AddWithValue("@CountryID", city.intCountryID);

                        command.ExecuteNonQuery();
                    }

                }
            }


            public void DeleteCity(int intCityID)
            {
                using (var connection = new NpgsqlConnection(_connectionstring))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("select * from Delete_city(@city_id) ;", connection))
                    {
                        command.Parameters.AddWithValue("city_id", intCityID);
                        command.ExecuteNonQuery();
                    }
                }
            }


            public City GetCityByName(string StrCityName)
            {
                using (var connection = new NpgsqlConnection(_connectionstring))
                {
                    connection.Open();

                    var command = new NpgsqlCommand("SELECT * FROM get_city_by_name(@name)", connection);
                    command.Parameters.AddWithValue("name", StrCityName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new City
                            {
                                intCityID = Convert.ToInt32(reader["intCityID"]),
                                intStateID = Convert.ToInt32(reader["intStateID"]),
                                intCountryID = Convert.ToInt32(reader["intCountryID"]),
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }



        public dropdown GetCountrybystateID(int intStateID)
        {
            using (var connection = new NpgsqlConnection(_connectionstring))
            {
                connection.Open();

                var command = new NpgsqlCommand("select * from \"tblCountryMaster\" cm \r\nwhere cm.intcountryid = (select sm.intcountryid from \"tblStateMaster\" sm where sm.intstateid = @intstateid);", connection);
                command.Parameters.AddWithValue("@intstateid", intStateID);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new dropdown
                        {
                            strcountryName = reader["strcountryName"].ToString(),
                            intCountryID = Convert.ToInt32(reader["intCountryID"]),
                        };
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

