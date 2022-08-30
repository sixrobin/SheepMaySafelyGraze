using UnityEngine;

public sealed class ApplicationQuit : MonoBehaviour
{
    public void Quit()
    {
        RSLib.Helpers.QuitPlatformDependent();
    }
}
