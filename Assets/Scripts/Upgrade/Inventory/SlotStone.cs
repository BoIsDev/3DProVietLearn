using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotStone : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
       
        if(transform.childCount > 0)
        {
            GameObject dropped = eventData.pointerDrag;
            ItemPickup itemPickup = dropped.GetComponent<ItemPickup>();
            GameObject dataChild = transform.GetChild(0).gameObject;
            ItemWeapon itemWeapon = dataChild.GetComponent<ItemWeapon>();
            
            if (itemWeapon.itemCode == ItemCode.Stone)
            {
                itemPickup.parenAfterDrag = transform;
            }
            else
            {
               

                GameObject current = transform.GetChild(0).gameObject;
                ItemPickup currentDraggable = current.GetComponent<ItemPickup>();

                currentDraggable.transform.SetParent(itemPickup.parenAfterDrag);
                itemPickup.parenAfterDrag = transform;
            }
           
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            ItemPickup itemPickup = dropped.GetComponent<ItemPickup>();



          
            itemPickup.parenAfterDrag = transform;
        }


    }
}
