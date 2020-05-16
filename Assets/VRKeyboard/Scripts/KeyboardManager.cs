/***
 * Author: Yunhan Li 
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace VRKeyboard.Utils {
	using UnityEngine.EventSystems;
    public class KeyboardManager : MonoBehaviour {
        #region Public Variables
        [Header("User defined")]
        [Tooltip("If the character is uppercase at the initialization")]
        public bool isUppercase = false;
        public int maxInputLength;
        
        [Header("UI Elements")]
	    public Text inputText;

        [Header("Essentials")]
        public Transform characters;
        #endregion

        #region Private Variables
        private string Input {
	        get {
	        	if(inputText==null){
	        		inputText = GameManager.Instance.menuManager.transform.GetChild(0).GetComponentInChildren<Game>().slot;
	        	}
		        return inputText.text;  }
	        set { 
	        	if(inputText==null){
	        		inputText = GameManager.Instance.menuManager.transform.GetChild(0).GetComponentInChildren<Game>().slot;
	        	}
		        inputText.text = value;
	        }
        }

        private Dictionary<GameObject, Text> keysDictionary = new Dictionary<GameObject, Text>();

        private bool capslockFlag;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake() {
            
            for (int i = 0; i < characters.childCount; i++) {
                GameObject key = characters.GetChild(i).gameObject;
                Text _text = key.GetComponentInChildren<Text>();
                keysDictionary.Add(key, _text);

                key.GetComponent<Button>().onClick.AddListener(() => {
	                //  GenerateInput(_text.text);
                });
	            key.GetComponent<Button>().enabled = false;
	            EventTrigger trigger = key.gameObject.AddComponent<EventTrigger>();
	            EventTrigger.Entry entry = new EventTrigger.Entry();
	            entry.eventID = EventTriggerType.PointerDown;
	            entry.callback.AddListener((eventData) => {
		            GenerateInput(_text.text);
	            });
	            trigger.triggers.Add(entry);
            }

            capslockFlag = isUppercase;
	        CapsLock();
	        
        }
        #endregion
		
	    public void AddMouseDownEvent(){
		    foreach(Button b in GetComponentsInChildren<Button>(true)){
			    EventTrigger trigger = b.gameObject.AddComponent<EventTrigger>();
			    EventTrigger.Entry entry = new EventTrigger.Entry();
			    entry.eventID = EventTriggerType.PointerDown;
			    entry.callback.AddListener( (eventData) => {
				    this.Invoke(b.onClick.GetPersistentMethodName (0),0f);
			    });
			    trigger.triggers.Add(entry);
			    b.enabled = false;
		    }
		
		
	    }
		
        #region Public Methods
        public void Backspace() {
            if (Input.Length > 0) {
                Input = Input.Remove(Input.Length - 1);
            } else {
                return;
            }
        }

        public void Clear() {
            Input = "";
        }

        public void CapsLock() {
            if (capslockFlag) {
                foreach (var pair in keysDictionary) {
                    pair.Value.text = ToUpperCase(pair.Value.text);
                }
            } else {
                foreach (var pair in keysDictionary) {
                    pair.Value.text = ToLowerCase(pair.Value.text);
                }
            }
            capslockFlag = !capslockFlag;
        }
        #endregion

        #region Private Methods
        public void GenerateInput(string s) {
            if (Input.Length > maxInputLength) { return; }
            Input += s;
        }

        private string ToLowerCase(string s) {
            return s.ToLower();
        }

        private string ToUpperCase(string s) {
            return s.ToUpper();
        }
        #endregion
    }
}