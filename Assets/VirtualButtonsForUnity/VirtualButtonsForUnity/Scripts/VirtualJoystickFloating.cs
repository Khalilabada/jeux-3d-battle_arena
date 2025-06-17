using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystickFloating : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;
    private Vector2 inputVector;

    [SerializeField] private bool hideOnPointerUp = true; // Cacher le joystick lorsqu'il n'est pas utilisé

    public void OnPointerDown(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        background.position = eventData.position;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x) * 2;
            pos.y = (pos.y / background.sizeDelta.y) * 2;
            inputVector = new Vector2(pos.x, pos.y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handle.anchoredPosition = new Vector2(
                inputVector.x * (background.sizeDelta.x / 2),
                inputVector.y * (background.sizeDelta.y / 2)
            );
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        if (hideOnPointerUp)
        {
            background.gameObject.SetActive(false);
        }
    }

    public float Horizontal()
    {
        return inputVector.x;
    
    }

    public float Vertical()
    {
        return inputVector.y;
    }
}
