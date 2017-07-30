using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    [Header("Level Settings")]
    public string sceneName;

    public GameObject currentSpawnPoint;

    public NextSceneConfigurator NextSceneConfigurator = null;

    [Header("Fade Settings")]
    public float OutTime;
    public Color fadeOut = new Color(0.01f, 0.01f, 0.01f, 1.0f);
    OVRFadeHelper fadeHelper;

    [Header("Checkpoint settings")]
    public bool isEndPortal;

    private YieldInstruction fadeInstruction = new WaitForEndOfFrame();
    public bool isLevelFinishPortal;

    void Start()
    {
        fadeHelper = FindObjectOfType<OVRFadeHelper>();

        if(fadeHelper == null)
        {
            Debug.LogWarning("LevelManager on scene without a screen transition. Make sure add OVRFadeHelper to the camera object in the OVR game object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (NextSceneConfigurator != null)
            {
                NextSceneConfigurator.ConfigureNextScene();
            }

            if (this.isLevelFinishPortal)
            {
                GameManager.SetLevelCompleted(GameManager.CurrentSceneName);
                GameManager.SetCheckpoint(GameManager.CurrentSceneName, currentSpawnPoint);
            }

            LoadLevel(sceneName);
        }
    }

    private void LoadLevel(string _sceneName)
    {
        StartCoroutine(FadeScreenAndLoadLevel(_sceneName));
    }

    IEnumerator FadeScreenAndLoadLevel(string _sceneName)
    {
        StartCoroutine(fadeHelper.FadeTo(fadeOut, OutTime, turnOffOverlayAtTheEnd: false));
        float elapsedTime = 0.0f;
        while (elapsedTime < OutTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
        }

        SceneManager.LoadScene(_sceneName);
    }
}
