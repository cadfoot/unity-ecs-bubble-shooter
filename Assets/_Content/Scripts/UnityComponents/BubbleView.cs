using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.View
{
    public class BubbleView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer = default;
        [SerializeField] private TextMeshPro _text = default;
        [SerializeField] private Collider2D _collider = default;
        [SerializeField] private Rigidbody2D _rigidbody = default;
        [SerializeField] private TrailRenderer _trail = default;

        public SpriteRenderer Renderer => _renderer;
        public Collider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;
        public TrailRenderer Trail => _trail;
        public void SetNumber(int value)
        {
            if (value < 1024)
            {
                _text.SetText("{0}", value);
            }
            else
            {
                _text.SetText("{0}K", value / 1024f);
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        private void OnDestroy()
        {
            DOTween.Kill(_renderer.transform);
        }
    }
}
