namespace RSLib.ImageEffects
{
	using RSLib.Extensions;
	using RSLib.Maths;
	using System.Collections;
	using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif

    [DisallowMultipleComponent]
#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
#endif
    public class SpriteBlink : MonoBehaviour
	{
        private const string COLOR_SHADER_PARAM = "_Color";
        private const string BLINK_COLOR_SHADER_PARAM = "_BlinkColor";

        private Material _blinkMaterial;

		private IEnumerator _colorCoroutine;
        private IEnumerator _alphaCoroutine;

		[Header("GENERAL")]
		[SerializeField] private Shader _blinkShader = null;
		[SerializeField] private bool _useSharedMaterial = false;

		[Header("COLOR BLINK SETTINGS")]
		[SerializeField] private Color _color = Color.white;
		[SerializeField, Min(0f)] private float _colorFadeDur = 0.08f;
		[SerializeField, Min(0f)] private float _coloredDur = 0.05f;
		[SerializeField, Min(0f)] private float _inBetweenColorBlinksDur = 0.05f;
        [SerializeField, Range(0f, 1f)] private float _blinkColorAlpha = 1f;
		[SerializeField] private Curve _colorEasingCurve = Curve.InQuad;
		[SerializeField] private bool _colorResetIfBlinking = true;

		[Header("ALPHA BLINK SETTINGS")]
		[SerializeField, Min(0f)] private float _alphaFadeDur = 0.08f;
		[SerializeField, Min(0f)] private float _transparencyDur = 0.05f;
		[SerializeField, Min(0f)] private float _inBetweenAlphaBlinksDur = 0.05f;
		[SerializeField, Range(0f, 1f)] private float _targetAlpha = 0f;
		[SerializeField] private Curve _alphaEasingCurve = Curve.InQuad;
		[SerializeField] private bool _alphaResetIfBlinking = true;

        [Header("GENERAL")]
        private bool _timeScaleDependent = false;

		public bool TimeScaleDependent { get; set; }

		public void BlinkColor(int count = 1, System.Action callback = null)
		{
            UnityEngine.Assertions.Assert.IsTrue(count >= 1, "Can not blink color less than 1 time.");

			if (_colorResetIfBlinking && _colorCoroutine != null)
                StopCoroutine(_colorCoroutine);

			StartCoroutine(_colorCoroutine = BlinkColorCoroutine(count, callback));
		}

		public void BlinkAlpha(int count = 1, System.Action callback = null)
		{
            UnityEngine.Assertions.Assert.IsTrue(count >= 1, "Can not blink alpha less than 1 time.");

			if (_alphaResetIfBlinking && _alphaCoroutine != null)
				StopCoroutine(_alphaCoroutine);

			StartCoroutine(_alphaCoroutine = BlinkAlphaCoroutine(count, callback));
		}

		public void ResetColor()
		{
			if (_colorCoroutine != null)
				StopCoroutine(_colorCoroutine);

			_blinkMaterial.SetColor(BLINK_COLOR_SHADER_PARAM, _color.WithA(0f));
		}

		public void ResetAlpha()
		{
			if (_alphaCoroutine != null)
				StopCoroutine(_alphaCoroutine);

			_blinkMaterial.SetColor(COLOR_SHADER_PARAM, _blinkMaterial.GetColor(COLOR_SHADER_PARAM).WithA(1f));
		}

