using Microsoft.AspNetCore.Mvc;
using PatStraPro.Entities;
using PatStraProWebAPI.Model;
using PatStraProWebAPI.Service;

namespace PatStraProWebAPI.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class EmergencyScoreController : ControllerBase
	{
		private readonly IEmergencyScoreService _emergencyScoreService;
		private readonly IPatientRecordService _patientRecordService;

		public EmergencyScoreController(IEmergencyScoreService emergencyScoreService, IPatientRecordService patientRecordService)
		{
			_emergencyScoreService = emergencyScoreService;
			_patientRecordService = patientRecordService;
		}
		
		[HttpGet("GetEHRInformation")]
		public Task<PatientRecord> GetEHRInformation([FromBody] PatientBaseInformation patientJsonInfo)
		{
			return _patientRecordService.GetPatientRecordFromHealthProvider(patientJsonInfo);
		}

		[HttpPost("GetPatientEmergencyScore")]
		public Task<IActionResult> GetPatientEmergencyScore([FromBody] AudioRequest request)
		{
			if (string.IsNullOrEmpty(request.PatientInformationAudioText))
			{
				return Task.FromResult<IActionResult>(BadRequest("Audio text cannot be null or empty"));
			}

			if (request.PatientInformationAudioText.Length < 25)
			{
				return Task.FromResult<IActionResult>(BadRequest("Audio is too short"));
			}
			//TODO Find the right method for emergency service score
			return _emergencyScoreService.GetEmergencyScoreOpenAI(request.PatientInformationAudioText)
				.ContinueWith(task =>
				{
					if (string.IsNullOrEmpty(task.Result))
					{
						return BadRequest("Data not found");
					}
					else
					{
						return (IActionResult)Ok(task.Result);
					}
				});


		}
	}
}
