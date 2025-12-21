using Microsoft.EntityFrameworkCore;

namespace Visitor.Module.DMS.Infrastructure.Repositories;

internal class DmsRepository : IDmsRepository
{
    public Task CreateDocumentAsync(Document dto)
    {
        throw new NotImplementedException();
    }

    public Task CreateDocumentCategoryAsync(DocumentCategory dto)
    {
        throw new NotImplementedException();
    }

    public Task CreateDocumentTypeAsync(DocumentType dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDocumentAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDocumentCategoryAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDocumentTypeAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DocumentDetail> GetDocumentByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DocumentCategoryDetail> GetDocumentCategoryByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DocumentCategory> GetDocumentCategoryByNameAsync(string category_nm)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<DocumentCategoryList>> GetDocumentCategoryListAsync(DocumentCategory dto, int index, int size)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<DocumentList>> GetDocumentListAsync(Document dto, int index, int size)
    {
        throw new NotImplementedException();
    }

    public Task<DocumentTypeDetail> GetDocumentTypeByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DocumentType> GetDocumentTypeByNameAsync(string type_Nm)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<DocumentTypeList>> GetDocumentTypeListAsync(DocumentType dto, int index, int size)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDocumentAsync(Document dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDocumentCategoryAsync(DocumentCategory dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDocumentTypeAsync(DocumentType dto)
    {
        throw new NotImplementedException();
    }
}

////internal class DmsRepository : IDmsRepository
////{


////    #region Document Category
////    public async Task<PaginatedList<DocumentCategoryList>> GetDocumentCategoryListAsync(DocumentCategory dto, int index, int size)
////    {
////        return await _dbContext.GetDocumentCategoryListAsync(dto, index, size);
////    }

////    public async Task<DocumentCategoryDetail> GetDocumentCategoryByIdAsync(Guid id)
////    {
////        return await _dbContext.GetDocumentCategoryByIdAsync(id);
////    }

////    public async Task<DocumentCategory> GetDocumentCategoryByNameAsync(string category_nm)
////    {
////        return await _dbContext.GetDocumentCategoryByNameAsync(category_nm);
////    }

////    public async Task CreateDocumentCategoryAsync(DocumentCategory dto)
////    {

////        await using var transaction = await _dbContext.BeginTransactionAsync();
////        _dbContext.CreateDocumentCategory(dto);
////        await _dbContext.CommitTransactionAsync(transaction!);
////    }

////    public async Task UpdateDocumentCategoryAsync(DocumentCategory dto)
////    {
////        await using var transaction = await _dbContext.BeginTransactionAsync();
////        _dbContext.UpdateDocumentCategory(dto);
////        await _dbContext.CommitTransactionAsync(transaction!);
////    }

////    public async Task DeleteDocumentCategoryAsync(Guid id)
////    {

////    }
////    #endregion

////    #region Document Type
////    Task<PaginatedList<DocumentTypeList>> GetDocumentTypeListAsync(DocumentType dto, int index, int size);

////    Task<DocumentTypeDetail> GetDocumentTypeByIdAsync(Guid id);

////    Task<DocumentType> GetDocumentTypeByNameAsync(string type_Nm);

////    Task CreateDocumentTypeAsync(DocumentType dto);

////    Task UpdateDocumentTypeAsync(DocumentType dto);

////    Task DeleteDocumentTypeAsync(Guid id);
////    #endregion

////    #region Document
////    Task<PaginatedList<DocumentList>> GetDocumentListAsync(Document dto, int index, int size);

////    Task<DocumentDetail> GetDocumentByIdAsync(Guid id);

////    Task CreateDocumentAsync(Document dto);

////    Task UpdateDocumentAsync(Document dto);

////    Task DeleteDocumentAsync(Guid id);
////    #endregion

////}
