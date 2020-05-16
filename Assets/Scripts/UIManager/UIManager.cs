using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Ghilman
public enum AuthenticateState
{
    Login,
    UpdateProfile,
    Signup,
    changePassword,
}
//Ghilman

public class UIManager : MonoBehaviour {

	public enum Mode
	{
		Prefabs = 0,
		Objects = 1,
	}
	public enum State
	{
		Splash = 0,
		MainMenu = 1,
		Authenticate = 2,
		Settings = 3,
		NewRound = 4,
		Game = 5,
		Win = 6,
		Lose = 7,
		Card = 8,
		Shop = 9,
        Resume = 10,
        GiveUPPopUP = 11,
        Ads = 12,
		Profile = 13,
    }
   

    #region Variables
    public BaseUI[] menus = null;
	public UIManager.Mode mode = Mode.Prefabs;
	public State initialState = State.Splash;
	public State currentState = State.Splash;
	public State previousState = State.Splash;
	public System.Action<State> OnStateChange = null;
	public bool isPaused = false;
	public System.Collections.Generic.Dictionary<string, System.Collections.Stack> navigationStacks = new System.Collections.Generic.Dictionary<string, System.Collections.Stack>();
	private System.Collections.Hashtable menuTable = null;
	public System.Collections.Stack navigationStack = null;
	private System.Collections.Hashtable createdMenus = null;
	private bool isUIStateDirty = false;
	private bool isBackBtnPressed = false;
	private bool applicationIsQuitting = false;
    #endregion Variables
    void Awake() 
	{
		Screen.fullScreen = true;
		currentState = initialState;
		OnStateChange += HandleStateChanged;
	}

