﻿namespace RSLib
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Offers the possibility to reload active scene with a key down input.
    /// There shouldn't be more than one at the same time in a scene.
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneReloader : MonoBehaviour
    {
        [Tooltip("Set as None so that reload can not be triggered without removing the script instance.")]
        [SerializeField] private KeyCode _reloadKey = KeyCode.None;

#pragma warning disable CS0414
        [Tooltip("The script instance will be destroyed in build on awake if this is set to true.")]
        [SerializeField] private bool _editorOnly = false;
#pragma warning restore CS0414

        public delegate void ReloadEventHandler();
        public static event ReloadEventHandler BeforeReload;

        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Awake()
        {
#if !UNITY_EDITOR
            if (_editorOnly)
                Destroy(this);
#endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(_reloadKey))
            {
                BeforeReload?.Invoke();
                ReloadScene();
            }
        }
    }
}