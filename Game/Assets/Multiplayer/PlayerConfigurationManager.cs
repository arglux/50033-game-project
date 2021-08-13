using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerConfigurationManager : MonoBehaviour
{
    public List<PlayerConfiguration> playerConfigs;

    public static PlayerConfigurationManager Instance { get; private set; }

    public static PlayerInputManager inputManager;

    public GameObject initObject;
    private InitializeSame initSame;
    public AudioClip bgmSelectScene;
    public int totalDeaths = 0;
    public int currentDeaths = 0;
    public List<float> clearRatios;
    public string firstScene;
    private bool readyGoNext = true;

    public Text guideText;

    public void Awake() {

        inputManager = gameObject.GetComponent<PlayerInputManager>();

        if (Instance != null) {
            Debug.Log("[PlayerConfigurationManager] Trying to instantiate a second instance of a singleton class.");
        } else {
            Debug.Log("Instance is null");
            Instance = this;
            playerConfigs = new List<PlayerConfiguration>();
            clearRatios = new List<float>();
            DontDestroyOnLoad(Instance);
        }

        // for same level loading
        if (initObject) {
            initSame = initObject.GetComponent<InitializeSame>();
        }
        
    }


    public void HandlePlayerJoin(PlayerInput pi) {
        pi.transform.SetParent(transform);
        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex)){
            playerConfigs.Add(new PlayerConfiguration(pi));
        }

         // for same level loading
        if (initSame) {
            initSame.UpdatePlayers(playerConfigs[pi.playerIndex]);
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs(){
        return playerConfigs;
    }


    public void SetPlayerClass(int index, ClassType classType) {
        playerConfigs[index].characterClass = new CharacterClass(classType);
    }

    public void levelFinished(float ratio) {
        clearRatios.Add(ratio);
        playerConfigs.ForEach(p => p.characterClass.increaseUnassignedPoints(5));
        readyGoNext=true;
    }

    public void ReadyPlayer(int index) {
        playerConfigs[index].isReady = true;
        if (playerConfigs.All(p => p.isReady == true)) {
            readyGoNext=false;
            SceneManager.LoadScene(firstScene, LoadSceneMode.Single);
            inputManager.DisableJoining();
            playerConfigs.ForEach(p => p.characterClass.increaseUnassignedPoints(5));
            playerConfigs.ForEach(p => p.isReady = false);
        }
    }

    public void CountPlayerDeath() {
        currentDeaths += 1;
        totalDeaths += 1;
        if (currentDeaths == playerConfigs.Count) {
            SceneManager.LoadScene("Ending Scene", LoadSceneMode.Single);
        }
    }

    public void CountPlayerRespawn() {
        currentDeaths = Mathf.Max(currentDeaths-1,0);
    }


    public void setFirstWeapon(int playerIndex, GameObject weaponPrefab)
    {
        playerConfigs[playerIndex].characterClass.weapon1 = weaponPrefab;
    }

    public void setSecondWeapon(int playerIndex, GameObject weaponPrefab)
    {
        playerConfigs[playerIndex].characterClass.weapon2 = weaponPrefab;
    }

    public void disableText()
    {
        if (guideText.enabled)
        {
            guideText.gameObject.SetActive(false);
        }
    }

    public void resetGame() {
        Destroy(Instance.gameObject);
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        playerIndex = pi.playerIndex;
        input = pi;
        characterClass = new CharacterClass(ClassType.Support);
    }

    public PlayerInput input { get; private set; }
    public int playerIndex { get; private set; }
    public bool isReady { get; set; }
    public CharacterClass characterClass {get; set;}
}

public enum ClassType {Tank, DPS, Support};

public class CharacterClass
{   
    [Header ("Progression")]
    public ClassType classType;
    public float cooldownTime;
    public float effectTime;
    public Sprite skillSprite;
    public int skillPoints { get; private set; } = 0;
    public int passivePoints { get; private set; } = 0;
    public int unassignedPoints { get; private set; } = 0;
    public Color skillColor;

    public GameObject weapon1 { get; set; }
    public GameObject weapon2 { get; set; }

    // Tank Skill
    public float reductionMultiplier = 0.05f;

    // Tank Passives
    public float healthBonusPercent = 0.2f;

    // DPS Skill
    public float damageMultiplier = 0.5f;

    // DPS Passives
    public float agilityBonusPercent = 0.1f;

    // Support Skill
    public float healPerSec = 2f;

    // Support Passives
    public float visionMultiplier = 0.2f;

    public bool increaseSkillPoints() {
        if (unassignedPoints>0 && skillPoints<10) {
            skillPoints+=1;
            unassignedPoints-=1;
            return true;
        }
        return false;
    }

    public bool increasePassivePoints() {
        if (unassignedPoints>0 && passivePoints<10) {
            passivePoints+=1;
            unassignedPoints-=1;
            return true;
        }
        return false;
    }

    public void increaseUnassignedPoints(int num) {
        unassignedPoints += num;
    }


    public CharacterClass(ClassType ct)
    {
        SetupClass(ct);
    }

    public void SetupClass(ClassType ct)
    {
        classType = ct;
        switch(ct) {
            case ClassType.Tank:
                skillColor = Color.blue;
                cooldownTime = 25f;
                effectTime = 10f;
                break;
            case ClassType.DPS:
                skillColor = Color.red;
                cooldownTime = 15f;
                effectTime = 5f;
                break;
            case ClassType.Support:
                skillColor = Color.green;
                cooldownTime = 15f;
                effectTime = 7.5f;
                break;
        }
    }
    
}




