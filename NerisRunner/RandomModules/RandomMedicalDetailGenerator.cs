using NerisLibrary.Models.ElementModels.Incident.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerisRunner.RandomModules
{
    internal static class RandomMedicalDetailGenerator
    {
        public static MedicalDetail Generate() 
        {
            MedicalDetail medDetail = new MedicalDetail();
            medDetail.Patient_Care_Evaluation = RandomModulesUtils.GetRandomEnum<PatientCareEvaluationEnum>();
            if (RandomModulesUtils.TryGetRandomEnumOrNull<PatientStatusEnum>(out PatientStatusEnum status))
            {
                medDetail.Patient_Status = status;
            }

            if (RandomModulesUtils.TryGetRandomEnumOrNull<TransportDispositionEnum>(out TransportDispositionEnum td))
            {
                medDetail.Transport_Disposition = td;
            }

            medDetail.Patient_Care_Report_Id = RandomModulesUtils.GetRandomGuidStringOrNull();
            return medDetail;
        }
    }
}
