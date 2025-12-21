namespace Visitor.Module.DMS.Domain.Responses.Lists;

public class DocumentList : BaseResponse
{
    public string Reference_No { get; set; } = null!;
    public string Document_Nm { get; set; }
    public string Document_No { get; set; }
    public string Document_Category { get; set; }
    public string Document_Type { get; set; }    
    public string Document_Cd { get; set; }
    public string Document_Desc { get; set; }
    public short Document_Version { get; set; }    
    public string Source_System { get; set; }
    public short Filter_Year { get; set; }    
}