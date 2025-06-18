using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.Goals.Dtos
{
    public class UpdateGoalDto
    {
    
        public int Id { get; set; }

    
        public string Title { get; set; } = default!;

    
        public string? Description { get; set; }

        public DateTime? TargetDate { get; set; }


        public GoalStatus Status { get; set; }

  
        public Priority Priority { get; set; }

 
        public List<CreateWorkItemDto>? NewWorkItems { get; set; }

   
        public List<UpdateWorkItemDto>? UpdatedWorkItems { get; set; }


        public List<int>? DeletedWorkItemIds { get; set; }

        public byte[]? RowVersion { get; set; }

        //// Custom validation method
        //public bool IsValid(out List<string> errors)
        //{
        //    errors = new List<string>();

        //    // Validate target date is not in the past
        //    if (TargetDate.HasValue && TargetDate.Value.Date < DateTime.UtcNow.Date)
        //    {
        //        errors.Add("Target date cannot be in the past");
        //    }

        //    // Validate work item operations don't exceed limits
        //    var totalOperations = (NewWorkItems?.Count ?? 0) +
        //                        (UpdatedWorkItems?.Count ?? 0) +
        //                        (DeletedWorkItemIds?.Count ?? 0);

        //    if (totalOperations > 100)
        //    {
        //        errors.Add("Too many work item operations in single request (max 100)");
        //    }

        //    return errors.Count == 0;
        //}
    }
}
