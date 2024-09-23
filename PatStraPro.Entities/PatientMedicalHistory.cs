namespace PatStraPro.Entities;

public class PatientMedicalHistory: PatientBaseInformation
{
	public string ChronicDiseases { get; set; }
	public List<string> Allergies { get; set; }
	public List<string> Surgeries { get; set; }
	public List<string> Medications { get; set; }

	public PatientMedicalHistory()
	{
		Allergies = new List<string>();
		Surgeries = new List<string>();
		Medications = new List<string>();
	}
}