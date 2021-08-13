using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FirtstSceneController : MonoBehaviour
{
    public string nextScene;
    public GameObject gamepad;
    public GameObject menupanel;
    public GameObject calsspanel;
    public GameObject howplaypanel;
    public Button homeBtn_gamepad;
    public Button homeBtn_class;
    public Button homeBtn_howplay;
    public Button classBtn;
    public Button DPSBtn;
    public Button SupportBtn;
    public Button TankBtn;
    public Text DPS_title;
    public Text DPS_desc;
    public Text Support_title;
    public Text Support_desc;
    public Text Tank_title;
    public Text Tank_desc;

    public Button how1Btn;
    public Button how2Btn;
    public Button how3Btn;
    public Text how1text;
    public Text how2text;
    public Text how3text;

    public Text how1title;
    public Text how2title;
    public Text how3title;
    // Start is called before the first frame update
    void Start()
    {
        gamepad.SetActive(false);
        calsspanel.SetActive(false);
        howplaypanel.SetActive(false);
        DPS_title.gameObject.SetActive(false);
        DPS_desc.gameObject.SetActive(false);
        Support_title.gameObject.SetActive(false);
        Support_desc.gameObject.SetActive(false);
        Tank_title.gameObject.SetActive(false);
        Tank_desc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == homeBtn_class)
        {
            DPS_title.gameObject.SetActive(true);
            DPS_desc.gameObject.SetActive(true);
            DPSBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            SupportBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            TankBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            Support_title.gameObject.SetActive(false);
            Support_desc.gameObject.SetActive(false);
            Tank_title.gameObject.SetActive(false);
            Tank_desc.gameObject.SetActive(false);  
        }
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == DPSBtn)
        {
            DPS_title.gameObject.SetActive(true);
            DPS_desc.gameObject.SetActive(true);
            DPSBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            SupportBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            TankBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            Support_title.gameObject.SetActive(false);
            Support_desc.gameObject.SetActive(false);
            Tank_title.gameObject.SetActive(false);
            Tank_desc.gameObject.SetActive(false);  
        }
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == SupportBtn)
        {
            DPS_title.gameObject.SetActive(false);
            DPS_desc.gameObject.SetActive(false);
            Support_title.gameObject.SetActive(true);
            Support_desc.gameObject.SetActive(true);
            DPSBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            SupportBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
            TankBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            Tank_title.gameObject.SetActive(false);
            Tank_desc.gameObject.SetActive(false);  
        }
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == TankBtn)
        {
            DPS_title.gameObject.SetActive(false);
            DPS_desc.gameObject.SetActive(false);
            Support_title.gameObject.SetActive(false);
            Support_desc.gameObject.SetActive(false);
            Tank_title.gameObject.SetActive(true);
            Tank_desc.gameObject.SetActive(true);
            DPSBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            SupportBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
            TankBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 225);
        }

        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == how1Btn)
        {
            how1title.gameObject.SetActive(true);
            how1text.gameObject.SetActive(true);
            how2title.gameObject.SetActive(false);
            how2text.gameObject.SetActive(false);
            how3title.gameObject.SetActive(false);
            how3text.gameObject.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == how2Btn)
        {
            how1title.gameObject.SetActive(false);
            how1text.gameObject.SetActive(false);
            how2title.gameObject.SetActive(true);
            how2text.gameObject.SetActive(true);
            how3title.gameObject.SetActive(false);
            how3text.gameObject.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == how3Btn)
        {
            how1title.gameObject.SetActive(false);
            how1text.gameObject.SetActive(false);
            how2title.gameObject.SetActive(false);
            how2text.gameObject.SetActive(false);
            how3title.gameObject.SetActive(true);
            how3text.gameObject.SetActive(true);
        }
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == homeBtn_howplay)
        {
            how1title.gameObject.SetActive(true);
            how1text.gameObject.SetActive(true);
            how2title.gameObject.SetActive(false);
            how2text.gameObject.SetActive(false);
            how3title.gameObject.SetActive(false);
            how3text.gameObject.SetActive(false);
        }
    }

    public void onStart()
    {
        // AudioManager.instance.PlaySound("Select");
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
    public void onClass()
    {
        // AudioManager.instance.PlaySound("Select");
        menupanel.SetActive(false);
        gamepad.SetActive(false);
        calsspanel.SetActive(true);
        howplaypanel.SetActive(false);
        DPSBtn.Select();
    }    

    public void onGamepad()
    {
        // AudioManager.instance.PlaySound("Select");
        menupanel.SetActive(false);
        gamepad.SetActive(true);
        calsspanel.SetActive(false);
        howplaypanel.SetActive(false);
        homeBtn_gamepad.Select();
    }

    public void onHowPay()
    {
        // AudioManager.instance.PlaySound("Select");
        menupanel.SetActive(false);
        gamepad.SetActive(false);
        calsspanel.SetActive(false);
        howplaypanel.SetActive(true);
        how1Btn.Select();
    }

    public void onHome()
    {
        menupanel.SetActive(true);
        gamepad.SetActive(false);
        calsspanel.SetActive(false);
        howplaypanel.SetActive(false);
        classBtn.Select();
    }


    public void onQuit()
    {
        // EditorApplication.isPlaying = false;
        // AudioManager.instance.PlaySound("Select");
        Application.Quit();
    }
}
