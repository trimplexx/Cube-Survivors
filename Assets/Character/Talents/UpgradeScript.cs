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
        if (shootingScript == null)
        {
            Debug.LogError("Shooting script not found on any GameObject in the scene.");
        }
        if (playerMovement == null)
        {
            Debug.LogError("Shooting script not found on any GameObject in the scene.");
        }
    }

    public void UpgradeLevel()
    {
        if (shootingScript.level == 6 && shootingScript.points >= 1)
        {
            shootingScript.points -= 1;
            shootingScript.level = 1;
            upgradeButtonText.text = "1";
        }
        else
        {
            Debug.LogError("Level already upgraded or not enough points.");
        }
    }

    public void UpgradeShootingSpeed()
    {
        if (shootingScript.shootingSpeed <= 20 && shootingScript.points >= 1)
        {
            shootingScript.shootingSpeed += 10;
            shootingScript.points -= 1;
            shootingSpeedButtonText.text = "1";
        }
        else
        {
            Debug.LogError("Speed already upgraded or not enough points.");
        }
    }

    public void UpgradeFireBall()
    {
        if (shootingScript.isFire == false && shootingScript.isFrost == false && shootingScript.points >= 1)
        {
            shootingScript.points -= 1;
            shootingScript.isFire = true;
            fireBallButtonText.text = "1";
        }
        else
        { 
            Debug.LogError("FireBall already upgraded, FrozenBall is active or not enough points.");
        }
    }

    public void UpgradeFrozenBall()
    {
        if (shootingScript.isFrost == false && shootingScript.isFire == false && shootingScript.points >= 1)
        {
            shootingScript.points -= 1;
            shootingScript.isFrost = true;
            frozenBallButtonText.text = "1";
        }
        else
        {
            Debug.LogError("FrozenBall already upgraded, FireBall is active or not enough points.");
        }
    }

    public void UpgradeDash()
    {
        if (dashButtonText.text == "0" && shootingScript.points >= 1)
        {
            shootingScript.points -= 1;
            dashButtonText.text = "1";
            playerMovement.SetDashAbility(true);
        }
        else
        {
            Debug.LogError("Dash already upgraded or not enough points.");
        }
    }

    public void UpgradeFinisher()
    {
        if (finisherButtonText.text == "0" && shootingScript.points >= 1)
        {
           
            shootingScript.points -= 1;
            finisherButtonText.text = "1";
            shootingScript.ActivateShootInAllDirections(true);
        }
        else
        {
            Debug.LogError("Finisher already upgraded to maximum or not enough points.");
        }
    }

    public void ResetUpgrades()
    {
        int pointsRefunded = 0;

        // Przywracamy punkty za ka¿de dokonane ulepszenie
        if (upgradeButtonText.text == "1")
        {
            pointsRefunded += 1; // Liczba punktów do przywrócenia za ulepszenie poziomu
            shootingScript.level = 6; // Resetujemy poziom na domyœlny
            upgradeButtonText.text = "0";
        }

        if (shootingSpeedButtonText.text == "1")
        {
            pointsRefunded += 1; // Liczba punktów do przywrócenia za ulepszenie prêdkoœci strzelania
            shootingScript.shootingSpeed = 20; // Resetujemy prêdkoœæ strzelania na domyœln¹
            shootingSpeedButtonText.text = "0";
        }

        if (fireBallButtonText.text == "1")
        {
            pointsRefunded += 1;
            shootingScript.isFire = false;
            fireBallButtonText.text = "0";
        }

        if (frozenBallButtonText.text == "1")
        {
            pointsRefunded += 1;
            shootingScript.isFrost = false; 
            frozenBallButtonText.text = "0"; 
        }

        if (dashButtonText.text == "1")
        {
            pointsRefunded += 1;
            dashButtonText.text = "0";
            playerMovement.SetDashAbility(false); 
        }

        if (finisherButtonText.text == "1")
        {
            pointsRefunded += 1;
            finisherButtonText.text = "0";
            shootingScript.ActivateShootInAllDirections(false);
        }

        if (pointsRefunded > 0)
        {
            shootingScript.points += pointsRefunded; // Przywracamy punkty
        }
    }
}
