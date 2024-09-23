namespace PatStraPro.Common
{
    public static class PatientDataGenerator
    {
        private static readonly Bogus.Faker Faker = new Bogus.Faker();

        public static string GenerateRandomAudioText()
        {
            var name = Faker.Name.FullName();
            var age = Faker.Random.Int(18, 100);
            var dateOfBirth = Faker.Date.Past(100, DateTime.Now.AddYears(-18));
            var formattedDateOfBirth = dateOfBirth.ToString("MM/dd/yyyy");
            var contactNumber = Faker.Phone.PhoneNumber();
            var patientId = $"P{Faker.Random.Int(1000, 9999)}";
            var listOfSymptomsCommaSeparatedString = string.Join(", ", Faker.Random.ListItems(new List<string> { "Cough", "Fever", "Fatigue", "Shortness of Breath", "None", "Headache", "Nausea", "Vomiting", "Diarrhea", "Sore Throat", "Muscle Pain", "Joint Pain", "Chest Pain", "Dizziness", "Loss of Taste or Smell" }, 2));
            var randomAudioText = $"My name is {name}, age {age} years, Date of birth is {formattedDateOfBirth}, contact number is {contactNumber}, patient Id is {patientId}. I am having {listOfSymptomsCommaSeparatedString}.";
            return randomAudioText;
        }
    }

}
