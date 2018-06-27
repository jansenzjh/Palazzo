using Pazzino.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pazzino.Models
{
    public class UserJobModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid JobId { get; set; }
        public JobPostModel Post { get; set; }

        private PalazzoDBDataContext DataContext = new PalazzoDBDataContext();

        public UserJobModel() { }

        public UserJobModel(UserJob obj, bool isLoadJob = true)
        {
            ShareFunctions.CopyPropertiesTo<UserJob, UserJobModel>(obj, this);
            if (isLoadJob)
            {
                Post = new JobPostModel(this.JobId);
            }
        }

        public static List<UserJobModel> ToModels(List<UserJob> objs)
        {
            List<UserJobModel> returnObjs = new List<UserJobModel>();
            if (objs != null && objs.Count > 0)
            {
                foreach (var o in objs)
                {
                    returnObjs.Add(new UserJobModel(o));
                }
            }
            return returnObjs;
        }

        public static List<UserJobModel> GetUserJobs(string userId)
        {
            List<UserJobModel> returnObjs = UserJobModel.ToModels(new UserJobModel().Read(userId));
            return returnObjs;
        }

        public List<UserJob> Read(string userId)
        {
            List<UserJob> objs = new List<UserJob>();
            if (DataContext.UserJobs.Any(x => x.UserId.Equals(userId)))
            {
                objs = DataContext.UserJobs.Where(x => x.UserId.Equals(userId)).ToList();

            }
            return objs;
        }

        public ResultModel Insert(string userId, UserJob obj)
        {
            UserJob objt = new UserJob() { };
            LoadModel(obj, objt);
            objt.Id = Guid.NewGuid();

            try
            {
                DataContext.UserJobs.InsertOnSubmit(objt);
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DataContext.SubmitChanges();
            }
            return ResultModel.SuccessResult();
        }

        public ResultModel Update(UserJob obj)
        {
            try
            {
                UserJob objt = DataContext.UserJobs.Where(x => x.Id.Equals(obj.Id)).FirstOrDefault();
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

        public ResultModel Delete(UserJob obj)
        {
            try
            {
                UserJob objt = DataContext.UserJobs.Where(x => x.JobId.Equals(obj.JobId) && x.UserId.Equals(obj.UserId)).FirstOrDefault();
                if (objt != null && !objt.Id.Equals(Guid.Empty))
                {
                    DataContext.UserJobs.DeleteOnSubmit(objt);
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

        private void LoadModel(UserJob source, UserJob dest)
        {
            ShareFunctions.CopyPropertiesTo<UserJob, UserJob>(source, dest);
            //dest.AspNetUser = DataContext.AspNetUsers.SingleOrDefault(x => x.Id.Equals(source.UserId));
        }


    }

}
