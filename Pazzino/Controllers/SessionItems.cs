using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pazzino.Database;
using Pazzino.Models;
using System.Configuration;

public class SessionItems
{
    public static String WebsiteName
    {
        get
        {
            HttpContext context = HttpContext.Current;
            if (context.Application[ApplicationItems.WebsiteName] == null)
            {
                string name = ConfigurationManager.AppSettings["WebsiteName"];
                if (!string.IsNullOrEmpty(name)) 
                {
                    
                    context.Application[ApplicationItems.WebsiteName] = name;
                    return name;
                }
                else
                {
                    return "Find Job Faster";
                }
            }
            else
            {
                return ConfigurationManager.AppSettings["WebsiteName"] as string;
            }
        }
    }

    public static CompanyModel UserCompany
    {
        get
        {
            HttpContext context = HttpContext.Current;
            if (context.Application[ApplicationItems.UserCompany] == null)
            {
                Guid companyId = CurrentUser.CompanyID;
                if(companyId != null && companyId != Guid.Empty)
                {
                    CompanyModel comp = new CompanyModel(companyId);
                    context.Application[ApplicationItems.UserCompany] = comp;
                    return comp;
                }
                else
                {
                    return new CompanyModel();
                }
            }
            else
            {
                return context.Application[ApplicationItems.UserCompany] as CompanyModel;
            }
        }
    }

    public static UserModel CurrentUser
    {
        get
        {
            HttpContext context = HttpContext.Current;
            if (context.Application[ApplicationItems.CurrentUser] == null)
            {
                return UpdatedCurrentUser();
            }
            else
            {
                return context.Application[ApplicationItems.CurrentUser] as UserModel;
            }
        }
    }

    public static  UserModel UpdatedCurrentUser()
    {
        
            HttpContext context = HttpContext.Current;
            
            var userName = context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                var um = new UserModel().GetUser(userName);
                if (!um.Id.Equals(Guid.Empty.ToString()))
                {
                    context.Application[ApplicationItems.CurrentUser] = um;
                    return um;
                }
            }
            return new UserModel();

    }
}
