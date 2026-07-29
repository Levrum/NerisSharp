namespace NerisLibrary.Models.ElementModels.Incident.Modules
{
    public class MedicalDetail
    {
        public PatientCareEvaluationEnum Patient_Care_Evaluation { get; set; }
        public PatientStatusEnum? Patient_Status { get; set; }
        public TransportDispositionEnum? Transport_Disposition { get; set; }
        public string? Patient_Care_Report_Id { get; set; }
    }

    public enum PatientCareEvaluationEnum
    {
        PATIENT_DEAD_ON_ARRIVAL,
        PATIENT_EVALUATED_CARE_PROVIDED,
        PATIENT_EVALUATED_NO_CARE_REQUIRED,
        PATIENT_EVALUATED_REFUSED_CARE,
        PATIENT_REFUSED_EVALUATION_CARE,
        PATIENT_SUPPORT_SERVICES_PROVIDED,
    }

    public enum PatientStatusEnum
    {
        IMPROVED,
        UNCHANGED,
        WORSE,
    }

    public enum TransportDispositionEnum
    {
        NONPATIENT_TRANSPORT,
        NO_TRANSPORT,
        OTHER_AGENCY_TRANSPORT,
        PATIENT_REFUSED_TRANSPORT,
        TRANSPORT_BY_EMS_UNIT,
    }
}
