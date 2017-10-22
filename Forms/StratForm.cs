using UnityEngine;

namespace JagaJaga {

public class StratForm : Display {

	private Display Parent;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		StratPanel = GameObject.Find ("StratPanel").GetComponent<CanvasGroup>();
	}


	public bool ShowStratForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowStratPanel (true);
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
