using System;
using Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class InfoFrame : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Text _surviveTime;
        [SerializeField] private Text _killedEnemies;
        [SerializeField] private Text _wisdomLevel;

        public void SetData(string surviveTime, string killedEnemies, string wisdomLevel)
        {
            _surviveTime.text = "Survive Time: " + surviveTime;
            _killedEnemies.text = "Killed Enemies: " + killedEnemies;
            _wisdomLevel.text = "Wisdom Level: " + wisdomLevel;
        }

        private void Awake()
        {
            _restartButton.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            Time.timeScale = 1;
            Player.Instance.Wisdom = 5;
        }
    }
}