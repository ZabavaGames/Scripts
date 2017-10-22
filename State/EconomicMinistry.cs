using UnityEngine;

namespace JagaJaga
{

public class EconomicMinistry : Ministry {

	public Person Steward; // казначей
	
	// налоги и подати, пошлины на торговлю, содержание таможни, 
	// строительство инфраструктуры (мосты, дороги, почта) 
	// коррупция (контроль клерков), персональная корруппция (феодалы), 
 	// финансы и развитие торговли (эдикты)
	// сельхоз (фермы) и промышленность (шахты, плавильни, мастерские), 
	// энергетика (мельницы, уголь), 
	// эдикты и проекты развития, 
	// запросы гильдий
	// комитеты (подразделения)
	public int Treasure, TaxLevel, ImportTariff;
	public double GrossIncome, GrossExpense, MonthIncome, MonthExpense, LastMonthIncome, LastMonthExpense, GrossSaldo, MonthlySaldo;
	public double LostOnCorruption, SpentOnProjects;
	public struct Projects {
		public int x;
	}
	
	
	void Start () {
			MonthIncome = MonthExpense = LastMonthIncome = LastMonthExpense = GrossIncome = GrossExpense = 0;
			Treasure = 100;		TaxLevel = 10;
	}


	public double Expense (double Minus) {
			MonthExpense += Minus;
			Treasure -= (int)Minus;
			return (GrossExpense += Minus);
		}

	public double Income (double Plus) {
			MonthIncome += Plus;
			Treasure += (int)Plus;
			return (GrossIncome += Plus);
		}

	public double MonthUpdate () {
			GrossSaldo = GrossIncome - GrossExpense;
			MonthlySaldo = MonthIncome - MonthExpense;
			LastMonthIncome = MonthIncome;
			LastMonthExpense = MonthExpense;
			MonthIncome = MonthExpense = 0;
			return MonthlySaldo;
		}

}
}
