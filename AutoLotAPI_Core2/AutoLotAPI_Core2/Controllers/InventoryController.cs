﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoLotDAL_Core2.EF;
using AutoLotDAL_Core2.Models;
using AutoLotDAL_Core2.Repos;
using AutoMapper;
using Newtonsoft.Json;

namespace AutoLotAPI_Core2.Controllers
{
    [Produces("application/json")]
    [Route("api/Inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
       
        private readonly IInventoryRepo _repo;

        static InventoryController()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Inventory, Inventory>().ForMember(x => x.Orders, opt => opt.Ignore());
            });
        }

        public InventoryController(IInventoryRepo repo)
        {
            _repo = repo;
            
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetCars()
        {
            var inventories = _repo.GetAll();
            return Mapper.Map<List<Inventory>, List<Inventory>>(inventories); 
        }

        // GET: api/Inventory/5
        [HttpGet("{id}", Name ="DisplayRoute")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventory = _repo.GetOne(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Inventory,Inventory>(inventory));
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory([FromRoute]int id, [FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != inventory.Id)
            {
                return BadRequest();
            }
            
            try
            {
                _repo.Update(inventory);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory([FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _repo.Add(inventory);
            return CreatedAtRoute("displayRoute", new { id = inventory.Id }, inventory);

            //return CreatedAtAction("GetInventory", new { id = inventory.Id }, inventory);
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}/{timestamp}")]
        public async Task<ActionResult<Inventory>> DeleteInventory([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
            {
                timestamp = $"\"{timestamp}\"";
            }

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            _repo.Delete(id, ts);
            return Ok();
        }

        private bool InventoryExists(int id)
        {
            return _repo.Any(id);
        }
    }
}
