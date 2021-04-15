using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericInputs : MonoBehaviour
{
    public SpawnItemIcons spawnItemIcons;
    public ItemDescription itemDescription;
    public ButtonGroup buttonGroup;
    public GameObject inventoryUI;
    public PlayerMovement PlayerMovementVar { get; private set; }
    public CameraMomement cameraMomement;

    private void Start()
    {
        SetPlayerVariables();
    }

    void SetPlayerVariables()
    {
        //don't just put in start incase i want more player characters

        PlayerMovementVar = cameraMomement.player.GetComponent<PlayerMovement>();
        inventory = PlayerMovementVar.GetComponent<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) //print F
        {
            StopPrintF();
            curr = StartCoroutine(PrintF());
        }

        if (Input.GetButtonDown("Main Menu"))
        {
            OpenMainManu();
        }
    }

    Coroutine curr;
    public Image f;
    IEnumerator PrintF()
    {
        //print F

        Color color = f.color;
        for (int i = 0; i < 24; i++)
        {
            color.a = 1f - i * .042f;
            f.color = color;
            yield return new WaitForSeconds(.042f);
        }

        color.a = 0;
        f.color = color;
        curr = null;
    }

    void StopPrintF()
    {
        //stop print F

        if (curr != null)
        {
            StopCoroutine(curr);
            curr = null;
        }
    }

    bool menuOpen;
    public GameObject menu;
    public void OpenMainManu()
    {
        //shows and hides the main menu
        menuOpen = !menuOpen;

        if (menuOpen)
        {
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cameraMomement.canLook = false;
            PlayerMovementVar.canMove = false;
        }
        else
        {
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameraMomement.canLook = true;
            PlayerMovementVar.canMove = true;
        }
    }

    public void Save() //called from save button event
    {
        SaveFile saveFile = new SaveFile(inventory.Items.ToArray(), inventory.HandIndex());
        SaveManager.Save(saveFile);
    }

    Inventory inventory;
    public void Load() //called from load button event
    {
        SaveFile saveFile = SaveManager.Load();
        inventory.Items = new List<IndividualItem>(saveFile.items);
        if (saveFile.handIndex != -1) //if not empty
        {
            inventory.SetHand(inventory.Items[saveFile.handIndex]);
        }
        else
        {
            inventory.SetHand(null);
        }
    }

    public void Exit() //called from exit button event
    {
        Application.Quit();
    }
}
