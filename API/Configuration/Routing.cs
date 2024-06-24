using API.Configuration.ExecutionContext;
using API.Configuration.Validation;
using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Hellang.Middleware.ProblemDetails;

namespace API.Configuration;

public static class Routing
{
    public static void InitRouting(this IServiceCollection s)
    {
        s.AddControllers();

        s.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        s.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

        s.AddProblemDetails(x =>
        {
            x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
            x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
        });
    }

    public static void InitRouting(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationMiddleware>();

        if (Startup.Env.IsDevelopment())
        {
            app.UseProblemDetails();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}