using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextColorStates : MonoBehaviour
{
    //sets color of text in the tabs ui

    TextMeshProUGUI textMeshPro;
    public Color active;
    public Color idle;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    //called from a unity eventsystem in group button
    public void Active()
    {
        textMeshPro.color = active;
    }

    //called from a unity eventsystem in group button
    public void Idle()
    {
        textMeshPro.color = idle;
    }
}