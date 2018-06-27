using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pazzino.Database;
using Pazzino.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Pazzino.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Index()
        {
            return View();
        }
        
        #region Company Job

        public ActionResult EditJobPost(Guid JobPostID)
        {
            JobPostModel post = new JobPostModel();
            if (JobPostID != Guid.Empty)
            {
                //edit mode
                post = new JobPostModel(JobPostID);
                
            }
            return View(post);
        }

        [HttpPost]
        public ActionResult EditJobPost(JobPostModel model)
        {
            model.SaveOrUpdate(SessionItems.CurrentUser.Id);
            return RedirectToAction("index", "Manage");
        }


        public ActionResult CompanyJob_Read(DataSourceRequest request)
        {
            List<JobPostModel> returnObjs = SessionItems.UserCompany.GetCompanyJobs();
            return Json(Helper.ToUIDataSourceResult<JobPostModel>(false, returnObjs, request, returnObjs.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyJob_Destroy(JobPostModel obj)
        {
            new JobPostModel().Delete(obj.ToObject());
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region User Job


        public ActionResult UserJob_Read(DataSourceRequest request)
        {
            List<JobPostModel> returnObjs = new List<JobPostModel>();
            List<UserJobModel> objs = UserJobModel.GetUserJobs(SessionItems.CurrentUser.Id);
            if (objs != null && objs.Count > 0)
            {
                returnObjs = objs.Select(x => x.Post).ToList();
                
            }
            return Json(Helper.ToUIDataSourceResult<JobPostModel>(false, returnObjs, request, returnObjs.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserJob_Destroy(JobPostModel obj)
        {
            new UserJobModel().Delete(new UserJob() { JobId = obj.Id, UserId = SessionItems.CurrentUser.Id });
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUserJob(string jobId)
        {
            ResultModel result = new UserJobModel().Insert(SessionItems.CurrentUser.Id, new UserJob() { JobId = new Guid(jobId), UserId= SessionItems.CurrentUser.Id });
            return new JsonResult() { Data = result };
        }

        public ActionResult JobsView(string category, string location, int page = 1, int lastMonthsNumber = 1)
        {
            return View("JobSearchResultView");
        }

        public ActionResult JobDetailView(string jobId)
        {
            Guid gid = new Guid(jobId);
            return View("JobDetailView", new Models.JobDetailModel(gid));
        }

        public ActionResult Search([DataSourceRequest] DataSourceRequest request,string category, string location, int page = 1, int lastMonthsNumber = 1)
        {
            List<JobPost> processObjs = new List<JobPost>();
            string state = string.Empty;
            string city = location;
            int total = 0;

            JobSearchParameter oldJs = Session[typeof(JobSearchParameter).FullName] as JobSearchParameter;
            JobSearchParameter js = new JobSearchParameter();

            js.Category = category;
            js.Location = location;

            if (oldJs != null)
            {
                js.Total = oldJs.Total;
                if (js.Equals(oldJs))
                {
                    total = oldJs.Total;
                }
            }
            

            //Process Location Value
            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(location) && location.Contains(","))
            {
                string[] loc = location.Split(',');
                if (loc != null && loc.Length > 1)
                {
                    city = loc[0].Trim();
                    state = loc[1].Trim();
                }
                else
                {
                    city = location.Trim(',');
                }
            }
            
            processObjs = new Job().Search(category, city, state, ref total, page, lastMonthsNumber);
            js.Total = total;
            Session[typeof(JobSearchParameter).FullName] = js;
            return Json(Helper.ToUIDataSourceResult<JobPost>(true, processObjs, request, total), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}