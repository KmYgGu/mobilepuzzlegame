using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeItem : BaseMovingObject
{
    public void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        //Ŭ���ϸ� ��� �ֱ�
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
