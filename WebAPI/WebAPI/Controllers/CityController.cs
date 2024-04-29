using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using WebAPI.DataAcess;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class CityController : ControllerBase
        {
            private readonly CityDataAccess _cityDataAccess;

            public CityController(CityDataAccess cityDataAccess)
            {
                _cityDataAccess = cityDataAccess;
            }

        
        [HttpGet]
        [Authorize]
        public IActionResult Getcitybyfunction()
            {

                //try
                //{
                //    var std = _cityDataAccess.GetCity();
                //    return Ok(std);
                //}
                //catch (Exception ex)
                //{
                //    return StatusCode(500, $"Internal server error: {ex.Message}");
                //}

                //by response model
                try
                {
                    var cities = _cityDataAccess.GetCity();
                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Data = cities,
                        Message = "Cities retrieved successfully"
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = new ApiResponse<string>
                    {
                        Status = "error",
                        Data = null,
                        Message = $"Internal server error: {ex.Message}"
                    };
                    return StatusCode(500, errorResponse);
                }



            }

            [HttpGet("{id}")]
            public IActionResult citybyID(int id)
            {
                //var std = _cityDataAccess.GetCitybyID(id);
                //if (std == null)
                //{
                //    return BadRequest();
                //}
                //else
                //{
                //    return Ok(std);
                //}


                //by response model
                try
                {
                    var city = _cityDataAccess.GetCitybyID(id);

                    if (city == null)
                    {
                        var errorResponse = new ApiResponse<string>
                        {
                            Status = "error",
                            Data = null,
                            Message = "City not found"
                        };
                        return NotFound(errorResponse);
                    }

                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Data = city,
                        Message = "City retrieved successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = new ApiResponse<string>
                    {
                        Status = "error",
                        Data = null,
                        Message = $"Internal server error: {ex.Message}"
                    };
                    return StatusCode(500, errorResponse);
                }
            }


            [HttpPost]
            public IActionResult CreateCity(City std)
            {
                //_cityDataAccess.IncertCity(std);
                //return CreatedAtAction(nameof(citybyID), new { id = std.intCityID }, std);


                //by response model
                try
                {
                    _cityDataAccess.IncertCity(std);

                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Data = std,
                        Message = "City created successfully"
                    };

                    // Returning 201 Created status with the location header to the newly created resource
                    return CreatedAtAction(nameof(citybyID), new { id = std.intCityID }, response);
                }
                catch (Exception ex)
                {
                    var errorResponse = new ApiResponse<string>
                    {
                        Status = "error",
                        Data = null,
                        Message = $"Internal server error: {ex.Message}"
                    };
                    return StatusCode(500, errorResponse);
                }
            }

            [HttpPut("{id}")]
            public IActionResult updatecity(int id, City std)
            {

                //try
                //{
                //    var stud = _cityDataAccess.GetCitybyID(id);
                //    if (stud == null || id != std.intCityID)
                //    {
                //        return BadRequest("Invalid student data");
                //    }
                //    else
                //    {
                //        _cityDataAccess.UpdateCity(std);
                //        return Ok();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    return StatusCode(500, $"Internal server error: {ex.Message}");


                // by response model
                try
                {
                    var city = _cityDataAccess.GetCitybyID(id);

                    if (city == null || id != std.intCityID)
                    {
                        var errorResponse = new ApiResponse<string>
                        {
                            Status = "error",
                            Data = null,
                            Message = "Invalid city data"
                        };
                        return BadRequest(errorResponse);
                    }

                    _cityDataAccess.UpdateCity(std);

                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Data = std,
                        Message = "City updated successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = new ApiResponse<string>
                    {
                        Status = "error",
                        Data = null,
                        Message = $"Internal server error: {ex.Message}"
                    };
                    return StatusCode(500, errorResponse);
                }
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteStudent(int id)
            {
                //var stud = _cityDataAccess.GetCitybyID(id);
                //if (stud == null)
                //{
                //    return NotFound();
                //}

                //_cityDataAccess.DeleteCity(id);
                //return NoContent();

                //by response model

                try
                {
                    var city = _cityDataAccess.GetCitybyID(id);

                    if (city == null)
                    {
                        var errorResponse = new ApiResponse<string>
                        {
                            Status = "error",
                            Data = null,
                            Message = "City not found"
                        };
                        return NotFound(errorResponse);
                    }

                    _cityDataAccess.DeleteCity(id);

                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Data = null,
                        Message = "City deleted successfully"
                    };

                    return NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = new ApiResponse<string>
                    {
                        Status = "error",
                        Data = null,
                        Message = $"Internal server error: {ex.Message}"
                    };
                    return StatusCode(500, errorResponse);
                }
            }

            [HttpGet("byname")]
            public IActionResult GetCityByName(string cityName)
            {
                //var std = _cityDataAccess.GetCityByName(cityName);
                //if (std == null)
                //{
                //    return BadRequest();
                //}
                //else
                //{
                //    return Ok(std);
                //}

                //by response model
                try
                {
                    var city = _cityDataAccess.GetCityByName(cityName);

                    if (city == null)
                    {
                        var errorResponse = new ApiResponse<string>
                        {
                            Status = "error",
                            Data = null,
                            Message = "City not found"
                        };
                        return NotFound(errorResponse);
                    }

                    var response = new ApiResponse<object>
                    {
                        Status = "success",
                        Data = city,
                        Message = "City retrieved successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = new ApiResponse<string>
                    {
                        Status = "error",
                        Data = null,
                        Message = $"Internal server error: {ex.Message}"
                    };
                    return StatusCode(500, errorResponse);
                }
            }


        [HttpGet("countrybystateid/{id}")]
        public IActionResult countrybystateID(int id)
        {
            //var std = _cityDataAccess.GetCitybyID(id);
            //if (std == null)
            //{
            //    return BadRequest();
            //}
            //else
            //{
            //    return Ok(std);
            //}


            //by response model
            try
            {
                var city = _cityDataAccess.GetCountrybystateID(id);

                if (city == null)
                {
                    var errorResponse = new ApiResponse<string>
                    {
                        Status = "error",
                        Data = null,
                        Message = "City not found"
                    };
                    return NotFound(errorResponse);
                }

                var response = new ApiResponse<object>
                {
                    Status = "success",
                    Data = city,
                    Message = "Country retrieved successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>
                {
                    Status = "error",
                    Data = null,
                    Message = $"Internal server error: {ex.Message}"
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}


