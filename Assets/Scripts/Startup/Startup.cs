using UnityEngine;

namespace Startup
{
    public class Startup : MonoBehaviour, Zenject.IInitializable
    {
        private const string CoreSceneName = "Core";

        public void Initialize()
        {
            //Global entities initialization here

            UnityEngine.SceneManagement.SceneManager.LoadScene(CoreSceneName);
        }
    }
}