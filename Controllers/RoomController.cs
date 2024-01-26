using Hotel_Reservation.Context;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RoomController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllRooms")]
        public async Task<ActionResult<Room>> GetAllRooms()
        {
            var availableRooms = await _context.Rooms
                                                    .Where(Room => Room.IsAvailable)
                                                    .ToListAsync();

            return Ok(availableRooms);


        }

        [HttpPost("AddRooms")]
        public async Task<ActionResult<Room>> AddRooms([FromBody] Room roomObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _context.Rooms.AddAsync(roomObj);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Room Added"
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log or return an error response)
                return StatusCode(500, new
                {
                    message = "Error adding room",
                    error = ex.Message
                });
            }
        }



    }
}
