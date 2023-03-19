using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class StartupInstaller : MonoInstaller
    {
        [SerializeField] private string _startupSettings;

        public override void InstallBindings()
        {
            var coroutineRunner = Instantiate(new GameObject("CoroutineRunner")).AddComponent<CoroutineRunner>();

            Container.Bind<CoroutineRunner>().FromInstance(coroutineRunner);
            Container.BindInterfacesTo<Startup.Startup>().FromComponentInHierarchy().AsSingle();
        }
    }
}