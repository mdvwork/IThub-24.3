using IncidentDesk;

namespace IncidentDesk.Tests;

public class ExistingTests
{
    private static readonly User Reporter = new("reporter-a", "alpha", Role.Reporter);
    private static readonly User Responder = new("responder-a", "alpha", Role.Responder);
    private static readonly User Coordinator = new("coordinator-a", "alpha", Role.Coordinator);
    private static readonly User Auditor = new("auditor", "audit", Role.Auditor);

    [Fact]
    public void Reporter_creates_a_new_incident_and_reads_it()
    {
        var desk = new IncidentDeskService();

        var incident = desk.Create(Reporter, "Payment outage", Severity.Critical);

        Assert.Equal(IncidentStatus.New, incident.Status);
        Assert.Equal("alpha", incident.TeamId);
        Assert.Equal(Reporter.Id, incident.ReporterId);
        Assert.Equal(incident, desk.Get(Reporter, incident.Id));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Blank_title_is_rejected(string title)
    {
        var desk = new IncidentDeskService();

        Assert.Throws<ArgumentException>(() => desk.Create(Reporter, title, Severity.Normal));
    }

    [Fact]
    public void Reporter_finds_their_incident_in_the_list()
    {
        var desk = new IncidentDeskService();
        var incident = desk.Create(Reporter, "Login broken", Severity.Normal);

        Assert.Contains(desk.List(Reporter), item => item.Id == incident.Id);
    }

    [Fact]
    public void Coordinator_assigns_and_the_assigned_responder_starts_work()
    {
        var desk = new IncidentDeskService();
        var incident = desk.Create(Reporter, "Queue delay", Severity.Critical);

        var assigned = desk.Assign(Coordinator, incident.Id, Responder);
        var started = desk.Start(Responder, incident.Id);

        Assert.Equal(Responder.Id, assigned.AssignedResponderId);
        Assert.Equal(IncidentStatus.InProgress, started.Status);
    }

    [Fact]
    public void Coordinator_closes_an_incident_after_work_has_started()
    {
        var desk = new IncidentDeskService();
        var incident = desk.Create(Reporter, "Search slow", Severity.Normal);
        desk.Assign(Coordinator, incident.Id, Responder);
        desk.Start(Responder, incident.Id);

        var closed = desk.Close(Coordinator, incident.Id);

        Assert.Equal(IncidentStatus.Closed, closed.Status);
    }

    [Fact]
    public void Auditor_reads_an_incident_from_another_team()
    {
        var desk = new IncidentDeskService();
        var incident = desk.Create(Reporter, "Sensitive incident", Severity.Critical);

        Assert.Equal(incident.Id, desk.Get(Auditor, incident.Id).Id);
    }
}
