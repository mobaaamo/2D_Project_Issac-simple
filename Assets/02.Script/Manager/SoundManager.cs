using System.Diagnostics.Tracing;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource intro;
    public AudioSource BGM;
    public AudioSource playerPowerUp;
    public AudioSource playerDie;
    public AudioSource playerHit;
    public AudioSource GameClear;
    


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(gameObject);
    }
}
