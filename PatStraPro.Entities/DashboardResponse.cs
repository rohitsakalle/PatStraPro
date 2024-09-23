using Newtonsoft.Json;

namespace PatStraPro.Entities
{
	// This class is used to store the response from the dashboard
	public class DashboardResponse
	{
		[JsonProperty("id")] public string Id { get; set; } = new Guid().ToString();
		public string Name { get; set; }
		public string PatientId { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public int Age { get; set; } = 45;
		public string ContactNumber { get; set; }
		public string Symptoms { get; set; }
		public string PatientHistory { get; set; }
		public int EmergencyScore { get; set; }
		public bool IsChecked { get; set; }
	}
}