	void Start () 
	{
		makeUIDirtyState();
		PopulateMenuHashTable();
		InitializeNavigationStack ();
		HideAllMenus ();
		if (navigationStack.Count == 0) 
		{
			PushMenu (initialState);
		}
		else 
		{
			ShowMenu(GetMenuForState(NavigationStackPeek()));
		}
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			
			if (next!= null) {
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield !=null) {
					//if it's an input field, also set the text caret
					inputfield.OnPointerClick(new PointerEventData(EventSystem.current));
				}
				EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
			}
		}
		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape) && !isBackBtnPressed && !GameManager.Instance.menuManager.isPaused) 
		{ 
			isBackBtnPressed=true;
		}
		#endif
	}		
		
	void HandleStateChanged (State g)
	{
		//Debug.unityLogger.Log (g);
	}

	public void ChangeStateTo (State g)
	{
		OnStateChange (g);
	}

	public void SwitchSceneAndState(string sceneName, UIManager.State g)
	{
		SceneManager.LoadScene (sceneName);
		ChangeStateTo(g);
	}

	public void makeUIDirtyState()
	{
		isUIStateDirty = true;
		Invoke ("stopUIDirtyState",2.0f);
	}
	private void stopUIDirtyState()
	{
		isUIStateDirty = false;
	}
	
	private void OnDestroy () 
	{
		
		applicationIsQuitting = true;

		if (navigationStacks.ContainsKey(SceneManager.GetActiveScene().name))
		{
			navigationStacks.Remove(SceneManager.GetActiveScene().name);
		}
		navigationStacks.Add(SceneManager.GetActiveScene().name, navigationStack);
	}


	public void PushMenu(UIManager.State g)
	{	
		if (GetMenuForState(g).isPopup == false) {
			if (navigationStack.Count != 0)
			{
				HideMenuAtState (NavigationStackPeek ());
			}
		}
		navigationStack.Push (g);
		InformUIManager (g);
		ShowMenuAtState (g);
		previousState = currentState;
		currentState = g;
	}
	public void PopMenu()
	{
		if (navigationStack.Count != 0)
		{
			HideMenuAtState (NavigationStackPeek ());
		}
		if(navigationStack.Count==1){
			return;
		}
		navigationStack.Pop ();
		UIManager.State g = NavigationStackPeek ();
		InformUIManager (g);
		ShowMenuAtState (g);
		previousState = currentState;
		currentState = g;
		//GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
	}

	public void PopMenuToState(UIManager.State g)
	{
		if (navigationStack.Count != 0)
		{
			HideMenuAtState (NavigationStackPeek ());
		}
		while (NavigationStackPeek() != g)
		{
			navigationStack.Pop ();

			HideMenuAtState (NavigationStackPeek ());
		}
		InformUIManager (g);
		ShowMenuAtState (g);
		
	}
		
	private void InformUIManager(UIManager.State g)
	{
		ChangeStateTo (g);
	}
		
	private void ShowMenuAtState (UIManager.State g)
	{
		switch (g) 
		{
		default:
			ShowMenu(GetMenuForState(g));	
			break;
		}
	}

	private void HideMenuAtState (UIManager.State g)
	{
		switch (g) 
		{
		default:
			HideMenu(GetMenuForState(g));		
			break;
		}
	}
		
	private void HideAllMenus() 
	{
		foreach (DictionaryEntry de in menuTable) 
		{
			BaseUI bm = de.Value as BaseUI;
			//bm.gameObject.SetActive(false);
			HideMenu(bm);
		}
	}
		
	private void ShowMenu(BaseUI bm) 
	{
		if (mode == Mode.Prefabs)
		{
			BaseUI tempBaseUI = createdMenus [bm.state] as BaseUI;
			if (tempBaseUI != null)
			{
				tempBaseUI.MenuWillAppear ();
				tempBaseUI.gameObject.SetActive (true);
				tempBaseUI.MenuDidAppear ();
				return; 
			}
			BaseUI newBM = Instantiate (bm) as BaseUI;
			newBM.transform.SetParent( this.transform.GetChild(0));
			newBM.transform.localScale = bm.transform.localScale;
			//RectTransformExtensions.SetSize( newBM.GetComponent<RectTransform>(), new Vector2(0f,0f));
			//RectTransformExtensions.SetDefaultScale(newBM.GetComponent<RectTransform>());
			newBM.GetComponent<RectTransform>().sizeDelta = bm.GetComponent<RectTransform>().sizeDelta;
			newBM.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
			if (newBM.isPopup) {
				newBM.GetComponent<RectTransform> ().localPosition = new Vector3 (0, 0, 0f);
			} else {
				newBM.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0f);
			}
			newBM.MenuWillAppear ();
			newBM.gameObject.SetActive (true);
			newBM.MenuDidAppear ();
			createdMenus.Add (bm.state, newBM);
		}
		else if (mode == Mode.Objects)
		{
			bm.MenuWillAppear ();
			bm.gameObject.SetActive (true);
			bm.MenuDidAppear ();
		}
	}

	private void HideMenu(BaseUI bm) 
	{
		if (mode == Mode.Prefabs)
		{
			BaseUI previousBM = createdMenus [bm.state] as BaseUI;
			if (previousBM != null) 
			{
				previousBM.MenuWillDisappear ();
				previousBM.gameObject.SetActive (false);
				previousBM.MenuDidDisappear ();
				createdMenus.Remove(bm.state);
				Destroy(previousBM.gameObject);
			}
		}
		else if (mode == Mode.Objects)
		{
			bm.MenuWillDisappear ();
			bm.gameObject.SetActive (false);
			bm.MenuDidDisappear ();
		}
	}
		
	private void PopulateMenuHashTable() 
	{
		menuTable = new Hashtable ();
		createdMenus = new Hashtable ();
		for (int i = 0; i < menus.Length; i++)
		{
			BaseUI bm = menus[i];
			//Debug.Log("Creating Menu " + bm.state);
			menuTable.Add(bm.state, bm);
			//bm.gameObject.SetActive(true);
			bm.gameObject.SetActive(false);
		}
	}
		
	private void InitializeNavigationStack()
	{
		if (navigationStacks.ContainsKey(SceneManager.GetActiveScene().name) == false)
		{
			navigationStack = new Stack ();
		}
		else 
		{
			navigationStack = navigationStacks[SceneManager.GetActiveScene().name];
			navigationStacks.Remove (SceneManager.GetActiveScene().name);
		}
	}

	public BaseUI GetMenuForState(UIManager.State g)
	{
		return menuTable[g] as BaseUI;
	}

	public UIManager.State NavigationStackPeek ()
	{
		return (UIManager.State)navigationStack.Peek ();
	}

	public void PurgeNavigationStack() 
	{
		if (navigationStacks.ContainsKey(SceneManager.GetActiveScene().name) == false)
		{
			navigationStack.Clear();
		}
		else 
		{
			navigationStack.Clear();

			navigationStacks.Remove (SceneManager.GetActiveScene().name);
		}
	}
}