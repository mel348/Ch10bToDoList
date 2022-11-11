using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private ToDoContext context;
        public HomeController(ToDoContext ctx) => context = ctx;

        /*Index method is loading up (grabbing id) categories, status's from database into viewBags
         Then grabbing DueFilter values from our filters class (setup by dictionaries).
         these are the values that it's grabbing.*/

        public IActionResult Index(string id)               
        {   
            ToDoViewModel model = new ToDoViewModel();

            // load current filters and data needed for filter drop downs in ViewBag
            var filters = new Filters(id);
            model.Filters = filters;
            model.Categories = context.Categories.ToList();
            model.Statuses = context.Statuses.ToList();
            model.DueFilters = Filters.DueFilterValues;

            // get ToDo objects from database based on current filters
            //Query object including foreignKeys and then it's checking if filters.Category is turned on
            //if turned on it is adjusting the query.  Makes sure category selected is what we are looking at.
            //Then it looks at status...can be on or off.

            IQueryable<ToDo> query = context.ToDos
                .Include(t => t.Category).Include(t => t.Status);
            if (filters.HasCategory) {
                query = query.Where(t => t.CategoryId == filters.CategoryId);
            }
            if (filters.HasStatus) {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }
            if (filters.HasDue) {                               //checks if filters past, future, or today is on
                var today = DateTime.Today;                             
                if (filters.IsPast)                                 
                    query = query.Where(t => t.DueDate < today);
                else if (filters.IsFuture)                          
                    query = query.Where(t => t.DueDate > today);
                else if (filters.IsToday)                           
                    query = query.Where(t => t.DueDate == today);
            }
            model.Tasks = query.OrderBy(t => t.DueDate).ToList();     //sort by due date, goes in list, stores in tasks, passes it on to our view
            return View(model);                                         //model.Tasks instead of var tasks, to pull from new property within our ToDoViewModel
        }

        public IActionResult Add()
        {
            ToDoViewModel model = new ToDoViewModel();

            model.Categories = context.Categories.ToList();
            model.Statuses = context.Statuses.ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ToDoViewModel model)
        {
            if (ModelState.IsValid)
            {
                context.ToDos.Add(model.CurrentTask);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                model.Categories = context.Categories.ToList();
                model.Statuses = context.Statuses.ToList();
                return View(model);
            }
        }
        //FILTER ACTION METHOD
        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);                   //how filters are getting SET
            return RedirectToAction("Index", new { ID = id });      //then redirected back to the Index
        }

        [HttpPost]
        public IActionResult Edit([FromRoute]string id, ToDo selected)
        {
            if (selected.StatusId == null) {
                context.ToDos.Remove(selected);
            }
            else {
                string newStatusId = selected.StatusId;
                selected = context.ToDos.Find(selected.Id);
                selected.StatusId = newStatusId;
                context.ToDos.Update(selected);
            }
            context.SaveChanges();

            return RedirectToAction("Index", new { ID = id });
        }
    }
}