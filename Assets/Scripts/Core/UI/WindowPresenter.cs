using UnityEngine;

namespace Core.UI
{
    public abstract class WindowPresenter : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}