﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace ResearchManager.Controllers
{
    public class AssociateController : Controller
    {
        // GET: AssociateDean
        public ActionResult Index()
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }
            ViewBag.DashboardText = "Associate Dean Dashboard";
            string label = HelperClasses.SharedControllerMethods.IdToLabel(active.staffPosition); 
;

            // Create new Entities object. This is a reference to the database.
            Entities db = new Entities();


            var projects = db.projects.Where(p => p.projectStage == label);
            // RIS can view all projects
            if (active.staffPosition == "RIS")
            {
                projects = db.projects;
            }
            // everybody else is limited
            else
            {
                projects = db.projects.Where(p => p.projectStage == label);
            }

            return View(projects.ToList());
        }

        public ActionResult Details(int id)
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            ViewBag.DashboardText = "Associate Dean Dashboard";

            try
            {   //Use searchTerm to query the database for project details and store this in a variable project
                Entities db = new Entities();
                var project = db.projects.Where(p => p.projectID == id).First();
                return View(project);
            }
            catch
            {
                //Return to Index if error occurs
                return RedirectToAction("Index");
            }
        }

        public FileResult Download(int projectID)
        {
            Entities db = new Entities();
            var dProject = db.projects.Where(p => p.projectID == projectID).First();

            return File(dProject.projectFile, "application/" + Path.GetExtension(dProject.projectFile), dProject.pName + "-ExpenditureFile" + Path.GetExtension(dProject.projectFile));
        }

        public ActionResult Sign(int projectID)
        {
            user active = TempData["ActiveUser"] as user;
            if (active == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            else
            {
                TempData["ActiveUser"] = active;
            }

            int id = projectID;
            string label = HelperClasses.SharedControllerMethods.IdToLabel(active.staffPosition);

            // return our project to be changed (should be only 1)
            var db = new Entities();
            var projectToEdit = db.projects.Where(p => p.projectID == id).First();

            projectToEdit.projectStage = HelperClasses.SharedControllerMethods.Signature(active.staffPosition);

            // update database
            db.Set<project>().Attach(projectToEdit);
            db.Entry(projectToEdit).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            TempData["alert"] = "You have signed " + projectToEdit.pName;

            var projects = db.projects.Where(p => p.projectStage == label);

            string email = HelperClasses.SharedControllerMethods.PositionToNewPosition(active.staffPosition);
            HelperClasses.SharedControllerMethods.EmailHandler(email, projectToEdit.pName, projectToEdit.pDesc);

            // show all projects without previously changed one
            if (active.staffPosition == "RIS")
            {
                projects = db.projects.Where(p => p.projectStage == label);
            }
            return RedirectToAction("Index", projects.ToList());
        }
    }
}