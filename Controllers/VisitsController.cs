using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Controllers
{
    public class VisitsController : Controller
    {
        public VisitsController()
        {
        }

        // GET: Visits
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Visits/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(id);
        }

        // GET: Visits/Create
        public IActionResult Create()
        {
            //ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID");
            //ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitID,PatientID,VisitDate,Notes,AttendingStaffID")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            // ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            // ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            // ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
            return View(id);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VisitID,PatientID,VisitDate,Notes,AttendingStaffID")] Visit visit)
        {
            if (!Guid.Equals(id, visit.VisitID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            //ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(id);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
