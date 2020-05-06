using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Project01.DTOs.Requests;
using Project01.Helpers;
using Project01.Services;

namespace Project01.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        public readonly IDbService _iService;

        public AnimalController(IDbService iService)
        {
            _iService = iService;
        }

        [HttpGet("add-animal")]
        public IActionResult GetAnimals(string orderBy = "AdmissionDate", string inOrder = "DESC")
        {
            try
            {
                var result = _iService.GetAnimals(orderBy, inOrder);
                if (result.Count() < 1) 
                {
                    return NotFound("There is no record!");
                }
                return Ok(result);
            }
            catch (MyException ex) 
            {
                if (ex.Type == MyException.ExceptionType.BadRequest) 
                {
                    return StatusCode(400);
                }
                else
                {
                    return StatusCode(500);
                }
            }
        }

        [HttpPost]
        public IActionResult AddAnimal(AddAnimalRequest request)
        {
            /*try
            {
                return Ok(_iService.AddAnimal(request));
            }
            catch (DbException ex) 
            {
                if (ex.Type == DbException.ExceptionType.NotFound) 
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    return StatusCode(500);
                }
            }*/

            var result = _iService.AddAnimal(request);
            if (result == null) 
            {
                return BadRequest("Please enter all the values!");
            }
            return Ok(result);
        }
    }
}
