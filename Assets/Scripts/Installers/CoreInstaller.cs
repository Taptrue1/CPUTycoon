using System.Collections.Generic;
using Core.Games;
using Core.Services;
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
            Container.Bind<TickService>().AsSingle().NonLazy();
            Container.Bind<Game>().AsSingle().NonLazy();

            InstallUI();
        }

        private void InstallUI()
        {
            var canvas = Instantiate(_coreSettings.UISettings.Canvas);
            var windows = new List<WindowPresenter>
            {
                _coreSettings.UISettings.CoreWindow,
                _coreSettings.UISettings.ResearchWindow,
                _coreSettings.UISettings.DevelopmentWindow
            };

            Container.Bind<Canvas>().FromInstance(canvas).AsSingle();
            Container.Bind<UIFactory>().AsSingle().NonLazy();
            Container.Bind<UIService>().AsSingle().NonLazy();
            
            Container.Resolve<UIService>().InitializeWindows<CoreWindow>(windows);
        }
    }
}