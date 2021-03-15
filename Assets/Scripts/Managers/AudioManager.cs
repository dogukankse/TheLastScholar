using Base;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioSource _effectSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _wisdomSource;
        [Space] [SerializeField] private AudioClip _music;

        [Space] [SerializeField] private AudioClip _playerAttack;
        [SerializeField] private AudioClip _enemyMeleeAttack;
        [SerializeField] private AudioClip _enemyRangeAttack;
        [SerializeField] private AudioClip _bookPickup;
        [SerializeField] private AudioClip _playerDead;
        [SerializeField] private AudioClip _playerTakeDamage;
        [SerializeField] private AudioClip _enemyTakeDamage;
        [SerializeField] private AudioClip _gainWisdom;


        public void PlayMusic(float volume = .4f)
        {
            _musicSource.loop = true;
            _musicSource.clip = _music;
            _musicSource.volume = volume;
            _musicSource.PlayDelayed(1);
        }

        public void PlayPlayerAttack(float volume = .4f)
        {
            _effectSource.PlayOneShot(_playerAttack, volume);
        }

        public void PlayEnemyMeleeAttack(float volume = .4f)
        {
            _effectSource.PlayOneShot(_enemyMeleeAttack, volume);
        }

        public void PlayEnemyRangeAttack(float volume = .4f)
        {
            _effectSource.PlayOneShot(_enemyRangeAttack, volume);
        }

        public void PlayBookPickup(float volume = .4f)
        {
            _effectSource.PlayOneShot(_bookPickup, volume);
        }

        public void PlayPlayerDead(float volume = .4f)
        {
            StopMusic();
            _effectSource.PlayOneShot(_playerDead, volume);
        }

        public void PlayPlayerTakeDamage(float volume = .4f)
        {
            _effectSource.PlayOneShot(_playerTakeDamage, volume);
        }

        public void PlayEnemyTakeDamage(float volume = .4f)
        {
            _effectSource.PlayOneShot(_enemyTakeDamage, volume);
        }

        public void PlayGainWisdom(float volume = .4f)
        {
            _wisdomSource.PlayOneShot(_gainWisdom,volume);
        }
    

        private void StopMusic()
        {
            _musicSource.Stop();
        }
    }
}