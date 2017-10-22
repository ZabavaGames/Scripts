using UnityEngine;

namespace JagaJaga {

public class DiploForm : Display {

	private Display Parent;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		DiploPanel = GameObject.Find ("DiploPanel").GetComponent<CanvasGroup>();
	}


	public bool ShowDiploForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowDiploPanel (true);
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
