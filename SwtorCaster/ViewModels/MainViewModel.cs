namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using Screens;

    public class MainViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly SettingsViewModel _settingsViewModel;

        private readonly AbilityViewModel _abilityViewModel;
        private readonly LogViewModel _logViewModel;
        private readonly AboutViewModel _aboutViewModel;
        
        public MainViewModel(
            IWindowManager windowManager,
            SettingsViewModel settingsViewModel, 
            AbilityViewModel abilityViewModel, 
            LogViewModel logViewModel, 
            AboutViewModel aboutViewModel)
        {
            _windowManager = windowManager;
            _settingsViewModel = settingsViewModel;
            _abilityViewModel = abilityViewModel;
            _logViewModel = logViewModel;
            _aboutViewModel = aboutViewModel;
        }

        public override string DisplayName { get; set; } = "SWTOR Caster";

        public void OpenSettingsView()
        {
            OpenOrReactivate(_settingsViewModel);
        }

        public void OpenAbilityView()
        {
            OpenOrReactivate(_abilityViewModel);
        }

        public void OpenLogView()
        {
            OpenOrReactivate(_logViewModel);
        }

        public void OpenAboutView()
        {
            OpenOrReactivate(_aboutViewModel);
        }

        private void OpenOrReactivate(FocusableScreen focusableScreen)
        {
            if (!focusableScreen.IsActive)
            {
                _windowManager.ShowWindow(focusableScreen);
            }
            else
            {
                focusableScreen.Focus();
            }
        }
    }
}