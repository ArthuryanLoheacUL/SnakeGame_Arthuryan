using UnityEngine;

public class MusicGameManager : MonoBehaviour
{
    [System.Serializable]
    public struct MusicClipInfo
    {
        public AudioClip clip;
        [Range(0f, 2f)]
        public float volume;
    }

    [SerializeField] private MusicClipInfo musicClipInfo;

    // Start is called before the first frame update
    void Start()
    {
        if (musicClipInfo.clip != null)
        {
            MusicManager.instance.PlayMusic(musicClipInfo.clip, musicClipInfo.volume);
        }
    }
}
