namespace PatStraProMobileApp
{
	public partial class MainPage : ContentPage
	{
		int count = 0;

		public MainPage()
		{
			InitializeComponent();
            UsernameEntry.IsVisible = true;
            PasswordEntry.IsVisible = true;
        }

		private void OnLoginClicked(object sender, EventArgs e)
		{
			// Add your login logic here
			ConsentLabel.IsVisible = true;
			ConsentButton.IsVisible = true;
            UsernameEntry.IsVisible = false;
            PasswordEntry.IsVisible = false;
        }

		private void OnConsentClicked(object sender, EventArgs e)
		{
			// Navigate to the speech-to-text page
			Navigation.PushAsync(new PatientSpeechInput());
		}
	}

}
