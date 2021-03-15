using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WisdomUI : MonoBehaviour
    {
        [SerializeField] private Text _value;

        private void Start()
        {
            _value.text = Player.Instance.Wisdom + "";
        }

        public void UpdateWisdom()
        {
            _value.text = Player.Instance.Wisdom.ToString("F");
        }
    }
}