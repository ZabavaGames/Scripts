using UnityEngine;
using System;
using System.Collections.Generic;


namespace JagaJaga {

public class Sequence : SubStory {

	private List<SubStory> StoryLine = new List<SubStory>();
	public new int count;
	public Story MainStory;

	// Use this for initialization
	void Start () {
			count = 0;	
	}

	// делаем простую линейную последовательность диалогов
	public SubStory PrepSubStory (string subname, int mode, Dialogue[] d, bool isReady) {
			SubStory sub;
			if ((sub = Seq_GetOne (subname)) != null)
				return sub;
			sub = new SubStory ();
			sub.Sub_Init (null);
	//		sub.Sub_Set (null, 0);
			sub.SubName = subname;
			sub.category = (int)mode;
			sub.MainStory = MainStory;
			for (int i=0; i < d.Length; i++)
				sub.Sub_AddDialogue (d[i], null);
			sub.isReady = isReady;
			Seq_Add (sub);
			return sub;
	}

	public SubStory Seq_GetOne (int i) {
			if (i < StoryLine.Count)		
				return StoryLine[i];
			else return null;
	}

	public SubStory Seq_GetOne (string s) {
			SubStory sub;
			for (int i=0; i < StoryLine.Count; i++) {
				sub = StoryLine[i];
				if (String.Compare(sub.SubName, s) == 0)
					return sub;
				}
			return null;
	}

	private void Seq_Add (SubStory s) {
		StoryLine.Add (s);
		count = StoryLine.Count;
Debug.Log ("добавляем " + s.SubName + " " + count);
	}

	public void Seq_ClearFinished () {
			for (int i=0; i<StoryLine.Count; i++) {
Debug.Log ("проверяем " + StoryLine[i].SubName);
				if (StoryLine[i].isFinished) {
Debug.Log ("удаляем " + StoryLine[i].SubName);
					StoryLine.RemoveAt (i);
				}
			}
	}

}
}
