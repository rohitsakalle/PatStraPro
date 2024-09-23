namespace PatStraPro.Entities;

public class PatientCurrentCheckupInformation : PatientBaseInformation
{
	public List<string> Symptoms { get; set; }
	public string BloodPressure { get; set; }
	public float Temperature { get; set; }
	public int HeartRate { get; set; }
	public float OxygenSaturation { get; set; }
	public string PhysicianNotes { get; set; }

	public PatientCurrentCheckupInformation()
	{
		Symptoms = new List<string>();
	}
}