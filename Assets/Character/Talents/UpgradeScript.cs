using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour
{
    public Shooting shootingScript; // Referencja do skryptu Shooting
    public Player_Movement playerMovement;
    public Second_Movement secondMovement;

    public TextMeshProUGUI upgradeButtonText; // Referencja do tekstu na przycisku ulepszenia
    public TextMeshProUGUI shootingSpeedButtonText;
    public TextMeshProUGUI fireBallButtonText;
    public TextMeshProUGUI frozenBallButtonText;
    public TextMeshProUGUI dashButtonText;
    public TextMeshProUGUI finisherButtonText;


    void Start()
    {
        shootingScript = FindObjectOfType<Shooting>();
        playerMovement = FindObjectOfType<Player_Movement>();
        secondMovement = FindObjectOfType<Second_Movement>();
        if (shootingScript == null)
        {
            Debug.LogError("Shooting script not found on any GameObject in the scene.");
        }
        if (playerMovement == null)
        {
            Debug.LogError("player movement script not found on any GameObject in the scene.");
        }
        if (secondMovement == null)
        {
            Debug.LogError("second movement script not found on any GameObject in the scene.");
        }
    }

    public void UpgradeLevel()
    {
        if (shootingScript.level < 3 && shootingScript.points >= 1)
        {
            shootingScript.points -= 1;
            shootingScript.pointsToRefund += 1;
            shootingScript.level ++;
            shootingScript.ballLvl++;
            upgradeButtonText.text = shootingScript.ballLvl.ToString();
        }
        else
        {
            Debug.LogError("Level already upgraded or not enough points.");
        }
    }

    public void UpgradeShootingSpeed()
    {
        if (shootingScript.shootingSpeed <= 40 && shootingScript.points >= 1)
        {
            shootingScript.shootingSpeed += 2;
            shootingScript.points -= 1;
            shootingScript.pointsToRefund += 1;
            shootingScript.speed++;
            shootingSpeedButtonText.text = shootingScript.speed.ToString();
        }
        else
        {
            Debug.LogError("Speed already upgraded or not enough points.");
        }
    }

    public void UpgradeFireBall()
    {
        if (shootingScript.isFire == false && shootingScript.isFrost == false && shootingScript.points >= 1
            && shootingScript.fire < 2)
        {
            shootingScript.points -= 1;
            shootingScript.pointsToRefund += 1;
            shootingScript.fire++;
            if (shootingScript.fire == 2)
            {
                shootingScript.isFire = true;
            }
            fireBallButtonText.text = shootingScript.fire.ToString();
        }
        else
        { 
            Debug.LogError("FireBall already upgraded, FrozenBall is active or not enough points.");
        }
    }

    public void UpgradeFrozenBall()
    {
        if (shootingScript.isFrost == false && shootingScript.isFire == false && shootingScript.points >= 1
            && shootingScript.frost < 2)
        {
            shootingScript.points -= 1;
            shootingScript.pointsToRefund += 1;
            shootingScript.frost++;
            if (shootingScript.frost == 2)
            {
                shootingScript.isFrost = true;
            }
            frozenBallButtonText.text = shootingScript.frost.ToString();
        }
        else
        {
            Debug.LogError("FrozenBall already upgraded, FireBall is active or not enough points.");
        }
    }

    public void UpgradeDash()
    {
        if (shootingScript.points >= 1 && shootingScript.dash < 3)
        {
            shootingScript.points -= 1;
            shootingScript.pointsToRefund += 1;
            shootingScript.dash++;
            if(shootingScript.dash == 3)
            {
                playerMovement.SetDashAbility(true);
                secondMovement.SetDashAbility(true);
            }
            dashButtonText.text = shootingScript.dash.ToString();
        }
        else
        {
            Debug.LogError("Dash already upgraded or not enough points.");
        }
    }

    public void UpgradeFinisher()
    {
        if (shootingScript.points >= 1 && shootingScript.finisher < 3)
        {
            shootingScript.points -= 1;
            shootingScript.pointsToRefund += 1;
            shootingScript.finisher += 1;
            if (shootingScript.finisher == 3)
            {
                shootingScript.ActivateShootInAllDirections(true);
            }
            finisherButtonText.text = shootingScript.finisher.ToString();
        }
        else
        {
            Debug.LogError("Finisher already upgraded to maximum or not enough points.");
        }
    }

    public void ResetUpgrades()
    {
        shootingScript.points = shootingScript.points + shootingScript.pointsToRefund;
        shootingScript.pointsToRefund = 0;

        shootingScript.finisher = 0;
        shootingScript.ActivateShootInAllDirections(false);
        shootingScript.dash = 0;
        playerMovement.SetDashAbility(false);
        secondMovement.SetDashAbility(false);
        shootingScript.fire = 0;
        shootingScript.isFrost = false;
        shootingScript.frost = 0;
        shootingScript.isFire = false;
        shootingScript.speed = 0;
        shootingScript.shootingSpeed = 20;
        shootingScript.ballLvl = 0;
        shootingScript.level = 1;

        fireBallButtonText.text = shootingScript.fire.ToString();
        frozenBallButtonText.text = shootingScript.frost.ToString();
        dashButtonText.text = shootingScript.dash.ToString();
        finisherButtonText.text = shootingScript.finisher.ToString();
        shootingSpeedButtonText.text = shootingScript.speed.ToString();
        upgradeButtonText.text = shootingScript.ballLvl.ToString();

    }
}
