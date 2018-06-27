using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class JobCategory
    {
        public string Category { get; set; }
        public string Title { get; set; }
        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        

        public static List<string> GetCategories()
        {
            PalazzoDBDataContext context = new PalazzoDBDataContext();
            return context.JobCategories.Select(x => x.Category).Distinct().ToList();
        }

        public List<string> GetCategories(string text)
        {
            List<String> objs = new List<String>();
            if (!string.IsNullOrEmpty(text))
            {
                objs = (from cat
                            in DataContext.JobCategories
                        where (cat.Title.Contains(text)
                        || cat.Category.Contains(text))
                        select string.Format("{0}", cat.Title)).Take(100).Distinct().ToList();

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