using UnityEngine.UI;
using UnityEngine;

namespace JagaJaga {

	public class PopInfo : Display {

		private Text PopInfoText; 
		private Button ConfirmPopInfo;
		private Display Parent;

	void Start () {

		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		PopInfoPanel = GameObject.Find ("PopInfoPanel").GetComponent<CanvasGroup>();
		PopInfoText = GameObject.Find ("PopInfoText").GetComponent<Text>();
//		ConfirmPopInfo = GameObject.Find ("ConfirmPopInfo").GetComponent<Button>();
	}


	public bool ShowPopInfoForm (int choice) {
		bool value = true;

		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowPopInfoPanel (true);
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


	public void SetWindow (string s) {

		TextvOkno (PopInfoText, s);
		Parent.ShowForm (FormList.PopInfo, 0);
	}


}	
}
