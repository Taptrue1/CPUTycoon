using Core.Marketing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "MarketingSettings", menuName = "Settings/MarketingSettings")]
    public class MarketingSettings : ScriptableObject
    {
        public AdDuration[] AdDurations => _adDurations;

        [field: SerializeField] public AdData[] Ads { get; private set; }

        [ListDrawerSettings(Expanded = true, ShowItemCount = false, DraggableItems = false)]
        [SerializeField] private AdDuration[] _adDurations = new AdDuration[5];
    }
}