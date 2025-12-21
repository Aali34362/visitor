using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Visitor.Core.Db.ContextExtension;

internal class AuditInterceptor : SaveChangesInterceptor
{
    ////private readonly IDocumentStoreWrapper<AuditRecord> _documentStore;

    ////public AuditInterceptor(IDocumentStoreWrapper<AuditRecord> documentStore)
    ////{
    ////    _documentStore = documentStore;
    ////}

    ////public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    ////    DbContextEventData eventData,
    ////    InterceptionResult<int> result,
    ////    CancellationToken cancellationToken = default)
    ////{
    ////    var context = eventData.Context;
    ////    if (context is null) return result;

    ////    var entries = context.ChangeTracker.Entries()
    ////        .Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted)
    ////        .ToList();

    ////    foreach (var entry in entries)
    ////    {
    ////        if (entry.Entity is not BaseModel entity) continue;

    ////        var moduleName = entry.Entity.GetType().Namespace?.Split('.').LastOrDefault() ?? "Unknown";

    ////        var oldValues = entry.OriginalValues.Properties.ToDictionary(
    ////            p => p.Name,
    ////            p => entry.OriginalValues[p]?.ToString()
    ////        );

    ////        var auditRecord = new AuditRecord
    ////        {
    ////            EntityName = entry.Entity.GetType().Name,
    ////            EntityId = entity.Id,
    ////            Operation = entry.State.ToString(),
    ////            OldValues = oldValues,
    ////            Timestamp = DateTime.UtcNow,
    ////            Version = entity.Version
    ////        };

    ////        // Insert audit record to module-specific collection
    ////        await _documentStore.InsertOneAsync(auditRecord, moduleName);

    ////        // Increment version for the modified/deleted entity
    ////        entity.Version += 1;
    ////    }

    ////    return await base.SavingChangesAsync(eventData, result, cancellationToken);
    ////}

    //where and how to use
    ////services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
    ////services.AddSingleton(typeof(IDocumentStoreWrapper<>), typeof(DocumentStoreWrapper<>));
    ////services.AddScoped<AuditInterceptor>();
    
    ////services.AddDbContext<AppDbContext>((sp, options) =>
    ////{
    ////    var interceptor = sp.GetRequiredService<AuditInterceptor>();
    ////    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    ////    options.AddInterceptors(interceptor);
    ////});

}
