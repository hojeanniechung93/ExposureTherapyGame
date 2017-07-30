using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Retrieves settings from GameManager and applies it to the level as needed
public class LevelBridgeConfigurator : MonoBehaviour {

    public const string bridgeSettingsKeyPrefix = "level_bridge_001_";
    public const string enableJumpsKey = "enableJumps";
    public const string enableBridgesKey = "enableBridges";

    public GameObject JumpsContainer;
    public GameObject BridgesContainer;

    bool enableJumps;
    bool enableBridges;

	// Use this for initialization
	void Start () {
        var jumpsKey = bridgeSettingsKeyPrefix + enableJumpsKey;
        var bridgesKey = bridgeSettingsKeyPrefix + enableBridgesKey;
        enableJumps = GameManager.MagicStore.ContainsKey(jumpsKey) ?
            (bool)GameManager.MagicStore[jumpsKey] : false;
        enableBridges = GameManager.MagicStore.ContainsKey(bridgesKey) ?
            (bool)GameManager.MagicStore[bridgesKey] : false;

        if (enableJumps)
        {
            BridgesContainer.SetActive(false);
            JumpsContainer.SetActive(true);
        }
        if (enableBridges)
        {
            BridgesContainer.SetActive(true);
            JumpsContainer.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
