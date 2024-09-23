using PatStraPro.Common;
using PatStraPro.Entities;

namespace PatStraProWebAPI.Service
{
	public class PatientRecordService: IPatientRecordService
	{
		public Task<PatientRecord> GetPatientRecordFromHealthProvider(PatientBaseInformation pr)
		{
			var patientRandomData = PatientDataGenerator.GenerateRandomAudioText();
			return null;
		}
	}
}
