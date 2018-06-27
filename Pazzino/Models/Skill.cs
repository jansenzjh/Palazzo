using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class Skill//: CandidateSkill
    {
        public Guid Id { get; set; }
        public string SkillName { get; set; }
        public string SkillLevel { get; set; }
        public string UserID { get; set; }
        //public AspNetUser AspNetUser { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public Skill() { }

        public Skill(CandidateSkill obj)
        {
            ShareFunctions.CopyPropertiesTo<CandidateSkill, Skill>(obj, this);
        }

        public static List<Skill> ToModels(List<CandidateSkill> objs)
        {
            List<Skill> returnObjs = new List<Skill>();
            foreach (var o in objs)
            {
                returnObjs.Add(new Skill(o));
            }
            return returnObjs;
        }

        public List<CandidateSkill> Read(string userId)
        {
            List<CandidateSkill> objs = new List<CandidateSkill>();
            if (DataContext.CandidateSkills.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateSkills.Where(x => x.UserID.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel Insert(string userId, CandidateSkill obj)
        {
            CandidateSkill objt = new CandidateSkill();
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();

            try
            {
                DataContext.CandidateSkills.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(CandidateSkill obj)
        {
            try
            {
                CandidateSkill objt = DataContext.CandidateSkills.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(CandidateSkill obj)
        {
            try
            {
                CandidateSkill objt = DataContext.CandidateSkills.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.CandidateSkills.DeleteOnSubmit(objt);
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

        private void LoadModel(CandidateSkill source, CandidateSkill dest)
        {
            ShareFunctions.CopyPropertiesTo<CandidateSkill, CandidateSkill>(source, dest);
            dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserID));
        }


    }
}