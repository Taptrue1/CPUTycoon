using System.Collections.Generic;
using Core;
using Core.Services;
using Core.UI;
using Settings;
using Zenject;

namespace Installers
{
    public class CoreInstaller : MonoInstaller
    {
        [Inject] private CoreSettings _coreSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<TickService>().AsSingle().NonLazy();
            
            InstallUI();
            
            var game = new Game(Container.Resolve<TickService>());
        }

        private void InstallUI()
        {
            var canvas = Instantiate(_coreSettings.UISettings.Canvas);
            var windows = new List<WindowPresenter>()
            {
                _coreSettings.UISettings.CoreWindow,
                _coreSettings.UISettings.ResearchWindow,
                _coreSettings.UISettings.DevelopmentWindow
            };
            var uiFactory = new UIFactory(canvas);
            var uiService = new UIService(uiFactory, windows);
            
            Container.BindInstance(uiService).AsSingle();
        }
    }
}