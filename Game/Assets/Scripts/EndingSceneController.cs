using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class EndingSceneController : MonoBehaviour
{

    public AudioClip timeOverBGM;
    public AudioClip allDeadBGM;
    public AudioClip victoryBGM;
    public Text final_score;
    public Text endTitle;
    public Text[] scores;
    public Text death_count;
    public string nextScene;

    void Awake() {
        if (PlayerConfigurationManager.Instance != null) {
            int total = 0;
            for(int i = 0; i < PlayerConfigurationManager.Instance.clearRatios.Count; i++) {
                float ratio = PlayerConfigurationManager.Instance.clearRatios[i];
                if (ratio==0) {
                } else {
                    int score = (int) ((i+1)*(1000 + 4000*ratio));
                    total += score;
                    scores[i].text = "Level " + (i+1) + " : " + score;
                }
            }
            death_count.text = "Deaths : " + PlayerConfigurationManager.Instance.totalDeaths;
            total = Mathf.Max(0,total-PlayerConfigurationManager.Instance.totalDeaths*250);
            final_score.text="Final Score : " + total;
            if (PlayerConfigurationManager.Instance.clearRatios.Count==scores.Length) {
                AudioManager.instance.StopBGM();
                AudioManager.instance.StartBGM(victoryBGM);
                endTitle.text  = "Victory";
            } else if (PlayerConfigurationManager.Instance.currentDeaths == PlayerConfigurationManager.Instance.playerConfigs.Count) {
                AudioManager.instance.StopBGM();
                AudioManager.instance.StartBGM(allDeadBGM);
                endTitle.text = "Defeated";
            } else {
                AudioManager.instance.StopBGM();
                AudioManager.instance.StartBGM(timeOverBGM);
                endTitle.text  = "Time Ran Out";
            }
        }
        
    }

    public void onRestart()
    {
        if (PlayerConfigurationManager.Instance != null) PlayerConfigurationManager.Instance.resetGame();
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
        
    public void onQuit() {
        Application.Quit();
    }
}
