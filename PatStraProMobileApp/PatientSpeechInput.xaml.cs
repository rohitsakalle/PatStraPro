using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using PatStraPro.Db;
using PatStraPro.Entities;


namespace PatStraProMobileApp;

public partial class PatientSpeechInput : ContentPage
{
	
	public PatientSpeechInput()
	{
		InitializeComponent();
	}
	
	private async void OnStartSpeakingClicked(object sender, EventArgs e)
	{
		var status = await Permissions.RequestAsync<Permissions.Microphone>();
		if (status != PermissionStatus.Granted)
		{
			await DisplayAlert("Permission Denied", "Microphone permission is required for speech recognition.", "OK");
			return;
		}
		//38e7bea168bf46379fc8a0ef75431ba6
		var speechConfig = SpeechConfig.FromSubscription("e770dd75e05e42619aac3f7c3f1699d1", "eastus");
		speechConfig.SpeechRecognitionLanguage = "en-US";
		using var recognizer = new SpeechRecognizer(speechConfig);
		recognizer.Canceled += (s, e) =>
		{
			Console.WriteLine($"CANCELED: Reason={e.Reason}");

			if (e.Reason == CancellationReason.Error)
			{
				Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
			}
		};
		recognizer.SessionStarted += (s, e) => Console.WriteLine("Session started.");
		recognizer.SessionStopped += (s, e) => Console.WriteLine("Session stopped.");
		recognizer.Recognizing += (s, e) => Console.WriteLine($"Recognizing: {e.Result.Text}");
        await Task.Delay(2000);
        TranscriptionLabel.Text = "Start speaking...";
        var result = await recognizer.RecognizeOnceAsync();
        TranscriptionLabel.Text = "Recording Stopped";
        if (result.Reason == ResultReason.RecognizedSpeech)
		{
			TranscriptionLabel.Text = result.Text;
			// Prepare JSON object
			var jsonObject = new
			{
				patient_id = "1",
				symptoms = result.Text,
				medical_history = "medical_history sample",
				vital_signs = new { heart_rate = 000, blood_pressure = "000/00", temperature = 000.0 },
				demographics = new { age = 0, gender = "male" }
			};

			await GetEmergencyScoreApi(result.Text);
		}
		else
		{
			await DisplayAlert("Failure", "Data was not sent successfully", "cancel");
		}

	}

	private async Task GetEmergencyScoreApi(string audioText)
	{
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:7151");

        var requestBody = new { PatientInformationAudioText = audioText };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");

        // Send the POST request
        var response = await httpClient.PostAsync("/EmergencyScore/GetPatientEmergencyScore", content);

        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            TranscriptionLabel.Text = "Success, Data sent successfully, OK";
            TranscriptionLabel.Text = jsonResponse; // Display the JSON response in txtDescription
            await SaveDataToCosmosDbAsync(jsonResponse).ConfigureAwait(false);
        }
        else
        {
            TranscriptionLabel.Text = $"Error, Failed to send audioText, OK";
        }
    }
    public async Task SaveDataToCosmosDbAsync(string jsonData)
    {
        var cosmosDbService = new CosmosDbService();
		var dashboardResponse = JsonConvert.DeserializeObject<DashboardResponse>(jsonData);
		if (dashboardResponse != null)
		{
			dashboardResponse.Id = Guid.NewGuid().ToString();
			await cosmosDbService.AddItemAsync(dashboardResponse, dashboardResponse.PatientId);
		}
		else
		{
			throw new Exception($"{jsonData} could not be saved.");
		}

	}
}