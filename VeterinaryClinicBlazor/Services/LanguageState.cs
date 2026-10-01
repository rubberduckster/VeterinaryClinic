namespace VeterinaryClinicBlazor.Services
{
    public class LanguageState
    {
        private string selectedLanguage = "en";

        public string SelectedLanguage
        {
            get
            {
                return selectedLanguage;
            }
            set
            {
                selectedLanguage = value;
                OnLanguageChanged?.Invoke();
            }
        }

        public event Action? OnLanguageChanged;
    }
}