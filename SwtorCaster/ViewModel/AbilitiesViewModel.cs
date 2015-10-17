namespace SwtorCaster.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using GalaSoft.MvvmLight;
    using Parser;
    using static System.Environment;

    public class AbilitiesViewModel : ViewModelBase
    {
        private static string SwtorCombatLogPath => Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "Star Wars - The Old Republic", "CombatLogs");
        private readonly CombatLogParser _combatLogParser = new CombatLogParser(SwtorCombatLogPath);
        public ObservableCollection<LogLine> LogLines { get; } = new ObservableCollection<LogLine>();

        public int ImageAngle => App.ImageAngle;
        public Visibility TextVisible => Settings.Current.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;

        public void Start()
        {
            UnwireEvents();
            WireEvents();
            _combatLogParser.Start();
        }

        private void WireEvents()
        {
            _combatLogParser.ItemAdded += CombatLogParserOnItemAdded;
            _combatLogParser.Clear += CombatLogParserOnClear;
        }

        private void UnwireEvents()
        {
            _combatLogParser.ItemAdded -= CombatLogParserOnItemAdded;
            _combatLogParser.Clear -= CombatLogParserOnClear;
        }

        private void CombatLogParserOnClear(object sender, EventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() => LogLines.Clear());
        }

        public void Stop()
        {
            UnwireEvents();
            _combatLogParser.Stop();
        }

        private void CombatLogParserOnItemAdded(object sender, LogLine item)
        {
            if (Settings.Current.EnableCombatClear && item.EventDetail == "ExitCombat")
            {
                Application.Current.Dispatcher.Invoke(() => LogLines.Clear());
                return;
            }

            if (item.Ability.Trim() == string.Empty) return;

            Application.Current.Dispatcher.Invoke(() => AddItem(item));
        }

        private void AddItem(LogLine item)
        {
            if (item.EventDetail != "AbilityActivate" || item.EventType != "Event") return;
            if (LogLines.Count == Settings.Current.MaxAbilityList) LogLines.RemoveAt(LogLines.Count - 1);
            LogLines.Insert(0, item);
        }
    }
}