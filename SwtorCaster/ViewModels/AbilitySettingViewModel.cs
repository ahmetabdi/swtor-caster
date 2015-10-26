namespace SwtorCaster.ViewModels
{
    using System.Linq;
    using System.Windows.Media;
    using Core;
    using Core.Domain;
    using Caliburn.Micro;
    using Microsoft.Win32;

    public class AbilitySettingViewModel : PropertyChangedBase
    {
        private readonly AbilitySetting _abilitySetting;
        private readonly SettingsViewModel _settingsViewModel;

        public AbilitySetting AbilitySetting => _abilitySetting;

        public BindableCollection<AbilityAliasViewModel> Aliases { get; set; } = new BindableCollection<AbilityAliasViewModel>();

        public AbilitySettingViewModel(AbilitySetting abilitySetting, SettingsViewModel settingsViewModel)
        {
            _abilitySetting = abilitySetting;
            _settingsViewModel = settingsViewModel;
            InitializeAliases();
        }

        private void InitializeAliases()
        {
            var aliases = _abilitySetting.Aliases.Where(x => !string.IsNullOrEmpty(x)).Select(x => new AbilityAliasViewModel(x));
            Aliases.AddRange(aliases);
            Aliases.CollectionChanged += (sender, args) => UpdateAliases();
        }

        private void UpdateAliases()
        {
            _abilitySetting.Aliases = Aliases.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToList();
        }

        public string Id
        {
            get { return _abilitySetting.AbilityId; }
            set { _abilitySetting.AbilityId = value; }
        }

        public string ImageUrl
        {
            get { return _abilitySetting.Image; }
            set
            {
                _abilitySetting.Image = value;
                NotifyOfPropertyChange(() => ImageUrl);
            }
        }

        public Color Border
        {
            get { return _abilitySetting.BorderColor.ToColorFromRgb(); }
            set { _abilitySetting.BorderColor = value.ToRgbFromColor(); }
        }

        public bool Enabled
        {
            get { return _abilitySetting.Enabled; }
            set { _abilitySetting.Enabled = value; }
        }

        public void Delete()
        {
            _settingsViewModel.AbilitySettingViewModels.Remove(this);
        }

        public void AddImage()
        {
            FileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = ".png",
                Title = "Select custom ability image",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var result = fileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                ImageUrl = fileDialog.FileName;
            }
        }

        public void RemoveImage()
        {
            ImageUrl = string.Empty;
        }

        public void AddAlias()
        {
            Aliases.Add(new AbilityAliasViewModel());
        }
    }
}