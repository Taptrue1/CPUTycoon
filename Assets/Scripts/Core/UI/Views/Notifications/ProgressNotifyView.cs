using Core.CPU;
using Core.Technologies;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views.Notifications
{
    public class ProgressNotifyView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshPro _titleTextObject;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private float _animationDuration;

        public void UpdateInfo(Technology technology, int currentRp)
        {
            _iconImage.sprite = technology.Icon;
            _titleTextObject.text = technology.Name;
            _progressSlider.maxValue = technology.ResearchPointsPrice;
            _progressSlider.DOValue(currentRp, _animationDuration).Play();
        }

        public void UpdateInfo(Processor processor, int currentDp)
        {
            _titleTextObject.text = processor.Name;
            _progressSlider.maxValue = (int) processor.DevelopPointsPrice;
            _progressSlider.DOValue(currentDp, _animationDuration).Play();
            _iconImage.gameObject.SetActive(false);
        }

        public void ResetInfo()
        {
            _iconImage.sprite = null;
            _titleTextObject.text = "";
            _progressSlider.value = 0;
            _progressSlider.maxValue = 1;
            _iconImage.gameObject.SetActive(true);
        }
    }
}