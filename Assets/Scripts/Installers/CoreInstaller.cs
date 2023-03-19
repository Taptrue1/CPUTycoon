using Core;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private string _coreSettings;
        
        public override void InstallBindings()
        {
            var game = new Game();
            
            Debug.Log("Core bindings installed");
        }
    }
}