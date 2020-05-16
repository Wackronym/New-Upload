using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayJumpAnimation : MonoBehaviour {
	public Vector3 pointA;
	public Vector3 pointB;
	public Transform LinkedCell;
	public bool isJump;
	public int jumpRatio;
	public int jumpVal = 0;
	public ParticleSystem effects;
	public GameObject keyBoardPressEffect;
	void Start(){
		effects = null;
		pointA = transform.localPosition;
		if (transform.parent == null) {
			return;
		}
		if (transform.parent.parent.name.Contains ("Keyboard")) {
			jumpVal = 5;

		} else {
			jumpVal = 5;

		}

		//Invoke("LinkTheCell", 1f);
	}
	
	
	public void LinkTheCell(){
		if(transform.parent.name.Contains("Letters")){
			string numericString = new String(transform.parent.name.ToCharArray().Where(c => Char.IsDigit(c)).ToArray());
			int Pos = int.Parse(numericString);
			//LinkedCell = transform.parent.parent.parent.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetChild(Pos-1);
			//transform.parent.parent.parent.parent.parent.GetChild(1).GetChild(0).GetChild(0).GetChild(Pos).name = Pos.ToString();
			//transform.parent.name = Pos.ToString();
		}
		
	}
	void OnCollisionExit(Collision coll) {
        if (coll.gameObject.tag == "Step" && !isJump){
			StopAllCoroutines();
			transform.localPosition = pointA;
			transform.parent.parent.parent.parent.parent.GetChild(0).BroadcastMessage("Jump");
			print (transform.parent.parent.parent.parent.parent.name);
		}
        
    }
	
	void OnCollisionwEnter(Collision coll) {
        if (coll.gameObject.tag == "Step" && !isJump){
			
			transform.parent.parent.parent.parent.parent.GetChild(0).BroadcastMessage("Jump");
		}
        
    }
	public IEnumerator Jump()
     {
			isJump = true;
			while (isJump) {
				
			pointB = new Vector3(transform.localPosition.x, transform.localPosition.y+jumpVal, transform.localPosition.z-jumpVal);
				yield return StartCoroutine(MoveObject(transform, pointA, pointB, 3.0f));
				yield return StartCoroutine(MoveObject(transform, pointB, pointA, 3.0f));
				isJump = false;
			}
			
		
     }
  	

	public IEnumerator PressButton()
	{
		Debug.Log ("called");
		isJump = true;
		if (effects == null && !transform.parent.parent.name.Contains ("Keyboard")) {
			//effects = Instantiate (GameManager.Instance.effects, new Vector3 (0, 0, 0), Quaternion.identity);
			//effects.transform.parent = this.transform;
			//effects.gameObject.SetActive (true);
			//effects.transform.localPosition = new Vector3 (0, 40, 45);
		} else if (transform.parent.parent.name.Contains ("Keyboard")){
			if (keyBoardPressEffect == null) {
				keyBoardPressEffect = Instantiate (this.gameObject);
				keyBoardPressEffect.transform.position = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 15, this.gameObject.transform.position.z);
				keyBoardPressEffect.transform.localScale = new Vector3 (0.2f, 0.25f, 0.2f);
				keyBoardPressEffect.transform.parent = this.gameObject.transform.parent.parent;
				Invoke ("DeleteDummy", .2f);
			}
		}
		while (isJump) {

			pointB = new Vector3(transform.localPosition.x, transform.localPosition.y-jumpVal, transform.localPosition.z-jumpVal);
			yield return StartCoroutine(MoveObject(transform, pointA, pointB, 1.0f));
			yield return StartCoroutine(MoveObject(transform, pointB, pointA, 1.0f));

			isJump = false;
		}


	}
	void DeleteDummy() {
		if(keyBoardPressEffect!=null)
			DestroyImmediate (keyBoardPressEffect);
	}

     IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
     {
         var i= 0.0f;
         var rate= 10.0f/time;
         while (i < 1.0f) {
             i += Time.deltaTime * rate;
             thisTransform.localPosition = Vector3.Lerp(startPos, endPos, i);
             yield return null; 
         }
     }

	public void StopAllEffects(){
		if(effects!=null)
			DestroyImmediate (effects.gameObject);
	}
	
}
