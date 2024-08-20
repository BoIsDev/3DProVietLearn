using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            ItemPickup itemPickup = dropped.GetComponent<ItemPickup>();
            itemPickup.parenAfterDrag = transform;
            AudioManager.Instance.DropItem();
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            ItemPickup itemPickup = dropped.GetComponent<ItemPickup>();
            GameObject current = transform.GetChild(0).gameObject;
            ItemPickup currentDraggable = current.GetComponent<ItemPickup>();
            currentDraggable.transform.SetParent(itemPickup.parenAfterDrag);
            itemPickup.parenAfterDrag = transform;
            AudioManager.Instance.DropItem();
        }
    }
}
