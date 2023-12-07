using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SNDCharacterToggle : MonoBehaviour
{
    public GameObject secondCharacter;
    public Toggle toggle;
    private Rigidbody secondCharacterRigidbody;

    private void Start()
    {
        if(toggle == null)
        {
            Debug.LogError("Toggle nie został przypisany!");
            return;
        }

        // Sprawdź, czy obiekt Second_Character ma komponent Rigidbody
        secondCharacterRigidbody = secondCharacter.GetComponent<Rigidbody>();
        if (secondCharacterRigidbody == null)
        {
            Debug.LogError("Obiekt Second_Character musi mieć komponent Rigidbody!");
            return;
        }

        // Dodaj metodę obsługi zdarzenia dla zmiany stanu toggle
        toggle.onValueChanged.AddListener(ChangeHeight);
    }

    private void ToggleSecondCharacter(bool isActivated)
    {
        if (secondCharacter != null)
        {
            secondCharacter.SetActive(isActivated);
        }
    }

    private void ChangeHeight(bool isActivated)
    {
        float targetHeight = isActivated ? 1f : -4f;
        Vector3 newPosition = new Vector3(secondCharacter.transform.position.x, targetHeight, secondCharacter.transform.position.z);
        secondCharacter.transform.position = newPosition;

        // Kontrola grawitacji w zależności od pozycji Y
        secondCharacterRigidbody.useGravity = targetHeight == 0f;
    }

}
