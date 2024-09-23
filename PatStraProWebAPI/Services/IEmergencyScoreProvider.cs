using PatStraPro.Entities;

namespace PatStraProWebAPI.Service
{
	public interface IEmergencyScoreService
	{
		/// <summary>
		/// Method provides sorted list of patients based on their emergency score. Patients with higher emergency score will be at the top of the list.
		/// </summary>
		/// <returns>list of patients</returns>
		Task<string> GetEmergencyScore(string patientAudioText);

		Task<string> GetEmergencyScoreOpenAI(string patientAudioText);


	}
}

