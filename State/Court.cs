using System.Collections;
using UnityEngine;
using System.IO;
using System.Text;

namespace JagaJaga 
{

public class Court : MonoBehaviour {

	public ArrayList People = new ArrayList ();
	public int CourtSize, StartCourtSize;
	public Person NewPerson;

	public string[] RusNameBase = {
		"Иван", "Петр", "Степан"
		};
	private int RusNameBaseSize; 
	public string[] RusNameBaseWomen = {
		"Анна", "Ольга", "Прасковья"
		};
	private int RusNameBaseSizeW; 
	public string[] RusFamilyBase = {
		"Петров", "Иванов", "Сидоров"
		};
	private int RusFamilyBaseSize;
	private int PortraitDBSizeMen, PortraitDBSizeMenOld, PortraitDBSizeMenZnat, PortraitDBSizeWomen;
	
	Display Output;

	void Start () {
	}

	
	// заполняем двор при первом запуске, у нас будет 60 чел.
	public void Init () {
		// получаем кол-во портретов из объектов character в классе display (весь вывод)
		Output = GameObject.Find ("Output").GetComponent<Display>();	
		PortraitDBSizeMen = Output.GetPortraitsCount1();
		PortraitDBSizeMenOld = Output.GetPortraitsCount3();
		PortraitDBSizeMenZnat = Output.GetPortraitsCount2();
		PortraitDBSizeWomen = Output.GetPortraitsCount4();

		// считываем имена
		GetNamesFromFile ();

		// заполняем двор персонами; нам нужно пропорцию между возрастами, поэтому вначале пихаем
		// стариков и знатных, потом молодых; 
		//	не забыть, что это влияет на рспределение постов в Office.FillOffice ()
		StartCourtSize = 60;			
		for (int i=0; i < 10; i++)	{
			AddNewPersonMenZnat ();
			}
		for (int j=0; j < 10; j++)	{
			AddNewPersonMenOld ();
			}
		for (int k=20; k < StartCourtSize; k++)	{
			AddNewPersonMen ();
			}
	}
 
	private void AddNewPerson (string name, Sex sex, int age, int face) {
		Person P = Instantiate (NewPerson);	 // делаем новый экземпляр персоны
		P.gameObject.name = "Персонаж "+CourtSize;
		P.Init (name, sex, age, face);  // рандомизируем
		AddPerson (P);  // 
	}

		public void AddNewPersonMen () {
			string Name = RandomRusName() + " " + RandomRusFamily();
			int face = RandomPortraitMen();
			int age = Random.Range (18, 45);
			AddNewPerson (Name, Sex.Man, age, face);
		}

		public void AddNewPersonMenZnat () {
			string Name = RandomRusName() + " " + RandomRusFamily();
			int face = RandomPortraitMenZnat();
			int age = Random.Range (45, 60);
			AddNewPerson (Name, Sex.Man, age, face);
		}

		public void AddNewPersonMenOld () {
			string Name = RandomRusName() + " " + RandomRusFamily();
			int face = RandomPortraitMenOld();
			int age = Random.Range (60, 75);
			AddNewPerson (Name, Sex.Man, age, face);
		}

		public void AddNewPersonWomen () {
			string Name = RandomRusNameWomen() + " " + RandomRusFamilyWomen();
			int face = RandomPortraitWomen();
			int age = Random.Range (18, 30);
			AddNewPerson (Name, Sex.Women, age, face);
		}


	public void AddPerson (Person P) {
		People.Add (P);
		CourtSize ++;
		}

	public void InsertPerson (Person P, int index) {
		People.Insert (index, P);
		CourtSize ++;
		}

	public void RemovePerson (Person P) {
		People.Remove (P);
		CourtSize --;
	}

	public void SwapPersons (int i, int j) {
		if (i >= j) return;
		Person  P1 = ((Person)People[i]), 
				P2 = ((Person)People[j]);
		People.RemoveAt (i);
		People.Insert (i, P1);
		People.RemoveAt (j);
		People.Insert (j, P2);
	}

	public Person GetPerson (int i) {
		if (i >= 0 && i < CourtSize) return (Person)People[i];
		else return null;
		}

// генерация случайных имен и фамилий для мужчин и женщин
	private string RandomRusName () {
			return RusNameBase[Random.Range (0, RusNameBaseSize)];
		}

	private string RandomRusFamily () {
			return RusFamilyBase[Random.Range (0, RusFamilyBaseSize)];
		}

	private string RandomRusNameWomen () {
			return RusNameBaseWomen[Random.Range (0, RusNameBaseSizeW)];
		}

	private string RandomRusFamilyWomen () {
			string s = RusFamilyBase[Random.Range (0, RusFamilyBaseSize)];
			int l = s.Length - 1;
			if (s[l] == 'в' || s[l] == 'н')
				s += 'а';
			else if (s[l] == 'й') {
				//s = cut(s, 2) + "ая";
				s = s.Substring (0, l - 1) + "ая";
				}
			return s;
		}

/*		private string cut (string s, int rest) {
			string s1;
			for (int i=0; i < s.Length - rest; i++)
				s1[i] = s[i];
			return s1;
		}
*/

	// считывание массива имен из файла
	private void GetNamesFromFile () {
		GetRussianNamesFromFile ();
		}

	private void GetRussianNamesFromFile () {
		TextArray t = Output.MainStory.Massive;
		RusNameBase = t.GetRussianNamesFromFile ();
		RusNameBaseSize = RusNameBase.Length;
		RusNameBaseWomen = t.GetRussianWomenNamesFromFile ();
		RusNameBaseSizeW = RusNameBaseWomen.Length;
		RusFamilyBase = t.GetRussianFamiliesFromFile ();
		RusFamilyBaseSize = RusFamilyBase.Length;
		}

	private int RandomPortraitMen () {
			return Random.Range (0, PortraitDBSizeMen);
	}

	private int RandomPortraitMenOld () {
			return Random.Range (0, PortraitDBSizeMenOld);
	}

	private int RandomPortraitMenZnat () {
			return Random.Range (0, PortraitDBSizeMenZnat);
	}

	private int RandomPortraitWomen () {
			return Random.Range (0, PortraitDBSizeWomen);	
	}


}
}
