//GameManager...
//.....

public void Do_Touch()
	{
		XInt upMoney = StatData.pTouchEarnmoney;
		bool isCritical = UnityEngine.Random.Range(0f, 100f) < StatData.pTouchCriticalPercent;
		if (isCritical)	
			upMoney = XIntegerExtension.MultiplyWithFloat(upMoney, StatData.pTouchCriticalMul);

		//bool isUltraBonus = UnityEngine.Random.Range(0f, 100f) < StatData.ptouch
		if (UnityEngine.Random.Range(0, 50) == 1)
			(UIManager.instance.GetPopUp(E.POPUP_TYPE.PANEL_HUD) as PopUp_Panel_HUD).SetUltraBonus();
		_gameData.Money_Get(upMoney);

		subj_Touch.OnNext(new TouchSubj(Input.mousePosition, isCritical, upMoney));
	}
	
	public void Do_HeroLvUp(long value)
	{
		long heroLv = _gameData.pHeroLevel;
		long maxUpgradeHeroLv = DataContainer.GetCaculate_CanUpgradeHeroLv(heroLv, GameData.instance.pMoney);
		long before = _gameData.pHeroLevel;
		if (value == -1) value = maxUpgradeHeroLv;
		if (maxUpgradeHeroLv < value)
		{
			UIManager.instance.ToastMessage(E.TOASTMESSAE_TYPE.NOT_ENOUGH_MONEY);
			return;
		}
		if (heroLv + value > GlobalValues.MAXHEROLV)
		{
			UIManager.instance.ToastMessage(E.TOASTMESSAE_TYPE.MAX_LEVEL);
			return;
		}
		if (_gameData.Money_Use(DataContainer.GetCaculate_HeroLvUpPriceVl(heroLv, value)) == false)
		{
			UIManager.instance.ToastMessage(E.TOASTMESSAE_TYPE.NOT_ENOUGH_MONEY);
			return;
		}
		//SoundManager.Instance.PlaySfx_Upgrade();
		_gameData.pHeroLevel += (int)value;
		long after = _gameData.pHeroLevel;
		subj_HeroLvUp.OnNext(new HeroLvUpSubj((int)before, (int)after));
		StatData.RefreshTouchEarnMoney();
		CheckSomeHeroLv();
		Save();
	}
