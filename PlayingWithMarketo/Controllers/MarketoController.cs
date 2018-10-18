using PagedList;
using PlayingWithMarketo.Core;
using PlayingWithMarketo.Core.Models;
using PlayingWithMarketo.Core.ViewModels;
using PlayingWithMarketo.Marketo.Enums;
using PlayingWithMarketo.Persistance;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;

namespace PlayingWithMarketo.Controllers
{
    public class MarketoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MarketoHelper _marketoHelper;

        public MarketoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _marketoHelper = new MarketoHelper(_unitOfWork);

        }

        public ViewResult Home()
        {
            var model = new List<ActivityViewModel>().ToPagedList(1, 3);
            return View("Activities", model);
        }

        public ViewResult Activities(string startDate, string endDate, int? page)
        {
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue;

            if (startDate != null || endDate != null)
            {
                page = page ?? 1;
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
            }

            start = DateTime.Parse(ViewBag.StartDate);
            end = DateTime.Parse(ViewBag.EndDate);


            var dataList = new List<ActivityViewModel>();
            var leadsActivitiesList = new List<LeadActivity>();
            leadsActivitiesList = _unitOfWork.LeadActivities.GetLeadActivities(start, end);

            if (LeadActivity.ToBeUpdated(start, end, leadsActivitiesList))
            {
                var exportJobId = _marketoHelper.CreateExportJob(start, end);
                _marketoHelper.QueueJob(exportJobId);

                string jobStatus = "";
                Status? jobStatusEnum;
                jobStatus = _unitOfWork.ExportJobs.GetJobStatus(exportJobId);
                jobStatusEnum = (Status)Enum.Parse(typeof(Status), jobStatus, true);

                while (jobStatusEnum != Status.Completed && jobStatusEnum != Status.Failed)
                {
                    jobStatusEnum = _marketoHelper.GetJobStatus(exportJobId);
                    Thread.Sleep(2000);
                }

                _marketoHelper.RetreiveData(exportJobId);
            }


            var leadActivities = _unitOfWork.LeadActivities.GetLeadActivitiesWithIncludes(start, end);

            foreach (var leadActivity in leadActivities)
            {
                var activityType = _unitOfWork.Activities.GetActivity(leadActivity.ActivityId);
                var activityViewModel = new ActivityViewModel()
                {
                    LeadId = leadActivity.Lead.LeadId.ToString(),
                    ActivityType = activityType,
                    ActivityDate = leadActivity.ActivityDate.ToString(),
                    CampaignId = leadActivity.Lead.CampaignId,
                    SFDCId = leadActivity.Lead.SFDCId,
                    Attributes = leadActivity.Attributes
                };
                dataList.Add(activityViewModel);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(dataList.ToPagedList(pageNumber, pageSize));
        }
    }
}
