using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUplevel : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ItemPickup itemPickup = dropped.GetComponent<ItemPickup>();

        if (dropped.name == "StoneRed")
        {
            dropped.transform.SetParent(itemPickup.parenAfterDrag);
        }
        else
        {
            itemPickup.parenAfterDrag = transform;
        }
    }
}
