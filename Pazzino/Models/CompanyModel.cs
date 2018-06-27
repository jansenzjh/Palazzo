using Pazzino.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pazzino.Models
{
    public class CompanyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string URL { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public bool IsConfirmed { get; set; }
        public string RepresentativeUserId { get; set; }


        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public CompanyModel() { }

        public CompanyModel(Guid companyId) {
            if (DataContext.Companies.Any(x => x.Id.Equals(companyId)))
            {
                var obj = DataContext.Companies.Where(x => x.Id.Equals(companyId)).FirstOrDefault();
                ShareFunctions.CopyPropertiesTo<Company, CompanyModel>(obj, this);
            }
        }

        public CompanyModel(Company obj)
        {
            ShareFunctions.CopyPropertiesTo<Company, CompanyModel>(obj, this);
            
        }

        public List<JobPostModel> GetCompanyJobs()
        {
            List<JobPostModel> returnList = new List<JobPostModel>();
            PalazzoDBDataContext context = new PalazzoDBDataContext();
            List<JobPost> objs = context.JobPosts.Where(x => x.CompanyID.Equals(this.Id)).ToList();
            if (objs != null && objs.Count > 0)
            {
                foreach(var o in objs)
                {
                    returnList.Add(new JobPostModel(o));
                }
                return returnList;
            }
            return new List<JobPostModel>();
        }

        public static string GetCompanyName(Guid id)
        {
            PalazzoDBDataContext context = new PalazzoDBDataContext();
            if (context.Companies.Any(x => x.Id.Equals(id)))
            {
                return context.Companies.Where(x => x.Id.Equals(id)).Select(x => x.Name).FirstOrDefault();
            }
            return string.Empty;
        }

        public static List<CompanyModel> ToModels(List<Company> objs)
        {
            List<CompanyModel> returnObjs = new List<CompanyModel>();
            if (objs != null && objs.Count > 0)
            {
                foreach (var o in objs)
                {
                    returnObjs.Add(new CompanyModel(o));
                }
            }
            return returnObjs;
        }
        
        public static List<CompanyModel> GetCompanies(string userId)
        {
            List<CompanyModel> returnObjs = CompanyModel.ToModels(new CompanyModel().Read(userId));
            return returnObjs;
        }

        public Company Read(Guid companyId)
        {
            Company obj = new Company();
            if (DataContext.Companies.Any(x => x.Id.Equals(companyId)))
            {
                obj = DataContext.Companies.Where(x => x.Id.Equals(companyId)).FirstOrDefault();

            }
            return obj;
        }

        

        public List<Company> Read(string userId)
        {
            List<Company> objs = new List<Company>();
            if (DataContext.Companies.Any(x => x.RepresentativeUserId.Equals(userId)))
            {
                objs = DataContext.Companies.Where(x => x.RepresentativeUserId.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel SaveOrUpdate(string userId, Company obj)
        {
            if (obj.Id == null || obj.Id == new Guid())
            {
                return Insert(userId, obj);
            }
            else
            {
                return Update(obj);
            }
        }

        public ResultModel Insert(string userId, Company obj)
        {
            Company objt = new Company() { };
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();
            objt.RepresentativeUserId = userId;
            try
            {
                DataContext.Companies.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
                SessionItems.CurrentUser.UpdateUserCompanyRep(objt.Id);
                SessionItems.UpdatedCurrentUser();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(Company obj)
        {
            try
            {
                Company objt = DataContext.Companies.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(Company obj)
        {
            try
            {
                Company objt = DataContext.Companies.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.Companies.DeleteOnSubmit(objt);
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

        private void LoadModel(Company source, Company dest)
        {
            //ShareFunctions.CopyPropertiesTo<Company, Company>(source, dest);
            dest.Name = source.Name;
            dest.Address1 = source.Address1;
            dest.Address2 = source.Address2;
            dest.State = source.State;
            dest.Country = source.Country;
            dest.Zip = source.Zip;
            dest.Phone = source.Phone;
            dest.Email = source.Email;
            dest.Fax = source.Fax;
            dest.URL = source.URL;
            dest.ContactName = source.ContactName;
            dest.ContactEmail = source.ContactEmail;
            dest.ContactPhone = source.ContactPhone;
            dest.IsConfirmed = source.IsConfirmed;
        //dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserId));
    }


    }

}
