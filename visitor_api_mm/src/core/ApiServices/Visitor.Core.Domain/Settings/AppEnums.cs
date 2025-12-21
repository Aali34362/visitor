namespace Visitor.Core.Domain.Settings;

public enum AppEnums
{
}

public enum SqlProvider { SqlServer, PostgreSql, MySql, Oracle }

public enum AccessLevel
{
    Read = 1,
    Write = 2
}

public enum PageLevel
{
    Root = 1,
    Child = 2,
    SubChild = 3
}

public enum PageAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    View = 4,
    Export = 5,
    Import = 6
}

public enum PageActionType
{
    Button = 1,
    Link = 2,
    Dropdown = 3,
    Modal = 4
}

public enum PageActionStatus
{
    Active = 1,
    Inactive = 2,
    Pending = 3,
    Deleted = 4
}

public enum PageActionIcon
{
    Add = 1,
    Edit = 2,
    Delete = 3,
    View = 4,
    Export = 5,
    Import = 6,
    Settings = 7,
    Search = 8,
    Refresh = 9
}

public enum PageActionPosition
{
    Top = 1,
    Bottom = 2,
    Left = 3,
    Right = 4
}

public enum PageActionVisibility
{
    Visible = 1,
    Hidden = 2,
    Disabled = 3
}

public enum PageActionPermission
{
    Allow = 1,
    Deny = 2,
    Conditional = 3
}

public enum PageActionTrigger
{
    Click = 1,
    Hover = 2,
    DoubleClick = 3,
    RightClick = 4,
    Focus = 5,
    Blur = 6
}

public enum PageActionBehavior
{
    Navigate = 1,
    Submit = 2,
    OpenModal = 3,
    ShowTooltip = 4,
    ExecuteScript = 5,
    ToggleVisibility = 6
}

public enum PageActionTarget
{
    Self = 1,
    Blank = 2,
    Parent = 3,
    Top = 4,
    Modal = 5
}

public enum PageActionConfirmation
{
    None = 0,
    Required = 1,
    Optional = 2
}

public enum PageActionFeedback
{
    None = 0,
    Success = 1,
    Error = 2,
    Warning = 3,
    Info = 4
}

public enum PageActionCondition
{
    Always = 1,
    IfLoggedIn = 2,
    IfAdmin = 3,
    IfUserHasPermission = 4,
    IfFeatureEnabled = 5
}

public enum PageActionOrder
{
    Ascending = 1,
    Descending = 2
}

public enum PageActionCategory
{
    Navigation = 1,
    DataEntry = 2,
    Reporting = 3,
    Settings = 4,
    UserManagement = 5,
    SystemManagement = 6
}

public enum PageActionTypeCategory
{
    Primary = 1,
    Secondary = 2,
    Tertiary = 3,
    Quaternary = 4
}

public enum PageActionState
{
    Enabled = 1,
    Disabled = 2,
    Archived = 3,
    Draft = 4,
    Published = 5
}

public enum PageActionScope
{
    Global = 1,
    Module = 2,
    Page = 3,
    User = 4,
    Role = 5
}

public enum PageActionPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum PageActionConflictResolution
{
    Ignore = 1,
    Overwrite = 2,
    Merge = 3,
    Prompt = 4
}

public enum PageActionAudit
{
    None = 0,
    Log = 1,
    Notify = 2,
    Alert = 3,
    Track = 4
}

public enum PageActionNotification
{
    None = 0,
    Email = 1,
    SMS = 2,
    PushNotification = 3,
    InAppNotification = 4
}


public enum PageActionLogging
{
    None = 0,
    Basic = 1,
    Detailed = 2,
    Full = 3
}

public enum PageActionAnalytics
{
    None = 0,
    Basic = 1,
    Advanced = 2,
    Full = 3
}

public enum PageActionIntegration
{
    None = 0,
    API = 1,
    Webhook = 2,
    ThirdPartyService = 3,
    CustomScript = 4
}

public enum PageActionLocalization
{
    None = 0,
    Enabled = 1,
    Required = 2
}

