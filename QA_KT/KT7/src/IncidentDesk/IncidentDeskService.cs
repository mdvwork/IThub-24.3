namespace IncidentDesk;

public sealed class IncidentDeskService
{
    private readonly Dictionary<int, Incident> _incidents = new();
    private int _nextId = 1;

    public Incident Create(User actor, string title, Severity severity)
    {
        ArgumentNullException.ThrowIfNull(actor);

        if (actor.Role != Role.Reporter)
            throw new AccessDeniedException();

        ValidateTitle(title);

        var incident = new Incident(
            _nextId++, title.Trim(), actor.TeamId, actor.Id,
            null, severity, IncidentStatus.New);

        _incidents.Add(incident.Id, incident);
        return incident;
    }

    public Incident Get(User actor, int id)
    {
        ArgumentNullException.ThrowIfNull(actor);
        var incident = Find(id);

        if (!CanRead(actor, incident))
            throw new AccessDeniedException();

        return incident;
    }

    public IReadOnlyList<Incident> List(User actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        return _incidents.Values
            .Where(incident => actor.Role switch
            {
                Role.Auditor => true,
                Role.Reporter => incident.TeamId == actor.TeamId,
                Role.Responder or Role.Coordinator => incident.TeamId == actor.TeamId,
                _ => false
            })
            .OrderBy(incident => incident.Id)
            .ToArray();
    }

    public Incident Rename(User actor, int id, string newTitle)
    {
        ArgumentNullException.ThrowIfNull(actor);
        var incident = Find(id);

        if (actor.Role != Role.Reporter || incident.ReporterId != actor.Id)
            throw new AccessDeniedException();

        if (incident.Status != IncidentStatus.New)
            throw new InvalidIncidentStateException();

        ValidateTitle(newTitle);
        return Save(incident with { Title = newTitle.Trim() });
    }

    public Incident Assign(User actor, int id, User responder)
    {
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(responder);
        var incident = Find(id);

        if (actor.Role != Role.Coordinator || actor.TeamId != incident.TeamId ||
            responder.Role != Role.Responder || responder.TeamId != incident.TeamId)
            throw new AccessDeniedException();

        if (incident.Status != IncidentStatus.New)
            throw new InvalidIncidentStateException();

        return Save(incident with { AssignedResponderId = responder.Id });
    }

    public Incident Start(User actor, int id)
    {
        ArgumentNullException.ThrowIfNull(actor);
        var incident = Find(id);

        if (actor.Role != Role.Responder || actor.TeamId != incident.TeamId ||
            incident.AssignedResponderId != actor.Id)
            throw new AccessDeniedException();

        if (incident.Status != IncidentStatus.New)
            throw new InvalidIncidentStateException();

        return Save(incident with { Status = IncidentStatus.InProgress });
    }

    public Incident Close(User actor, int id)
    {
        ArgumentNullException.ThrowIfNull(actor);
        var incident = Find(id);

        if (actor.Role != Role.Coordinator || actor.TeamId != incident.TeamId)
            throw new AccessDeniedException();

        if (incident.Status != IncidentStatus.InProgress)
            throw new InvalidIncidentStateException();

        return Save(incident with { Status = IncidentStatus.Closed });
    }

    private Incident Find(int id) => _incidents.TryGetValue(id, out var incident)
        ? incident
        : throw new KeyNotFoundException($"Incident {id} does not exist.");

    private Incident Save(Incident incident)
    {
        _incidents[incident.Id] = incident;
        return incident;
    }

    private static bool CanRead(User actor, Incident incident) => actor.Role switch
    {
        Role.Auditor => true,
        Role.Reporter => incident.ReporterId == actor.Id,
        Role.Responder or Role.Coordinator => incident.TeamId == actor.TeamId,
        _ => false
    };

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Trim().Length > 80)
            throw new ArgumentException("Title must contain 1 to 80 characters.", nameof(title));
    }
}
