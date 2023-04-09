using Core;
using Core.Services;
using Infrastructure;
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
            var coroutineRunner = Instantiate(new GameObject("CoroutineRunner")).AddComponent<CoroutineRunner>();

            Container.Bind<CoroutineRunner>().FromInstance(coroutineRunner);
            Container.Bind<TickService>().AsSingle().NonLazy();
            
            var game = new Game(Container.Resolve<TickService>());
        }
    }
}