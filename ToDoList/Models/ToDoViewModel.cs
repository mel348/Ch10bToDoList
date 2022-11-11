using System.Collections.Generic;

namespace ToDoList.Models
{
    public class ToDoViewModel
    {
        //this model provides all the properties necessary to store the data that the app currently stores in the ViewBag
        public ToDoViewModel()                                      
        {
            CurrentTask = new ToDo();
        }
        public Filters Filters { get; set; }
        public List<Status> Statuses { get; set; }
        public List<Category> Categories { get; set; }
        public Dictionary<string, string> DueFilters { get; set; }
        public List<ToDo> Tasks { get; set; }
        public ToDo CurrentTask { get; set; }                       //used for Add
    }
}
