using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Guid CompanyID { get; set; }
        public AspNetUser UserObject { get; set; }
        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public UserModel() {
            Id = Guid.Empty.ToString();
        }
        public UserModel(AspNetUser user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.UserName = user.UserName;
            this.UserObject = user;
            if (user.CompanyID != null)
            {
                this.CompanyID = user.CompanyID.Value;
            }
            
        }

        public UserModel GetUser(string email)
        {
            if (DataContext.AspNetUsers.Any(x => x.Email.Equals(email)))
            {
                var obj = DataContext.AspNetUsers.Where(x => x.Email.Equals(email)).FirstOrDefault();
                return new UserModel(obj);
            }
            else
            {
                return new UserModel();
            }
        }

        public ResultModel UpdateUserCompanyRep(Guid companyId)
        {
            try
            {
                var obj = DataContext.AspNetUsers.Where(x => x.Id.Equals(this.Id)).FirstOrDefault();
                if (obj != null)
                {
                    obj.CompanyID = companyId;
                    DataContext.SubmitChanges();
                }
                return ResultModel.SuccessResult();
            }
            catch (Exception e)
            {
                return ResultModel.FailResult();
            }
            
        }
    }
}
