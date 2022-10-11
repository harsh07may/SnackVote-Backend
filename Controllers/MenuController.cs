using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnackVote_Backend.DTO;
using SnackVote_Backend.Models;
using System.Data;
using System.Xml.Linq;

namespace SnackVote_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        // GET: api/<MenuController>
        private static List<Menu> item = new List<Menu>
            {   
                new Menu
                {
                    MenuId = 1,
                    MenuName = "Cookie",
                    MenuCategory = "Vegetarian",
                    MenuDescription = "Description 123"
                },
                new Menu
                {
                    MenuId = 2,
                    MenuName = "Chicken Wings",
                    MenuCategory = "NonVeg",
                    MenuDescription = "Fried Chicken"
                }
            };
        private readonly DataContext _context;

        public MenuController(DataContext context)
        {
            _context = context;
        }

        //Add Single Item
        [HttpPost("additem"),Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Menu>>> AddItem([FromForm]MenuUploadDTO menuUpload)
        {
            var item = new Menu();

            using (var memoryStream = new MemoryStream())
            {
                await menuUpload.Image.CopyToAsync(memoryStream);
                if (memoryStream.Length < 5242880)
                {
                    item = new Menu()
                    {
                        MenuName = menuUpload.Name,
                        MenuCategory = menuUpload.Category,
                        MenuDescription = menuUpload.Description,
                        MenuImage = memoryStream.ToArray()
                        
                    };
                }
                else
                {
                    return BadRequest(new { title = "File too large" });
                }


            }

            await _context.Menus.AddAsync(item);
            await _context.SaveChangesAsync();
            return Ok(await _context.Menus.ToListAsync());
        }


        [HttpGet("getitems"),Authorize]
        public async Task<ActionResult<List<MenuDownloadDTO>>> GetAll()
        {
            return Ok(await _context.Menus.ToListAsync());
        }
        
        
        [HttpDelete("deleteItem"),Authorize]
        public async Task<ActionResult<MenuDownloadDTO>> DeleteItem(int id)
        {
            var item = await _context.Menus.FindAsync(id);

            if (item == null)
            { 
                return NotFound(); 
            }
            _context.Menus.Remove(item);
            _context.SaveChangesAsync();
            return new MenuDownloadDTO()
            {
                Name = item.MenuName,
                Category = item.MenuCategory,
                Description = item.MenuDescription,
                Image = "data:image/jpeg;base64,"+ Convert.ToBase64String(item.MenuImage)
            };
        }


        [HttpGet("getItem"),Authorize]
        public async Task<ActionResult<MenuDownloadDTO>> GetItem(int id)
        {
            var item = await _context.Menus.FindAsync(id);

            if (item == null)
            { 
                return NotFound(); 
            }
            return new MenuDownloadDTO()
            {
                Name = item.MenuName,
                Category = item.MenuCategory,
                Description = item.MenuDescription,
                Image = "data:image/jpeg;base64,"+ Convert.ToBase64String(item.MenuImage)
            };
        }

    }
}
