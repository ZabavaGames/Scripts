using UnityEngine;

namespace JagaJaga
{

public class Army : MonoBehaviour {

	public Person Commander;
	public int TotalNumber, Cavalry, HCav, LCav, Infantry, Archers, HInf, LInf;
	public double Morale, Wages;
	public bool IsPaid;

	void Start () {
		Morale = 100; IsPaid = true;
//		TotalNumber = Cavalry = HCav = LCav = Infantry = Archers = HInf = LInf = 0;
	}

	void Update () {
	}
	
	void CalculateNum () {
		Cavalry = LCav + HCav; 
		Infantry = LInf + HInf + Archers;
		TotalNumber = Cavalry + Infantry;
	}

	public void SetNumbers (int HC, int LC, int HI, int LI, int A) {
		HCav = HC; LCav = LC; HInf = HI; LInf = LI; Archers = A;
		CalculateNum ();
	}

	public void UpdateNumbers (int HC, int LC, int HI, int LI, int A) {
		HCav += HC; LCav += LC; HInf += HI; LInf += LI; Archers += A;
		CalculateNum ();
	}

	public double CalculateWages () {
		return Wages = (double)(HCav * 3 + LCav * 2 + HInf * 1.5 + LInf * 0.5 + Archers * 0.5);
	}

	public void MonthlyUpdate () {	
// если не заплатили или по другим причинам мораль падает ниже 50%, люди дезертируют
// сюда надо прикрутить авторитет командира
		if (IsPaid == false) {
			if (Morale > 30) Morale -= 30;
			else Morale = 0;
			}
		else {
				if ( Morale <70) Morale += 30;
				else Morale = 100;
			}
		if (Morale < 50) Deserting ();

	}

	void Deserting () {
		double Lost = (50 - Morale) / 100;
		HCav -= (int)(HCav * Lost); 
		LCav -= (int)(LCav * Lost); 
		HInf -= (int)(HInf * Lost); 
		LInf -= (int)(LInf * Lost); 
		Archers -= (int)(Archers * Lost);
		CalculateNum ();
	}

	}
}
