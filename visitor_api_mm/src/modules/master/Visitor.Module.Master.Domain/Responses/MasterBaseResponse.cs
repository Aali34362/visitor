using Visitor.Module.Master.Domain.Models;

namespace Visitor.Module.Master.Domain.Responses;

public abstract class MasterBaseResponse : BaseResponse
{
    public string name { get; set; } = string.Empty;
    public string code { get; set; } = string.Empty;
}


//Geographical Models
public class CountryDetail : MasterBaseResponse;
public class CountryList : MasterBaseResponse;
public class StateDetail : MasterBaseResponse
{
    public Guid country_Id { get; set; }
}
public class CityDetail : MasterBaseResponse
{
    public Guid state_Id { get; set; }
}
public class AreaDetail : MasterBaseResponse
{
    public string zip_Code { get; set; } = string.Empty;
    public Guid city_Id { get; set; }
}

//Core Geo Entity
public class GeoLocationDetail : BaseResponse
{
    public double latitude { get; set; }     // 19.0708
    public double longitude { get; set; }    // 72.8781
    public Guid area_Id { get; set; }
    public string? display_Name { get; set; }  // Kurla West
}
public class GeoBoundaryDetail : BaseResponse
{
    public Guid geoLocation_Id { get; set; }
    public string boundary_Type { get; set; } = string.Empty; // POLYGON / RADIUS
    public string boundary_Data { get; set; } = string.Empty; // GeoJSON
}
public class SecurityZoneDetail : MasterBaseResponse
{
    public Guid geoLocation_Id { get; set; }
    public string zoneRisk_Level { get; set; } = string.Empty; // HIGH_RISK / NORMAL / LOW_RISK
}
public class SecurityGuardRateDetail : BaseResponse
{
    public Guid securityZone_Id { get; set; }
    public string guard_Type { get; set; } = string.Empty; // UNARMED, ARMED, SUPERVISOR
    public string shift_Type { get; set; } = string.Empty; // DAY / NIGHT / FULL_DAY
    public decimal hourly_Rate { get; set; }
    public string CurrencyCode { get; set; } = "INR";
}


//Premise & Unit Classification
public class PremiseTypeDetail : MasterBaseResponse; // SOCIETY, BUSINESS_PARK
public class BuildingTypeDetail : MasterBaseResponse; // RESIDENTIAL, COMMERCIAL
public class UnitTypeDetail : MasterBaseResponse; // FLAT, OFFICE, SHOP

//Visitor Classification
public class VisitorTypeDetail : MasterBaseResponse; // GUEST, DELIVERY, VENDOR
public class VisitPurposeDetail : MasterBaseResponse; // PERSONAL, PARCEL, MAINTENANCE
public class VehicleTypeDetail : MasterBaseResponse; // BIKE, CAR, TRUCK

//Approval & Access Modes
public class ApprovalModeDetail : MasterBaseResponse; // MANUAL, OTP, AUTO
public class PassTypeDetail : MasterBaseResponse; // SINGLE_ENTRY, MULTI_ENTRY

//Status Masters
public class EntryStatusDetail : MasterBaseResponse; // PENDING, APPROVED, REJECTED
public class PassStatusDetail : MasterBaseResponse; // ACTIVE, USED, EXPIRED
public class UserStatusDetail : MasterBaseResponse; // ACTIVE, BLOCKED


//Notification Masters
public class NotificationTypeDetail : MasterBaseResponse; // SMS, EMAIL, PUSH
public class NotificationTemplateDetail : MasterBaseResponse
{
    public Guid notificationType_Id { get; set; }
    public string template_Content { get; set; } = string.Empty;
}
public class NotificationEventDetail : MasterBaseResponse; // VISITOR_ARRIVAL, PASS_EXPIRY
public class NotificationRecipientDetail : MasterBaseResponse; // VISITOR, HOST, SECURITY
public class NotificationSettingDetail : BaseResponse
{
    public Guid notificationEvent_Id { get; set; }
    public Guid notificationType_Id { get; set; }
    public bool is_Enabled { get; set; } = true;
}

