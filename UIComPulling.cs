using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComPulling<T> where T : UIComponent
{
    //
    public enum SAMPLETYPE
    {
        PREFAB  = 0 ,
        INHIERACHY = 1,
    }
    
    //
    GameObject _gbjSample = null;
    RectTransform _rtParent = null;
    SAMPLETYPE _sampleType = SAMPLETYPE.PREFAB;
    
    //
    public List<T> ComLists { get { return _comLists; } }
    List<T> _comLists = new List<T>();
    
    //
    public void InIt(GameObject gbjsample, RectTransform rtParent , SAMPLETYPE sampleType)
    {
        this._gbjSample = gbjsample;
        this._rtParent = rtParent;
        this._sampleType = sampleType;
        
        if (sampleType == SAMPLETYPE.INHIERACHY)
        {
            _comLists.Add(gbjsample.GetComponent<T>());
            gbjsample.SetActive(false);
        }
    }
    
    //
    public T GenerateObj()
    {
        for (int i = 0; i < _comLists.Count; i++)
        {
            if (_comLists[i].gameObject.activeSelf == false)
            {
                _comLists[i].gameObject.SetActive(true);
                return _comLists[i];
            }
        }

        GameObject gbj = GameObject.Instantiate(_gbjSample, _rtParent);
        gbj.SetActive(true);
        T com = gbj.GetComponent<T>();
        _comLists.Add(com);

        com.Rt.SetParent(_rtParent);
        return com;

    }
    
    //
    public T GetLastActivatedCom()
    {
        for (int i = _comLists.Count - 1; i > -1; i--)
        {
            if (_comLists[i].Gbj.activeSelf == true)
            {
                return _comLists[i];
            }
        }
        return null;
    }
}