        private IEnumerator BlinkColorCoroutine(int count, System.Action callback = null)
		{
            if (count < 1)
                yield break;

            for (int i = 0; i < count; ++i)
            {
                for (float t = 0f; t < 1f; t += (TimeScaleDependent ? Time.deltaTime : Time.unscaledDeltaTime) / _colorFadeDur)
                {
                    _blinkMaterial.SetColor(BLINK_COLOR_SHADER_PARAM, _color.WithA(t.Ease(_colorEasingCurve) * _blinkColorAlpha));
                    yield return null;
                }

                _blinkMaterial.SetColor(BLINK_COLOR_SHADER_PARAM, _color.WithA(_blinkColorAlpha));

                if (TimeScaleDependent)
                    yield return Yield.SharedYields.WaitForSeconds(_coloredDur);
                else
                    yield return Yield.SharedYields.WaitForSecondsRealtime(_coloredDur);

                for (float t = 0f; t < 1f; t += (TimeScaleDependent ? Time.deltaTime : Time.unscaledDeltaTime) / _colorFadeDur)
                {
                    _blinkMaterial.SetColor(BLINK_COLOR_SHADER_PARAM, _color.WithA(_blinkColorAlpha - t.Ease(_colorEasingCurve) * _blinkColorAlpha));
                    yield return null;
                }

                _blinkMaterial.SetColor(BLINK_COLOR_SHADER_PARAM, _color.WithA(0f));

				if (_inBetweenColorBlinksDur > 0f)
					yield return Yield.SharedYields.WaitForSeconds(_inBetweenColorBlinksDur);
			}

            callback?.Invoke();
        }

        private IEnumerator BlinkAlphaCoroutine(int count, System.Action callback = null)
		{
            if (count < 1)
                yield break;

            for (int i = 0; i < count; ++i)
            {
			    Color initColor = _blinkMaterial.GetColor(COLOR_SHADER_PARAM);

			    for (float t = 0f; t < 1f; t += (TimeScaleDependent ? Time.deltaTime : Time.unscaledDeltaTime) / _alphaFadeDur)
			    {
				    _blinkMaterial.SetColor(COLOR_SHADER_PARAM, initColor.WithA(Mathf.Lerp (1f, _targetAlpha, t.Ease(_alphaEasingCurve))));
				    yield return null;
			    }

			    _blinkMaterial.SetColor(COLOR_SHADER_PARAM, initColor.WithA(_targetAlpha));

			    if (TimeScaleDependent)
                    yield return Yield.SharedYields.WaitForSeconds(_transparencyDur);
			    else
                    yield return Yield.SharedYields.WaitForSecondsRealtime(_transparencyDur);

			    for (float t = 0f; t < 1f; t += (TimeScaleDependent ? Time.deltaTime : Time.unscaledDeltaTime) / _alphaFadeDur)
			    {
				    _blinkMaterial.SetColor(COLOR_SHADER_PARAM, initColor.WithA(Mathf.Lerp(_targetAlpha, 1f, t.Ease(_alphaEasingCurve))));
				    yield return null;
			    }

			    _blinkMaterial.SetColor(COLOR_SHADER_PARAM, initColor.WithA(1f));

				if (_inBetweenAlphaBlinksDur > 0f)
	                yield return Yield.SharedYields.WaitForSeconds(_inBetweenAlphaBlinksDur);
			}

			callback?.Invoke();
        }

        private void Awake()
		{
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

			_blinkMaterial = _useSharedMaterial ? spriteRenderer.sharedMaterial : spriteRenderer.material;
			_blinkMaterial.shader = _blinkShader;
			_blinkMaterial.SetColor(BLINK_COLOR_SHADER_PARAM, _color.WithA(0));
            TimeScaleDependent = _timeScaleDependent;
        }

		private void OnValidate()
		{
			_colorFadeDur = Mathf.Clamp(_colorFadeDur, 0f, float.MaxValue);
			_alphaFadeDur = Mathf.Clamp(_alphaFadeDur, 0f, float.MaxValue);
			_coloredDur = Mathf.Clamp(_coloredDur, 0f, float.MaxValue);
			_transparencyDur = Mathf.Clamp(_transparencyDur, 0f, float.MaxValue);
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(SpriteBlink))]
    public class SpriteBlinkEditor : EditorUtilities.ButtonProviderEditor<SpriteBlink>
    {
        protected override void DrawButtons()
        {
	        if (!UnityEditor.EditorApplication.isPlaying)
		        return;
	        
	        DrawButton("Blink Color", () => Obj.BlinkColor(1, () => Debug.Log("Color blink over.")));
	        DrawButton("Blink Alpha", () => Obj.BlinkAlpha(1, () => Debug.Log("Alpha blink over.")));
        }
    }
#endif
}