using PatStraPro.Entities;

namespace PatStraProEhrApi.Service
{
    public interface IPatientRecordService
    {
		/// <summary>
		/// Method provides sorted list of patients based on their emergency score. Patients with higher emergency score will be at the top of the list.
		/// </summary>
		/// <returns>list of patients</returns>
		Task<PatientRecord> GetPatientRecordFromHealthProvider(PatientRecord PatientRecord);
	}
}
