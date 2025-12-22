using System.Reflection;

namespace Visitor.Module.Master.Domain.Models;

public abstract class MasterBaseModel : BaseModel
{
    public string name { get; set; } = string.Empty;
    public string code { get; set; } = string.Empty;
}

//Geographical Models
public class Country : MasterBaseModel;
public class State : MasterBaseModel
{
    public Guid country_Id { get; set; }
}
public class City : MasterBaseModel
{
    public Guid state_Id { get; set; }
}
public class Area : MasterBaseModel
{
    public string zip_Code { get; set; } = string.Empty;
    public Guid city_Id { get; set; }
}

//Core Geo Entity
public class GeoLocation : BaseModel
{
    public double latitude { get; set; }     // 19.0708
    public double longitude { get; set; }    // 72.8781
    public Guid area_Id { get; set; }
    public string? display_Name { get; set; }  // Kurla West
}
public class GeoBoundary : BaseModel
{
    public Guid geoLocation_Id { get; set; }
    public string boundary_Type { get; set; } = string.Empty; // POLYGON / RADIUS
    public string boundary_Data { get; set; } = string.Empty; // GeoJSON
}
public class SecurityZone : MasterBaseModel
{
    public Guid geoLocation_Id { get; set; }
    public string zoneRisk_Level { get; set; } = string.Empty; // HIGH_RISK / NORMAL / LOW_RISK
}
public class SecurityGuardRate : BaseModel
{
    public Guid securityZone_Id { get; set; }
    public string guard_Type { get; set; } = string.Empty; // UNARMED, ARMED, SUPERVISOR
    public string shift_Type { get; set; } = string.Empty; // DAY / NIGHT / FULL_DAY
    public decimal hourly_Rate { get; set; }
    public string CurrencyCode { get; set; } = "INR";   
}


//Premise & Unit Classification
public class PremiseType : MasterBaseModel; // SOCIETY, BUSINESS_PARK
public class BuildingType : MasterBaseModel; // RESIDENTIAL, COMMERCIAL
public class UnitType : MasterBaseModel; // FLAT, OFFICE, SHOP

//Visitor Classification
public class VisitorType : MasterBaseModel; // GUEST, DELIVERY, VENDOR
public class VisitPurpose : MasterBaseModel; // PERSONAL, PARCEL, MAINTENANCE
public class VehicleType : MasterBaseModel; // BIKE, CAR, TRUCK

//Approval & Access Modes
public class ApprovalMode : MasterBaseModel; // MANUAL, OTP, AUTO
public class PassType : MasterBaseModel; // SINGLE_ENTRY, MULTI_ENTRY

//Status Masters
public class EntryStatus : MasterBaseModel; // PENDING, APPROVED, REJECTED
public class PassStatus : MasterBaseModel; // ACTIVE, USED, EXPIRED
public class UserStatus : MasterBaseModel; // ACTIVE, BLOCKED


//Notification Masters
public class NotificationType : MasterBaseModel; // SMS, EMAIL, PUSH
public class NotificationTemplate : MasterBaseModel
{
    public Guid notificationType_Id { get; set; }
    public string template_Content { get; set; } = string.Empty;
}
public class NotificationEvent : MasterBaseModel; // VISITOR_ARRIVAL, PASS_EXPIRY
public class NotificationRecipient : MasterBaseModel; // VISITOR, HOST, SECURITY
public class NotificationSetting : BaseModel
{
    public Guid notificationEvent_Id { get; set; }
    public Guid notificationType_Id { get; set; }
    public bool is_Enabled { get; set; } = true;
}

//Security Equipment Masters
public class SecurityEquipmentType : MasterBaseModel; // CAMERA, METAL_DETECTOR
public class SecurityEquipmentBrand : MasterBaseModel; // BRAND_A, BRAND_B
public class SecurityEquipmentModel : MasterBaseModel
{
    public Guid securityEquipmentBrand_Id { get; set; }
}

//Emergency Response Masters
public class EmergencyType : MasterBaseModel; // FIRE, MEDICAL, SECURITY_BREACH
public class EmergencyContact : BaseModel
{
    public Guid emergencyType_Id { get; set; }
    public string contact_Name { get; set; } = string.Empty;
    public string contact_Number { get; set; } = string.Empty;
}

