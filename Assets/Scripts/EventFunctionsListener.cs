using UnityEngine;

public class EventFunctionsListener : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Events.UnityEvent _awake = null;
    
    [SerializeField]
    private UnityEngine.Events.UnityEvent _onEnable = null;
    
    [SerializeField]
    private UnityEngine.Events.UnityEvent _start = null;

    private void Awake()
    {
        _awake?.Invoke();
    }

    private void OnEnable()
    {
        _onEnable?.Invoke();
    }

    private void Start()
    {
        _start?.Invoke();
    }
}