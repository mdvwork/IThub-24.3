namespace IncidentDesk;

public enum Role
{
    Reporter,
    Responder,
    Coordinator,
    Auditor
}

public enum Severity
{
    Normal,
    Critical
}

public enum IncidentStatus
{
    New,
    InProgress,
    Closed
}

public sealed record User(string Id, string TeamId, Role Role);

public sealed record Incident(
    int Id,
    string Title,
    string TeamId,
    string ReporterId,
    string? AssignedResponderId,
    Severity Severity,
    IncidentStatus Status);

public sealed class AccessDeniedException : Exception
{
    public AccessDeniedException() : base("The user is not allowed to perform this action.") { }
}

public sealed class InvalidIncidentStateException : Exception
{
    public InvalidIncidentStateException() : base("The incident is not in the required state.") { }
}