//Security Equipment Masters
public class SecurityEquipmentTypeDetail : MasterBaseResponse; // CAMERA, METAL_DETECTOR
public class SecurityEquipmentBrandDetail : MasterBaseResponse; // BRAND_A, BRAND_B
public class SecurityEquipmentModelDetail : MasterBaseResponse
{
    public Guid securityEquipmentBrand_Id { get; set; }
}

//Emergency Response Masters
public class EmergencyTypeDetail : MasterBaseResponse; // FIRE, MEDICAL, SECURITY_BREACH
public class EmergencyContactDetail : BaseResponse
{
    public Guid emergencyType_Id { get; set; }
    public string contact_Name { get; set; } = string.Empty;
    public string contact_Number { get; set; } = string.Empty;
}

//Facility & Service Masters
public class FacilityTypeDetail : MasterBaseResponse; // SWIMMING_POOL, GYM
public class ServiceTypeDetail : MasterBaseResponse; // CLEANING, MAINTENANCE
public class ServiceProviderDetail : BaseResponse
{
    public Guid serviceType_Id { get; set; }
    public string provider_Name { get; set; } = string.Empty;
    public string contact_Info { get; set; } = string.Empty;
}

//Banking & Financial Masters
public class BankDetail : MasterBaseResponse
{
    public string bank_Type { get; set; } = string.Empty; // PUBLIC, PRIVATE, FOREIGN
};
public class BankBranchDetail : MasterBaseResponse
{
    public Guid bank_Id { get; set; }
    public string IFSCCode { get; set; } = string.Empty;
}
public class CurrencyDetail : MasterBaseResponse
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
public class CurrencyRateDetail : BaseResponse
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
public class ContractTypeDetail : MasterBaseResponse; // LEASE, SERVICE_AGREEMENT
public class ContractStatusDetail : MasterBaseResponse; // ACTIVE, EXPIRED, TERMINATED

//Audit & Compliance Masters
public class AuditTypeDetail : MasterBaseResponse; // INTERNAL, EXTERNAL
public class ComplianceStandardDetail : MasterBaseResponse; // ISO_27001, GDPR

//Miscellaneous Masters
public class HolidayDetail : BaseResponse
{
    public DateOnly? holiday_Date { get; set; }
    public string? holiday_Name { get; set; }
    public int? holiday_Year { get; set; }
}
public class AgeBandDetail : MasterBaseResponse
{
    public int? Min_Age { get; set; }
    public int? Max_Age { get; set; }
}
public class CardTypeDetail : MasterBaseResponse
{
    public string Card_Type { get; set; } = null!;
}
public class ClausesDetail : MasterBaseResponse;
public class ConditionsDetail : MasterBaseResponse;
public class DepartmentDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
    public string? Category { get; set; }
    public string? Remarks { get; set; }
}
public class DisabilityType : MasterBaseResponse;
public class DocumentDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Remarks { get; set; }
}
public class DocumentCategoryDetail : BaseResponse
{
    public string Document_Category { get; set; } = null!;
    public string? Remarks { get; set; }
}
public class DocumentCheckListDetail : MasterBaseResponse
{
    public string? DocumentCheckList_Description { get; set; }
    public string? Remarks { get; set; }
}
public class DocumentOriginDetail : MasterBaseResponse;
public class DocumentSourceDetail : MasterBaseResponse;
public class DocumentTypeDetail : MasterBaseResponse;
public class FacilitiesDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
}
public class IndustryDetail : MasterBaseResponse;
public class MaritalStatusDetail : MasterBaseResponse;
public class MedicalTestDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal? MedicalTest_Cost { get; set; }
    public string? Dcn_No { get; set; }
}
public class OccupationDetail : MasterBaseResponse;
public class PackagesDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
}
public class PanelsDetail : MasterBaseResponse;
public class ReligionDetail : MasterBaseResponse;
public class SalutationDetail : MasterBaseResponse;
public class PayableTypeDetail : MasterBaseResponse
{
    public string? Payment_Mode { get; set; }
    public string? Process_Type { get; set; }
}
public class ServicesDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
}
public class SpecializationsDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
}
public class TariffsDetail : MasterBaseResponse
{
    public string Type { get; set; } = null!;
}
public class QuestionDetail : MasterBaseResponse
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
public class QuestionRuleDetail : BaseResponse
{
    public Guid QuestionId { get; set; }
    public string TriggerAnswer { get; set; } = string.Empty;
    public string? Action { get; set; }   // Allow, Hold, Reject, Escalate
    public string? HoldReason { get; set; }
    public string? Remarks { get; set; }
}