public enum PageActionSecurity
{
    None = 0,
    Basic = 1,
    Advanced = 2,
    Full = 3
}

public enum PageActionCompliance
{
    None = 0,
    GDPR = 1,
    HIPAA = 2,
    PCI_DSS = 3,
    CCPA = 4
}

public enum PageActionCustomization
{
    None = 0,
    Basic = 1,
    Advanced = 2,
    Full = 3
}

public enum PageActionDependency
{
    None = 0,
    Optional = 1,
    Required = 2,
    Conditional = 3
}

public enum PageActionExecution
{
    Synchronous = 1,
    Asynchronous = 2,
    Deferred = 3,
    Immediate = 4
}

public enum PageActionTimeout
{
    None = 0,
    Short = 1, // e.g., 5 seconds
    Medium = 2, // e.g., 30 seconds
    Long = 3 // e.g., 60 seconds
}

public enum PageActionRetry
{
    None = 0,
    Single = 1,
    Multiple = 2,
    Infinite = 3
}

public enum PageActionFallback
{
    None = 0,
    Default = 1,
    Custom = 2,
    ErrorPage = 3
}

public enum PageActionThrottling
{
    None = 0,
    Low = 1, // e.g., 1 request per second
    Medium = 2, // e.g., 5 requests per second
    High = 3 // e.g., 10 requests per second
}

public enum PageActionRateLimiting
{
    None = 0,
    Low = 1, // e.g., 100 requests per hour
    Medium = 2, // e.g., 500 requests per hour
    High = 3 // e.g., 1000 requests per hour
}

public enum PageActionCaching
{
    None = 0,
    ShortTerm = 1, // e.g., 5 minutes
    MediumTerm = 2, // e.g., 30 minutes
    LongTerm = 3 // e.g., 1 hour
}

public enum PageActionDependencyType
{
    None = 0,
    Direct = 1,
    Indirect = 2,
    Conditional = 3
}

public enum PageActionExecutionMode
{
    Immediate = 1,
    Scheduled = 2,
    OnDemand = 3,
    Background = 4
}

public enum PageActionExecutionContext
{
    User = 1,
    System = 2,
    ScheduledTask = 3,
    BackgroundService = 4
}

public enum PageActionExecutionResult
{
    Success = 1,
    Failure = 2,
    PartialSuccess = 3,
    Skipped = 4,
    NotExecuted = 5
}

public enum PageActionExecutionStatus
{
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5
}

public enum PageActionExecutionPriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Critical = 4
}

public enum PageActionExecutionModeType
{
    Immediate = 1,
    Scheduled = 2,
    OnDemand = 3,
    Background = 4
}

public enum PageActionExecutionContextType
{
    User = 1,
    System = 2,
    ScheduledTask = 3,
    BackgroundService = 4
}
public enum PageActionExecutionResultType
{
    Success = 1,
    Failure = 2,
    PartialSuccess = 3,
    Skipped = 4,
    NotExecuted = 5
}

public enum PageActionExecutionStatusType
{
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5
}

public enum PageActionExecutionPriorityType
{
    Low = 1,
    Normal = 2,
    High = 3,
    Critical = 4
}

public enum PageActionExecutionEnvironment
{
    Development = 1,
    Testing = 2,
    Staging = 3,
    Production = 4
}

public enum PageActionExecutionModeCategory
{
    Immediate = 1,
    Scheduled = 2,
    OnDemand = 3,
    Background = 4
}

public enum PageActionExecutionContextCategory
{
    User = 1,
    System = 2,
    ScheduledTask = 3,
    BackgroundService = 4
}

public enum PageActionExecutionResultCategory
{
    Success = 1,
    Failure = 2,
    PartialSuccess = 3,
    Skipped = 4,
    NotExecuted = 5
}

public enum PageActionExecutionStatusCategory
{
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5
}

public enum PageActionExecutionPriorityCategory
{
    Low = 1,
    Normal = 2,
    High = 3,
    Critical = 4
}

public enum PageActionExecutionEnvironmentCategory
{
    Development = 1,
    Testing = 2,
    Staging = 3,
    Production = 4
}