using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication52.Models;

namespace WebApplication52.Controllers
{
    public class VDevicesController : Controller
    {
        private readonly MyDBContext _context;

        public VDevicesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: VDevices
        public async Task<IActionResult> Index()
        {
            return View(await _context.VDevices.ToListAsync());
        }

        // GET: VDevices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vDevice = await _context.VDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vDevice == null)
            {
                return NotFound();
            }

            return View(vDevice);
        }

        // GET: VDevices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("設備位置,報修原因,報修人員工號,維修人員工號,維修進度,問題原因,Id,備註,日期")] VDevice vDevice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vDevice);
        }

        // GET: VDevices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vDevice = await _context.VDevices.FindAsync(id);
            if (vDevice == null)
            {
                return NotFound();
            }
            return View(vDevice);
        }

        // POST: VDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("設備位置,報修原因,報修人員工號,維修人員工號,維修進度,問題原因,Id,備註,日期")] VDevice vDevice)
        {
            if (id != vDevice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VDeviceExists(vDevice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vDevice);
        }

        // GET: VDevices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vDevice = await _context.VDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vDevice == null)
            {
                return NotFound();
            }

            return View(vDevice);
        }

        // POST: VDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vDevice = await _context.VDevices.FindAsync(id);
            if (vDevice != null)
            {
                _context.VDevices.Remove(vDevice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VDeviceExists(int id)
        {
            return _context.VDevices.Any(e => e.Id == id);
        }
    }
}
