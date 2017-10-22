using UnityEngine;
using System.IO;
using System.Text;

namespace JagaJaga
{

	public enum PersonParameter {
		Health, Courage, Piety, Diplomacy, Economy, Command, Fighting, Intrigue, Intellect, Will
	}
	public enum PersonAbility {

	}

	public enum Sex { Man, Women }

	public enum Culture { Russian, Armenian, Bulgarian, Byzantine, Turks, VikingNorse, German, Slavic, Nomad }
	public enum Religion { Orthodox, Catholic, Jude, Muslim, Slavic, Nordic, Pagan, Sun, Zoroastrian, Shaman, Heretic, Atheist }
	public enum Social { Nobility, Merchants, Peasants, Priests, Craftsmen, Slave }
	public enum Political { Left, Right, Middle, NotInterested }

	public enum Psychotype { Choleric, Flegmatic, Sanguinic, Melancholic } // ???
	public enum Kharakter { Gentle, Average, Rude }


	public class Person : MonoBehaviour {

		public string Name;
		public int Portrait;
		private Posts CurrentPost;
		public int BirthDate, Age;
		public bool isSpecial, isMarried, isFeudal, isPrisoner, isRewarded, isCounsillor, isInvited, isInTrouble;
		public Sex Sex;
		public Culture Culture;
		public Religion Religion;
		public Social SocialGroup;
		public Political PoliticalView;
		public Psychotype Psychology;
		public Kharakter Kharakter;
		public int Relation, RelationPositive, RelationNegative, RelationModifiers;
		public bool RelationView;
		public int PersonalWealth, Prestige, Influence;
		public int MinorHoldings, DomainSize, MaxDomainSize;  // из класса феодал

	//	public string[] ParamString;

		public struct Stat {
			public PersonParameter[] Par;
			public bool[] isRevealed;
			public Stat (int x) {
				Par = new PersonParameter[10];
				isRevealed = new bool[10];
				for (int i=0; i<10; i++) {
					Par[i] = (PersonParameter)Random.Range (0, x);
					isRevealed[i] = false;
					}
			}
			public void SetStat (int[] a) {
				for (int i=0; i<10; i++)
					Par[i] = (PersonParameter)a[i];
			}
			public int[] GetStat () {
				int[] a = new int[10];
				for (int i=0; i<10; i++)
					a[i] = (int)Par[i];
				return a;
			}
			public int CheckStat (PersonParameter x) {
				return (int)Par[(int)x];
			}
		}
		public Stat Stats;
		
		public struct Trait {
			public int Number, Trait1;
			public Trait (int x) {
				Number = 1;
				Trait1 = Random.Range (0, x);
				}
		}
		public Trait Traits;
		public string[] TraitString = { /* пока не придумал */ };


	void Start () {
//			Init ();
		}


		public void Init (string name, Sex sex, int age, int face) {
			Name = name;	Portrait = face;			
			Sex = sex;		Age = age;
			CurrentPost = null;

			isSpecial = isMarried = isFeudal = isPrisoner = isCounsillor = isInvited = isInTrouble = RelationView = false;
			BirthDate = Data.GameDate - Age;
			Stats = new Stat (10); // random 0 - 10
			Traits = new Trait (1); // random 0 - 1
			RelationNegative = Random.Range (-25, 0);
			RelationPositive = Random.Range (0, 25); // для начала рандомно
			Relation = RelationNegative + RelationPositive;
			Religion = Religion.Orthodox;
			Culture = Culture.Russian;
			SocialGroup = Social.Nobility;
			PoliticalView = Political.NotInterested;
			PersonalWealth = Random.Range (0, 20);
			Prestige = Influence = 0;
			Psychology = (Psychotype)Random.Range ((int)Psychotype.Choleric, (int)Psychotype.Melancholic + 1);
			Kharakter = (Kharakter)Random.Range ((int)Kharakter.Gentle, (int)Kharakter.Rude + 1);

		}


		public Posts GetPost () {
			return CurrentPost;
		}

		public Posts SetPost (Posts P) {
			CurrentPost = P;
			if (P == null) isCounsillor = false;	
			else isCounsillor = P.isCounsillor;
			return CurrentPost;
		}

		public int CalculateRelation (Person x) {
			int MutualRelation = 0;
			for (int i=0; i<Traits.Number; i++)
				MutualRelation += x.Traits.Trait1 - this.Traits.Trait1;
			return MutualRelation;
		}

		public int GetRelation () {
			return (Relation = RelationPositive + RelationNegative);
		}

		public string ShowRelation () {
			string s;
			if (!RelationView) s = "?";
			else if (Relation >= 75) s = "[o][o][o][o][o]";
			else if (Relation >= 50) s = "[o][o][o][o][-]";
			else if (Relation >= 0) s = "[o][o][o][-][-]";
			else if (Relation >= -25) s = "[o][o][-][-][-]";
			else if (Relation >= -50) s = "[o][-][-][-][-]";
			else s = "[-][-][-][-][-]";
			return s;
		}

		public string StatsToString () {
			string s1 = "";				
			for (int i=0; i<10; i++)
				s1 += Data.PersonParamString[i] + ": " + (Stats.isRevealed[i] ? ((int)Stats.Par[i]).ToString() : "?") + "  ";
			return s1;
		}


// перенесено из класса феодал
		private void AddHoldings (int i) {
			MinorHoldings += i;
			DomainSize += i;
		}

		private void RemoveHoldings (int i) {
			MinorHoldings -= i;
			DomainSize -= i;
		}


		public bool GrantHoldings (Person Pers) {
			if (this.MinorHoldings > 0) {
				Pers.AddHoldings (1);
				Pers.isFeudal = true;
				this.RemoveHoldings (1);
				if (DomainSize < 1)  this.isFeudal = false;
				return true;
				}
			else return false;
		}

		public bool Invite () {
		// согласен или нет
			return (isInvited = (GetRelation () > -100));
		}
		
		public void Dismiss () {
			isInvited = false;
		}

		// спорная ф-ия определения амбициозности, нужно отталкиваться от персональных качеств
		public int GetAmbition () {
			int flag;
			if (Psychology == Psychotype.Choleric) flag = 2; // амбициозен
			else if (Psychology != Psychotype.Flegmatic) flag = 1; // умеренно
			else flag = 0;										// нет
			return flag;
		}
	
	}
}
