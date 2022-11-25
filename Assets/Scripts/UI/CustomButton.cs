using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite clickSprite;

    private Image image;

    void Awake() {
        image = GetComponent<Image>();
        image.sprite = baseSprite;
    }

    public void OnDeselect(BaseEventData eventData) {
        image.sprite = baseSprite;
    }

    public void OnPointerDown(PointerEventData eventData) {
        image.sprite = clickSprite;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerUp(PointerEventData eventData) {
        image.sprite = hoverSprite;
    }

    public void OnSelect(BaseEventData eventData) {
        image.sprite = hoverSprite;
    }
}
