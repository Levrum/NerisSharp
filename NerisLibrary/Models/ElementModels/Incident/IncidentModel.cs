using NerisLibrary.Models.ElementModels.Incident.Modules;
using NerisLibrary.Models.ElementModels.Incident.PatchObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    /// <summary>
    /// Top level class for holding incident information. Mainly contains metadata, incident type, and objects for more specific information.
    /// </summary>
    public abstract class IncidentModelBase
    {
        public string Neris_Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public DepartmentInfo Department { get; set; }
        public SubmitterAccountTypes? Submitter_Account_Type { get; set; } = null; //cannot be submitted
        public IncidentBase Base { get; set; }
        public List<IncidentType> Incident_Types { get; set; }
        public IncidentStatus Incident_Status { get; set; }
        public DispatchModel Dispatch { get; set; }
        public TacticsTimestamps Tactic_Timestamps { get; set; }
        public List<UnitResponse> Unit_Responses { get; set; }
        public List<IncidentAid> Aids { get; set; }

        /// <summary>
        /// Fire Module only possible if at least one FIRE incident type is present.
        /// </summary>
        public FireDetail Fire_Detail { get; set; }

    }
    public class IncidentModel : IncidentModelBase
    {
        public List<IncidentSpecialModifiers> Special_Modifiers { get; set; }
    }

    public class IncidentModelPayload : IncidentModelBase
    {
        public List<IncidentSpecialModifierEnum> Special_Modifiers { get; set; }
    }

    public class DepartmentInfo
    {
        public string Time_Zone { get; set; }
    }

    public enum SubmitterAccountTypes
    {
        CAD,
        RMS,
        USER
    }
    public class IncidentPatchPayload
    {
        public IncidentPatchPayload(string nerisId, IncidentPatchProperties patchProperties)
        {
            Neris_Id = nerisId;
            Properties = patchProperties;
        }
        public string Neris_Id { get; set; }
        public ActionTypes Action { get; } = ActionTypes.patch;
        public IncidentPatchProperties Properties { get; set; }
    }
    public class IncidentPatchProperties
    {
        public BasePatchObject Tactic_Timestamps { get; set; }
        public BasePatchObject Base { get; set; }
    }


}
