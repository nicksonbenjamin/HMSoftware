using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Models;

namespace ClinicApp.Controllers
{
    public class AttendingStaffController : Controller
    {
        public AttendingStaffController()
        {
        }

        // GET: Visits
        public IActionResult Index()
        {
            return View();
        }

        /*

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
        public async Task<IActionResult> Create(ClinicApp.Models.AttendingStaff attendingStaff)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            // ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            // ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
            return View(attendingStaff);
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
        public async Task<IActionResult> Edit(Guid id, ClinicApp.Models.AttendingStaff attendingStaff)
        {
            if (!Guid.Equals(id, attendingStaff.StaffID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AttendingStaffID"] = new SelectList(_context.AttendingStaff, "StaffID", "StaffID", visit.AttendingStaffID);
            //ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FirstName", visit.PatientID);
            return View(attendingStaff);
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
        */
    }
}
