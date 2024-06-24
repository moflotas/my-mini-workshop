using System.Security.Claims;
using BuildingBlocks.Application;

namespace API.Configuration.ExecutionContext;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    public Guid? UserId
    {
        get
        {
            var userIdStr = httpContextAccessor
                .HttpContext?
                .User
                .Claims
                .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdStr is null)
            {
                return null;
            }
            
            var hasUserId = Guid.TryParse(userIdStr.Value, out var userId);
            
            return hasUserId ? userId : null;
        }
    }

    public Guid CorrelationId
    {
        get
        {
            if (!IsAvailable ||
                httpContextAccessor.HttpContext!.Request.Headers.Keys.All(x =>
                    x != CorrelationMiddleware.CorrelationHeaderKey))
                throw new ApplicationException("Http context and correlation id is not available");

            var hasId = Guid.TryParse(
                httpContextAccessor.HttpContext!.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey],
                out var correlationId);

            if (hasId)
            {
                return correlationId;
            }

            throw new ApplicationException("Http context and correlation id is not available");
        }
    }

    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}