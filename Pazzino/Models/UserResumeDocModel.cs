using Pazzino.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pazzino.Models
{
    public class UserResumeDocModel
    {
        public string UserID { get; set; }
        public string DocText { get; set; }
        public Guid Id { get; set; }

        public UserResumeDocModel() { }


        public ResultModel Insert()
        {
            
            try
            {
                PalazzoDBDataContext DataContext = new PalazzoDBDataContext();
                UserResumeDoc objt = new UserResumeDoc();

                objt.Id = Guid.NewGuid();
                objt.UserId = this.UserID;
                objt.DocText = this.DocText;
                DataContext.UserResumeDocs.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
                return ResultModel.SuccessResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResultModel.FailResult();
            }
            
        }


        public ResultModel Update()
        {
            //update
            PalazzoDBDataContext DataContext = new PalazzoDBDataContext();
            try
            {
                UserResumeDoc objt = DataContext.UserResumeDocs.Where(x => x.Id.Equals(this.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    //CopyProperties(obj, ref objt);
                    objt.UserId = this.UserID;
                    objt.DocText = this.DocText;
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

        public ResultModel SaveOrUpDate()
        {
            PalazzoDBDataContext DataContext = new PalazzoDBDataContext();
            if (DataContext.UserResumeDocs.Any(x => x.UserId.Equals(this.UserID)))
            {
                return Update();
            }
            else
            {
                return Insert();
            }
            
        }

    }
}