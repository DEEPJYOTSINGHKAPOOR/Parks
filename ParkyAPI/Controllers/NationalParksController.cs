using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;


        public NationalParksController(INationalParkRepository repo, IMapper mapper)
        {
            _nationalParkRepository = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [Authorize]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalParks()
        {
            var objList = _nationalParkRepository.GetNationalParks();
            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{nationalParkId:int}" , Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                //Model state contains error if any encountered
                return BadRequest(ModelState);
            }

            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("","National Park Exists!");
                return StatusCode(404,ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("",$"Something went wrong while saving the record {nationalParkObj.Name}");
                //Server error
                return StatusCode(500,ModelState);
            }
            Console.WriteLine("All Ok");
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id } , nationalParkObj); ;
        }

        [HttpPatch("{nationalParkId:int}" , Name ="UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                //Model state contains error if any encountered
                return BadRequest(ModelState);
            }

            if (!_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Does not Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong while saving the record {nationalParkObj.Name}");
                //Server error
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }



        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
            {
                //Model state contains error if any encountered
                return NotFound();
            }

            var nationalParkObj = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (!_nationalParkRepository.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", "Something Went wRONG!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
