using UnityEngine;

public class Resume : BaseUI {


	void Start () {

		base.AddMouseDownEvent();
    }

	public void MainMenu(){
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
	
}
