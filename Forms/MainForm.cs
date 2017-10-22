using UnityEngine.UI;
using UnityEngine;

namespace JagaJaga {

public class MainForm : Display {

	private Display Parent;
	private string[] MainFormStrings;
	private bool MainFirstRun;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		MainPanel = GameObject.Find ("MainPanel").GetComponent<CanvasGroup>();

		Text MainText = GameObject.Find ("MainText").GetComponent<Text>();
		// тест
		MainText.text = Parent.MainStory.Massive.GetStringFromAssets ("maintext");

		MainFirstRun = false;
		MainFormStrings = new string[] {
			"Совет",
			"Приказы",
			"Двор",
			"Армия",
			"Казна",
			"Дворец",
			"Проекты",
			"Задачи",
			"Культура",
			"Шпионаж",
			"Золото: ",
			"Влияние: ",
			"Неделя: ",
			""};
	}


	public bool ShowMainForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowMainPanel (true);
			//		Parent.ShowBackPanel (false);
					FillMainForm ();
				// прогоняем туториал либо сюжетные ивенты
					if (!MainFirstRun) {
						MainFirstRun = true;
						Parent.StoryRun ((int)StoryMode.tutor, FormList.Main_Form);
						}
					else
						Parent.StoryRun ((int)StoryMode.events, FormList.Main_Form);
					}
					break; 
			case 1: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Counsil_Form, 0);
					}
					break;
			case 2: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Office_Form, 0);
					}
					break;
			case 3: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Court_Form, 0);
					}
					break;
			case 4: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Army_Form, 0);
					}
					break;
			case 5: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Trade_Form, 0);
					}
					break;
			case 6: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Dvor_Form, 0);
					}
					break;
			case 7: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Project_Form, 0);
					}
					break;
			case 8: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Task_Form, 0);
					}
					break;
			case 9: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Cult_Form, 0);
					}
					break;
			case 10: {
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Spy_Form, 0);
					}
					break;
			case 11: {
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

	private void FillMainForm () {
		Button Knop;	Text tt;	
		// это просто названия кнопок, переводить не надо
		string[] objectname = {"Совет",	"Правительство","Придворные","Армия","Экономика","Дворец",
			"Проекты","Задачи",	"Культура",	"Шпионаж", "GoldString", "InfoString", "TimeString", ""};

		for (int j = 0; j<10; j++) {
				Knop = GameObject.Find (objectname[j]).GetComponent<Button>();
				TextvOkno (Knop.GetComponentInChildren<Text>(), MainFormStrings[j]);
			}

		tt = GameObject.Find (objectname[10]).GetComponent<Text>();
		TextvOkno (tt, MainFormStrings[10] + Strana.Treasure);
		tt = GameObject.Find (objectname[11]).GetComponent<Text>();
		TextvOkno (tt, MainFormStrings[11] + Strana.Ruler.Influence);
		tt = GameObject.Find (objectname[12]).GetComponent<Text>();
		TextvOkno (tt, MainFormStrings[12] + Parent.MainStory.WeeksNumber ());
		
	}

}
}
