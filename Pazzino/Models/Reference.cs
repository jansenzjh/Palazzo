using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class Reference//: CandidateReference
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserID { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public Reference() { }

        public Reference(CandidateReference obj)
        {
            ShareFunctions.CopyPropertiesTo<CandidateReference, Reference>(obj, this);
        }

        public static List<Reference> ToModels(List<CandidateReference> objs)
        {
            List<Reference> returnObjs = new List<Reference>();
            foreach (var o in objs)
            {
                returnObjs.Add(new Reference(o));
            }
            return returnObjs;
        }

        public List<CandidateReference> Read(string userId)
        {
            List<CandidateReference> objs = new List<CandidateReference>();
            if (DataContext.CandidateReferences.Any(x => x.UserID.Equals(userId)))
            {
                objs = DataContext.CandidateReferences.Where(x => x.UserID.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel Insert(string userId, CandidateReference obj)
        {
            CandidateReference objt = new CandidateReference();
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();

            try
            {
                DataContext.CandidateReferences.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(CandidateReference obj)
        {
            try
            {
                CandidateReference objt = DataContext.CandidateReferences.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(CandidateReference obj)
        {
            try
            {
                CandidateReference objt = DataContext.CandidateReferences.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.CandidateReferences.DeleteOnSubmit(objt);
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

        private void LoadModel(CandidateReference source, CandidateReference dest)
        {
            ShareFunctions.CopyPropertiesTo<CandidateReference, CandidateReference>(source, dest);
            dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserID));
        }


    }
}