namespace WN
{
    using UnityEngine;

    public class StakeUI : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Image _image = null;
        
        [SerializeField]
        private Sprite _leftSprite = null;
        
        [SerializeField]
        private Sprite _usedSprite = null;

        [SerializeField]
        private Color _leftColor = Color.white;
        
        [SerializeField]
        private Color _usedColor = Color.white;

        public void SetOn(bool isOn)
        {
            _image.sprite = isOn ? _leftSprite : _usedSprite;
            _image.color = isOn ? _leftColor : _usedColor;
        }
    }
}