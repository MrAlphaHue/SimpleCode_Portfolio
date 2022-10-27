using System.Collections;
using DG.Tweening;
using ScottGarland;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// It Manage All the PopUp
/// </summary>
public class UIManager : PrelocatedSingleton<UIManager> , ITickable {

	//
	private Dictionary<string, PopUp> _popupDic = new Dictionary<string, PopUp>();
	private List<PopUp> _popupList = new List<PopUp>();
	
	//
    public void InIt()
    {
		//
		_popupDic.Clear();
		_popupList.Clear();

		//
        foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>())
        {
            foreach (PopUp popup in canvas.gameObject.GetComponentsInChildren<PopUp>(true))
            {
				string popupName = popup.name.ToUpper();
				popupName = popupName.Replace("POPUP_", "");

				_popupDic.Add(popupName,popup);
				_popupList.Add(popup);
            }
        }

		foreach (var popUpPair in _popupDic)
		{
			var popUp = popUpPair.Value;

			if (popUp.Gbj.activeSelf == true)
			{
				popUp.Gbj.SetActive(false);
			}


			if (popUp.IsShowAtStart)
			{
				popUp.Open();
			}
		}
    }

    //
    void Update () 
    {
	    if (Input.GetKeyDown(KeyCode.Escape)
#if UNITY_EDITOR
	        || Input.GetKeyDown(KeyCode.B)
#endif
            )
        {
			DebugX.Debug("Here Is Back Key");
			BackKeyInPut();
        }
    }

	void BackKeyInPut()
	{
		if(CloseLastDepthPopUp() == false)
			OpenPopUp(E.POPUP_TYPE.EXIT);
	}
	
	#region POPUPMANAGEMENT
	public PopUp GetPopUp(E.POPUP_TYPE _type)
	{
		if (_popupDic.ContainsKey(_type.ToString().ToUpper()))
			return _popupDic[_type.ToString().ToUpper()];

		return null;
	}

	public PopUp OpenPopUp(E.POPUP_TYPE _type)
	{
		//
		PopUp popup = GetPopUp(_type);

		//
		if(popup == null)
		{
			// Something Error
			DebugX.DebugError("Pop Up Not Found  : " + _type.ToString());
			return null;
		}

		//

		//
		if (popup.Open() == false) 
			DebugX.DebugWarning("Pop Up cant Open , May be Already Opened : " + _type.ToString());

		return popup;
	}
	
	public void ClosePopUP(E.POPUP_TYPE _type)
	{
		//
		GetPopUp(_type).Close();
	}

	public bool IsAllClosablePopUpClosed()
	{
		foreach(PopUp popup in _popupDic.Values)
		{
			if (popup.IsClosable) return false;
		}
		return true;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>return if BackButton Enable At Once</returns>
	public bool CloseLastDepthPopUp()
	{
		for(int i = System.Enum.GetValues(typeof(E.POPUP_TYPE)).Length; i > 0; i --)
		{
			E.POPUP_TYPE _type = (E.POPUP_TYPE)i;
			if (_popupDic.ContainsKey(_type.ToString().ToUpper()))
			{
				PopUp popUp = _popupDic[_type.ToString().ToUpper()];
				if (popUp.IsOpen == false) continue;
				if (_popupDic[_type.ToString().ToUpper()].BackButtonAction() == true)
					return true;
			}
		}

		return false;
	}

    #endregion

    

    #region ConvenienceFunction
	public void ToastMessage(E.TOASTMESSAE_TYPE msgType)
	{
		string msg = TextPlayManager.GetText_ToastMessage(msgType);
		(GetPopUp(E.POPUP_TYPE.PANEL_TOASTMESSAGE) as PopUp_Panel_ToastMessage).InPutToastMessage(msg);
	}

    public void ToastMessage(string message)
    {
		(GetPopUp(E.POPUP_TYPE.PANEL_TOASTMESSAGE) as PopUp_Panel_ToastMessage).InPutToastMessage(message);
    }

	public void AddMessageBox(string mainMsg)
	{
		(GetPopUp(E.POPUP_TYPE.PANEL_MESSAGEBOX) as PopUp_Panel_MessageBox).AddMessage(mainMsg);
	}

	public void AddMessageBox(string mainMsg, string confirmMsg, Action onBtnConfirm)
	{
		(GetPopUp(E.POPUP_TYPE.PANEL_MESSAGEBOX) as PopUp_Panel_MessageBox).AddMessage(mainMsg);
	}
	#endregion


	#region ITickable

	public void Tick_Frame()
	{
		for(int index_popup = 0; index_popup < _popupList.Count; index_popup++)
		{
			//
			if (_popupList[index_popup].IsOpen == false)
				continue;

			//
			_popupList[index_popup].Tick_Update();
		}
	}

	public void Tick_1Sec()
	{
		for (int index_popup = 0; index_popup < _popupList.Count; index_popup++)
		{
			//
			if (_popupList[index_popup].IsOpen == false)
				continue;

			//
			_popupList[index_popup].Tick_1Second();
		}
	}

	public void Tick_3Sec()
	{
		for (int index_popup = 0; index_popup < _popupList.Count; index_popup++)
		{
			//
			if (_popupList[index_popup].IsOpen == false)
				continue;

			//
			_popupList[index_popup].Tick_3Second();
		}
	}

	public void Tick_1Min()
	{
		for (int index_popup = 0; index_popup < _popupList.Count; index_popup++)
		{
			//
			if (_popupList[index_popup].IsOpen == false)
				continue;

			//
			_popupList[index_popup].Tick_1Min();
		}
	}
	
	#endregion


}
