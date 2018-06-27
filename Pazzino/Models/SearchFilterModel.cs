using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pazzino.Models
{
    public class SearchFilterBase
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Keyword { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ResumeSearchFilter: SearchFilterBase
    {
        public string DegreeSearch { get; set; }
        public string ExperienceYearSearch { get; set; }
        public string LanguageSearch { get; set; }
        public string EducationSearch { get; set; }
        public string SkillSearch { get; set; }

        public ResumeSearchFilter() { }
    }
}