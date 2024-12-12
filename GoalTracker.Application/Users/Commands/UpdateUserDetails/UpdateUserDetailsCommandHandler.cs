
using GoalTracker.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using GoalTracker.Domain.Entities;

namespace GoalTracker.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger, IUserContext userContext,
    IUserStore<Domain.Entities.User> userStore
    ) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();


        logger.LogInformation("Updating user:{UsetId}, with {@Request}", user!.Id, request);
        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);
        if (dbUser == null)
        {
            throw new NotFoundException(nameof(User), user!.Id);
        }


        dbUser.DateOfBirth = request.DateOfBirth;
        await userStore.UpdateAsync(dbUser, cancellationToken);

    }
}
