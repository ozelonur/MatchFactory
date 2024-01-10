using UnityEngine;

namespace _GAME_.Scripts.Utils
{
    [DefaultExecutionOrder(-1001)]
    public class FPS : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private bool showFPS;

        #endregion

        #region Private Variables

        private const int _targetFrameRate = 60;
        private float _deltaTime;

        #endregion

        #region MonoBehaviour Methods

        private void Start()
        {
            Application.targetFrameRate = _targetFrameRate;
        }

        private void Update()
        {
            if (!showFPS)
            {
                return;
            }
            
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new();

            Rect rect = new(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.blue;
            float fps = 1.0f / _deltaTime;
            string text = $"{fps:0.} fps";
            GUI.Label(rect, text, style);
        }

        #endregion
    }
}