//Policy & Regulation Masters
public class PolicyTypeDetail : MasterBaseResponse; // PRIVACY_POLICY, TERMS_OF_SERVICE
public class RegulationTypeDetail : MasterBaseResponse; // DATA_PROTECTION, SAFETY_COMPLIANCE
public class PolicyDocumentDetail : BaseResponse
{
    public Guid policyType_Id { get; set; }
    public string document_Version { get; set; } = string.Empty;
    public DateOnly effective_Date { get; set; }
}
public class RegulationDocumentDetail : BaseResponse
{
    public Guid regulationType_Id { get; set; }
    public string document_Version { get; set; } = string.Empty;
    public DateOnly effective_Date { get; set; }
}

//Life and Health Insurance Masters
public class InsuranceTypeDetail : MasterBaseResponse; // LIFE, HEALTH, PROPERTY
public class InsuranceProviderDetail : MasterBaseResponse; // PROVIDER_A, PROVIDER_B
public class InsurancePolicyDetail : BaseResponse
{
    public Guid insuranceType_Id { get; set; }
    public Guid insuranceProvider_Id { get; set; }
    public string policy_Number { get; set; } = string.Empty;
    public DateOnly start_Date { get; set; }
    public DateOnly end_Date { get; set; }
}

//PPF & Retirement Masters
public class PPFPlanDetail : MasterBaseResponse; // STANDARD, SENIOR_CITIZEN
public class RetirementPlanDetail : MasterBaseResponse; // PLAN_A, PLAN_B
public class PPFAccountDetail : BaseResponse
{
    public Guid ppfPlan_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public DateOnly opening_Date { get; set; }
    public decimal initial_Deposit { get; set; }
}
public class RetirementAccountDetail : BaseResponse
{
    public Guid retirementPlan_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public DateOnly opening_Date { get; set; }
    public decimal initial_Deposit { get; set; }
}

//Taxation Masters
public class TaxTypeDetail : MasterBaseResponse; // INCOME_TAX, GST
public class TaxBracketDetail : BaseResponse
{
    public Guid taxType_Id { get; set; }
    public decimal min_Income { get; set; }
    public decimal max_Income { get; set; }
    public decimal tax_Rate { get; set; } // Percentage
}


//Investment & Savings Masters
public class InvestmentTypeDetail : MasterBaseResponse; // STOCKS, BONDS, MUTUAL_FUNDS
public class SavingsAccountTypeDetail : MasterBaseResponse; // REGULAR, HIGH_YIELD
public class InvestmentAccountDetail : BaseResponse
{
    public Guid investmentType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal initial_Investment { get; set; }
}
public class SavingsAccountDetail : BaseResponse
{
    public Guid savingsAccountType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal initial_Deposit { get; set; }
}

//Loan & Mortgage Masters
public class LoanType : MasterBaseResponse; // PERSONAL_LOAN, HOME_LOAN, AUTO_LOAN
public class MortgageType : MasterBaseResponse; // FIXED_RATE, ADJUSTABLE_RATE
public class LoanAccountDetail : BaseResponse
{
    public Guid loanType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal loan_Amount { get; set; }
    public DateOnly start_Date { get; set; }
    public DateOnly end_Date { get; set; }
}
public class MortgageAccountDetail : BaseResponse
{
    public Guid mortgageType_Id { get; set; }
    public string account_Number { get; set; } = string.Empty;
    public decimal mortgage_Amount { get; set; }
    public DateOnly start_Date { get; set; }
    public DateOnly end_Date { get; set; }
}

//Credit & Debit Card Masters
public class CardProviderDetail : MasterBaseResponse; // VISA, MASTERCARD, AMEX
public class CardCategoryDetail : MasterBaseResponse; // CREDIT, DEBIT, PREPAID