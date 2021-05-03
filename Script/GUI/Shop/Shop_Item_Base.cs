using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Item_Base : MonoBehaviour
{
    protected ExcelData_Google_IAPDataInfo m_pIAPDataInfo = null;

    public void Init(ExcelData_Google_IAPDataInfo pIAPDataInfo)
    {
        m_pIAPDataInfo = pIAPDataInfo;
    }

    public float GetHeight() 
    {
        RectTransform contentRect = gameObject.GetComponent<RectTransform>();
        return contentRect.rect.height;
    } 
}
