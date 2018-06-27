using Pazzino.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pazzino.Models
{
    public class JobPostModel
    {

        public string JobName { get; set; }
        public string CompanyName { get; set; }
        public string JobLocation { get; set; }
        public string JobDescription { get; set; }
        public DateTime JobPostDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Salary { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsIndeed { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string UserID { get; set; }
        public Guid CompanyID { get; set; } 

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public JobPostModel() { }
        public JobPostModel(Guid guid)
        {
            JobPost jd = new JobPost();
            JobPost obj = DataContext.JobPosts.Where(x => x.Id.Equals(guid)).FirstOrDefault();
            if (obj != null)
            {
                ShareFunctions.CopyPropertiesTo<JobPost, JobPostModel>(obj, this);
                this.Description = DataContext.JobDetails.Where(x => x.JobId.Equals(guid)).Select(x => x.JobDetailContent).FirstOrDefault();
            }
        }

        public JobPostModel(JobPost obj)
        {
            if (obj != null)
            {
                ShareFunctions.CopyPropertiesTo<JobPost, JobPostModel>(obj, this);
            }
        }

        public ResultModel SaveOrUpdate(string userId)
        {
            if (this.Id == null || this.Id == new Guid())
            {
                return Insert(userId);
            }
            else
            {
                return Update();
            }
        }

        public ResultModel Insert(string userId)
        {
            JobPost objt = new JobPost() { };
            LoadModel(this, objt);
            objt.Id = Guid.NewGuid();
            try
            {
                DataContext.JobPosts.InsertOnSubmit(objt);
                DataContext.SubmitChanges();

                JobDetail jd = new JobDetail();
                jd.JobId = this.Id;
                jd.JobDetailContent = Description;
                DataContext.JobDetails.InsertOnSubmit(jd);

                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update()
        {
            try
            {
                JobPost objt = DataContext.JobPosts.Where(x => x.Id.Equals(this.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    //CopyProperties(obj, ref objt);
                    LoadModel(this, objt);
                    DataContext.SubmitChanges();

                    JobDetail jd = DataContext.JobDetails.Where(x => x.JobId.Equals(this.Id)).FirstOrDefault();
                    if (jd != null)
                    {
                        jd.JobDetailContent = this.Description;
                        DataContext.SubmitChanges();
                    }
                    else
                    {
                        jd = new JobDetail();
                        jd.JobId = this.Id;
                        jd.JobDetailContent = Description;
                        DataContext.JobDetails.InsertOnSubmit(jd);
                        DataContext.SubmitChanges();
                    }
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

        public ResultModel Delete(JobPost obj)
        {
            try
            {
                JobPost objt = DataContext.JobPosts.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.JobPosts.DeleteOnSubmit(objt);
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

        public JobPost ToObject()
        {
            JobPost obj = new JobPost();
            ShareFunctions.CopyPropertiesTo<JobPostModel, JobPost>(this, obj);
            return obj;
        }

        private void LoadModel(JobPostModel source, JobPost dest)
        {
            ShareFunctions.CopyPropertiesTo<JobPostModel, JobPost>(source, dest);

            //dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserId));
        }
    }
}