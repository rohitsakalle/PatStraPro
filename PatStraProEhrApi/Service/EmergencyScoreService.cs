using Newtonsoft.Json;
using System.Text;

namespace PatStraProEhrApi.Service
{
	public class EmergencyScoreService : IEmergencyScoreService
	{
		private readonly HttpClient _httpClient;
		private readonly string apiKey = "a082e6dd02b3420d9b80a9641ed8656a";
		//private readonly string endpoint = "https://api.openai.com/v1/completions";
		private readonly string endpoint = "https://openairohhackthon.openai.azure.com/";
		

		public EmergencyScoreService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		//<inheritdoc />
		public async Task<string> GetEmergencyScore(string audioText)
		{
			var instruction = @"Extract the following information as JSON from the Audio text: Name, Age, Symptoms or medical problems person is facing. If patient says any of the words e card number, patient id, health insurance number, id treat that as PatientId. In generated output add some random patient history for example any operation in past or any existing health condition.";

			var prompt = $"{instruction}\n\nAudio text: {audioText}";

			var requestBody = new
			{
				prompt = prompt,
				max_tokens = 150,
				temperature = 0.7,
				top_p = 1.0,
				n = 1,
				stop = new string[] { "\n" }
			};

			var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

			_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

			var response = await _httpClient.PostAsync(endpoint, content);
			
			var responseString = await response.Content.ReadAsStringAsync();

			dynamic result = JsonConvert.DeserializeObject(responseString);

			return result.choices.text;
		}


	}
}