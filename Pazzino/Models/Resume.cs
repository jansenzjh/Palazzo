using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;
namespace Pazzino.Models
{
    public class Resume
    {

        public UserDetailModel UserDetail { get; set; }
        public List<CandidateWorkExperience> WorkExperiences { get; set; }
        public List<CandidateTraining> Trainings { get; set; }
        public List<CandidateSkill> Skills { get; set; }
        public List<CandidateLanguage> Languages { get; set; }
        public List<CandidateEducation> Educations { get; set; }
        public List<CandidateReference> References { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public Resume() { }

        public Resume(string userId, bool basicInformationOnly = false)
        {
            this.UserDetail = GetUserDetail(userId);
            LoadExtraInfo(userId, basicInformationOnly);

        }

        private void LoadExtraInfo(string userId, bool basicInformationOnly)
        {
            if (!basicInformationOnly)
            {
                this.WorkExperiences = GetUserWorkExperiences(userId);
                this.Trainings = GetUserTrainings(userId);
                this.Skills = GetUserSkills(userId);
                this.Languages = GetUserLanguages(userId);
                this.Educations = GetUserEducations(userId);
                this.References = GetUserResumeReferences(userId);
            }
        }

        public Resume(UserDetail userDetail, bool basicInformationOnly = false)
        {
            if (!userDetail.UserID.Equals(Guid.Empty.ToString()))
            {
                string userId = userDetail.UserID;
                this.UserDetail = GetUserDetail(userId);
                LoadExtraInfo(userId, basicInformationOnly);
            }

        }

        //public static List<Resume> GetResumes(ResumeSearchFilter filter)
        //{
        //    List<Resume> returnList = new List<Resume>();
        //    using (PalazzoDBDataContext context = new PalazzoDBDataContext())
        //    {
        //        var ud = context.procSearchResume(filter.FromDate, filter.ToDate, filter.Keyword, filter.Page, filter.PageSize, filter.DegreeSearch, 0, filter.LanguageSearch, filter.EducationSearch, filter.SkillSearch);
        //        //List<UserDetail> ud = context.UserDetails.Skip((filter.Page - 1) * filter.PageSize).ToList();
        //        if (ud != null)
        //        {
        //            foreach(procSearchResumeResult u in ud)
        //            {
                        
        //                //returnList.Add(new Resume() { d});
        //            }
        //        }
        //    }
        //    return returnList;
        //}

        public UserDetailModel GetUserDetail(string userId)
        {
            UserDetail ud = new UserDetail();
            if (DataContext.UserDetails.Any(x => x.UserID.Equals(userId)))
            {
                ud = DataContext.UserDetails.Where(x => x.UserID.Equals(userId)).FirstOrDefault();
            }
            return new UserDetailModel(ud);
        }

        public List<CandidateWorkExperience> GetUserWorkExperiences(string userId)
        {
            List<CandidateWorkExperience> objs = new List<CandidateWorkExperience>();
            if (DataContext.CandidateWorkExperiences.Any(x => x.UserID.Equals(userId))){
                objs = DataContext.CandidateWorkExperiences.Where(x => x.UserID.Equals(userId)).ToList();
            }
            return objs;
        }

        public List<CandidateTraining> GetUserTrainings(string userId)
        {
            List<CandidateTraining> objs = new List<CandidateTraining>();
            if (DataContext.CandidateTrainings.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateTrainings.Where(x => x.UserID.Equals(userId)).ToList();
            }
            return objs;
        }

        public List<CandidateSkill> GetUserSkills(string userId)
        {
            List<CandidateSkill> objs = new List<CandidateSkill>();
            if (DataContext.CandidateSkills.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateSkills.Where(x => x.UserID.Equals(userId)).ToList();
            }
            return objs;
        }

        public List<CandidateLanguage> GetUserLanguages(string userId)
        {
            List<CandidateLanguage> objs = new List<CandidateLanguage>();
            if (DataContext.CandidateLanguages.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateLanguages.Where(x => x.UserID.Equals(userId)).ToList();
            }
            return objs;
        }

        public List<CandidateEducation> GetUserEducations(string userId)
        {
            List<CandidateEducation> objs = new List<CandidateEducation>();
            if (DataContext.CandidateEducations.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateEducations.Where(x => x.UserID.Equals(userId)).ToList();
            }
            return objs;
        }

        public List<CandidateReference> GetUserResumeReferences(string userId)
        {
            List<CandidateReference> objs = new List<CandidateReference>();
            if (DataContext.CandidateReferences.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateReferences.Where(x => x.UserID.Equals(userId)).ToList();
            }
            return objs;
        }
    }
}