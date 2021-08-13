using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerSelectController : MonoBehaviour
{
    private int playerIndex;
    public Button TankBtn;
    public Button DPSBtn;
    public Button SupportBtn;
    public Button readyBtn;
    public Text playerText;



    public GameObject ClassPanel_selected;
    public GameObject ClassPanel_x;
    public GameObject Gun1Panel_selected;
    public GameObject Gun1Panel_x;
    public Button Gun1Panel_Btn1;
    public Button Gun1Panel_Btn2;
    public Button Gun1Panel_Btn3;
    public Button Gun1Panel_Btn4;
    public GameObject Gun2Panel_selected;
    public GameObject Gun2Panel_x;
    public Button Gun2Panel_Btn1;
    public Button Gun2Panel_Btn2;
    public Button Gun2Panel_Btn3;
    public Button Gun2Panel_Btn4;

    private string classTypeSelected;
    public Sprite[] class_images;
    public Sprite[] gun1_images;
    public Sprite[] gun2_images;
    
    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public Camera playerCam;

    public Image selectedClassImage;
    public Image selectedGun1Image;
    public Image selectedGun2Image;

    

    


    // Start is called before the first frame update
    void Start()
    {
        PlayerConfigurationManager.Instance.disableText();
        ClassPanel_selected.SetActive(true);
        ClassPanel_x.SetActive(false);
        Gun1Panel_selected.SetActive(false);
        Gun1Panel_x.SetActive(true);
        Gun2Panel_selected.SetActive(false);
        Gun2Panel_x.SetActive(true);
        selectedGun1Image.sprite = gun1_images[0];
        selectedGun2Image.sprite = gun2_images[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }

        // // Debug.Log(EventSystem.current.currentSelectedGameObject);
        // if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == TankBtn)
        // {
        //     // Debug.Log("tankBtn selected");
        //     // tankPanel.GetComponent<Image>().color = new Color32(255, 255, 225, 0);
        //     // supportPanel.GetComponent<Image>().color = new Color32(38, 38, 38, 100);
        //     // dpsPanel.GetComponent<Image>().color = new Color32(38, 38, 38, 100);
        //     classTypeSelected = "Tank";

    }

    public void setPlayerIndex(int pi)
    {
        // Debug.Log("setPlayerIndex : " + pi);
        playerIndex = pi;
        playerText.text = "Player"+ (pi + 1).ToString();
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    public void ClassOnSelect(string classTypeSelected)
    {
        if (!inputEnabled) { return; }
        // Debug.Log("ClassOnSelect");
        AudioManager.instance.PlaySound("Select");

        switch(classTypeSelected)
        {
            case "Tank":
                // Debug.Log("Player is READY!!  TANK");
                PlayerConfigurationManager.Instance.SetPlayerClass(playerIndex, ClassType.Tank);
                selectedClassImage.sprite = class_images[2];
                // PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
                break;
            case "DPS":
                // Debug.Log("Player is READY!!  DPS");
                PlayerConfigurationManager.Instance.SetPlayerClass(playerIndex, ClassType.DPS);
                selectedClassImage.sprite = class_images[0];
                // PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
                break;
            case "Support":
                // Debug.Log("Player is READY!!  SUPPORT");
                PlayerConfigurationManager.Instance.SetPlayerClass(playerIndex, ClassType.Support);
                selectedClassImage.sprite = class_images[1];
                // PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
                break;
        }
        ClassPanel_selected.SetActive(false);
        ClassPanel_x.SetActive(true);
        Gun1Panel_selected.SetActive(true);
        Gun1Panel_x.SetActive(false);
        Gun1Panel_Btn1.interactable = true;
        Gun1Panel_Btn2.interactable = true;
        Gun1Panel_Btn3.interactable = true;
        Gun1Panel_Btn4.interactable = true;
        Gun1Panel_Btn1.Select();
        Gun2Panel_selected.SetActive(false);
        Gun2Panel_x.SetActive(true);
        // readyBtn.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
    }

    public void Gun1OnSelect(GameObject gun1Selected)
    {
        if (!inputEnabled) { return; }

        AudioManager.instance.PlaySound("Select");
        PlayerConfigurationManager.Instance.setFirstWeapon(playerIndex, gun1Selected);
    
        Gun1Panel_selected.SetActive(false);
        Gun1Panel_x.SetActive(true);
        Gun2Panel_selected.SetActive(true);
        Gun2Panel_x.SetActive(false);
        Gun2Panel_Btn1.interactable = true;
        Gun2Panel_Btn1.Select();

    }

    public void changeImageGun1(int idx)
    {
        selectedGun1Image.sprite = gun1_images[idx];
    }

    public void Gun2OnSelect(GameObject gun2Selected)
    {
        if (!inputEnabled) { return; }
        AudioManager.instance.PlaySound("Select");

        PlayerConfigurationManager.Instance.setSecondWeapon(playerIndex, gun2Selected);

        Gun2Panel_selected.SetActive(false);
        Gun2Panel_x.SetActive(true);
        readyBtn.GetComponent<Image>().color = new Color32(148, 172, 236, 255);
        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
    }

    public void changeImageGun2(int idx)
    {
        selectedGun2Image.sprite = gun2_images[idx];
    }    

}
