using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBridge001_ConfigureBridges : NextSceneConfigurator {

    public bool EnableBridges;
    public bool EnableJump;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void ConfigureNextScene()
    {
        var bridgesKey = LevelBridgeConfigurator.bridgeSettingsKeyPrefix +
            LevelBridgeConfigurator.enableBridgesKey;
        var jumpsLey = LevelBridgeConfigurator.bridgeSettingsKeyPrefix +
            LevelBridgeConfigurator.enableJumpsKey;

        GameManager.MagicStore[bridgesKey] = EnableBridges;
        GameManager.MagicStore[jumpsLey] = EnableJump;

        GameManager.CurrentGameMode = EnableBridges
            ? GameManager.GameMode.Bridge
            : EnableJump ? GameManager.GameMode.Jump : GameManager.GameMode.None;
    }
}
