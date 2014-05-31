using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;

using Nop.Services.Events;
using Nop.Plugin.Widgets.ProductSpecialSale.Domain;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Serivces
{
    public partial class PromptActivityService:IPromptActivityService
    {
        private readonly IRepository<ActivityRemind> _actRepository;
        private readonly IEventPublisher _eventPublisher;

        public PromptActivityService(IRepository<ActivityRemind> actRepository, IEventPublisher eventPublisher)
        {
            this._actRepository = actRepository;
            this._eventPublisher = eventPublisher;
        }

        public virtual void AddPromptActivity(ActivityRemind promptActivity)
        {
            _actRepository.Insert(promptActivity);
            _eventPublisher.EntityInserted(promptActivity);
        }

        public virtual ActivityRemind GetSmsPromptActivity(int productId, int activityId, int customerId)
        {

            var promptActivity = _actRepository.Table.Where(n => n.ActivityId == activityId && n.ProductId == productId  && n.CustomerId == customerId && n.Status == (int)ActivityRemindStatus.New).ToList().FirstOrDefault();
            return promptActivity;
        }

        public virtual ActivityRemind GetEmailPromptActivity(int productId, int activityId, int customerId)
        {

            var promptActivity = _actRepository.Table.Where(n => n.ActivityId == activityId && n.ProductId == productId  && n.CustomerId == customerId && n.Status == (int)ActivityRemindStatus.New).ToList().FirstOrDefault();
            return promptActivity;
        }

        public virtual void UpdatePromptActivity(ActivityRemind promptActivity)
        {
            if (promptActivity == null)
                throw new ArgumentNullException("promptActivity");

            _actRepository.Update(promptActivity);

            //event notification
            _eventPublisher.EntityUpdated(promptActivity);
        }

        public IPagedList<ActivityRemind> SearchUnProcessedActivityRemind(int aheadMinutes, int pageIndex, int pageSize)
        {
            var query = _actRepository.Table;
            var currentUtcDate = DateTime.UtcNow;
            var beginRemindTime = currentUtcDate.AddMinutes(aheadMinutes);
            query = query.Where(a => a.Status != (int)ActivityRemindStatus.Completed);
            query = query.Where(a => a.StartDateTimeUtc <= beginRemindTime && a.EndDateTimeUtc > currentUtcDate);
            query = query.OrderBy(qe => qe.CreatedOnUtc);
            var unProcessedActivityReminds = new PagedList<ActivityRemind>(query, pageIndex, pageSize);
            return unProcessedActivityReminds;
        }

        public IList<Core.Domain.Catalog.Product> GetRemindProductsByDate(DateTime dateTime, int? maxNumber)
        {
            var table = _actRepository.Table;
            var maxDateTime = dateTime.AddDays(1);
            var query = table.Where(m => m.StartDateTimeUtc>=dateTime&&m.StartDateTimeUtc<maxDateTime).OrderBy(a=>a.StartDateTimeUtc);
            return maxNumber != null ? query.Take(maxNumber.Value).Select(a => a.Product).OrderBy(m => m.Id).ToList() : query.Select(a => a.Product).OrderBy(m => m.Id).ToList();
        }
    }
}
