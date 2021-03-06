﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using ProjectITP.Models;

namespace ProjectITP.Controllers
{
    [Authorize]
    public class ProfitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profits
        public ActionResult Index()
        {
            return View(db.profits.ToList());
        }




        //filter
        [HttpPost]
       public ActionResult Index(int? Year, int? Month)
        {

            var profits1 = from emp in db.profits select emp;

            if ((Year != null))
            {
                profits1 = profits1.Where((e => e.Year == Year));
                profits1 = profits1.Where(e => e.Month == Month);


            }

            return View(profits1);
        }
        


        // GET: Profits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profit profit = db.profits.Find(id);
            if (profit == null)
            {
                return HttpNotFound();
            }
            return View(profit);
        }

        // GET: Profits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Year,Month,TeacherPayment,NonAcadamicPayment,UtilityPayment,Others,MonthlyIncome")] Profit profit)
        {
            if (ModelState.IsValid)
            {
                db.profits.Add(profit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profit);
        }

        // GET: Profits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profit profit = db.profits.Find(id);
            if (profit == null)
            {
                return HttpNotFound();
            }
            return View(profit);
        }

        // POST: Profits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Year,Month,TeacherPayment,NonAcadamicPayment,UtilityPayment,Others,MonthlyIncome")] Profit profit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profit);
        }

        // GET: Profits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profit profit = db.profits.Find(id);
            if (profit == null)
            {
                return HttpNotFound();
            }
            return View(profit);
        }

        // POST: Profits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profit profit = db.profits.Find(id);
            db.profits.Remove(profit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Report
        public ActionResult Reports(string ReportType)
        {

            var profits = db.profits;


            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/ProfitReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "ProfitDataSet";
            reportDataSource.Value = profits.ToList();
            localReport.DataSources.Add(reportDataSource);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            if (reportType == "Excel")
            {
                fileNameExtension = "xlsx";
            }
            else if (reportType == "Word")
            {
                fileNameExtension = "docx";
            }
            else if (reportType == "Pdf")
            {
                fileNameExtension = "pdf";
            }
            else
            {
                fileNameExtension = "jpg";
            }


            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render(reportType, "",
                out mimeType, out encoding, out fileNameExtension, out streams,
                out warnings);
            Response.AddHeader("content-disposition", "attachment;filename=Profit_report." + fileNameExtension);
            return File(renderedByte, fileNameExtension);









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
