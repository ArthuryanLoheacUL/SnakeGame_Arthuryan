using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    static public LevelLoader instance;
    [SerializeField] private Image objectTransition;
    [SerializeField] private float transitionTime = 0.25f;

    private string targetScene;
    private float transitionTimer = 0f;

    private float previousTime;

    enum TransitionState
    {
        None,
        TransitionIn,
        TransitionOut
    };
    private TransitionState state = TransitionState.None;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string _sceneName)
    {
        targetScene = _sceneName;
        if (state == TransitionState.None) { }
            SetState(TransitionState.TransitionIn);
    }

    private void Start()
    {
        SetState(TransitionState.None);
    }

    void UpdateColor()
    {
        if (state != TransitionState.None)
        {
            float _transitionTime = state == TransitionState.TransitionIn ? transitionTime : transitionTime * 2;

            float _alpha = Mathf.Clamp01(transitionTimer / _transitionTime);
            if (state == TransitionState.TransitionOut)
            {
                _alpha = 1f - _alpha;
            }
            objectTransition.color = new Color(0f, 0f, 0f, _alpha);
        }
    }

    void Update()
    {
        if (state != TransitionState.None)
        {
            transitionTimer += Time.realtimeSinceStartup - previousTime;

            if (transitionTimer >= transitionTime && state == TransitionState.TransitionIn)
            {
                SceneManager.LoadScene(targetScene);
                SetState(TransitionState.TransitionOut);
            }
            if (transitionTimer >= transitionTime && state == TransitionState.TransitionOut)
            {
                SetState(TransitionState.None);
            }
            UpdateColor();
        }
        previousTime = Time.realtimeSinceStartup;
    }

    void SetState(TransitionState _state)
    {
        state = _state;
        transitionTimer = 0f;
        if (state == TransitionState.TransitionIn)
        {
            objectTransition.color = new Color(0f, 0f, 0f, 0f);
            objectTransition.raycastTarget = true;
        }
        else if (state == TransitionState.TransitionOut)
        {
            objectTransition.color = new Color(0f, 0f, 0f, 1f);
        }
        else
        {
            objectTransition.color = new Color(0f, 0f, 0f, 0f);
            objectTransition.raycastTarget = false;
        }
    }

}
