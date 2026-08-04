using FluentAssertions;
using NerisSharp;
using NerisSharp.Exceptions;
using NerisSharp.Models.ElementModels;
using NerisSharp.Models.ElementModels.Incident;
using NerisSharpTest.TestHelpers;

namespace NerisSharpTest.QueryMethods;

public class WriteProtectionTests
{
    public static IEnumerable<object[]> WriteActions()
    {
        yield return Case("PostIncident", n => n.PostIncident("ENT1", new IncidentModelPayload()));
        yield return Case("PutIncident", n => n.PutIncident("ENT1", new IncidentModelPayload { Neris_Id = "INC1" }));
        yield return Case("PatchEntity", n => n.PatchEntity(new EntityModel { Neris_Id = "ENT1" }));
        yield return Case("PostStation", n => n.PostStation("ENT1", new StationModel()));
        yield return Case("PatchStation", n => n.PatchStation("ENT1", new StationModel { Neris_Id = "ST1" }));
        yield return Case("DeleteStation", n => n.DeleteStation("ENT1", "ST1"));
        yield return Case("PostUnit", n => n.PostUnit("ENT1", "ST1", new UnitModel()));
        yield return Case("PatchUnit", n => n.PatchUnit("ENT1", "ST1", new UnitModel { Neris_Id = "U1" }));
        yield return Case("DeleteUnit", n => n.DeleteUnit("ENT1", "ST1", "U1"));
    }

    private static object[] Case(string name, Func<NerisBase, Task> action) => new object[] { name, action };

    [Theory]
    [MemberData(nameof(WriteActions))]
    public async Task WriteMethod_WhenWritesDenied_ThrowsNoAccessExceptionWithoutAnyHttpCall(
        string methodName, Func<NerisBase, Task> action)
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase(denyWriteActions: true);

        Func<Task> act = () => action(neris);

        await act.Should().ThrowAsync<NoAccessException>($"{methodName} must be blocked when writes are denied");
        fixture.Handler.Requests.Should().BeEmpty("no HTTP traffic may occur before the write gate");
    }

    [Fact]
    public async Task WriteMethod_WhenWritesAllowed_Proceeds()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"NEW1\"}");
        NerisBase neris = fixture.CreateNerisBase(denyWriteActions: false);

        string id = await neris.PostStation("ENT1", new StationModel());

        id.Should().Be("NEW1");
    }
}
