using System.Data;
using Microsoft.EntityFrameworkCore;
using RobotCleaner.Api.Features.Clean;

namespace RobotCleaner.Api;

public static class EfMigration
{
    public static void UseCustomEfMigration(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        var context = serviceScope?.ServiceProvider.GetRequiredService<CleanContext>();

        context?.Database.BeginTransaction(IsolationLevel.Serializable);
        context?.Database.Migrate();
        context?.Database.CommitTransaction();
    }
}