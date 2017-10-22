using UnityEngine;

namespace JagaJaga
{

public class Posts : Bureaucracy {

		public string Titul;
		public double Salary;
		public int MonthlyPrestigeGain, Influence;
		protected Person Holder;
		protected Posts Chief; 		
		public bool isCounsillor;

	void Start () {
		// вводим посты вручную
			
	}
	
		// для первоначальной инициализации
		public Posts SetPost (string Name, double Wage, int Prestige, Person Owner, Posts Up) 	{
			Titul = Name; 	Salary = Wage; 	MonthlyPrestigeGain = Prestige;
			Holder = Owner;		Chief = Up;
			isCounsillor = false;
			return this;
		} 

		// для смены владельца либо начальника
		public Posts Replace (Person Owner, Posts Up) {
//			if (Owner != null) 
				Holder = Owner; // если нулл, значит пост упразднен
			if (Up != null) Chief = Up;  // если нулл, значит без изменений; без подчинения только король
			return this;
		}
		
		public Person GetHolder () {
			if (Holder != null) return Holder;
			else 	return null;
		}

		public Posts GetChief () {
			if (Chief != null) return Chief;
			else 	return null;
		}

}

}
