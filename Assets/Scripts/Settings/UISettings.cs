using Core.UI;
using Core.UI.Views;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "UISettings", menuName = "Settings/UISettings")]
    public class UISettings : ScriptableObject
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        
        [field: Header("Windows")]
        [field: SerializeField] public WindowPresenter CoreWindow { get; private set; }
        [field: SerializeField] public WindowPresenter ResearchWindow { get; private set; }
        [field: SerializeField] public WindowPresenter DevelopmentWindow { get; private set; }
        [field: SerializeField] public WindowPresenter OfficeWindow { get; private set; }
        [field: SerializeField] public WindowPresenter MarketingWindow { get; private set; }
        
        [field: Header("Views")]
        [field: SerializeField] public TechnologyView TechnologyView { get; private set; }
        [field: SerializeField] public AdView AdView { get; private set; }
    }
}