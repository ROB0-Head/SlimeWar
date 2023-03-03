using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _damagePrice;
    [SerializeField] private int _damageBoost;
    [SerializeField] private int _ASPDPrice;
    [SerializeField] private float _ASPDBoost;
    [SerializeField] private int _healthPrice;
    [SerializeField] private int _healthBoost;
    
	[SerializeField] private TMP_Text _atk;
	[SerializeField] private TMP_Text _aspd;
	[SerializeField] private TMP_Text _hp;

	[SerializeField] private TMP_Text _atkPrice;
	[SerializeField] private TMP_Text _aspdPrice;
	[SerializeField] private TMP_Text _hpPrice;

	public event UnityAction<int> PowerPrice;
	public event UnityAction<int> ASPDPrice;
	public event UnityAction<int> HealthPrice;

   private void OnEnable()
   {
	  _aspd.text = _player.CurrentASPD.ToString();
      _atk.text = _player.CurrentDamage.ToString();
      _hp.text = _player.CurrentHealth.ToString();
	_aspdPrice.text = _ASPDPrice.ToString();
      _atkPrice.text = _damagePrice.ToString();
      _hpPrice.text = _healthPrice.ToString();
      _player.PowerChanged += OnPowerChanged;
      _player.ASPDChanged += OnASPDChanged;
		PowerPrice += OnAtkPriceChanged;
      	ASPDPrice += OnASPDPriceChanged;
      	HealthPrice += OnHpPriceChanged;
   }

   private void OnDisable()
   {
      _player.PowerChanged -= OnPowerChanged;
      _player.ASPDChanged -= OnASPDChanged;
      PowerPrice -= OnAtkPriceChanged;
      ASPDPrice -= OnASPDPriceChanged;
      HealthPrice -= OnHpPriceChanged;
   }

   private void OnPowerChanged(int power)
   {
      _atk.text = power.ToString();
   }
 	private void OnASPDChanged(float aspd)
   {
      _aspd.text = aspd.ToString();
   }
		private void OnAtkPriceChanged(int price)
   {
      _atkPrice.text = price.ToString();
   }
		private void OnASPDPriceChanged(int price)
   {
      _aspdPrice.text = price.ToString();
   }
		private void OnHpPriceChanged(int price)
   {
      _hpPrice.text = price.ToString();
   }
	
    public void BuyPower()
    {
        if (_player.Money >= _damagePrice)
        {
            _player.ChangePrice(_damagePrice);
            _player.ChangePower(_damageBoost);
        }
    }
    private void BuyASPD()
    {
        if (_player.Money >= _ASPDPrice)
        {
            _player.ChangePrice(_ASPDPrice);
            _player.ChangeASPD(_ASPDBoost);
        }
    }
    private void BuyHealth()
    {
        if (_player.Money >= _healthPrice)
        {
            _player.ChangePrice(_healthPrice);
            _player.ChangeHealth(_healthBoost);
        }
    }
}
