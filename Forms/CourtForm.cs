using UnityEngine;

namespace JagaJaga {

public class CourtForm : Display {

	private Display Parent;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		CourtPanel = GameObject.Find ("CourtPanel").GetComponent<CanvasGroup>();
	}


	public bool ShowCourtForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowCourtPanel (true);
					}
					break; 
			case 10: {
					Parent.ClearPersonFlag ();
					Parent.DisplayClear ();
					return (value = false);
					}
			default: {
					}
					break;
			}

		return value;
	}	


}
}
