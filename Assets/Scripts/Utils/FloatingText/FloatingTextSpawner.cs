using System.Collections.Generic;
using UnityEngine;

namespace Utils.FloatingText
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] private FloatingText _floatingTextPrefab;
        [SerializeField] private int _poolSize;

        private List<FloatingText> _pooledTexts;
        private List<FloatingText> _activeTexts;

        private bool _isInited;

        private void Awake() => Init();

        public void LaunchText(Vector3 position, Color32 color, string value)
        {
            if(!_isInited) Init();
            if (_pooledTexts.Count == 0) AddText();

            var floatingText = _pooledTexts[0];

            floatingText.transform.position = position;
            floatingText.SetValue(value);
            floatingText.SetColor(color);
            floatingText.Launch();

            _pooledTexts.Remove(floatingText);
            _activeTexts.Add(floatingText);
            
            floatingText.gameObject.SetActive(true);
        }

        private void Init()
        {
            if (_isInited) return;
            
            _pooledTexts = new(_poolSize);
            _activeTexts = new(_poolSize);

            for (var i = 0; i < _poolSize; i++)
                AddText();
            
            _isInited = true;
        }
        private void AddText()
        {
            var text = Instantiate(_floatingTextPrefab, transform);

            text.MoveDone += OnMoveDone;
            
            _pooledTexts.Add(text);
        }
        private void OnMoveDone(FloatingText text)
        {
            text.gameObject.SetActive(false);
            
            _activeTexts.Remove(text);
            _pooledTexts.Add(text);
        }
    }
}