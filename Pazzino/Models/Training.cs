using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class Training//: CandidateTraining
    {
        public Guid Id { get; set; }
        public string Field { get; set; }
        public string OrganizationName { get; set; }
        public string TrainingTitle { get; set; }
        public string Location { get; set; }
        public string UserID { get; set; }
        //public AspNetUser AspNetUser { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public Training() { }

        public Training(CandidateTraining obj)
        {
            ShareFunctions.CopyPropertiesTo<CandidateTraining, Training>(obj, this);
        }

        public static List<Training> ToModels(List<CandidateTraining> objs)
        {
            List<Training> returnObjs = new List<Training>();
            foreach (var o in objs)
            {
                returnObjs.Add(new Training(o));
            }
            return returnObjs;
        }

        public List<CandidateTraining> Read(string userId)
        {
            List<CandidateTraining> objs = new List<CandidateTraining>();
            if (DataContext.CandidateTrainings.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateTrainings.Where(x => x.UserID.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel Insert(string userId, CandidateTraining obj)
        {
            CandidateTraining objt = new CandidateTraining();
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();

            try
            {
                DataContext.CandidateTrainings.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(CandidateTraining obj)
        {
            try
            {
                CandidateTraining objt = DataContext.CandidateTrainings.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(CandidateTraining obj)
        {
            try
            {
                CandidateTraining objt = DataContext.CandidateTrainings.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.CandidateTrainings.DeleteOnSubmit(objt);
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

        private void LoadModel(CandidateTraining source, CandidateTraining dest)
        {
            ShareFunctions.CopyPropertiesTo<CandidateTraining, CandidateTraining>(source, dest);
            dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserID));
        }


    }
}