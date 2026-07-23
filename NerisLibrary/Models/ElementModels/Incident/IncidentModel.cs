using NerisLibrary.Models.ElementModels.Incident.Modules;
using NerisLibrary.Models.ElementModels.Incident.PatchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    /// <summary>
    /// Top level class for holding incident information. Mainly contains metadata, incident type, and objects for more specific information.
    /// </summary>
    public abstract class IncidentModelBase
    {
        protected IncidentModelBase() { }
        protected IncidentModelBase(IncidentModelBase toCopy)
        {
            this.Neris_Id = toCopy.Neris_Id;
            this.Last_Modified = toCopy.Last_Modified;
            this.Department = toCopy.Department;
            this.Submitter_Account_Type = toCopy.Submitter_Account_Type;
            this.Base = toCopy.Base;
            this.Incident_Types = toCopy.Incident_Types;
            this.Incident_Status = toCopy.Incident_Status;
            this.Dispatch = toCopy.Dispatch;
            this.Tactic_Timestamps = toCopy.Tactic_Timestamps;
            this.Unit_Responses = toCopy.Unit_Responses;
            this.Aids = toCopy.Aids;
            this.Smoke_Alarm = toCopy.Smoke_Alarm;
            this.Fire_Detail = toCopy.Fire_Detail;
        }
        public string Neris_Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
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
        /// Required for structure fire types
        /// </summary>
        public SmokeAlarm Smoke_Alarm { get; set; }
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
        public IncidentModelPayload() { }
        public IncidentModelPayload(IncidentModel iModel) : base(iModel)
        {
            if (iModel.Special_Modifiers != null)
            {
                List<IncidentSpecialModifierEnum> specialMods = new List<IncidentSpecialModifierEnum>(iModel.Special_Modifiers.Select(x => x.Type));
                if (specialMods.Count > 0)
                {
                    this.Special_Modifiers = specialMods;
                }
            }
            
        }
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
