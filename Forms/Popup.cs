using UnityEngine.UI;
using UnityEngine;

namespace JagaJaga {

	public class Popup : Display {

		private Text PopupText, ParamText; 
		private Button ConfirmPopup, RefusePopup, Plus, Minus;
		private Display Parent;
		private int param, result, step;

	void Start () {
		param = result = step = 0;

		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		PopupPanel = GameObject.Find ("PopupPanel").GetComponent<CanvasGroup>();
		PopupText = GameObject.Find ("PopupText").GetComponent<Text>();
//		ConfirmPopup = GameObject.Find ("ConfirmPopup").GetComponent<Button>();
//		RefusePopup = GameObject.Find ("RefusePopup").GetComponent<Button>();
//		Plus = GameObject.Find ("Plus").GetComponent<Button>();
//		Minus = GameObject.Find ("Minus").GetComponent<Button>();
		ParamText = GameObject.Find ("ParameterText").GetComponent<Text>();
	}


	public bool ShowPopupForm (int choice) {
		bool value = true;
			Debug.Log (" " + choice);	
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowPopupPanel (true);
					}
					break; 
			case 1: {	// plus
					result += step;
					TextvOkno (ParamText, "" + result);					
					}
					break;
			case 2: {
					if ((result - step) >= 0) { // minus
						result -= step;
						TextvOkno (ParamText, "" + result);
						}
					}
					break;

			case 4: {
					result = param;
					value = false;
					}
					break;
			case 3: {
					value = false;
					}
					break;
			case 10: {
					value = false;
					}
					break;
			default: {
					}
					break;
			}
		
		if (value == false) {
				Parent.ClearPersonFlag ();
				Parent.DisplayClear ();
			}	
		return value;
	}	


	public void SetWindow (string s, int Param, int Step) {
		bool ok;
		result = param = Param;
		step = Step;

		TextvOkno (PopupText, s);
		TextvOkno (ParamText, "" + param);

		ok = Parent.ShowForm (FormList.Popup, 0);

//		if (ok) return result;
//		else return param;
	}

	public int GetResult () {
			return result;
	}
	

}	
}