//Facility & Service Masters
public class FacilityType : MasterBaseModel; // SWIMMING_POOL, GYM
public class ServiceType : MasterBaseModel; // CLEANING, MAINTENANCE
public class ServiceProvider : BaseModel
{
    public Guid serviceType_Id { get; set; }
    public string provider_Name { get; set; } = string.Empty;
    public string contact_Info { get; set; } = string.Empty;
}

//Banking & Financial Masters
public class Bank : MasterBaseModel
{
    public string bank_Type { get; set; } = string.Empty; // PUBLIC, PRIVATE, FOREIGN
};
public class BankBranch : MasterBaseModel
{
    public Guid bank_Id { get; set; }
    public string IFSCCode { get; set; } = string.Empty;
}
public class Currency : MasterBaseModel
{
    public string Country_Cd { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public decimal? Exchange_Rate { get; set; } = null!;
    public decimal? Selling_Rate { get; set; } = null!;
    public decimal? Buying_Rate { get; set; } = null!;
    public string? Base_Currency_Cd { get; set; }
    public string? Remarks { get; set; }
    public string? Currency_Stability { get; set; }
}
public class CurrencyRate : BaseModel
{
    public string Currency_Cd { get; set; } = null!;
    public decimal? Exchange_Rate { get; set; }
    public decimal? Selling_Rate { get; set; }
    public decimal? Buying_Rate { get; set; }
    public DateOnly Transaction_Dt { get; set; }
    public DateOnly Effective_From { get; set; }
    public DateOnly Effective_Upto { get; set; }

}

//Contract & Agreement Masters
public class ContractType : MasterBaseModel; // LEASE, SERVICE_AGREEMENT
public class ContractStatus : MasterBaseModel; // ACTIVE, EXPIRED, TERMINATED

//Audit & Compliance Masters
public class AuditType : MasterBaseModel; // INTERNAL, EXTERNAL
public class ComplianceStandard : MasterBaseModel; // ISO_27001, GDPR

//Miscellaneous Masters
public class Holiday : BaseModel
{
    public DateOnly? holiday_Date { get; set; }
    public string? holiday_Name { get; set; }
    public int? holiday_Year { get; set; }
}
public partial class AgeBand : MasterBaseModel
{
    public int? Min_Age { get; set; }
    public int? Max_Age { get; set; }
}
public class CardType : MasterBaseModel
{
    public string Card_Type { get; set; } = null!;
}
public class Clauses : MasterBaseModel;
public class Conditions : MasterBaseModel;
public class Department : MasterBaseModel
{
    public string Type { get; set; } = null!;
    public string? Category { get; set; }
    public string? Remarks { get; set; }
}
public class DisabilityType : MasterBaseModel;
public class Document : MasterBaseModel
{
    public string Type { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Remarks { get; set; }
}
public class DocumentCategory : BaseModel
{
    public string Document_Category { get; set; } = null!;
    public string? Remarks { get; set; }
}
public class DocumentCheckList : MasterBaseModel
{
    public string? DocumentCheckList_Description { get; set; }
    public string? Remarks { get; set; }
}
public class DocumentOrigin : MasterBaseModel;
public class DocumentSource : MasterBaseModel;
public class DocumentType : MasterBaseModel;
public class Facilities : MasterBaseModel
{
    public string Type { get; set; } = null!;
}
public class Industry : MasterBaseModel;
public class MaritalStatus : MasterBaseModel;
public class MedicalTest : MasterBaseModel
{
    public string Type { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal? MedicalTest_Cost { get; set; }
    public string? Dcn_No { get; set; }
}
public class Occupation : MasterBaseModel;
public class Packages : MasterBaseModel
{
    public string Type { get; set; } = null!;
}
public class Panels : MasterBaseModel;
public class Religion : MasterBaseModel;
public class Salutation : MasterBaseModel;
public class PayableType : MasterBaseModel
{
    public string? Payment_Mode { get; set; }
    public string? Process_Type { get; set; }
}
public class Services : MasterBaseModel
{
    public string Type { get; set; } = null!;
}
public class Specializations : MasterBaseModel
{
    public string Type { get; set; } = null!;
}
public class Tariffs : MasterBaseModel
{
    public string Type { get; set; } = null!;
}
public class Question : MasterBaseModel
{
    public string QuestionText { get; set; } = string.Empty;
    public string? Type { get; set; }       // YesNo, Text, Number, Dropdown
    public string? Category { get; set; } // Visitor, Guard, Tenant
    public string Section { get; set; } = string.Empty;
    public Guid? ParentQuestionId { get; set; }
    public int DisplayOrder { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public string? ApplicableGender { get; set; }
    public string? AllowedValues { get; set; } // JSON or CSV
}
public class QuestionRule : BaseModel
{
    public Guid QuestionId { get; set; }
    public string TriggerAnswer { get; set; } = string.Empty;
    public string? Action { get; set; }   // Allow, Hold, Reject, Escalate
    public string? HoldReason { get; set; }
    public string? Remarks { get; set; }
}

//Policy & Regulation Masters
public class PolicyType : MasterBaseModel; // PRIVACY_POLICY, TERMS_OF_SERVICE
public class RegulationType : MasterBaseModel; // DATA_PROTECTION, SAFETY_COMPLIANCE
public class PolicyDocument : BaseModel
{
    public Guid policyType_Id { get; set; }
    public string document_Version { get; set; } = string.Empty;
    public DateOnly effective_Date { get; set; }
}
public class RegulationDocument : BaseModel
{
    public Guid regulationType_Id { get; set; }
    public string document_Version { get; set; } = string.Empty;
    public DateOnly effective_Date { get; set; }
}

//Life and Health Insurance Masters
public class InsuranceType : MasterBaseModel; // LIFE, HEALTH, PROPERTY
public class InsuranceProvider : MasterBaseModel; // PROVIDER_A, PROVIDER_B
public class InsurancePolicy : BaseModel
{
    public Guid insuranceType_Id { get; set; }
    public Guid insuranceProvider_Id { get; set; }
    public string policy_Number { get; set; } = string.Empty;
    public DateOnly start_Date { get; set; }
    public DateOnly end_Date { get; set; }
}

//PPF & Retirement Masters
public class PPFPlan : MasterBaseModel; // STANDARD, SENIOR_CITIZEN
public class RetirementPlan : MasterBaseModel; // PLAN_A, PLAN_B
public class PPFAccount : BaseModel
{
    public Guid ppfPlan_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public DateOnly opening_Date { get; set; }
    public decimal initial_Deposit { get; set; }
}
public class RetirementAccount : BaseModel
{
    public Guid retirementPlan_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public DateOnly opening_Date { get; set; }
    public decimal initial_Deposit { get; set; }
}

//Taxation Masters
public class TaxType : MasterBaseModel; // INCOME_TAX, GST
public class TaxBracket : BaseModel
{
    public Guid taxType_Id { get; set; }
    public decimal min_Income { get; set; }
    public decimal max_Income { get; set; }
    public decimal tax_Rate { get; set; } // Percentage
}


//Investment & Savings Masters
public class InvestmentType : MasterBaseModel; // STOCKS, BONDS, MUTUAL_FUNDS
public class SavingsAccountType : MasterBaseModel; // REGULAR, HIGH_YIELD
public class InvestmentAccount : BaseModel
{
    public Guid investmentType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal initial_Investment { get; set; }
}
public class SavingsAccount : BaseModel
{
    public Guid savingsAccountType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal initial_Deposit { get; set; }
}

//Loan & Mortgage Masters
public class LoanType : MasterBaseModel; // PERSONAL_LOAN, HOME_LOAN, AUTO_LOAN
public class MortgageType : MasterBaseModel; // FIXED_RATE, ADJUSTABLE_RATE
public class LoanAccount : BaseModel
{
    public Guid loanType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal loan_Amount { get; set; }
    public DateOnly start_Date { get; set; }
    public DateOnly end_Date { get; set; }
}
public class MortgageAccount : BaseModel
{
    public Guid mortgageType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal mortgage_Amount { get; set; }
    public DateOnly start_Date { get; set; }
    public DateOnly end_Date { get; set; }
}

//Credit & Debit Card Masters
public class CardProvider : MasterBaseModel; // VISA, MASTERCARD, AMEX
public class CardCategory : MasterBaseModel; // CREDIT, DEBIT, PREPAID










