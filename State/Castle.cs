using UnityEngine;

namespace JagaJaga
{

public class Castle : Settlement {

	public Feudal Owner;
	public string CastleName;
	public int CastleSize, SiegeDefense;
	public double Expense, Payment;
	public struct Build {
		public int Walls, Keep;
		public int Barracks, Stables, Archery;
		public Build (int x) {
				this.Walls = this.Keep = this.Barracks = this.Archery = this.Stables = x;
		}
	}
	public Build Buildings;
	public Army FeudalLevy;

	void Start () {
		CastleSize = 1;
		Buildings = new Build (1);
		SiegeDefense = 1 + Buildings.Walls + Buildings.Keep;
		RaiseArmy ();
		Expense = FeudalLevy.CalculateWages ();
	}

	void Update () {
	}

	public void RaiseArmy () {  // первоначальный сбор армии, значения по умолчанию
		FeudalLevy.SetNumbers ( 50 * Buildings.Stables, 100 * Buildings.Stables, 200 * Buildings.Barracks, 250 * Buildings.Barracks, 250 * Buildings.Archery);
	}

	public void UpdateArmy (int newstables, int newbarracks, int newarchery) {  // пример UpdateArmy(0,1,0);
		FeudalLevy.UpdateNumbers ( 50 * newstables, 100 * newstables, 200 * newbarracks, 250 * newbarracks, 250 * newarchery);
	}
	
	public void RecruitArmy (int HC, int LC, int HI, int LI, int A) {	// пример RecruitArmy (10,0,0,20,30);
		FeudalLevy.UpdateNumbers ( HC, LC, HI, LI, A);
	}

	public void MonthlyUpdate () {
		Expense = FeudalLevy.CalculateWages ();
		SiegeDefense = 1 + Buildings.Walls + Buildings.Keep;
		CastleSize = (int)(Buildings.Walls + Buildings.Keep + Buildings.Archery + Buildings.Barracks + Buildings.Stables) / 5;
	}
	// update замка состоит из двух частей - после апдейте надо вызывать оплату, после этого апдейдится армия
	public void PayWages (double Paym) {
		Payment = Paym;
		if (Payment >= Expense) 	FeudalLevy.IsPaid = true; 
		else FeudalLevy.IsPaid = false;
		FeudalLevy.MonthlyUpdate ();
	}

}
}
