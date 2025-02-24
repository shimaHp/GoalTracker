using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Domain.Interfaces
{
    public interface IWorkItemAuthorizationService
    {
        bool Authorize(WorkItem workItem, ResourceOperation resourceOperation);
    }
}
