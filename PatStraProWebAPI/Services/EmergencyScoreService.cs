using Azure;
using Azure.AI.OpenAI;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace PatStraProWebAPI.Service
{
	public class EmergencyScoreService : IEmergencyScoreService
	{
		private const string Instruction =
							@"Extract the following information as JSON from the provided Audio text. 
							- Name: The patient's name.
							- DateOfBirth: Date of birth of patient.
							- ContactNumber: Date of birth of patient.
							- PatientId: The patient's identification number. If the patient mentions any of the following terms: 'e card number', 'patient id', 'health insurance number', 'id', 'id number', 'health card number', treat that as the PatientId.
							- Age: The patient's age.
							- Symptoms: The medical problems or symptoms the patient is experiencing.

							If either of (Patient name and date of Birth) or Patient Id is found return Error message. 'Need Patient Name and Date of birth or Patient Id to further process your request'.

							Additionally, include the following in the generated output:
							- PatientHistory: Add some random patient history, such as any past operations or existing health conditions.
							- EmergencyScore: An emergency score ranging from 1 to 10, where 10 indicates that the patient is in a serious condition. This score helps doctors in decision-making. Consider Symptoms and PatientHistory age for generating EmergencyScore.

							Example output:
							{
							    'Name': 'John Doe',
							    'PatientId': '123456',
							    'DateOfBirth': '12 December 2022',
							    'Age': 45,
							    'ContactNumber': '1234567890',
							    'Symptoms': 'chest pain, shortness of breath',
							    'PatientHistory': 'Had a heart surgery in 2015, diagnosed with diabetes',
							    'EmergencyScore': 8
								'IsChecked': false
							}";
		private readonly HttpClient _httpClient;
        //TODO: Get the API Key from Azure Key Vault
        private readonly string apiKey = "ApiKey";
		private readonly string endpoint = "YourOpenAIServiceEndPoint";

		public EmergencyScoreService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<string> GetEmergencyScoreOpenAI(string patientAudioText)
		{
			var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
			var prompt = $"{Instruction}\n\nAudio text: {patientAudioText}";

			var completionsOptions = new CompletionsOptions
			{
				DeploymentName = "rohHackathon",
				Prompts = { prompt },
				Temperature = 1.0f,
				MaxTokens = 150,
				NucleusSamplingFactor = 0.5f,
				FrequencyPenalty = 0.0f,
				PresencePenalty = 0.0f,
				GenerationSampleCount = 1,
			};

			// Get the completions response
			Response<Completions> completionsResponse = await client.GetCompletionsAsync(completionsOptions,CancellationToken.None);

			// Retrieve the completion text
			return completionsResponse.Value.Choices[0].Text;

		}


		//<inheritdoc />
		[Obsolete("Use GetEmergencyScoreOpenAI instead of this, This method tries to httpClient instead OpenAI client to make request call")]
		public async Task<string> GetEmergencyScore(string patientAudioText)
		{
			//TODO current instruction is to get the patient information from the audio text. This will be changed to get first object and then send it with Patient History.
			

			var prompt = $"{Instruction}\n\nAudio text: {patientAudioText}";

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

			_httpClient.DefaultRequestHeaders.Clear();
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

			var response = await _httpClient.PostAsync(endpoint, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
			}

			var responseString = await response.Content.ReadAsStringAsync();

			dynamic result = JsonConvert.DeserializeObject(responseString);

			return result?.choices[0]?.text ?? string.Empty;
		}
	}
}
