using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject draggedItem;

	public void OnBeginDrag(PointerEventData eventData){
		draggedItem = gameObject;
		draggedItem.transform.SetAsLastSibling();
	}

	public void OnDrag(PointerEventData eventData){
		if (draggedItem != null)
			draggedItem.transform.position += (Vector3)eventData.delta;	
	}

	public void OnEndDrag(PointerEventData eventData){
		draggedItem = null;
	}
}
