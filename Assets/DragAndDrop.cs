using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject draggedItem;
	private Vector3 screenPoint;
	public bool changeParent;
	public GameObject newParent;


	public void OnBeginDrag(PointerEventData eventData){
		draggedItem = gameObject;
		draggedItem.transform.SetAsLastSibling();
		if (changeParent == true)
			draggedItem.transform.parent = newParent.transform;
	}

	public void OnDrag(PointerEventData eventData){
		if (draggedItem != null) {
			//draggedItem.transform.position += (Vector3)eventData.delta;	
			screenPoint = Input.mousePosition;
			screenPoint.z = 100.0f; //distance of the plane from the camera
			transform.position = Camera.main.ScreenToWorldPoint (screenPoint);
		}
	}

	public void OnEndDrag(PointerEventData eventData){
		draggedItem = null;
	}
}
