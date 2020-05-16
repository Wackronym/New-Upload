//MD5Hash:f388d0aede379adcb27ad572418dd49d;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Collections;
using UnityEngine.Events;


public class BaseUI : MonoBehaviour
{
	public UIManager.State state = UIManager.State.Splash;
	public bool isPopup = false;
	private bool isSoundEnabled = false;


	void Awake(){

		if (PlayerPrefs.HasKey ("isSoundEnabled")) {
			if (PlayerPrefs.GetInt ("isSoundEnabled") == 0) {
				AudioListener.volume = 0f;

			} else {
				AudioListener.volume = 1f;
			}
		} else {
			PlayerPrefs.SetInt ("isSoundEnabled", 1);
		}

	}
	
	public void AddMouseDownEvent(){
		foreach(Button b in GetComponentsInChildren<Button>(true)){//|| b.transform.parent.name == "All" 
			if(b.transform.parent.name == "Characters" || b.transform.parent.name == "CardItem")
				return;
			EventTrigger trigger = b.gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			//if(b.onClick.GetPersistent (0))
			//MethodInfo info = UnityEventBase.GetValidMethodInfo (b.onClick.GetPersistentTarget (0), b.onClick.GetPersistentMethodName (0),new Type[]{b.onClick.GetPersistentTarget (0).GetType()});
			
			//UnityAction execute = () => info.Invoke (b.onClick.GetPersistentTarget (0), new Type[]{b.onClick.GetPersistentTarget (0).GetType()});
			//Debug.Log(b.onClick.GetPersistentTarget (0).GetType());
			entry.callback.AddListener( (eventData) => {this.Invoke(b.onClick.GetPersistentMethodName (0),0f);  } );
			trigger.triggers.Add(entry);
			b.enabled = false;
		}
		
		
	}
	
	public virtual void BackBtnPressed()
	{
		
		//if(GameManager.Instance.menuManager.navigationStack.Count<=2){
		GameManager.Instance.backButton.transform.parent.gameObject.SetActive(true);

		//}
		if (!GameManager.Instance.isProcessing) {
			if (!this.name.Contains ("Intro")) {
				/*GameManager.Instance.googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("Button")
					.SetEventAction("Click")
					.SetEventLabel("Back"));*/
				GameManager.Instance.menuManager.PopMenu ();
				GameManager.Instance.isProcessing = false;
			} else {
				Application.Quit ();
			}
		}


	}

	public void LogEvents(string eventCategory, string eventAction, string eventLabel, long value) {
		//GameManager.Instance.googleAnalytics.LogEvent(eventCategory,eventAction,eventLabel, value);
	}

	public void LogScreens() {
		//GameManager.Instance.googleAnalytics.LogScreen (gameObject.name);
	}

	public virtual void NextButtonPressed()
	{
	}

	public void MenuWillAppear()
	{
	}

	public void MenuDidAppear()
	{
	}

	public void MenuWillDisappear()
	{
	}

	public void MenuDidDisappear()
	{
	}
}


