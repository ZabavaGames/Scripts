using UnityEngine;

namespace JagaJaga {

public class GameForm : Display {

	private Display Parent;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		GamePanel = GameObject.Find ("GamePanel").GetComponent<CanvasGroup>();
	}


	public bool ShowGameForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowGamePanel (true);
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
