namespace Visitor.Module.DMS.Domain.Models;

public class Document : BaseModel
{
    // Duplicate -> Reference_No + Document_Nm + Document_Version
    // Path -> Filter_Year + Document_Category + Document_Type + File_Nm/Document_Nm

    public string Reference_No { get; set; } = null!;
    public string Document_Nm { get; set; }
    public Guid Document_Category_Id { get; set; }
    public Guid Document_Type_Id { get; set; }   
    public string Document_Cd { get; set; }    
    public string Document_No { get; set; } // Auto-generated, unique identifier for the document
    public string Document_Desc { get; set; }
    public short Document_Version { get; set; } = 1;    
    public string Source_System { get; set; }
    public short Filter_Year { get; set; }
    public string Content { get; set; }
    public string Metadata { get; set; }
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
    public string Tags { get; set; }
}
