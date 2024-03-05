using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    public Color hoverColor = Color.white;
    private Color originalColor;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = originalColor;
    }
}
