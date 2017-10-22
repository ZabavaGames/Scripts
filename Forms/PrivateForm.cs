using UnityEngine;

namespace JagaJaga {

public class PrivateForm : Display {

	private Display Parent;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		PrivatePanel = GameObject.Find ("PrivatePanel").GetComponent<CanvasGroup>();
	}


	public bool ShowPrivateForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowPrivatePanel (true);
					}
					break; 
			case 10: {
					Parent.ClearPersonFlag ();
					Parent.DisplayClear ();
					Parent.ShowForm (FormList.Empty, 10);
				// Parent.ShowForm (FormList.Main_Form, 10);
				//	Parent.MainStory.main ();
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
