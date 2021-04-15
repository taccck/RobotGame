using System.Collections.Generic;
using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    //https://www.youtube.com/watch?v=211t6r12XPQ
    //a group of buttons where only one can be selected at a time

    public List<GroupButton> buttons; //all buttons in this group
    public Color idle;
    public Color hover;
    public Color active;
    public GroupButton selectedButton;
    public List<GameObject> objectsToSwap; //gameobject that that show and hide when their respecive button is selected and deselected
    public bool startSelected = true; //if group system should start with anything selected

    private void Start()
    {
        OnActivate();
    }

    public void OnActivate()
    {
        if (startSelected)
        {
            if (selectedButton == null) //if a button is not already selected set it to the firt in the button list
            {
                selectedButton = buttons[0];
            }

            OnSelected(selectedButton);

            ResetButtons();
        }
    }

    public void OnEnter(GroupButton button)
    {
        ResetButtons();
        if (selectedButton == null || button != selectedButton) //set button to hover state if not selected
        {
            button.background.color = hover;
        }
    }

    public void OnExit(GroupButton button)
    {
        ResetButtons();
    }

    public void OnSelected(GroupButton button)
    {
        if (selectedButton != null) //calls previously selected buttons deselect method
        {
            selectedButton.Deselect();
        }

        selectedButton = button;
        selectedButton.Select(); //calls selected buttons select method

        ResetButtons();
        button.background.color = active;

        int selectedIndex = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++) //show or hide if its button is selected or deselected
        {
            if (i == selectedIndex)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetButtons()
    {
        foreach (GroupButton button in buttons) //set hover state to idle on all buttons
        {
            if (selectedButton != null && button == selectedButton) { continue; }
            button.background.color = idle;
        }
    }
}