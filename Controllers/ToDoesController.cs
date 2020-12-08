using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ToDoes
        public ActionResult Index()
        {
            var item = db.ToDos.ToList();
            var item2 = item.OrderBy(x => x.RowNo);
            db.ToDos.OrderBy(x => x.RowNo);
            return View();
        }

        public ActionResult UpdateItem(string itemIds, string itemNames, string itemDones)
        {
            int count = 1;
            List<int> itemIdList = new List<int>();
            List<string> itemNameList = new List<string>();
            itemDones = itemDones.ToLower();
            itemIdList = itemIds.Split("undefined,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            itemNameList = itemNames.Split("undefined,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            var String = Regex.Split(itemDones, "undefined,").ToList();
            List<bool> itemDoneList = new List<bool>(new bool[String.Count]);
            for (int j=0; j < String.Count; j++)
            {
                if(String[j] == "true,")
                {
                    itemDoneList[j] = true;
                }
                else
                {
                    itemDoneList[j] = false;
                }

            }

            int Number = itemIdList.Count();         
            for (int i=0; i<Number; i++)
            {
                ToDo thing = new ToDo { Id = itemIdList[i], Description = itemNameList[i], IsDone = itemDoneList[i+1], RowNo = count};
                count++;
                db.ToDos.AddOrUpdate(thing);
                db.ToDos.GroupBy(x => x.RowNo);

                db.SaveChanges();
            }

            
            return Json(true, JsonRequestBehavior.AllowGet);
            
        }

        private IEnumerable<ToDo> GetMyToDoes()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);
            IEnumerable<ToDo> myToDoes = db.ToDos.ToList().Where(x => x.User == currentUser);

            int completeCount = 0;
            foreach (ToDo toDo in myToDoes)
            {
                if (toDo.IsDone)
                {
                    completeCount++;
                }
            }
            ViewBag.Percent = Math.Round(100f * ((float)completeCount / (float)myToDoes.Count()));
            ViewBag.PercentPreview = ViewBag.Percent - Math.Round(100f * ( 1/ (float)myToDoes.Count()));
            ViewBag.PercentNext = ViewBag.Percent + Math.Round(100f * (1 / (float)myToDoes.Count()));
            return myToDoes.OrderBy(x =>x.RowNo).ToList();
        }

        public ActionResult BuildToDoTable()
        {
            return PartialView("_ToDoTable",GetMyToDoes());
        }

        // GET: ToDoes/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if(toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // GET: ToDoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Id,Description,IsDone,RowNo")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault
                    (x => x.Id == currentUserId);
                toDo.User = currentUser;
                db.ToDos.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDo);
        }

        [HttpGet]
        public PartialViewResult SomeAction()
        {
            return PartialView("_ToDoTable", GetMyToDoes());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description,IsDone,RowNo")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault
                    (x => x.Id == currentUserId);
                toDo.User = currentUser;
                toDo.IsDone = false;
                db.ToDos.Add(toDo);
                db.SaveChanges();
            }
            return PartialView("_ToDoTable", GetMyToDoes());
        }
        // GET: ToDoes/Edit/5
        public ActionResult Edit(int? id)
        {/*
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if(toDo == null)
            {
                return HttpNotFound();
            }
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);
            if(toDo.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            var std = db.ToDos.Where(s => s.Id == id).FirstOrDefault();
            return View(std);
        }

        //POST: ToDoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="Id,Description,IsDone,RowNo")] ToDo toDo)
        {/*
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDo);*/

            if (ModelState.IsValid)
            {
                try
                {
                    db.ToDos.AddOrUpdate(toDo);
                    //db.Update(toDo);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                }
                return RedirectToAction("Index");
            }
            return View(toDo);
        }

        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDo.IsDone = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_ToDoTable", GetMyToDoes());
            }  
        }

        //GET: ToDoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if(toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.ToDos.Remove(toDo);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
    }
}