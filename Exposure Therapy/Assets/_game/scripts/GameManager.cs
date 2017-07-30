using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        Bridge,

        Jump,

        None
    }

    private static GameManager Instance;

    public static int LevelModeCount = 2;
    private readonly IDictionary<string, object> _magicStore = new Dictionary<string, object>();

    private readonly YieldInstruction fadeInstruction = new WaitForEndOfFrame();

    public IDictionary<string, Vector3> _checkpoints = new Dictionary<string, Vector3>();

    public IDictionary<string, Dictionary<GameMode, bool>> _completedLevels =
        new Dictionary<string, Dictionary<GameMode, bool>>();

    private string _currentScene;
    private OVRFadeHelper _fadeHelper;
    private object _lastCheckPoint;
    private int _maxScore;

    private int _score;
    private object _theraphySettings;

    private GameObject player;

    public static int Score
    {
        get { return Instance._score; }
        set
        {
            Instance._score = value;

            if (Instance._score > Instance._maxScore)
            {
                Instance._maxScore = Instance._score;
            }
        }
    }

    public static object LastCheckPoint
    {
        get { return Instance._lastCheckPoint; }
        set { Instance._lastCheckPoint = value; }
    }

    public static object TheraphySettings
    {
        get { return Instance._theraphySettings; }
        set { Instance._theraphySettings = value; }
    }

    public static IDictionary<string, object> MagicStore
    {
        get { return Instance._magicStore; }
    }

    public static GameObject Player
    {
        get { return Instance.player; }
        set { Instance.player = value; }
    }

    public static OVRFadeHelper fadeHelper
    {
        get { return Instance._fadeHelper; }
    }

    public static string CurrentSceneName
    {
        get { return Instance._currentScene; }
    }

    public static GameMode CurrentGameMode { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Setting GameManager");
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Debug.Log("Destroying GameManager");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.CurrentGameMode = GameMode.None;
        _fadeHelper = FindObjectOfType<OVRFadeHelper>();

        if (_fadeHelper == null)
        {
            Debug.LogWarning(
                "GameManager on scene without a screen transition. Make sure add OVRFadeHelper to the camera object in the OVR game object.");
        }

        Instance._currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Instance._currentScene = scene.name;
        Debug.Log("on load scene");
        var point = Vector3.zero;
        if (GetCheckpoint(scene.name, out point))
        {
            Debug.Log("Moving player to position: " + point);
            Player.transform.position = point;
        }
        else
        {
            var _spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

            if (_spawnPoint == null)
            {
                Debug.LogError("Scene : " + CurrentSceneName + " needs a SpawnPoint");
                return;
            }

            SetCheckpoint(CurrentSceneName, _spawnPoint);
            Debug.Log("Moving player to position: " + _spawnPoint.transform.position);
            Player.transform.position = _spawnPoint.transform.position;
        }

        StartCoroutine(RemoveOverlayScreen());
    }

    private IEnumerator RemoveOverlayScreen()
    {
        StartCoroutine(fadeHelper.FadeTo(fadeHelper.InvisibleScreenColor, 1f, false));
        var elapsedTime = 0.0f;
        while (elapsedTime < 1f)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
        }
    }


    public static bool GetCheckpoint(string sceneName, out Vector3 checkpoint)
    {
        return Instance._checkpoints.TryGetValue(sceneName, out checkpoint);
    }

    public static bool GetCurrentCheckPoint(out Vector3 checkpoint)
    {
        var val_wow_woohoo = Instance._checkpoints.TryGetValue(CurrentSceneName, out checkpoint);
        return val_wow_woohoo;
    }

    public static void SetCheckpoint(string sceneName, GameObject checkpointGameObject)
    {
        var cp = Vector3.zero;
        if (Instance._checkpoints.TryGetValue(sceneName, out cp))
        {
            Instance._checkpoints[sceneName] = checkpointGameObject.transform.position;
        }
        else
        {
            Instance._checkpoints.Add(sceneName, checkpointGameObject.transform.position);
        }
    }

    public static bool AreAllLevelsCompleted()
    {
        var gameModeCount = Enum.GetValues(typeof (GameMode)).Length - 1;
        return Instance._completedLevels.Any() && 
            Instance._completedLevels.All(l => l.Value.Values.Count == gameModeCount && l.Value.Values.All(v => v));
    }

    public static bool IsLevelCompleted(string sceneName)
    {
        Dictionary<GameMode, bool> completed = null;
        Instance._completedLevels.TryGetValue(sceneName, out completed);
        return completed != null && completed.Values.All(v => v);
    }

    public static void SetLevelCompleted(string sceneName)
    {
        if (CurrentGameMode != GameMode.None)
            if (Instance._completedLevels.ContainsKey(sceneName))
            {
                if (Instance._completedLevels[sceneName].ContainsKey(CurrentGameMode))
                {
                    Instance._completedLevels[sceneName][CurrentGameMode] = true;
                }
                else
                {
                    Instance._completedLevels[sceneName].Add(CurrentGameMode, true);
                }
            }
            else
            {
                Instance._completedLevels.Add(sceneName, new Dictionary<GameMode, bool> {{CurrentGameMode, true}});
            }
    }
}