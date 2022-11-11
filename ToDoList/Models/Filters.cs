using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Filters                                            //just storing the status of the filters
    {
        public Filters(string filterstring)
        {
            FilterString = filterstring ?? "all-all-all";           //starting state of "all"
            string[] filters = FilterString.Split('-');             //string for filters is being split based on the dash
            CategoryId = filters[0];                                //array of filters with 3 things in it and each is going to say "all"
            Due = filters[1];                                       //subscript into position one which has "all" in Due
            StatusId = filters[2];                                  //subscript into position two which has "all" in statusID
        }
        //create properties which all have "get" no "set".  Retrival purposes only.
        public string FilterString { get; }                         
        public string CategoryId { get; }
        public string Due { get; }
        public string StatusId { get; }

        //additional "has" properties.  only set to "true" when they don't equal "all" 
        //only going to turn on if someone changes them from "all"
        public bool HasCategory => CategoryId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";

        //Different values that are acceptable for the "Due" filter
        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string> {
                { "future", "Future" },
                { "past", "Past" },
                { "today", "Today" }
            };
        //Checking 3 properties.  
        //Want to see if it is equal to "past", "future" or "Today and if so they turn on.
        public bool IsPast => Due.ToLower() == "past";
        public bool IsFuture => Due.ToLower() == "future";
        public bool IsToday => Due.ToLower() == "today";
    }
}
