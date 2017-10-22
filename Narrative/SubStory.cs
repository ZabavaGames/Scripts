using UnityEngine;
using System;
using System.Collections.Generic;


namespace JagaJaga {

public class SubStory : Dialogue {

	public string SubName;
//	public Action StartCondition;
//	public Action[] StageCondition;
	public bool isReady, isStarted, isFinished;
	public new int count;
	public int index, category;
	public List<Dialogue> DialogueList = new List<Dialogue>();
	public List<Action> StageCondition = new List<Action>();
	public Story MainStory;

	// Use this for initialization
	void Start () {
			isReady = isStarted = isFinished = false;
			count = index = category = 0;
			SubName = "";
		//	StartCondition = null;
	}
	
	public void Sub_Init (Action cond) {
	//	StartCondition = cond;
	}

	public void Sub_Set (Action cond, int stage) {
		StageCondition[stage] = cond;
	}


	public bool Sub_Check () {
			if (isFinished) isReady = false;
			else if (!isStarted && StageCondition[0] != null) {	
				StageCondition[0] ();
				isReady = MainStory.GetSubStoryFlag ();
				}
			else if (isStarted && StageCondition[index] != null) {
				StageCondition[index] ();
				isReady = MainStory.GetSubStoryFlag ();
				}
Debug.Log ("чекаем " + SubName + " " + isReady); 
			return isReady;
	}

	// история линейна, после перехода к следующему диалогу нельзя вернуться назад
	public Dialogue Sub_NextDialogue () {
		Dialogue d = null;
Debug.Log (" " +SubName + " " + index + "isFinished " + isFinished); 
			if (Sub_Check ()) {
				if (!isStarted) 
					isStarted = true;
				if (index < count)
					d = DialogueList[index++];
				if (index >= count)
					isFinished = true;
				}
			return d;
	}

	public void Sub_AddDialogue (Dialogue d, Action cond) {
			DialogueList.Add (d);
			StageCondition.Add (cond);
			count = DialogueList.Count;
	}


}
}
