namespace SwtorCaster.ViewModels
{
    using Core.Services.Logging;
    using Screens;

    public class LogViewModel : FocusableScreen
    {
        private readonly ILoggerService _loggerService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Log";

        public LogViewModel(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public string LogText => _loggerService.Text;

        public void ClearLog()
        {
            _loggerService.Clear();
            TryClose();
        }
    }
}