namespace Visitor.Module.DMS.Domain.Responses.Details;

public class DocumentDetail : BaseResponse
{
    public string Reference_No { get; set; } = null!;
    public string Document_Nm { get; set; }
    public string Document_Category { get; set; }
    public string Document_Type { get; set; }    
    public string Document_Cd { get; set; }    
    public string Document_No { get; set; } 
    public string Document_Desc { get; set; }
    public short Document_Version { get; set; }    
    public string Source_System { get; set; }
    public short Filter_Year { get; set; }
    public FileAttributeDetails Content { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public string InternalStream { get; set; }
    public string InternalPath { get; set; }
    public string InternalExtension { get; set; }
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
    public string Param4 { get; set; }
    public string Param5 { get; set; }
    public DateTime Date1 { get; set; }
    public DateTime Date2 { get; set; }
    public DateTime Date3 { get; set; }
    public DateTime Date4 { get; set; }
    public DateTime Date5 { get; set; }
    public Dictionary<string, string> Tags { get; set; }
}
