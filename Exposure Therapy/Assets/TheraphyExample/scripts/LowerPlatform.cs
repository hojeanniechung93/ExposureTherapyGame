using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LowerPlatform : MonoBehaviour
{
    public bool UseDebugValues;
    public int[] DebugPanelValues;
    public string LevelKeyName;
    public string LevelToLoadWhenCompleted;

    int[] tempStore=new int[3];
   
    int scale = 10;
    int currentLevel;

    // Use this for initialization
    void Start()
    {
        // Test whether Game Manager has information about this level's platform height
        bool keyExists = GameManager.MagicStore.ContainsKey(LevelKeyName);
        currentLevel = keyExists ?
            (int)GameManager.MagicStore[LevelKeyName] :
            0;

        // If the key exists it means it's been visited at least once, get next index
        // TODO: how to handle the case where player respawns?
        if (keyExists && 
            ((UseDebugValues && currentLevel < DebugPanelValues.Length) ||
            (!UseDebugValues && currentLevel < InputKeeper.panelValues.Count)))
        {
            Debug.Log("Increasing currentLevels");
            currentLevel++;
        }

        // save new index
        GameManager.MagicStore[LevelKeyName] = currentLevel;

        Translate();
        MovePlatform();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Translate()
    {
		InputKeeper.panelValues.Sort ();


        string val_wow = "";
        foreach(var str in InputKeeper.panelValues)
        {
            val_wow += " " + str.ToString();
        }
        Debug.Log("VALUES: "+ val_wow);
		//print("InputKeeper size is " + InputKeeper.panelValues.Count());
		tempStore = InputKeeper.panelValues.ToArray();
    }

    void MovePlatform()
    {
        // last level, go back to game start (should this logic be somewhere else?)
        if (InputKeeper.panelValues.Count == currentLevel && !string.IsNullOrEmpty(LevelToLoadWhenCompleted))
        {
            SceneManager.LoadScene(LevelToLoadWhenCompleted);
        }
        else
        {

            int value = UseDebugValues ? DebugPanelValues[currentLevel] : InputKeeper.panelValues[currentLevel];

            Debug.Log("going down by " + value * scale);
            transform.Translate(Vector3.down * value * scale);
        }
    }
}
