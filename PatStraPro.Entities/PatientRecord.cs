namespace PatStraPro.Entities
{
	public class PatientRecord
	{
		public PatientBaseInformation BaseInformation { get; set; }
		public PatientMedicalHistory MedicalHistory { get; set; }
		public PatientCurrentCheckupInformation CurrentCheckupInformation { get; set; }
		
		public PatientRecord()
		{
			BaseInformation = new PatientBaseInformation();
			MedicalHistory = new PatientMedicalHistory();
			CurrentCheckupInformation = new PatientCurrentCheckupInformation();
		}
	}
}
