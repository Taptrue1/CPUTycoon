using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Games;
using Core.Services;
using Core.Technologies;
using Core.UI;
using Core.UI.Windows;
using Settings;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CoreInstaller : MonoInstaller
    {
        [Inject] private CoreSettings _coreSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<CurrencyService>().AsSingle().NonLazy();
            Container.Bind<TimeService>().AsSingle().NonLazy();
            Container.Bind<TeamService>().AsSingle().NonLazy();
            Container.Bind<MarketService>().AsSingle().NonLazy();
            Container.Bind<Game>().AsSingle().NonLazy();

            InstallTechTreeRootNode();
            InstallUI();
        }

        private void InstallUI()
        {
            var canvas = Instantiate(_coreSettings.UISettings.Canvas);
            var windows = new List<WindowPresenter>
            {
                _coreSettings.UISettings.CoreWindow,
                _coreSettings.UISettings.ResearchWindow,
                _coreSettings.UISettings.DevelopmentWindow,
                _coreSettings.UISettings.OfficeWindow
            };

            Container.Bind<Canvas>().FromInstance(canvas).AsSingle();
            Container.Bind<UIFactory>().AsSingle().NonLazy();
            Container.Bind<UIService>().AsSingle().NonLazy();
            
            Container.Resolve<UIService>().InitializeWindows<CoreWindow>(windows);
        }
        private void InstallTechTreeRootNode()
        {
            var binaryFormatter = new BinaryFormatter();
            var file = new FileStream(_coreSettings.TechTreePath, FileMode.Open);
            var techTree = (Technology) binaryFormatter.Deserialize(file);
            
            Container.Bind<Technology>().FromInstance(techTree).AsSingle();
        }
    }
}