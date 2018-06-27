using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pazzino.Models
{
    public class JobSearchParameter
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Salary { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int LastMonthsNumber { get; set; }
        public int Total { get; set; }

        public JobSearchParameter() { }

        /// <summary>
        /// Return true if location and category is the same
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public bool Equals(JobSearchParameter js)
        {
            return (this.Location.Equals(js.Location)
                && this.Category.Equals(js.Category));
        }
    }
}