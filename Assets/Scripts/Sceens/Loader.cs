using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : BaseUI
{
    public RectTransform rectComponent;

	void Update () {
        rectComponent.Rotate(0f, 0f, 250f * Time.deltaTime);
    }
}
