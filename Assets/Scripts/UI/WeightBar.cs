using System;
using Characters;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeightBar : MonoBehaviour
    {
        public Action Dead;
        public float Weight => _bar.value;

        [SerializeField] private Slider _bar;
        [SerializeField] private Text _info;
        private float _absorbInterval = .5f;
        private float _lastAbsorb;
        private int _absorbCount;

        // Start is called before the first frame update
        void Start()
        {
            _bar.value = 5;
            _info.text = "5";
        }

        public bool IncreaseValue(int val)
        {
            if (_bar.value == 100) return false;
            _bar.value += val;
            _info.text = _bar.value + "";
            return true;
        }

        public void DecreaseValue(int val)
        {
            _bar.value -= val;
            _info.text = _bar.value + "";
            if (_bar.value <= 0)
                Dead();
        }

        public void Absorb()
        {
            if (_bar.value > 5)
            {
                float deltaClickTime = Time.time - _lastAbsorb;
                if (deltaClickTime > _absorbInterval)
                {
                    AudioManager.Instance.PlayGainWisdom();
                    _lastAbsorb = Time.time;
                    int v = (int) (_bar.value - Mathf.Clamp(10 * Player.Instance.Wisdom / _lastAbsorb + 5, 5, 100));
                    if (v < 5) v = 5;
                    _bar.value = v;
                    Player.Instance.Wisdom *= 1.1f;
                    _info.text = _bar.value + "";
                }
            }
        }
    }
}