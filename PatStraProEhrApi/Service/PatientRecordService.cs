using PatStraPro.Entities;

namespace PatStraProEhrApi.Service
{
	public class PatientRecordService: IPatientRecordService
	{
		public Task<PatientRecord> GetPatientRecordFromHealthProvider(PatientRecord pr)
		{
			var patientRandomData = PatientDataGenerator.GenerateRandomPatientRequest();
			var patientRequest = new PatientRecord
			{
				BaseInformation = patientRandomData.BaseInformation,
				CurrentCheckupInformation = patientRandomData.CurrentCheckupInformation,
				MedicalHistory = patientRandomData.MedicalHistory
			};
			return Task.FromResult(patientRequest);
		}
	}
}
