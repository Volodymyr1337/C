using UnityEngine;

// dont destroy on load
public class DDOL : MonoBehaviour   
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
