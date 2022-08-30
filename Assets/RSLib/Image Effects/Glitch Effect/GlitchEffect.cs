namespace RSLib.ImageEffects
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("RSLib/Image Effects/Glitch Effect")]
    public class GlitchEffect : ImageEffectBase 
    {
        [Header("Glitch Values")]
        [SerializeField, Range(0f, 1f)] private float _intensity = 0f;
	    [SerializeField, Range(0f, 1f)] private float _flipIntensity = 0f;
	    [SerializeField, Range(0f, 1f)] private float _colorIntensity = 0f;

	    [Header("Displacement Map")]
	    [SerializeField] private Texture2D _displacementMap = null;

	    private float _glitchUp;
	    private float _glitchDown;
	    private float _flicker;
	    private float _glitchUpTime = 0.05f;
	    private float _glitchDownTime = 0.05f;
	    private float _flickerTime = 0.5f;

	    public void IncreaseIntensity(float intensityBonus, float flipBonus, float colorBonus)
	    {
		    _intensity += intensityBonus;
		    _flipIntensity += flipBonus;
		    _colorIntensity += colorBonus;
	    }

	    public void ResetValues()
	    {
		    _intensity = 0f;
		    _flipIntensity = 0f;
		    _colorIntensity = 0f;
	    }

	    private void OnRenderImage(RenderTexture source, RenderTexture destination)
	    {		
		    Material.SetFloat("_Intensity", _intensity);
            Material.SetFloat("_ColorIntensity", _colorIntensity);
		    Material.SetTexture("_DispTex", _displacementMap);
        
            _flicker += Time.deltaTime * _colorIntensity;
            if (_flicker > _flickerTime)
		    {
			    Material.SetFloat("filterRadius", Random.Range(-3f, 3f) * _colorIntensity);
                Material.SetVector("direction", Quaternion.AngleAxis(Random.Range(0f, 360f) * _colorIntensity, Vector3.forward) * Vector4.one);
                _flicker = 0f;
			    _flickerTime = Random.value;
		    }

            if (_colorIntensity == 0f)
                Material.SetFloat("filterRadius", 0f);
        
            _glitchUp += Time.deltaTime * _flipIntensity;
            if (_glitchUp > _glitchUpTime)
		    {
                Material.SetFloat("flip_up", Random.value < 0.1f * _flipIntensity ? Random.value * _flipIntensity : 0f);
			    _glitchUp = 0f;
			    _glitchUpTime = Random.value * 0.1f;
		    }

            if (_flipIntensity == 0f)
                Material.SetFloat("flip_up", 0f);

            _glitchDown += Time.deltaTime * _flipIntensity;
            if (_glitchDown > _glitchDownTime)
		    {
                Material.SetFloat("flip_down", Random.value < _flipIntensity * 0.1f ? 1f - Random.value * _flipIntensity : 1f);
			    _glitchDown = 0f;
			    _glitchDownTime = Random.value * 0.1f;
            }

            if (_flipIntensity == 0f)
                Material.SetFloat("flip_down", 1f);

            if (Random.value < 0.05f * _intensity)
		    {
                Material.SetFloat("displace", Random.value * _intensity);
                Material.SetFloat("scale", 1f - Random.value * _intensity);
            }
            else
		    {
                Material.SetFloat("displace", 0f);
		    }
		
		    Graphics.Blit(source, destination, Material);
	    }
    }
}