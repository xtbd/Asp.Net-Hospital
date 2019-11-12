using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hospital.Models;
using Microsoft.AspNet.Identity;

namespace Hospital.Controllers
{
    [Authorize] //only for the users who logged in
    public class EventsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Reservations
        public ActionResult Index()
        {
            if (User.IsInRole("Admin")) //To check whether the role is admin, if it is return
            {
                return View(db.Events.Include(r => r.Doctor).ToList());
            }
            else
            {
                //var reservations = db.Events.Include(r => r.Doctor);
                string currentUserId = User.Identity.GetUserId();
                return View(db.Events.Include(r => r.Doctor).Where(m => m.UserId == currentUserId).ToList());
            }
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Start,End,DoctorId,Title")] Event @event)
        {
            //this is referenced from FIT5032 lecture
            @event.UserId = User.Identity.GetUserId();//to get the id of user from event table
            String currentUser = @event.UserId; //create a string and assign user id to it
            for (int i = 12; i < 100; i++) //check all user in event table from the first user whoes id is 12 
            {
                Event eve = @db.Events.Find(i);//find that events
                if (eve != null)//if the id is not null do the following steps
                {
                    if (eve.UserId.Equals(currentUser))//check whether the user id is current user id 
                    {
                        if (db.Events.Find(i).Start.ToString().Equals(@event.Start.ToString()))//check whether the date input by user is eqauls to his date created before 
                        {
                            return RedirectToAction("Conflict"); //if the date has already create before, return the conflict page.
                        }
                    }
                }
            }

            ModelState.Clear();
            TryValidateModel(@event);

            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", @event.UserId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", @event.DoctorId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", @event.UserId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", @event.DoctorId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Start,End,DoctorId,Title,UserId,Flag")] Event @event)
        { 
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                // to get the email of users from AspNetUser table based on there user id
                String id = @event.UserId;
                AspNetUser u = db.AspNetUsers.Find(id);
                String email = u.Email.ToString();
                //create a new email sender
                EmailSender es = new EmailSender();
                //just sends email when flag in events table is 1.(0: not confirmed)
                if (@event.Flag == 1)
                es.Send(email);
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", @event.UserId);
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "Name", @event.DoctorId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Conflict()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
