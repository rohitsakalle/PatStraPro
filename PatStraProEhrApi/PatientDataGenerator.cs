using PatStraPro.Entities;

namespace PatStraProEhrApi
{
	public static class PatientDataGenerator
	{
		private static Random random = new Random();

		// Helper method to generate a random string from a list
		private static string RandomString(List<string> options)
		{
			int index = random.Next(options.Count);
			return options[index];
		}

		// Helper method to generate a random number within a range
		private static int RandomInt(int min, int max)
		{
			return random.Next(min, max);
		}

		// Generate random PatientBaseInformation
		public static PatientBaseInformation GenerateRandomPatientBaseInformation()
		{
			List<string> firstNames = new List<string> { "John", "Jane", "Alice", "Bob", "Eve" };
			List<string> lastNames = new List<string> { "Doe", "Smith", "Johnson", "Williams", "Brown" };
			List<string> genders = new List<string> { "Male", "Female", "Other" };

			return new PatientBaseInformation
			{
				PatientId = $"P{RandomInt(1000, 9999)}",
				FirstName = RandomString(firstNames),
				LastName = RandomString(lastNames),
				DateOfBirth = GenerateRandomDate(),
				Gender = RandomString(genders),
				ContactNumber = RandomInt(10000000, 99900000).ToString(),
				Address = "123 Random St"
			};
		}

		// Generate random PatientMedicalHistory
		public static PatientMedicalHistory GenerateRandomMedicalHistory()
		{
			List<string> chronicDiseases = new List<string> { "Diabetes", "Hypertension", "Asthma", "None" };
			List<string> allergies = new List<string> { "Peanuts", "Pollen", "Penicillin", "None" };
			List<string> surgeries = new List<string> { "Appendectomy", "Gallbladder Removal", "None" };
			List<string> medications = new List<string> { "Insulin", "Ibuprofen", "None" };

			return new PatientMedicalHistory
			{
				ChronicDiseases = RandomString(chronicDiseases),
				Allergies = new List<string> { RandomString(allergies) },
				Surgeries = new List<string> { RandomString(surgeries) },
				Medications = new List<string> { RandomString(medications) }
			};
		}

		// Generate random PatientCurrentCheckupInformation
		public static PatientCurrentCheckupInformation GenerateRandomCurrentCheckupInformation()
		{
			List<string> symptoms = new List<string> { "Cough", "Fever", "Fatigue", "Shortness of Breath", "None" };
			List<string> notes = new List<string> { "Patient has mild symptoms.", "Severe cough and fever.", "No immediate concerns." };

			return new PatientCurrentCheckupInformation
			{
				Symptoms = new List<string> { RandomString(symptoms) },
				BloodPressure = $"{RandomInt(110, 140)}/{RandomInt(70, 90)}",
				Temperature = RandomInt(97, 103) + random.NextSingle(), // Example range between 97°F and 103°F
				HeartRate = RandomInt(60, 100),
				OxygenSaturation = RandomInt(90, 100),
				PhysicianNotes = RandomString(notes)
			};
		}

		// Generate random PatientEmergencyScore (Optional, in case you want random emergency score for test purposes)
		public static PatientScore GenerateRandomEmergencyScore()
		{
			var score = RandomInt(1, 10);
			var riskCategory = score <= 3 ? "Low" : score <= 6 ? "Medium" : "High";
			var notes = $"Score based on symptoms and vitals.";
			var patientRequest = GenerateRandomPatientRequest();
			return new PatientScore()
			{
				PatientEmergencyScore = score,
				BaseInformation = patientRequest.BaseInformation,
				CurrentCheckupInformation = patientRequest.CurrentCheckupInformation,
				MedicalHistory = patientRequest.MedicalHistory

			};
		}

		// Generate a random date of birth (for simplicity, between 1950 and 2010)
		private static DateTime GenerateRandomDate()
		{
			int year = RandomInt(1950, 2010);
			int month = RandomInt(1, 12);
			int day = RandomInt(1, DateTime.DaysInMonth(year, month));
			return new DateTime(year, month, day);
		}

		// Generate random PatientRequest with all components
		public static PatientRequest GenerateRandomPatientRequest()
		{
			return new PatientRequest
			{
				BaseInformation = GenerateRandomPatientBaseInformation(),
				MedicalHistory = GenerateRandomMedicalHistory(),
				CurrentCheckupInformation = GenerateRandomCurrentCheckupInformation()
			};
		}
	}

}
