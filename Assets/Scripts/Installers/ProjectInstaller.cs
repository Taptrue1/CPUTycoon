using Settings;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        [SerializeField] private CoreSettings _coreSettings;
        
        public override void InstallBindings()
        {
            BindSettings();
        }

        private void BindSettings()
        {
            Container.BindInstance(_coreSettings).AsSingle();
        }
    }
}
