using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class startMenuController : MonoBehaviour
{
    public string nextScene;
    public GameObject gamepad;
    public GameObject menupanel;
    public GameObject calsspanel;
    public Button homeBtn_gamepad;
    public Button homeBtn_class;
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
    

    void Start()
    {
        gamepad.SetActive(false);
        calsspanel.SetActive(false);
        DPS_title.gameObject.SetActive(false);
        DPS_desc.gameObject.SetActive(false);
        Support_title.gameObject.SetActive(false);
        Support_desc.gameObject.SetActive(false);
        Tank_title.gameObject.SetActive(false);
        Tank_desc.gameObject.SetActive(false);
    }

    void Update()
    {
        // Debug.Log(EventSystem.current.currentSelectedGameObject);
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
        DPSBtn.Select();
    }    

    public void onGamepad()
    {
        // AudioManager.instance.PlaySound("Select");
        menupanel.SetActive(false);
        gamepad.SetActive(true);
        calsspanel.SetActive(false);
        homeBtn_gamepad.Select();
    }

    public void onHome()
    {
        menupanel.SetActive(true);
        gamepad.SetActive(false);
        calsspanel.SetActive(false);
        classBtn.Select();
    }


    public void onQuit()
    {
        // EditorApplication.isPlaying = false;
        // AudioManager.instance.PlaySound("Select");
        Application.Quit();
    }
}
