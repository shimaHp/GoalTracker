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

    
    }
}
