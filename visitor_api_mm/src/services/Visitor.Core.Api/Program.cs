using Visitor.Core.Domain.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddDefaultApiServices();
builder.AddApiSettings();
builder.AddApiVersioningConfigured();
builder.AddCustomSwagger();

builder.AddHealthChecksEndpoint();
builder.AddGlobalExceptionHandler();
//builder.AddSerilogConfiguration();

builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<CorrelationService>();
builder.Services.AddSingleton<BaseService>();

builder.AddAllModuleServices();


var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseCustomSwagger(apiVersionDescriptionProvider);
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseGlobalExceptionHandler();

//app.UseSerilogLogging();
app.UseDefaultApiServices();
app.UseCorrelationMiddleware();
app.MapHealthChecksEndpoint();
//app.UseHttpRequestResponseMiddleware();
app.MapControllers();
await app.RunAsync();
