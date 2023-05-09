using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBar : MonoBehaviour

{
    [SerializeField] private RectTransform barTransform;

    private float barMaxSize;




    public void SetFill (float hpPercentage)
    {
       float size = hpPercentage * barMaxSize;

       barTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }


    // Calls Start Method
    private void Start()
    {
        barMaxSize = barTransform.rect.width;

    }
}
