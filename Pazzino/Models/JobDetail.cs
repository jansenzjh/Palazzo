using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pazzino.Database;

namespace Pazzino.Models
{
    public class JobDetailModel
    {
        public Guid JobId { get; set; }
        public String JobDetailContent { get; set; }
        public string Url { get; set; }
        
        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public JobDetailModel() { }

        public JobDetailModel(Guid guid) {
            Database.JobDetail jd = new Database.JobDetail();
            Database.JobDetail obj = DataContext.JobDetails.Where(x => x.JobId.Equals(guid)).FirstOrDefault();
            if (obj != null)
            {
                this.JobId = obj.JobId;
                this.JobDetailContent = obj.JobDetailContent;
                this.Url = DataContext.JobPosts.Where(x => x.Id.Equals(guid)).Select(x => x.Url).FirstOrDefault();
            }
        }

        public ResultModel Insert(string userId, JobDetail obj)
        {
            JobDetail objt = new JobDetail();
            LoadModel(obj, objt);
            try
            {
                DataContext.JobDetails.InsertOnSubmit(objt);

                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(JobDetail obj)
        {
            try
            {
                JobDetail objt = DataContext.JobDetails.Where(x => x.JobId.Equals(obj.JobId)).FirstOrDefault();
                if (objt != null && !objt.JobId.Equals(Guid.Empty))
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

        public ResultModel Delete(JobDetail obj)
        {
            try
            {
                JobDetail objt = DataContext.JobDetails.Where(x => x.JobId.Equals(obj.JobId)).FirstOrDefault();
                if (objt != null && !objt.JobId.Equals(Guid.Empty))
                {
                    DataContext.JobDetails.DeleteOnSubmit(objt);
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

        public JobDetail ToObject()
        {
            JobDetail obj = new JobDetail();
            ShareFunctions.CopyPropertiesTo<JobDetailModel, JobDetail>(this, obj);
            return obj;
        }

        private void LoadModel(JobDetail source, JobDetail dest)
        {
            ShareFunctions.CopyPropertiesTo<JobDetail, JobDetail>(source, dest);

            //dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserId));
        }

    }
}