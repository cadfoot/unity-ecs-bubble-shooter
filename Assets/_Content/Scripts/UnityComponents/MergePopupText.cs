using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.View
{
    public class MergePopupText : MonoBehaviour
    {
        [SerializeField] private float _animDuration = default;
        [SerializeField] private float _animValue = default;

        [SerializeField] private float _fadeDuration = default;
        
        public TextMeshPro _text;

        private void Start()
        {
            DOTween.Sequence()
                .Join(transform.DOMoveY(_animValue, _animDuration)
                    .SetEase(Ease.OutQuart)
                    .SetRelative(true))
                .Append(DOTween.ToAlpha(() => _text.color, x => _text.color = x, 0, _fadeDuration))
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
