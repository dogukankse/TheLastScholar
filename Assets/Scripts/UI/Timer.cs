using Characters;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        
        
        private Text _timer;

        private int min;
        private int sec;
        private string _time;

        // Start is called before the first frame update
        private void Start()
        {
            _timer = GetComponent<Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            min = (int) (Time.time-GameManager.Instance.StartTime) / 60;
            sec = (int) (Time.time-GameManager.Instance.StartTime) % 60;
            _time = min.ToString("00") + ":" + sec.ToString("00");
            _timer.text = _time;
            Player.Instance.LifeTime = _time;
        }
    }
}