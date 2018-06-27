using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pazzino.Database;
using Pazzino.Models;
using Kendo.Mvc.UI;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Flow.Model;
using System.Text.RegularExpressions;
using System.IO;

namespace Pazzino.Controllers
{
    public class ResumeController : Controller
    {
        // GET: Resume
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Resumes()
        {
            return View();
        }

        public ActionResult Resume_Read(DataSourceRequest request)
        {
            ResumeSearchFilter filter = Session[typeof(ResumeSearchFilter).FullName + "Resume_Read_Search"] as ResumeSearchFilter;
            if (filter == null)
            {
                filter = new ResumeSearchFilter();
            }
            List<UserDetailModel> returnObjs = UserDetailModel.SearchResumes(request.Page, request.PageSize, filter);
            return Json(Helper.ToUIDataSourceResult<UserDetailModel>(false, returnObjs, request, returnObjs.Count()), JsonRequestBehavior.AllowGet);

        }

        public ActionResult ViewResume(string userId)
        {
            Resume obj = new Resume(userId);
            return View(obj);
        }

        public ActionResult SearchResume(ResumeSearchFilter filter)
        {
            Session[typeof(ResumeSearchFilter).FullName + "Resume_Read_Search"] = filter;
            return Json(filter, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadResumeDoc(HttpPostedFileBase customDocument)
        {
            ResultModel result = new ResultModel();
            IFormatProvider<RadFlowDocument> fileFormatProvider = null;
            RadFlowDocument document = null;
            string mimeType = String.Empty;
            fileFormatProvider = new HtmlFormatProvider();
            if (customDocument != null && Regex.IsMatch(Path.GetExtension(customDocument.FileName), ".docx|.rtf|.html|.txt"))
            {
                document = fileFormatProvider.Import(customDocument.InputStream);
                HtmlFormatProvider provider = new HtmlFormatProvider();
                string html = provider.Export(document);
                UserResumeDocModel sd = new UserResumeDocModel() {UserID = SessionItems.CurrentUser.Id, DocText = html };
                result = sd.SaveOrUpDate();
                if (result.BooleanResult)
                {
                    result.BooleanResult = true;
                    result.StringResult = "Upload Successfully!";
                }
                else
                {
                    result.BooleanResult = false;
                    result.StringResult = "Upload fail! Please try again!";
                }
                
            }
            else
            {
                //invalid files
                result.BooleanResult = false;
                result.StringResult = "Invalid document! Please try again.";
            }

            return Json(result, JsonRequestBehavior.AllowGet); 

            
        }

        public ActionResult BuildResume()
        {
            UserModel um = SessionItems.CurrentUser;
            Resume resume = new Resume(um.Id);
            Session[typeof(Resume).FullName] = resume;
            return View(resume);
        }

        #region Resume Build


        public ActionResult SaveUserDetail(UserDetail detail)
        {
            ResultModel result = new UserDetailModel().SaveOrUpdate(SessionItems.CurrentUser.Id, detail);
            return new JsonResult() { Data = result };
        }

        #endregion


        #region WorkExperience RCUD

        public ActionResult EditingWorkExperience_Read(DataSourceRequest request, string userId)
        {
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            return Json(Helper.ToUIDataSourceResult<WorkExperience>(false, WorkExperience.ToModels(resume.WorkExperiences), request, resume.WorkExperiences.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingWorkExperience_Create(CandidateWorkExperience obj)
        {
            new WorkExperience().Insert(SessionItems.CurrentUser.Id, obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            resume.WorkExperiences.Add(obj);
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingWorkExperience_Update(CandidateWorkExperience obj)
        {
            new WorkExperience().Update(obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.WorkExperiences.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.WorkExperiences.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.WorkExperiences[idx] = obj;
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditingWorkExperience_Destroy(CandidateWorkExperience obj)
        {
            new WorkExperience().Delete(obj);
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.WorkExperiences.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.WorkExperiences.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.WorkExperiences.RemoveAt(idx);
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Language RCUD

        public ActionResult EditingLanguage_Read(DataSourceRequest request, string userId)
        {
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            return Json(Helper.ToUIDataSourceResult<Language>(false, Language.ToModels(resume.Languages), request, resume.Languages.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingLanguage_Create(CandidateLanguage obj)
        {
            new Language().Insert(SessionItems.CurrentUser.Id, obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            resume.Languages.Add(obj);
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingLanguage_Update(CandidateLanguage obj)
        {
            new Language().Update(obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Languages.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Languages.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Languages[idx] = obj;
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditingLanguage_Destroy(CandidateLanguage obj)
        {
            new Language().Delete(obj);
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Languages.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Languages.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Languages.RemoveAt(idx);
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Skill RCUD

        public ActionResult EditingSkill_Read(DataSourceRequest request, string userId)
        {
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            return Json(Helper.ToUIDataSourceResult<Skill>(false, Skill.ToModels(resume.Skills), request, resume.Skills.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingSkill_Create(CandidateSkill obj)
        {
            new Skill().Insert(SessionItems.CurrentUser.Id, obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            resume.Skills.Add(obj);
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingSkill_Update(CandidateSkill obj)
        {
            new Skill().Update(obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Skills.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Skills.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Skills[idx] = obj;
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditingSkill_Destroy(CandidateSkill obj)
        {
            new Skill().Delete(obj);
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Skills.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Skills.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Skills.RemoveAt(idx);
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Education RCUD

        public ActionResult EditingEducation_Read(DataSourceRequest request, string userId)
        {
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            return Json(Helper.ToUIDataSourceResult<Education>(false, Education.ToModels(resume.Educations), request, resume.Educations.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingEducation_Create(CandidateEducation obj)
        {
            new Education().Insert(SessionItems.CurrentUser.Id, obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            resume.Educations.Add(obj);
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingEducation_Update(CandidateEducation obj)
        {
            new Education().Update(obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Educations.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Educations.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Educations[idx] = obj;
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditingEducation_Destroy(CandidateEducation obj)
        {
            new Education().Delete(obj);
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Educations.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Educations.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Educations.RemoveAt(idx);
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reference RCUD

        public ActionResult EditingReference_Read(DataSourceRequest request, string userId)
        {
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            return Json(Helper.ToUIDataSourceResult<Reference>(false, Reference.ToModels(resume.References), request, resume.References.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingReference_Create(CandidateReference obj)
        {
            new Reference().Insert(SessionItems.CurrentUser.Id, obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            resume.References.Add(obj);
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingReference_Update(CandidateReference obj)
        {
            new Reference().Update(obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.References.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.References.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.References[idx] = obj;
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditingReference_Destroy(CandidateReference obj)
        {
            new Reference().Delete(obj);
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.References.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.References.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.References.RemoveAt(idx);
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Training RCUD

        public ActionResult EditingTraining_Read(DataSourceRequest request, string userId)
        {
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            return Json(Helper.ToUIDataSourceResult<Training>(false, Training.ToModels(resume.Trainings), request, resume.Trainings.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingTraining_Create(CandidateTraining obj)
        {
            new Training().Insert(SessionItems.CurrentUser.Id, obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            resume.Trainings.Add(obj);
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditingTraining_Update(CandidateTraining obj)
        {
            new Training().Update(obj);

            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Trainings.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Trainings.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Trainings[idx] = obj;
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditingTraining_Destroy(CandidateTraining obj)
        {
            new Training().Delete(obj);
            Resume resume = Session[typeof(Resume).FullName] as Resume;
            if (resume.Trainings.Any(x => x.Id.Equals(obj.Id)))
            {
                int idx = resume.Trainings.Select((v, i) => new { objt = v, index = i }).FirstOrDefault(x => x.objt.Id.Equals(obj.Id)).index;
                resume.Trainings.RemoveAt(idx);
            }
            Session[typeof(Resume).FullName] = resume;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}