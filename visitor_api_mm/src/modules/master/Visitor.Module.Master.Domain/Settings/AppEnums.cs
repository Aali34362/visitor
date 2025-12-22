namespace Visitor.Module.Master.Domain.Settings;

public enum GeoLocationType { Country, State, City, Area }
public enum ShiftType { Day, Night, FullDay }
public enum ZoneRiskLevel { Low, Normal, High }
public enum GuardType{ Unarmed, Armed, Supervisor, K9, Electronic, Patrol, Concierge}
public enum QuestionType { YesNo, Text, Number, Dropdown }
public enum RuleAction { Allow, Hold, Reject, Escalate }
public enum QuestionCategory { Visitor, Guard, Tenant }
public enum Gender { Male, Female, Other }
