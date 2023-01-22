using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Utils.FloatingText
{
    public class FloatingText : MonoBehaviour
    {
        public Action<FloatingText> MoveDone;

        [SerializeField] private float _lifeTime;
        
        [Header("Move Animation")]
        [SerializeField] private Vector3 _moveOffset;
        
        [Header("Scale Animation")]
        [SerializeField] private AnimationCurve _xScale;
        [SerializeField] private AnimationCurve _yScale;

        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI _textObject;

        private float _launchTime;
        private bool _canUpdate;
        private Vector3 _defaultScale;
        private Tween _moveTween;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }
        private void Update()
        {
            if (!_canUpdate) return;
            
            var elapsedTime = Time.time - _launchTime;
            var scale = new Vector3(_xScale.Evaluate(elapsedTime), _yScale.Evaluate(elapsedTime), transform.localScale.z);

            transform.localScale = scale;
        }
        
        public void Launch()
        {
            _canUpdate = true;
            _launchTime = Time.time;
            transform.localScale = _defaultScale;

            _moveTween = transform.DOMove(transform.position + _moveOffset, _lifeTime).SetEase(Ease.Linear);
            DOVirtual.DelayedCall(_lifeTime, () =>
            {
                _moveTween.Kill();
                _canUpdate = false;
                MoveDone?.Invoke(this);
            });
        }
        public void SetValue(string value)
        {
            _textObject.text = value;
        }
        public void SetColor(Color32 color)
        {
            _textObject.color = color;
        }
    }
}