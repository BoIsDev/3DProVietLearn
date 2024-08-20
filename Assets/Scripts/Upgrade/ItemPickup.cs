using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemPickup : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parenAfterDrag;
    public Image image;

    public void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parenAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        AudioManager.Instance.SelectItem();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDRag");
        transform.SetParent(parenAfterDrag);
        image.raycastTarget = true;
    }
}
