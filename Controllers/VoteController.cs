﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnackVote_Backend.DTO;
using SnackVote_Backend.Models;
using System.IO;
using System.Security.Claims;

namespace SnackVote_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly DataContext _context;

        public VoteController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("addvote"), Authorize]
        public async Task<ActionResult<List<Vote>>> Addvote(string vote_item)
        {
            var vote =new Vote();
            var userName = User.FindFirstValue(ClaimTypes.Name);
            
            vote = new Vote()
            {
                UserName = userName,
                MenuName = vote_item
            };
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
            return Ok(await _context.Votes.ToListAsync());

;       }

        [HttpGet("getvotes"), Authorize]
        public async Task<ActionResult<List<MenuType>>> Getvote()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
        

            var counts = await _context.Votes
                .GroupBy(x => x.MenuName)
                .Select(group => new MenuType()
                {
                    MenuName = group.FirstOrDefault().MenuName,
                    Count = group.Count()
                }).ToListAsync();

            return counts;
        
        }


        [HttpGet("deletevotes"),Authorize(Roles ="Admin")]
        public async Task<ActionResult<List<Vote>>> Deletevotes()
        {
            _context.Votes.RemoveRange(_context.Votes);
            await _context.SaveChangesAsync();
            return Ok(await _context.Votes.ToListAsync());

            ;
        }

    }
}
