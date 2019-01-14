using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoLotDAL_Core2.EF;
using AutoLotDAL_Core2.Models;

namespace AutoLotMVC_Core2.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AutoLotContext _context;

        public InventoryController(AutoLotContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Cars.ToListAsync());

            //Using View Component
            return View("IndexWithViewComponent");
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Make,Color,PetName,Id,Timestamp")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Cars.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Make,Color,PetName,Id,Timestamp")] Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var saved = false;
                while (!saved)
                {
                    try
                    {
                        _context.Update(inventory);
                        await _context.SaveChangesAsync();                        
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!InventoryExists(inventory.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            foreach(var entry in ex.Entries)
                            {
                                if(entry.Entity is Inventory)
                                {
                                    var proposedValues = entry.CurrentValues;
                                    var databaseValues = entry.GetDatabaseValues();

                                    foreach(var property in proposedValues.Properties)
                                    {
                                        var proposedValue = proposedValues[property];
                                        var databaseValue = databaseValues[property];

                                        //ToDo: decide which value should be written to the database
                                        proposedValues[property] = proposedValue; //value to be saved
                                    }

                                    //Refresh original values to bypass next concurrency check
                                    entry.OriginalValues.SetValues(databaseValues);
                                }
                                else
                                {
                                    throw new NotSupportedException("Don't know how to handle the concurrency conflicts for " + entry.Metadata.Name);
                                }
                            }
                        }
                    }                    
                }
                return RedirectToAction(nameof(Index)); //saved
            }
            return View(inventory); //invalid model status
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
