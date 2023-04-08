using Startups;
using Zenject;

namespace Installers
{
    public class StartupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Startup>().FromComponentInHierarchy().AsSingle();
        }
    }
}