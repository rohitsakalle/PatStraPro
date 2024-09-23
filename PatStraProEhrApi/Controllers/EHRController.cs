using Microsoft.AspNetCore.Mvc;
using PatStraProEhrApi.Service;
using System.Collections;
using PatStraPro.Entities;

namespace PatStraProEhrApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EHRController : ControllerBase
	{
		IEmergencyScoreService _emergencyScoreService;
		IPatientRecordService _patientRecordService;
		public EHRController(IEmergencyScoreService emergencyScoreService, IPatientRecordService patientRecordService)
		{
			_emergencyScoreService = emergencyScoreService;
			_patientRecordService = patientRecordService;
		}
		[HttpGet(Name = "GetEHRInformatio")]
		public async Task<PatientRecord> GetEHRInformation(PatientRecord patientJsonInfo)
		{
			return await _patientRecordService.GetPatientRecordFromHealthProvider(patientJsonInfo);
		}

		[HttpGet(Name = "GetPatientEmergencyScor")]
		public async Task<string> GetPatientEmergencyScore(string audioText)
		{
			if (audioText.Length < 25)
			{
				return "Audio is too short";
			}
			return await _emergencyScoreService.GetEmergencyScore(audioText);
		}
	}
}
