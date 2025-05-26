using UnityEngine;
using UnityEngine.UI;

public class ChairInteraction : MonoBehaviour
{
    public GameObject playerStanding, playerSitting;
    public GameObject sitButton, standButton;
    public Transform chairPosition;

    private bool playerIsNear = false;

    void Start()
    {
        sitButton.SetActive(false);
        standButton.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            sitButton.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            sitButton.SetActive(false);
        }
    }

    public void Sit()
    {
        if (!playerIsNear) return;

        playerStanding.SetActive(false);
        playerSitting.SetActive(true);
        playerSitting.transform.position = chairPosition.position;
        playerSitting.transform.rotation = chairPosition.rotation;

        sitButton.SetActive(false);
        standButton.SetActive(true);
    }

    public void Stand()
    {
        playerSitting.SetActive(false);
        playerStanding.SetActive(true);

        sitButton.SetActive(false);
        standButton.SetActive(false);
    }
}

