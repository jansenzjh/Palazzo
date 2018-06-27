using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class Language//: CandidateLanguage
    {
        public Guid Id { get; set; }
        public string LanguageName { get; set; }
        public string ListeningLevel { get; set; }
        public string SpeakingLevel { get; set; }
        public string ReadingLevel { get; set; }
        public string WritingLevel { get; set; }
        public string UserID { get; set; }
        //public AspNetUser AspNetUser { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public Language() { }

        public Language(CandidateLanguage obj)
        {
            ShareFunctions.CopyPropertiesTo<CandidateLanguage, Language>(obj, this);
        }

        public static List<Language> ToModels(List<CandidateLanguage> objs)
        {
            List<Language> returnObjs = new List<Language>();
            foreach (var o in objs)
            {
                returnObjs.Add(new Language(o));
            }
            return returnObjs;
        }

        public List<CandidateLanguage> Read(string userId)
        {
            List<CandidateLanguage> objs = new List<CandidateLanguage>();
            if (DataContext.CandidateLanguages.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateLanguages.Where(x => x.UserID.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel Insert(string userId, CandidateLanguage obj)
        {
            CandidateLanguage objt = new CandidateLanguage();
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();

            try
            {
                DataContext.CandidateLanguages.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(CandidateLanguage obj)
        {
            try
            {
                CandidateLanguage objt = DataContext.CandidateLanguages.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(CandidateLanguage obj)
        {
            try
            {
                CandidateLanguage objt = DataContext.CandidateLanguages.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.CandidateLanguages.DeleteOnSubmit(objt);
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

        private void LoadModel(CandidateLanguage source, CandidateLanguage dest)
        {
            ShareFunctions.CopyPropertiesTo<CandidateLanguage, CandidateLanguage>(source, dest);
            dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserID));
        }


    }
}