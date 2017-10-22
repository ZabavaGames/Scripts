using UnityEngine;

namespace JagaJaga
{

public class Province : MonoBehaviour {

	public string ProvinceName;
	public City Capital;
	public Castle Citadel;
	public int Villages;
	public double FoodProduction, FoodReserve, SupplyLimit, RevoltRisk;
	public Army ProvinceLevy;

	void Start () {
		Villages = 10; 
		FoodProduction = Villages * 1000; 
		FoodReserve = 0;
		RaiseArmy ();
	}

	void Update () {
	}

	void RaiseArmy () {  // первоначальный сбор армии, значения по умолчанию
		ProvinceLevy.SetNumbers ( Villages * 1, Villages * 2, Villages * 3, Villages * 10, Villages * 10);
	}

	virtual public void MonthlyUpdate () {
		FoodProduction = Villages * 1000; 
		SupplyLimit = FoodProduction - Capital.FoodConsumption;
		RevoltRisk = Capital.Disorder;		
		FoodReserve += SupplyLimit;
			if (FoodReserve >= 1500) {
				Villages ++;
				FoodReserve -= 1000;
				}
		RaiseArmy ();
		}

		public void BuildNewVillage (int x) {
			Villages += x;
		}

}
}
