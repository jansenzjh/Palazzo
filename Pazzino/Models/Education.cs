using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class Education//: CandidateEducation
    {
        public Guid Id { get; set; }
        public string Degree { get; set; }
        public string Subject { get; set; }
        public string Institute { get; set; }
        public Decimal Grade { get; set; }
        public int GraduationYear { get; set; }
        public string UserID { get; set; }
        //public AspNetUser AspNetUser { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public Education() { }

        public Education(CandidateEducation obj)
        {
            ShareFunctions.CopyPropertiesTo<CandidateEducation, Education>(obj, this);
        }

        public static List<Education> ToModels(List<CandidateEducation> objs)
        {
            List<Education> returnObjs = new List<Education>();
            foreach (var o in objs)
            {
                returnObjs.Add(new Education(o));
            }
            return returnObjs;
        }

        public List<CandidateEducation> Read(string userId)
        {
            List<CandidateEducation> objs = new List<CandidateEducation>();
            if (DataContext.CandidateEducations.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateEducations.Where(x => x.UserID.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel Insert(string userId, CandidateEducation obj)
        {
            CandidateEducation objt = new CandidateEducation();
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();

            try
            {
                DataContext.CandidateEducations.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(CandidateEducation obj)
        {
            try
            {
                CandidateEducation objt = DataContext.CandidateEducations.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(CandidateEducation obj)
        {
            try
            {
                CandidateEducation objt = DataContext.CandidateEducations.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.CandidateEducations.DeleteOnSubmit(objt);
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

        private void LoadModel(CandidateEducation source, CandidateEducation dest)
        {
            ShareFunctions.CopyPropertiesTo<CandidateEducation, CandidateEducation>(source, dest);
            dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserID));
        }


    }
}