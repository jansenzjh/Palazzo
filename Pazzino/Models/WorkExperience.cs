using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class WorkExperience//: CandidateWorkExperience
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }
        public string OrganizationName { get; set; }
        public string JobField { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserID { get; set; }
        //public AspNetUser AspNetUser { get; set; }
        
        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public WorkExperience() { }

        public WorkExperience(CandidateWorkExperience obj)
        {
            ShareFunctions.CopyPropertiesTo<CandidateWorkExperience, WorkExperience>(obj, this);
        }

        public static List<WorkExperience> ToModels(List<CandidateWorkExperience> objs)
        {
            List<WorkExperience> returnObjs = new List<WorkExperience>();
            foreach (var o in objs)
            {
                returnObjs.Add(new WorkExperience(o));
            }
            return returnObjs;
        }

        public List<CandidateWorkExperience> Read(string userId)
        {
            List<CandidateWorkExperience> objs = new List<CandidateWorkExperience>();
            if (DataContext.CandidateWorkExperiences.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateWorkExperiences.Where(x => x.UserID.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel Insert(string userId, CandidateWorkExperience obj)
        {
            CandidateWorkExperience objt = new CandidateWorkExperience();
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();
            
            try
            {
                DataContext.CandidateWorkExperiences.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(CandidateWorkExperience obj)
        {
            try
            {
                CandidateWorkExperience objt = DataContext.CandidateWorkExperiences.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    //CopyProperties(obj, ref objt);
                    LoadModel(obj, objt);
                    DataContext.SubmitChanges();
                }
                return ResultModel.SuccessResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
                return ResultModel.FailResult();
            }
        }

        public ResultModel Delete(CandidateWorkExperience obj)
        {
            try
            {
                CandidateWorkExperience objt = DataContext.CandidateWorkExperiences.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.CandidateWorkExperiences.DeleteOnSubmit(objt);
                    DataContext.SubmitChanges();
                }
                return ResultModel.SuccessResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
                return ResultModel.FailResult();
            }
        }

        private void LoadModel(CandidateWorkExperience source, CandidateWorkExperience dest)
        {
            ShareFunctions.CopyPropertiesTo<CandidateWorkExperience, CandidateWorkExperience>(source, dest);
            dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserID));
        }

        
    }
}