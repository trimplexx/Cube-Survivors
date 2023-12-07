using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public float baseDamage; // Podstawowe obra¿enia


    [SerializeField]
    public float damageScaleFactor = 1.3f;

    public Shooting shootingScript;

    public int damage
    {
        get
        {
            shootingScript = FindObjectOfType<Shooting>();
            // Oblicz obra¿enia na podstawie numeru fali
            if (shootingScript.level > 2)
            {
                return Mathf.RoundToInt(baseDamage * Mathf.Pow(damageScaleFactor, SpawnerScript.waveNumber)) + 30;
            }
            else
            {
                return Mathf.RoundToInt(baseDamage * Mathf.Pow(damageScaleFactor, SpawnerScript.waveNumber));
            }
        }
    }
}