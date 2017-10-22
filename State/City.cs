using UnityEngine;

namespace JagaJaga
{

public class City : Settlement {

	public Person Mayor;
	public string CityName;
	public int CitySize, SiegeDefense;
	public double Population, StartPop, BirthRate, FoodConsumption; 
	public double Nobles, Masters, Traders, Clerks, Guards, Monks, Commoners;
	public int TaxLevel, OptimalBurocracy, OptimalPolice;
	public double Income, BaseIncome, Expense, Tax, Efficienсy, Corruption, Criminal, Disorder;
	public bool TaxCollected;
	public int Religion, Culture, Tolerance;
	public double Technology;
	public struct Build {
		public int Walls, Kreml;
		public int GuardBarracks, Arsenal;
		public int Ratusha, Market, Port, Sobor, Hospital, University;
		public int MerchantGuild, KuznetsyGuild, BuildersGuild;
		public int Districts;
		public Build (int x) {
			this.Districts = 12;
			this.Walls = this.Kreml = this.GuardBarracks = this.Arsenal = this.Ratusha = this.Market = this.Port = this.Sobor = this.Hospital = this.University = this.MerchantGuild = this.KuznetsyGuild = this.BuildersGuild = x;
		}
	}
	public Build Buildings;
	public Army GorodovoyPolk;

	void Start () {
		CitySize = 1; SiegeDefense = 1;
		Buildings = new Build (1);
		Population = StartPop = 6000; BirthRate = 5; FoodConsumption = Population;
		Nobles = 100; Traders = Monks = 200; Masters = 500; Guards = Clerks = 50; Commoners = 4900;
		BaseIncome = 1.0; TaxLevel = 10; Technology = 0; OptimalBurocracy = 80; OptimalPolice = 60;
		CalculateTax ();
		RaiseArmy ();
		
	}
	
	double min (double a, double b) {
			if (a > b)
				return b;
			else
				return a;
	}

	
		// рассчитываем доходы и расходы, возвращаем разницу; информационная функция
	double CalculateIncome() {
	
			Criminal = Population / (Guards + 1);
			if (Criminal <= OptimalPolice)
				Criminal = 0.01;
			else
				Criminal = (1 - (OptimalPolice / Criminal)) / 2; // maximum криминала 0.5 или 50%
			
			Efficienсy = Population / (Clerks + 1);  // считаем соотношение госслужащих к населению
			if (Efficienсy <= OptimalBurocracy)
				Efficienсy = 1.0;
			else
				Efficienсy = OptimalBurocracy / Efficienсy; // эффективность сбора налогов стремится к 0 при недостатке клерков
			Corruption = (Guards + Clerks) * 2 / Population;  // коррупция растет вместе с относительным числом госслужащих

			// доходы разных групп отличаются, базовый учитывает общее благосостояние
			Income = BaseIncome * (Commoners * 1 + Masters * 2 + Nobles * 4 + Traders * 5); 
			Income -= Income * Criminal;  // криминал уменьшает доходы
			Expense = Commoners * 0.5 + (Monks + Masters + Clerks + Guards) * 1 + Nobles * 3 + Traders * 2;
			Expense += Expense * Corruption;  // коррупция увеличивает расходы

			return (Income - Expense);
		}

		// считаем сборы в зависимости от уровня налога, плюс все сопутствующее
	private void CalculateTax ()
		{
			double AccumulatedWealth, NaRuki;
			
			TaxCollected = false;
			AccumulatedWealth = CalculateIncome();   // это чистый доход, с которого берется налог

			if (AccumulatedWealth > 0)
				Tax = min (Income * TaxLevel / 100 * Efficienсy, AccumulatedWealth);
			else
				Tax = 0.0;
			NaRuki = (AccumulatedWealth - Tax) / Income;  // сколько остается у населения
			// BaseIncome - коэффициент общего благосостояния, переносится на след. расчетный период
			if (NaRuki < 0.1) {
				BaseIncome -= 0.1;
				NaRuki = -1;
			} else	BaseIncome = 1 + NaRuki / 2;	
		 // уровень недовольства населения
			Disorder = Disorder / 2 + Criminal + Corruption + TaxLevel / 100 - NaRuki / 2 - Buildings.Sobor / 10;  
			if (Disorder < 0)		Disorder = 0.01;
			if (Disorder > 1)		Disorder = 1.0;

		}

	private void CalculatePopulation () {	
		// Birthrate (на самом деле прирост населения) берется за год, а мы считаем среднемесячно, поэтому /12
		double PopPlus = (BirthRate + Buildings.Hospital) / 12 / 100;	// госпитали увеличивают рождаемость
		
		Commoners += Commoners * PopPlus;
		// торговцы плюсуются при низком уровне беспорядка и убегают при высоком, разные здания генерят дополнительных торговцев
		Traders += (PopPlus * Traders + Buildings.Market + Buildings.Port + Buildings.MerchantGuild) * (1 - Disorder * 2);	
		// остальным размноженцам пофиг на беспорядок
		Monks += PopPlus * Monks + Buildings.Sobor + Buildings.University;
		Masters += PopPlus * Masters + Buildings.Market + Buildings.University + Buildings.KuznetsyGuild;
		Nobles += PopPlus * Nobles + Buildings.Ratusha + Buildings.Kreml;
		// клерков и гвардов надо разводить самому, они не размножаются
		Clerks += Buildings.Ratusha + Buildings.University;
		Guards += Buildings.Kreml + Buildings.GuardBarracks;
		
		Population = FoodConsumption = Commoners + Traders + Monks + Guards + Clerks + Masters + Nobles;
	}

	private void RaiseArmy () {
		GorodovoyPolk.SetNumbers ( (int)Nobles / 10, (int)(Nobles + Traders/2) / 10, (int)(Masters + Traders/2) / 10, (int)Commoners / 2 / 20, (int)Commoners / 2 / 20 );
	}

	public void MonthlyUpdate () {
		CalculatePopulation ();
		BalancedAI ();  // автоматическая подгонка, убрать для персонажа!!!!!!!!!!!!!!!!!!
		CalculateTax ();
		CitySize = (int)(Population / StartPop);	
		SiegeDefense = 1 + Buildings.Walls + Buildings.Kreml;
		Technology += Masters / Population * (Buildings.KuznetsyGuild + Buildings.University + 1);
		RaiseArmy ();
	}

	public double CollectTax () {
			TaxCollected = true;
			return Tax;	
	}

	public void BalancedAI () {
		Guards = Population / OptimalPolice;
		Clerks = Population / OptimalBurocracy;
		TaxLevel = 25;
	}

		public void BuildNewDistrict ( int x ) {
			Buildings.Districts += x;
			CitySize = Buildings.Districts / 12;
			Population += 500; FoodConsumption = Population;
	//		Nobles += x * 10; Traders += Monks += x * 20; Masters += x * 50; Commoners += x * 400;
			Commoners += x * 500;
		}


	void Update () {
	}
}
}
