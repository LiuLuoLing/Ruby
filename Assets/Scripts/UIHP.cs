using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHP : MonoBehaviour
{
    public Image mask;

    private float originalSize;

    public static UIHP instance { get; private set; }

    public bool hasTask;
    public int fixedNum;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    void Update()
    {

    }

    //设置当前UI血条显示的值
    public void SetValue(float fill)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, originalSize * fill);

    }

}
