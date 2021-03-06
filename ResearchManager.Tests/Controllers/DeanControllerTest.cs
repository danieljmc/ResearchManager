﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResearchManager;
using ResearchManager.Controllers;

namespace ResearchManager.Tests.Controllers
{
    [TestClass]
    public class DeanControllerTest

    {
        [TestMethod]
        public void DeanDetailsRedirect()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser;
            DeanController deanNullProject = new DeanController();
            DeanController deanNullUser = new DeanController();
            deanNullProject.TempData = tempData;

            //remove test project before usage
            db.projects.Remove(testProject);
            db.SaveChanges();

            //Return view with invalid projectID
            RedirectToRouteResult result = (RedirectToRouteResult)deanNullProject.Details(testProject.projectID);
            RedirectToRouteResult result2 = (RedirectToRouteResult)deanNullUser.Details(testProject.projectID); 
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main Tests
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual("Dean", result.RouteValues["controller"].ToString());

            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result2.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("SignIn", result2.RouteValues["action"].ToString());
            Assert.AreEqual("Home", result2.RouteValues["controller"].ToString());
        }

        [TestMethod]
        public void DeanDetailsStandard()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser; 
            DeanController dean = new DeanController();
            dean.TempData = tempData;

            ViewResult action = (ViewResult)dean.Details(testProject.projectID);

            //Remove testing models from database
            db.projects.Remove(testProject);
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main tests 
            Assert.IsNotNull(action);
            Assert.IsNotNull(action.ViewData.Model);
            Assert.AreEqual(action.TempData["ActiveUser"], testUser);

        }

        public void DeanIndexRedirect()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Dean", db);

            //Create controller instance
            tempData["ActiveUser"] = testUser;
            DeanController deanNullProject = new DeanController();
            DeanController deanNullUser = new DeanController();
            deanNullProject.TempData = tempData;

            //Return view with invalid projectID
            RedirectToRouteResult result = (RedirectToRouteResult)deanNullProject.Index();
            RedirectToRouteResult result2 = (RedirectToRouteResult)deanNullUser.Index();
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main Tests
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual("Dean", result.RouteValues["controller"].ToString());

            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result2.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("SignIn", result2.RouteValues["action"].ToString());
            Assert.AreEqual("Home", result2.RouteValues["controller"].ToString());
        }

        [TestMethod]
        public void DeanIndexStandard()
        {
            //Connect to database
            Entities db = new Entities();

            //Create new TempData storage
            TempDataDictionary tempData = new TempDataDictionary();

            //Add test models to the database
            user testUser = DatabaseInsert.AddTestUser("Dean", db);
            project testProject = DatabaseInsert.AddTestProject(testUser, db);

            //Create controller instance
            tempData["ActiveUser"] = testUser;

            DeanController dean = new DeanController();
            dean.TempData = tempData; 
            ViewResult action = (ViewResult)dean.Index();
           
            //Remove testing models from database
            db.projects.Remove(testProject);
            db.users.Remove(testUser);
            db.SaveChanges();

            //Main Tests
            Assert.IsNotNull(action);
            Assert.IsNotNull(action.ViewData.Model);
            Assert.IsNotNull(action.TempData["ActiveUser"]); 
            Assert.IsNotNull(testUser); 
            Assert.AreEqual(action.TempData["ActiveUser"], testUser);

        }

    }
}
