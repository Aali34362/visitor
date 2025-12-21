global using AutoMapper;
global using FluentValidation;
global using Visitor.Core.Domain.Settings;
global using Visitor.Module.IAM.Data.Context;
global using Microsoft.EntityFrameworkCore;
global using Visitor.Core.InfraServices.Validation;
global using Visitor.Module.IAM.Application.BusinessServices;
global using Visitor.Module.IAM.Application.DataLayerServices;
global using Visitor.Module.IAM.Application.DataLayerServices.Contexts;
global using Visitor.Module.IAM.Application.DataLayerServices.Factories;
global using Visitor.Module.IAM.Application.DecoratorServices.CascadeManager;
global using Visitor.Module.IAM.Application.ModelDtos;
global using Visitor.Module.IAM.Application.ResponseDtos;
global using Microsoft.Extensions.Logging;
global using System.Text.Json.Serialization;
global using Visitor.Module.IAM.Domain.Models;
global using Visitor.Module.IAM.Domain.Responses;
global using Visitor.Module.IAM.Data.ContextExtension.IAM; 
global using Visitor.Core.InfraServices;
global using Visitor.Module.IAM.Application.AppServices;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Scrutor;
global using Visitor.Core.DesignPatterns.ResultPattern;
global using Visitor.Core.Db;
global using Visitor.Core.InfraServices.Extensions;
global using Visitor.Module.IAM.Application.Mappers;




/*
public static async Task<Result> ExecuteTransactionalAsync(
    ApplicationDbContext dbContext,
    ILogger logger,
    Func<Task> operation)
{
    await using var transaction = await dbContext.BeginTransactionAsync();
    try
    {
        await operation();
        await dbContext.CommitTransactionAsync(transaction);
        return Result.Success();
    }
    catch (Exception ex)
    {
        dbContext.RollbackTransaction();
        logger.LogError(ex, ex.Message);
        return Result.Failure(ErrorDetail.Internal(ex.Message));
    }
}
 */