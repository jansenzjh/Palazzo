using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;
using Pazzino.Models;

namespace Pazzino.Models
{
    public class UserDetailModel
    {
        public Guid Id { get; set; }
        public string UserID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Interest { get; set; }
        public string Achievement { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public UserDetailModel() { }

        public UserDetailModel(UserDetail obj)
        {
            ShareFunctions.CopyPropertiesTo<UserDetail, UserDetailModel>(obj, this);
        }

        public static List<UserDetailModel> SearchResumes(int page = 1, int size = 25, ResumeSearchFilter filter = null)
        {
            if (filter == null)
            {
                filter = new ResumeSearchFilter();
            }
            if (filter.FromDate == DateTime.MinValue)
            {
                filter.FromDate = DateTime.Today.AddDays(-30);
            }
            if (filter.ToDate == DateTime.MinValue)
            {
                filter.ToDate = DateTime.Today;
            }
            List<UserDetailModel> returnList = new List<UserDetailModel>();
            PalazzoDBDataContext context = new PalazzoDBDataContext();
            var ud = context.procSearchResume(filter.FromDate, filter.ToDate, filter.Keyword, filter.Page, filter.PageSize, filter.DegreeSearch, 0, filter.LanguageSearch, filter.EducationSearch, filter.SkillSearch);
            //List<UserDetail> objs = context.UserDetails.Skip((page - 1) * size).Take(size).ToList();
            foreach(procSearchResumeResult o in ud)
            {
                UserDetailModel obj = new UserDetailModel();
                ShareFunctions.CopyPropertiesTo<procSearchResumeResult, UserDetailModel>(o, obj);
                returnList.Add(obj);
            }
            return returnList;
        }

        public ResultModel SaveOrUpdate(string userId, UserDetail obj)
        {
            if (DataContext.UserDetails.Any(x => x.UserID.Equals(userId)))
            {
                //update
                return Update(obj);
            }
            else
            {
                //insert
                return Insert(userId, obj);
            }
        }

        public ResultModel Insert(string userId, UserDetail obj)
        {
            UserDetail objt = new UserDetail();
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();

            try
            {
                DataContext.UserDetails.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(UserDetail obj)
        {
            try
            {
                UserDetail objt = DataContext.UserDetails.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(UserDetail obj)
        {
            try
            {
                UserDetail objt = DataContext.UserDetails.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.UserDetails.DeleteOnSubmit(objt);
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

        private void LoadModel(UserDetail source, UserDetail dest)
        {
            ShareFunctions.CopyPropertiesTo<UserDetail, UserDetail>(source, dest);
            dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserID));
        }

        public UserDetail ToObject()
        {
            UserDetail obj = new UserDetail();
            ShareFunctions.CopyPropertiesTo<UserDetailModel, UserDetail>(this, obj);
            return obj;
        }
    }
}