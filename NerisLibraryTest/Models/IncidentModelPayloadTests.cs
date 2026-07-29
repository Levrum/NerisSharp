using FluentAssertions;
using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Models.ElementModels.Incident.Modules;
using System.Reflection;

namespace NerisLibraryTest.Models;

/// <summary>
/// Covers IncidentModelPayload's copy constructor — the conversion every consumer runs
/// before a POST/PUT. The reflection tests exist to catch drift: the copy constructor is a
/// hand-written assignment list, so a property added to IncidentModelBase later would
/// otherwise be silently dropped from every write.
/// </summary>
public class IncidentModelPayloadTests
{
    private static IEnumerable<PropertyInfo> BaseProperties() =>
        typeof(IncidentModelBase).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    /// <summary>
    /// Every property declared on IncidentModelBase must be set here to a non-default value.
    /// When this stops being true, SampleIncident_PopulatesEveryBaseProperty fails first and
    /// tells you to extend this builder.
    /// </summary>
    private static IncidentModel CreateFullyPopulatedIncident() => new IncidentModel
    {
        Neris_Id = "INC1",
        Last_Modified = DateTimeOffset.Parse("2026-07-01T10:00:00Z"),
        Department = new DepartmentInfo(),
        Submitter_Account_Type = SubmitterAccountTypes.CAD,
        Base = new IncidentBase { Incident_Number = "2026-0001" },
        Incident_Types = new List<IncidentType> { new IncidentType(IncidentTypeEnum.FIRE) },
        Incident_Status = new IncidentStatus(),
        Dispatch = new DispatchModel(),
        Tactic_Timestamps = new TacticsTimestamps(),
        Unit_Responses = new List<UnitResponse> { new UnitResponse() },
        Aids = new List<IncidentAid> { new IncidentAid() },
        Fire_Detail = new FireDetail(),
        Smoke_Alarm = new SmokeAlarm(),
        Fire_Alarm = new FireAlarm(),
        Other_Alarm = new OtherAlarm(),
        Fire_Suppression = new FireSuppression(),
        Cooking_Fire_Suppression = new CookingFireSuppression(),
        Medical_Details = new List<MedicalDetail> { new MedicalDetail() },
        Special_Modifiers = new List<IncidentSpecialModifiers>
        {
            new IncidentSpecialModifiers { Type = IncidentSpecialModifierEnum.MCI }
        }
    };

    [Fact]
    public void SampleIncident_PopulatesEveryBaseProperty()
    {
        IncidentModel incident = CreateFullyPopulatedIncident();

        foreach (PropertyInfo property in BaseProperties())
        {
            object? value = property.GetValue(incident);

            value.Should().NotBeNull(
                $"CreateFullyPopulatedIncident must set {property.Name} so the copy-constructor test can verify it");

            if (property.PropertyType.IsValueType && Nullable.GetUnderlyingType(property.PropertyType) == null)
            {
                value.Should().NotBe(Activator.CreateInstance(property.PropertyType),
                    $"{property.Name} must be a non-default value or a dropped copy would go unnoticed");
            }
        }
    }

    [Fact]
    public void CopyConstructor_CopiesEveryBasePropertyFromTheSourceIncident()
    {
        IncidentModel source = CreateFullyPopulatedIncident();

        var payload = new IncidentModelPayload(source);

        foreach (PropertyInfo property in BaseProperties())
        {
            property.GetValue(payload).Should().Be(property.GetValue(source),
                $"IncidentModelPayload's copy constructor must carry over {property.Name}");
        }
    }

    [Fact]
    public void CopyConstructor_ProjectsSpecialModifiersToEnumsPreservingOrder()
    {
        var source = new IncidentModel
        {
            Special_Modifiers = new List<IncidentSpecialModifiers>
            {
                new IncidentSpecialModifiers { Type = IncidentSpecialModifierEnum.MCI },
                new IncidentSpecialModifiers { Type = IncidentSpecialModifierEnum.ACTIVE_ASSAILANT },
                new IncidentSpecialModifiers { Type = IncidentSpecialModifierEnum.WORLD_CUP_2026 }
            }
        };

        var payload = new IncidentModelPayload(source);

        payload.Special_Modifiers.Should().Equal(
            IncidentSpecialModifierEnum.MCI,
            IncidentSpecialModifierEnum.ACTIVE_ASSAILANT,
            IncidentSpecialModifierEnum.WORLD_CUP_2026);
    }

    [Fact]
    public void CopyConstructor_NullSpecialModifiers_LeavesPayloadListNull()
    {
        var payload = new IncidentModelPayload(new IncidentModel { Special_Modifiers = null });

        payload.Special_Modifiers.Should().BeNull();
    }

    [Fact]
    public void CopyConstructor_EmptySpecialModifiers_LeavesPayloadListNull()
    {
        // PINS CURRENT BEHAVIOUR: the `Count > 0` guard turns an empty list into null, so the
        // field is omitted from the payload rather than sent as []. If NERIS treats [] as
        // "clear the modifiers", this is the wrong mapping and both this test and the
        // constructor need to change together.
        var payload = new IncidentModelPayload(new IncidentModel
        {
            Special_Modifiers = new List<IncidentSpecialModifiers>()
        });

        payload.Special_Modifiers.Should().BeNull();
    }

    [Fact]
    public void ParameterlessConstructor_LeavesSpecialModifiersNull()
    {
        new IncidentModelPayload().Special_Modifiers.Should().BeNull();
    }
}
