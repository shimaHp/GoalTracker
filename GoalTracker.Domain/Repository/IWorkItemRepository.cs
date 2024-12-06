using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Domain.Repository
{
    public interface IWorkItemRepository
    {
        Task<int> CreateAsync(WorkItem workItem);
     
        Task DeleteAsync(IEnumerable<WorkItem> entities);

       


 
    }
}
