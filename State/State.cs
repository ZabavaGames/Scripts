using UnityEngine;

namespace JagaJaga
{

public class State : Province {

	public string StateName;
	public Feudal Ruler;
	public int Treasure; 
	public Subjects Subjects; // сделать ненаследуемым?
	public Office Government;
	public Court Dvor;
	public EconomicMinistry Kazna;
	public MilitaryMinistry Voisko;
	public SocialMinistry Church;
	

	void Start () {
		SetInitialState ();

		Dvor.Init ();
		Government.Init ();
		// после инита двора и пр-ва нужно посадить в пр-во людей из двора
		Dvor = Government.FillOffice (Dvor); 

		InitRuler ();   // а также сажаем кинга
		InitMentor ();	// сажаем ментора

	}
	
	void SetInitialState () {
		Treasure = 100;
		StateName = "Larosskoe Knyazjestvo";
		Capital.CityName = "Larossa";
		Citadel.CastleName = "Krom";
		Subjects.Provinces[0].ProvinceName = "Larossa province";
		Church.StateReligion = Religion.Orthodox;
		}


	void InitRuler () {
		Ruler = (Feudal)GameObject.Find("Main Hero").GetComponent<Person>(); 
		Ruler.Init (Data.MainHeroName, Sex.Man, 18, Data.MainHeroFace);
		Ruler.isSpecial = true;
		int[] a = {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10};
		Ruler.Stats.SetStat ( a );
		Ruler.AddHolding (Capital);	
		Ruler.AddHolding (Citadel);	
		Ruler.MinorHoldings = 3; 	
		Ruler.DomainSize += 3;		
		Ruler.isFeudal = true;
		Ruler.SetPost (Government.King.Replace (Ruler, null));
		Dvor.InsertPerson (Ruler, 0);
	}


	void InitMentor () {
		Person M = GameObject.Find("Uncle Vasya").GetComponent<Person>(); 
		M.Init (Data.MentorName, Sex.Man, 50, Data.MentorFace);
		M.isSpecial = true;
		M.Relation = 100;
		M.SetPost (Government.Mentor.Replace (M, null));  
		Dvor.InsertPerson (M, 1);
	}

	// раз в месяц все пересчитываем
	override public void MonthlyUpdate () {
		double Bill;

		Subjects.Provinces[0].MonthlyUpdate ();

		Capital.TaxLevel = Kazna.TaxLevel;  // это значение могло поменяться в форме экрана
		Capital.MonthlyUpdate ();
		Bill = Capital.CollectTax ();
		Kazna.Income (Bill);    // Kazna.TaxLevel = Capital.TaxLevel;
		Treasure += (int)Bill;
		
		Citadel.MonthlyUpdate ();
		if ((Bill = Citadel.Expense) <= Treasure)	{
			Citadel.PayWages (Bill);	
			Treasure -= (int)Bill;
			Kazna.Expense (Bill);
			}
		else Citadel.PayWages (0);

		Kazna.MonthUpdate ();
// тут надо проапдейдить войско и прочих, подбить расходы, просчитать новые перки и отношения двора и т.д.

		}


	}	
}

