using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class Location
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string County { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();



        public Location() { }

        public List<String> GetLocation(string text)
        {
            List<String> objs = new List<String>();
            if (!string.IsNullOrEmpty(text))
            {
                objs = (from cities
                           in DataContext.CitiesExtendeds
                           where (cities.City.Contains(text)
                           || cities.Zip.Contains(text))
                           select string.Format("{0}, {1}", cities.City, cities.StateCode)).Take(100).Distinct().ToList();
                
            }
            if (objs == null || objs.Count == 0)
            {
                return new List<String>();
            }
            else
            {
                return objs;
            }
        }
    }
}