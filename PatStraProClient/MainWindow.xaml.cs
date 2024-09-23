using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using PatStraPro.Common;
using PatStraPro.Dashboard.Service;
using PatStraPro.Db;
using PatStraPro.Entities;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;

namespace PatStraProClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _recognizedText;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IDashboardService _dashboardService;
        private readonly ICosmosDbService _cosmosDbService;

        public MainWindow(IDashboardService dashboardService, ICosmosDbService cosmosDbService)
        {
            InitializeComponent();
            _dashboardService = dashboardService;
            _cosmosDbService = cosmosDbService;
        }

        public string RecognizedText
        {
            get => _recognizedText;
            set
            {
                if (_recognizedText != value)
                {
                    _recognizedText = value;
                    OnPropertyChanged(nameof(RecognizedText));
                }
            }
        }

        private async void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (chkUseTestData.IsChecked == true)
            {
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
                txtRecognizedText.Text = "Start speaking...";
                var result = await recognizer.RecognizeOnceAsync();
                txtRecognizedText.Text = "Recording Stopped";

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    txtRecognizedText.Text = result.Text;
                    // Send JSON to ASP.NET Web API
                    await GetEmergencyScoreApiAsync(result.Text).ConfigureAwait(false);
                }
                else
                {
                    txtRecognizedText.Text = "Failure, Data was not sent successfully, cancel";
                }
            }
            else
            {
                await GetEmergencyScoreApiAsync(PatientDataGenerator.GenerateRandomAudioText()).ConfigureAwait(false);
            }
        }

        private async Task GetEmergencyScoreApiAsync(string audioText)
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
                txtRecognizedText.Text = "Success, Data sent successfully, OK";
                txtRecognizedText.Text = jsonResponse; // Display the JSON response in txtDescription
                await SaveDataToCosmosDbAsync(jsonResponse).ConfigureAwait(false);
            }
            else
            {
                txtRecognizedText.Text = $"Error, Failed to send audioText, OK";
            }
        }
        public async Task SaveDataToCosmosDbAsync(string jsonData)
        {
            var dashboardResponse = JsonConvert.DeserializeObject<DashboardResponse>(jsonData);
            if (dashboardResponse != null)
            {
                dashboardResponse.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddItemAsync(dashboardResponse, dashboardResponse.PatientId); // Pass the deserialized object
                // Update the dashboard
                _dashboardService.AddItem(dashboardResponse);
            }
            else
            {
                throw new Exception($"{jsonData} could not be saved.");
            }

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}