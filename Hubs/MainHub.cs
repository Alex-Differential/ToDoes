using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web;
using Microsoft.AspNet.SignalR;
using ToDoList.Models;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Entity;


namespace ToDoList.Hubs
{
    public class MainHub : Hub 
    {
        public void Announce(string itemIds, string itemNames, string itemDones)
        {
            Clients.All.Announce(itemIds, itemNames, itemDones);
        }

        //public ActionResult UpdateItem(string itemIds, string itemNames, string itemDones)
        //{
        //    int count = 1;
        //    List<int> itemIdList = new List<int>();
        //    List<string> itemNameList = new List<string>();
        //    itemDones = itemDones.ToLower();
        //    itemIdList = itemIds.Split("undefined,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        //    itemNameList = itemNames.Split("undefined,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
        //    var String = Regex.Split(itemDones, "undefined,").ToList();
        //    List<bool> itemDoneList = new List<bool>(new bool[String.Count]);
        //    for (int j = 0; j < String.Count; j++)
        //    {
        //        if (String[j] == "true,")
        //        {
        //            itemDoneList[j] = true;
        //        }
        //        else
        //        {
        //            itemDoneList[j] = false;
        //        }

        //    }

        //    int Number = itemIdList.Count();
        //    for (int i = 0; i < Number; i++)
        //    {
        //        ToDo thing = new ToDo { Id = itemIdList[i], Description = itemNameList[i], IsDone = itemDoneList[i + 1], RowNo = count };
        //        count++;
        //        db.ToDos.AddOrUpdate(thing);
        //        db.ToDos.GroupBy(x => x.RowNo);

        //        db.SaveChanges();
        //    }
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}
    }
}