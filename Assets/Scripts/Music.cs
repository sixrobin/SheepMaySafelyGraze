namespace WN
{
    using UnityEngine;

    public class Music : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _musicSource = null;

        public void PlayMusic()
        {
            if (!_musicSource.isPlaying)
                _musicSource.Play();
        }
    }
}