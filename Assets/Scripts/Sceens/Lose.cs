using UnityEngine;

public class Lose : BaseUI {

	void Start () {
		Time.timeScale = 0;
        
		base.AddMouseDownEvent();
    }

	public void MainMenu(){
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
		GameManager.Instance.backButton.SetActive(false);
		GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive(true);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }

}
