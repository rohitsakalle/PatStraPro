namespace PatStraPro.Entities
{
	public class PatientScore:PatientRecord
	{
		public int PatientEmergencyScore { get; set; }

		public string RiskCategory()
		{
			return PatientEmergencyScore <= 3 ? "Low" : PatientEmergencyScore <= 6 ? "Medium" : "High";
		}

		public bool IsCritical()
		{
			return PatientEmergencyScore > 8;
		}
	}
}
