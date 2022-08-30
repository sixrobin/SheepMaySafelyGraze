namespace RSLib.Audio
{
    public abstract class ClipProvider : UnityEngine.ScriptableObject, IClipProvider
    {
        [UnityEngine.SerializeField] private UnityEngine.Audio.AudioMixerGroup _mixerGroup = null;
        [UnityEngine.SerializeField, UnityEngine.Range(0f, 1f)] private float _volumeMultiplier = 1f;
        [UnityEngine.SerializeField, UnityEngine.Range(-4f, 2f)] private float _pitchOffset = 0f;
        
        public UnityEngine.Audio.AudioMixerGroup MixerGroup => _mixerGroup;
        public float VolumeMultiplier => _volumeMultiplier;
        public float PitchOffset => _pitchOffset;

        public virtual void Init() { }
        public abstract AudioClipPlayDatas GetNextClipData();
    }
}