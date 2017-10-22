using UnityEngine;

namespace JagaJaga {

public class ProjectForm : Display {

	private Display Parent;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		ProjectPanel = GameObject.Find ("ProjectPanel").GetComponent<CanvasGroup>();
	}


	public bool ShowProjectForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowProjectPanel (true);
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
