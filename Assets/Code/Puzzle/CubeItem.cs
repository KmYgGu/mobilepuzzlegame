using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeItem : BaseMovingObject
{
    public void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        //클릭하면 들고 있기
    }

    public void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
}
