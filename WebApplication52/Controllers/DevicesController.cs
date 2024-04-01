using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication52.Models;

namespace WebApplication52.Controllers
{
    public class DevicesController : Controller
    {
        private readonly MyDBContext _context;

        public DevicesController(MyDBContext context)
        {
            _context = context;
        }
        private void PopulateDropdownsInViewBag()
        {
            ViewBag.Locations = _context.Locations.ToList();
            ViewBag.FixReasons = _context.FixReasons.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            ViewBag.Solutions = _context.Solutions.ToList();
        }
        // GET: Devices
        public async Task<IActionResult> Index()
        {
            var vdevices = await _context.VDevices.OrderByDescending(d => d.日期).ToListAsync();
            return View(vdevices);
        }



        private string GetRecommendedSolution(int fixReasonId)
        {
            var recommendedSolutions = _context.Devices
                .Where(d => d.FixReason == fixReasonId)
                .GroupBy(d => d.Solution)
                .OrderByDescending(g => g.Count())
                .Take(3) // 只选择前三名
                .Select(g => g.Key)
                .ToList();

            List<string> topThreeSolutions = new List<string>();

            foreach (var solutionId in recommendedSolutions)
            {
                string recommendedSolutionDesc = _context.Solutions
                    .Where(s => s.SolutionId == solutionId)
                    .Select(s => s.SolutionDesc)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(recommendedSolutionDesc))
                {
                    topThreeSolutions.Add(recommendedSolutionDesc);
                }
            }

            if (topThreeSolutions.Count >= 3)
            {
                return topThreeSolutions[0] + ", " + topThreeSolutions[1] + ", " + topThreeSolutions[2];
            }
            else if (topThreeSolutions.Count == 2)
            {
                return topThreeSolutions[0] + ", " + topThreeSolutions[1];
            }
            else if (topThreeSolutions.Count == 1)
            {
                return topThreeSolutions[0];
            }
            else
            {
                return ""; // 没有找到解决方案
            }
        }



        private void SendLineNotify(Device device)
        {
            // Replace 'YOUR_LINE_NOTIFY_API_TOKEN' with your actual Line Notify API token
            string lineNotifyApiToken = "0aGgnrk4cDe6w7ss6MIcSZ52pOmlG76smEo0VJpQDmf";
            string lineNotifyApiUrl = "https://notify-api.line.me/api/notify";
            string locationDesc = _context.Locations.Where(c => c.LocationId == device.Location).Select(c => c.LocationDesc).FirstOrDefault();
            string fixReasonDesc = _context.FixReasons.Where(c => c.FixReasonId == device.FixReason).Select(c => c.FixReasonDesc).FirstOrDefault();
            string statusDesc = _context.Statuses.Where(c => c.StatusId == device.Status).Select(c => c.StatusDesc).FirstOrDefault();
            string solutionDesc = _context.Solutions.Where(c => c.SolutionId == device.Solution).Select(c => c.SolutionDesc).FirstOrDefault();


            string recommendedSolutionDesc = GetRecommendedSolution(device.FixReason);


            // Build message content with form data
            string message = "\n" +
                             "報修原因: " + fixReasonDesc + "\n" +
                             "設備位置: " + locationDesc + "\n" +
                             "備註: " + device.Remark + "\n" +
                             "報修工號: " + device.EmpId + "\n" +
                             "維修進度: " + statusDesc + "\n" +
                             "維修工號: " + device.FixEmpId+ "\n" +
                             "解決方式: " + solutionDesc + "\n" +
                             "建議解決方式: " + recommendedSolutionDesc + "\n";


            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + lineNotifyApiToken);
                var content = new StringContent("message=" + message, Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = httpClient.PostAsync(lineNotifyApiUrl, content).Result;
                // You can handle response here if needed
            }
        }
        // GET: Devices/Create
        // GET: Devices/Create
        public IActionResult Create()
        {
            PopulateDropdownsInViewBag();
            
            return View();
        }


        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Location,FixReason,EmpId,Status,FixEmpId,Solution,Remark,Date")] Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                SendLineNotify(device);
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PopulateDropdownsInViewBag();
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,FixReason,EmpId,Status,FixEmpId,Solution,Remark")] Device device)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                SendLineNotify(device);
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}
