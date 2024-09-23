using PatStraPro.Entities;

namespace PatStraProEhrApi
{
	public class PatientRequest
	{
		public PatientBaseInformation BaseInformation { get; set; }
		public PatientMedicalHistory MedicalHistory { get; set; }
		public PatientCurrentCheckupInformation CurrentCheckupInformation { get; set; }

		public PatientRequest()
		{
			BaseInformation = new PatientBaseInformation();
			MedicalHistory = new PatientMedicalHistory();
			CurrentCheckupInformation = new PatientCurrentCheckupInformation();
		}
	}
}
