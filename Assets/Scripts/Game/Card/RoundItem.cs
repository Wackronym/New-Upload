using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using TMPro;
using UnityEngine.EventSystems;

public class RoundItem : MonoBehaviour {

    //Ghilman
    public Text mainText;
    public Toggle favoriteToggle;
    public RoundData roundData;
    bool isHistory;
    //Ghilman

    public void Init()
	{
   
	}
    //Ghilman 

    /// <summary>
    /// this function will set the values for this object
    /// </summary>
    /// <param name="_itemData"></param>
    /// <param name="_isHistory"></param>
    public void SetItemData(RoundData _itemData, bool _isHistory)
	{
        roundData = _itemData;
        isHistory = _isHistory;

        string[] srtingParts = roundData.mSenence.Split("_"[0]);
        string stringToShow = srtingParts[0];

        mainText.text = stringToShow+ " <color=Blue>"+ roundData.mAnswer+"</color> "+ srtingParts[srtingParts.Length-1];

        if (GameManager.Instance.menuManager.previousState == UIManager.State.MainMenu)
        {
            favoriteToggle.enabled = true;
            if (_isHistory)
            {
               
            }
            else
            {
                favoriteToggle.isOn = true;
            }
        }
        else
        {
            Debug.Log("there is a other thing not main menu");
        }
    } 
    /// <summary>
    /// this function will be called on share button pressed.
    /// </summary>
    public void ShareButtonCall()
    {
        Debug.Log("Share button is pressed");
    }
    /// <summary>
    /// this function will remove the Favorite.
    /// </summary>
    public void RemoveFromFavorite()
    {
        if (GameManager.Instance.menuManager.previousState == UIManager.State.MainMenu)
        {
            if (isHistory)
            {

            }
            else
            {
                GameManager.Instance.shouldSaveFavorites = true;
                GameManager.Instance.favorite[roundData.mainIndex].rData[roundData.innerIndex].reCheck = true;
                GameManager.Instance.favorite[roundData.mainIndex].rData[roundData.innerIndex] = null;
                //this.gameObject.SetActive(false);
                DeleteMySelf();
            }
        }
    }
    void DeleteMySelf()
    {
        //GameObject.Destroy(this.gameObject);
        foreach (Transform t in this.transform)
        {
            t.gameObject.SetActive(false);
        }
        RectTransform MyRectTransform = this.GetComponent<RectTransform>();
        MyRectTransform.sizeDelta = Vector2.zero;
        //this.transform.parent.parent.parent.GetComponent<LoopListView2>().RecycleAllItem();
    }
    void AddInFavorite()
    {
        if (isHistory)
        {
            GameManager.Instance.mItemDataList[GameManager.Instance.cardIndex].rData[roundData.innerIndex].reCheck = false;
        }
    }

    /// <summary>
    /// this will call on value change of toggle.
    /// </summary>
    void OnValuesChangeOfFavoriteToggle()
    {
        if (!favoriteToggle.isOn)
        {
            RemoveFromFavorite();
        }
        else
        {
            AddInFavorite();
        }
    }
    void OnDestroy()
    {
       // this.transform.parent.parent.parent.parent.GetComponent<Favorites>().ResetMyUI();
    }
    //Ghilman

}


