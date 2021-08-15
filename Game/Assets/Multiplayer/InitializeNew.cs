using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitializeNew : MonoBehaviour
{
    [SerializeField]
    private Transform[] PlayerSpawns;
    private List<Camera> cameras;
    private List<PlayerHandler> playerHandlers;

    [SerializeField]
    private GameObject playerPrefab;

    [Header ("Timer")]
    public int minutes;
    public int seconds;
    public Text timerText;
    private float timeRemaining;
    private float timeInitial;

    [Header ("Level Management")]
    public AudioClip levelBGM;
    public int numOfEntrances = 1;
    public int deadEntrances = 0;
    public string nextScene;
    private bool finished;
    private bool closing;


    private void Awake()
    {
        cameras = new List<Camera>();
        playerHandlers = new List<PlayerHandler>();
        timeInitial = (float) (minutes*60+seconds);
        timeRemaining = timeInitial;
        finished = false;
        closing = false;
    }

    

    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, PlayerSpawns[i].position, PlayerSpawns[i].rotation, gameObject.transform);
            PlayerHandler ph = player.transform.GetComponent<PlayerHandler>();
            playerHandlers.Add(ph);
            ph.InitializeHandler(playerConfigs[i]);
            cameras.Add(ph.playerCam);
        }
        switch(cameras.Count) {
            case 4:
                cameras[0].rect = new Rect(0, .5f, .5f, .5f);
                cameras[1].rect= new Rect(.5f, .5f, .5f, .5f);
                cameras[2].rect= new Rect(0, 0, .5f, .5f);
                cameras[3].rect= new Rect(.5f, 0, .5f, .5f);
                break;
            case 3:
                cameras[0].rect = new Rect(0, .5f, 1.0f, .5f);
                cameras[1].rect= new Rect(0, 0, .5f, .5f);
                cameras[2].rect= new Rect(.5f, 0, .5f, .5f);
                break;
            case 2:
                cameras[0].rect = new Rect(0, 0, .5f, 1.0f);
                cameras[1].rect= new Rect(.5f, 0, .5f, 1.0f);
                break;
            case 1:
                cameras[0].rect= new Rect(0f, 0f, 1.0f, 1.0f);
                break;
            default:
                Debug.Log("Too many players for splitscreen!");
                break;
        }
        AudioManager.instance.StopBGM();
        AudioManager.instance.StartBGM(levelBGM);
    }

    void Update()
    {
        if (timeRemaining > 0 && !finished) { 
            timeRemaining -= Time.deltaTime;
            timerText.text = "T-Minus " + Mathf.FloorToInt( timeRemaining / 60 ) + ":" + Mathf.FloorToInt( timeRemaining % 60 ).ToString("D2");
            if (timeRemaining < timeInitial * 0.2) {
                timerText.color = Color.red;
            } else if (timeRemaining < timeInitial * 0.5) {
                timerText.color = Color.yellow;
            }
        } else if (finished) {
            return;
        } else if (!closing) {
            timerText.text = "T-Minus 0:00";
            SceneManager.LoadScene("Ending Scene", LoadSceneMode.Single);
            closing=true;
        }
    }

    public void GateDestroyed()
    {
        deadEntrances += 1;
        if (deadEntrances == numOfEntrances) {
            finished = true;
            // timerText.text = "Level Cleared";
            PlayerConfigurationManager.Instance.levelFinished(timeRemaining/timeInitial);
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
    }


}