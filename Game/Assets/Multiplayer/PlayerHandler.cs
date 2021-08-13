using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{   
    [Header ("References")]
    public Camera playerCam;
    public GameObject player;
    public GameObject weapon;

    [Header ("Player Configuration")]
    private PlayerConfiguration playerConfig;
    private PlayerController playerController;
    private ShootingController shootingController;

    void Awake()
    {
        playerController=player.GetComponent<PlayerController>();
        shootingController=weapon.GetComponent<ShootingController>();
    }

    public void InitializeHandler(PlayerConfiguration pc)
    {
        playerConfig = pc;
        playerController.SetupClass(pc.characterClass);
        shootingController.SetupClass(pc.characterClass);
        pc.input.onActionTriggered += onInputCallback;
    }   

    private void OnDestroy() {
        unsubscribeInput();
    }

    public void unsubscribeInput() {
        playerConfig.input.onActionTriggered -= onInputCallback;
    }

    private void onInputCallback(InputAction.CallbackContext ctx)
    {
        switch (ctx.action.name) {
            case "Movement":
                playerController.Move(ctx);
                break;
            case "Dodging":
                playerController.Dodge(ctx);
                break;
            case "Shoot":
                shootingController.OnFire(ctx);
                break;
            case "Aim":
                shootingController.Aim(ctx);
                break;
            case "Reload":
                shootingController.OnReload(ctx);
                break;
            case "Switch Weapon 1":
                shootingController.OnPrevWeapon(ctx);
                break;
            case "Switch Weapon 2":
                shootingController.OnNextWeapon(ctx);
                break;
            case "Skill":
                playerController.UseSkill(ctx);
                break;
            // case "Ready": [deprecated]
            //     PlayerConfigurationManager.Instance.ReadyPlayer(playerConfig.playerIndex);
            //     break;
            case "Use":
                shootingController.OnReplaceWeapon(ctx);
                break;
            case "UpPassive":
                if (ctx.started && playerConfig.characterClass.increasePassivePoints()) {
                    playerController.UpdateClass();
                }
                break;
            case "UpSkill":
                if (ctx.started && playerConfig.characterClass.increaseSkillPoints()) {
                    playerController.UpdateClass();
                }
                break;
            default:
                // Debug.Log(ctx.action.name + " not implemented");
                break;
        }
    }

}
