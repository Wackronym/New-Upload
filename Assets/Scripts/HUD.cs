using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour {

	void Start()
	{
		AddMouseDownEvent();
	}
	public void AddMouseDownEvent(){
		foreach(Button b in GetComponentsInChildren<Button>(true)){//|| b.transform.parent.name == "All"
			EventTrigger trigger = b.gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener( (eventData) => {GameManager.Instance.Invoke(b.onClick.GetPersistentMethodName (0),0f);  } );
			trigger.triggers.Add(entry);
			b.enabled = false;
		}
		
		
	}
}
