using HotelListing.Models;
using HotelListing.Properties;
using HotelListing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] RequestParams requestParams)
        {
            var results = await _hotelRepository.GetHotels(requestParams);
            return Ok(new Repsonse(Resource.GET_SUCCESS, requestParams, results));
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            var result = await _hotelRepository.GetHotel(id);
            return Ok(new Repsonse(Resource.GET_SUCCESS, new { id = id }, result));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            var result = await _hotelRepository.CreateHotel(hotelDTO);
            return Ok(new Repsonse(Resource.CREATE_SUCCESS, null, result));
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel([FromQuery] int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            var result = await _hotelRepository.UpdateHotel(id, hotelDTO);
            return Ok(new Repsonse(result));
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel([FromQuery] int id)
        {
            var result = await _hotelRepository.DeleteHotel(id);
            return Ok(new Repsonse(result));
        }
    }
}