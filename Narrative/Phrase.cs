using UnityEngine;
using System;
using System.Collections.Generic;


namespace JagaJaga {

public class Phrase {

	public Person Actor;
	public List<string> Replika = new List<string>();
	public int count;
	private bool isChoice, isDecision, isFinal;
	private Action Condition, Result;

	// Use this for initialization
	void Start () {
			count = 0;
			Condition = Result = null;
			isChoice = isFinal = isDecision = false;
			Actor = null;
	}


	public bool IsChoice () {
		return isChoice;
	}

	public bool IsFinal () {
		return isFinal;
	}

	public void SetChoice (bool flag) {
		isChoice = flag;
	}

	public void SetDecision (bool flag) {
		isDecision = flag;
	}
 
	public void SetFinal (bool flag) {
		isFinal = flag;
	}

	public void Ph_Init (Action cond, Person actor, string[] words, bool choice, bool decision, bool final, Action result) {
			if (words != null) {
				for (int i=0; i < words.Length; i++)
					Replika.Add (words[i]);
				count += Replika.Count;
				}
			DoInit (cond, actor, choice, decision, final, result);
	}

	public void Ph_Init (Action cond, Person actor, string word, bool choice,  bool decision, bool final, Action result) {
			if (word != null) {
				Replika.Add (word);
				count += Replika.Count;
				}
			DoInit (cond, actor, choice, decision, final, result);
	}

	private void DoInit (Action cond, Person actor, bool choice,  bool decision, bool final, Action result) {
			if (cond != null)
				Condition = cond;
			if (result != null)
				Result = result;
			if (actor != null) Actor = actor;
			isChoice = choice;
			isFinal = final;
			isDecision = decision;
	}

	public void Ph_InitAct (Action cond, Action result) {
			if (cond != null)
				Condition = cond;
			if (result != null)
				Result = result;
	}


	public string Ph_GetReplika (Story MainStory) {
			int flag = 0;
			if (isChoice || isDecision) return null;
			if (Condition != null) {
				Condition ();
				flag = MainStory.GetReplikaFlag ();
				}
			if (flag >= Replika.Count) {
			//	Ph_GetResult (MainStory);  перенес в дисплей.диалог
				flag = 0;
				}
			return Replika[flag];
	}

	public string[] Ph_GetChoice () {
			if (!isChoice) return null;
			string[] r = new string[Replika.Count];
			for (int i = 0; i < Replika.Count; i ++)
				r[i] = Replika[i];
			return r;
	}

	public int Ph_GetDecision (Story MainStory) {
			int flag = 0;
			if (isDecision && Condition != null) {
				Condition ();
				flag = MainStory.GetReplikaFlag ();
				}
			return flag;
	}

	public int Ph_GetResult (Story MainStory) {
			int flag = 0;
			if (Result != null) {
				Result ();
				flag = MainStory.GetReplikaFlag ();
				}
			return flag;
	}


	public string Ph_GetStringbyN (int x) {
			int i = (x < Replika.Count) ? x : 0;
			return Replika[i];
	}



/*	public void StandartReplikaChoiceByRelation (Person Pers) {
			if (Pers.Relation > 25)	
				ExpressionFlag = 0;
			else if (Pers.Relation > -25)
				ExpressionFlag = 1;
			else if (Pers.Relation < -25)
				ExpressionFlag = 2;
	}

	public void StandartReplikaChoiceByPsycho (Person Pers) {
			if (Pers.Kharakter == Kharakter.Gentle)	
				ExpressionFlag = 0;
			else if (Pers.Kharakter == Kharakter.Average)
				ExpressionFlag = 1;
			else if (Pers.Kharakter == Kharakter.Rude)
				ExpressionFlag = 2;
	}

	public void CombinedReplikaChoice (Phrase ph, Person Pers) {
			var pr = Pers.Relation;
			var pk = Pers.Kharakter;
			var flag = ph.ExpressionFlag;
			if (pk == Kharakter.Gentle) {
				if (pr > 0)
					flag = 0;
				else 
					flag = 1;
				}
			if (pk == Kharakter.Average) {
				if (pr > 50)
					flag = 0;
				else if (pr > -50)
					flag = 1;
				else 
					flag = 2;
				}
			if (pk == Kharakter.Rude) {
				if (pr > 0)
					flag = 1;
				else 
					flag = 2;
				}
			ph.ExpressionFlag = flag;
	Debug.Log (pr + " " + pk + " " + flag);
	}
*/

}
}
