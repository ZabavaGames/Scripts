using UnityEngine;
using System;
using System.Collections.Generic;


namespace JagaJaga {

public class Dialogue : Phrase {

	public Person Pers1, Pers2;
	private List<Phrase> Beseda = new List<Phrase>();
	public new int count;
// схема развития диалога - какая из реплик во фразе-вопрос дает какую фразу-ответ
	private struct Links {
		public int FirstPhrase, ReplikaNumber, SecondPhrase;
		public Action Condition;
		public Links (int x, int y, int z, Action a) {
			FirstPhrase = x; ReplikaNumber = y; SecondPhrase = z;
			Condition = a;
			}
		}
	private List<Links> DialScheme = new List<Links> ();

	// Use this for initialization
	void Start () {
		count = 0;	
		Pers1 = Pers2 = null;	
		
	}
	
	private void D_PInit (Person p1, Person p2) {
			Pers1 = p1; 	Pers2 = p2;
		}

	// создаем диалог с линейным обменом репликами, без вопросов (будет редко использоваться)
	public Dialogue Prep1SDialog (Person p1, Person p2, string[] s, Action DefaultCond) {
			Phrase ph;
			D_PInit (p1, p2);
			for (int i = 0; i < s.Length; i++) {
				ph = new Phrase ();
				ph.Ph_Init (DefaultCond, (i%2 == 0)? p2 : p1, s[i], false, false, false, null);
				D_AddPhrase (ph);
				}
			return this;
	}

	// тоже самое, монолог
	public Dialogue PrepMonolog (Person p1, Person p2, string[] s, Action DefaultCond) {
			Phrase ph;
			D_PInit (p1, p2);
			for (int i = 0; i < s.Length; i++) {
				ph = new Phrase ();
				ph.Ph_Init (DefaultCond, p2, s[i], false, false, false, null);
				D_AddPhrase (ph);
				}
			return this;
	}

	// или создаем диалог с набором фраз, у каждой из которых свой набор строк (основной способ)
	public Dialogue PrepNSDialog (Person p1, Person p2, List<string[]> list, Action DefaultCond, int[][] links, int[] choices, int[] decisions, int[] endings) {
	//		Dialogue d = new Dialogue ();  // больше не нужно, т.к. это метод самого диалога, который уже создали раньше
			Phrase ph;
			D_PInit (p1, p2);
			for (int i = 0; i < list.Count; i++) {
				ph = new Phrase ();
				ph.Ph_Init (DefaultCond, p2, list[i], false, false, false, null);
				D_AddPhrase (ph);
				}

			D_Settings (links, choices, decisions, endings);

			return this;
	}

	// новая версия с DialogInfo
	public Dialogue PrepNSDialog (Person p1, Person p2, DialogueInfo d, Action DefaultCond) {
	//		Dialogue D = new Dialogue ();  // больше не нужно, т.к. это метод самого диалога, который уже создали раньше
			Phrase ph;
			D_PInit (p1, p2);
			for (int i = 0; i < d.list.Count; i++) {
				ph = new Phrase ();
				ph.Ph_Init (DefaultCond, p2, d.list[i], false, false, false, null);
				D_AddPhrase (ph);
				}

			D_Settings (d.links, d.choices, d.decisions, d.finals);

			return this;
	}

		public void D_AddPhrase (Phrase f) {
			Beseda.Add (f);
			count = Beseda.Count;
		}

		public Phrase D_GetPhrase (int x) {
			if (x < Beseda.Count) 
				return Beseda[x]; 
			return null;		
		}

		public Phrase D_GetPhrase (int x, int y) {
			int z;
			if (x < Beseda.Count) 
				if ((z = D_GetLink (x, y)) > 0)
					return Beseda[z]; 
			return null;		
		}

		public int D_GetLink (int x, int y) {
			Links choice;
			for (int i=0; i < DialScheme.Count; i++) {
				choice = DialScheme[i];
				if (choice.FirstPhrase == x && choice.ReplikaNumber == y)
					return choice.SecondPhrase;
			}
			return -1;
		}

		public void D_Settings (int[][] links, int[] choices, int[] decisions, int[] endings) {
			D_SetLinks (links);
			D_SetFlags (choices, decisions, endings);
			D_Set2Persons ();
		}


		public void D_SetLinks (int[][] x) {
			Links linux;
			for (int i = 0; i < x.Length; i++) {
				linux = new Links (x[i][0], x[i][1], x[i][2], null);
				DialScheme.Add (linux);
			}
		}
	
		// надо вызывать после setlinks
		public void D_SetFlags (int[] choices, int[] decisions, int[] endings) {
			D_SetChoices (choices);
			D_SetDecisions (decisions);
			D_SetEndings (endings);
		}

		private void D_SetChoices (int[] choices) {
			Phrase ph;
			for (int j = 0; j < choices.Length; j++) {
				if (choices[j] != 0 && (ph = D_GetPhrase (choices[j])) != null)
					ph.SetChoice (true);
			}
		}

		private void D_SetDecisions (int[] decisions) {
			Phrase ph;
			for (int j = 0; j < decisions.Length; j++) {
				if (decisions[j] != 0 && (ph = D_GetPhrase (decisions[j])) != null)
					ph.SetDecision (true);
			}
		}

		private void D_SetEndings (int[] endings) {
			Phrase ph;
			for (int j = 0; j < endings.Length; j++) {
				if (endings[j] != 0 && (ph = D_GetPhrase (endings[j])) != null)
					ph.SetFinal (true);
			}
		}

		private void D_Set2Persons () {
			Phrase ph;
			for (int i=0; i < count; i++) {
				if ((ph = D_GetPhrase (i)) != null)	{
					if (ph.IsChoice ())
						ph.Actor = Pers1;
					else ph.Actor = Pers2;
					}
				}		
		}

		public void D_SetNPersons (Person Pers, int[] phrases) {
			Phrase ph;
			for (int i=0; i < phrases.Length; i++) {
				if ((ph = D_GetPhrase (phrases[i])) != null)
					ph.Actor = Pers;
				}
		}


}
}
