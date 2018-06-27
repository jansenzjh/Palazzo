using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class Job
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Salary { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();
        private const int _PageSize = 10;

        public Job() { }

        public List<JobPost> Search(string desc, string city, string state, ref int total, int page = 1, int lastMonthsNumber = 2)
        {
            List<JobPost> returnList = new List<JobPost>();
            if (!string.IsNullOrEmpty(desc))
            {
                PalazzoDBDataContext context = new PalazzoDBDataContext();
                var results = context.procJobSearch(desc, city, state, lastMonthsNumber, page, _PageSize);
                
                    
                foreach (procJobSearchResult rlt in results.ToList())
                {
                    JobPost obj = new JobPost();
                    ShareFunctions.CopyPropertiesTo<procJobSearchResult, JobPost>(rlt, obj);
                    returnList.Add(obj);
                    total = rlt.Total.Value;
                }
                
                

                //if (total == 0)
                //{
                //    total = DataContext.JobPosts.Count((job => job.Title.Contains(desc)
                //        || job.JobName.Contains(desc)
                //        || job.CompanyName.Contains(desc)
                //        || job.Category.Contains(desc)
                //        //|| job.JobDescription.Contains(desc)
                //        &&
                //        ((job.JobLocation.Contains(city)
                //        || job.JobLocation.Contains(state))
                //        //|| job.JobDescription.Contains(city)
                //        )
                //        &&
                //        (job.JobPostDate >= DateTime.Today.AddMonths(lastMonthsNumber * -1))
                //        ));
                //}
                
                //objs = DataContext.JobPosts.Where((job => job.Title.Contains(desc)
                //        || job.JobName.Contains(desc)
                //        || job.CompanyName.Contains(desc)
                //        || job.Category.Contains(desc)
                //        //|| job.JobDescription.Contains(desc)
                //        &&
                //        ((job.JobLocation.Contains(city)
                //        || job.JobLocation.Contains(state))
                //        //|| job.JobDescription.Contains(city)
                //        )
                //        &&
                //        (job.JobPostDate >= DateTime.Today.AddMonths(lastMonthsNumber * -1))
                //        )).Skip((page - 1) * _PageSize).Take(_PageSize).ToList();

            }
            if (returnList == null || returnList.Count == 0)
            {
                return new List<JobPost>();
            }
            else
            {
                return returnList;
            }
        }
    }
}