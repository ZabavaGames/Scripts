using UnityEngine.UI;
using UnityEngine;
//using Fungus;

namespace JagaJaga {

public class OfficeForm : Display {

	private Display Parent;
	private Text OfficeText, OfficeName;
	private int OfficeFirstClick;
	private string[] InfoString;
//	public Person[] Pack;
//	public Office Gov;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();	

		OfficeText = GameObject.Find ("OfficeText").GetComponent<Text>();
//		OfficeName = GameObject.Find ("OfficeName").GetComponent<Text>();
		OfficePanel = GameObject.Find ("OfficePanel").GetComponent<CanvasGroup>();

		ArmyScrollView = GameObject.Find ("ArmyScrollView").GetComponent<CanvasGroup>();
		DvorScrollView = GameObject.Find ("DvorScrollView").GetComponent<CanvasGroup>();
		TradeScrollView = GameObject.Find ("TradeScrollView").GetComponent<CanvasGroup>();
		CultScrollView = GameObject.Find ("CultScrollView").GetComponent<CanvasGroup>();

		OfficeFirstClick = 0;

		InfoString = new string[] {
			"Армия",
			"Двор",
			"Экономика",
			"Культура",
			"Нажмите еще раз, чтобы вывести информацию об этом ведомстве."
			};

	}
	

	private Person GetOne (int i) {
		var Gov = Strana.Government;
		
		if (i==0) return Gov.PrimeMinister.GetHolder();
		if (i==1) return Gov.Voevoda.GetHolder();
		if (i==2) return Gov.Dvoretzky.GetHolder();
		if (i==3) return Gov.Kaznachei.GetHolder();
		if (i==4) return Gov.Patriarch.GetHolder();
		if (i==5) return Gov.Mentor.GetHolder();
		if (i==6) return Gov.Tiun.GetHolder();
		else return null;
	}


// показываем форму должностей
	public bool ShowOfficeForm (int choice) {
		bool value = true;		
		Person Pers = null;
		int form = 0;

		switch (choice) {
			case 0: {  // стартуем офис, отрисовка
					Parent.DisplayClear ();
					ShowOfficePanel (true);
					FillOfficeForm ();
					}
					break; 

			case 1:   // премьер
			case 2:   // маршал
			case 3:   // дворецкий
			case 4:   // казначей
			case 5:   // архиерей
			case 6:   // ментор
			case 7: { // тиун
					Pers = GetOne (choice - 1);
					}
					break;

/*			case 8: { // доп. список 
					if (OfficeFirstClick > 0) {  // показывает список подчиненных
						titul = GetOne (OfficeFirstClick).GetPost().Titul;
						Parent.ClearPersonFlag ();
						Parent.BranchList (0, titul);    // вылетает
						}
					}
					break;*/
			case 8: { // список персон
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Court_List, 0);
					}
					break;
			case 9: { // доп. список должностей
					Parent.ClearPersonFlag ();
//					Parent.CourtPostList (0);   // проглатывает тех министров, кто в форме
					Parent.ShowForm (FormList.Court_Post_List, 0);
					}
					break;
			case 11: { // доп. список постов
					Parent.ClearPersonFlag ();
//					Parent.PostsList (0);     // дублирует уволенных
					Parent.ShowForm (FormList.Posts_List, 0);
					}
					break;
			case 10: {  // выход
					OfficeFirstClick = 0;
					Parent.ClearPersonFlag ();
					Parent.DisplayClear ();
					return (value = false);
					}
			//		break;

			case 12: {
					form = 1;
					}
					break;
			case 13: {
					form = 2;
					}
					break;
			case 14: {
					form = 3;
					}
					break;
			case 15: {
					form = 4;
					}
					break;

			default : {
					}
					break;
		}

		if (choice >= 1 && choice <= 7) {
			if (OfficeFirstClick != choice) {
				OfficeFirstClick = choice;
				Parent.PersonInfo.ShowBriefInfo (Pers, OfficeText);
				// отрисовать кнопки быстрых действий с персом
				// ...
				}
			else  {
				if (Pers != null) {  // показать инфу по персу
					Parent.DisplayClear ();
					Parent.ShowPerson (Pers, 0, false);
					}
				else { // если пусто, взять нового перса из списка
					Parent.ClearPersonFlag ();
					Parent.ShowForm(FormList.Court_List, 0);
					}
				}
			}
		else if (choice > 11 && choice < 16) {
				if (OfficeFirstClick != choice) {
					OfficeFirstClick = choice;
					TextvOkno (OfficeText, InfoString[4]);
				}
				else CallExternForm (form);
		}

		return value;
	}


	private void CallExternForm (int form) {
			if (form == 1) 		Parent.ShowForm (FormList.Army_Form, 0);
			else if (form == 2) Parent.ShowForm (FormList.Dvor_Form, 0);
			else if (form == 3) Parent.ShowForm (FormList.Trade_Form, 0);
			else if (form == 4) Parent.ShowForm (FormList.Cult_Form, 0);
	}

	private void OfficeAct () {
	// тут разные действия в офисе по кнопкам, которых пока нет

	}


	// забить офис текущими мордами на местах, вписать инфу
	private void FillOfficeForm () {
		Button Knop;		Image Img; 		
		Sprite 	Sprite1;	Person Pers;			
		// это просто названия кнопок, переводить не надо
		string[] objectname = {"PrimeMinisterButton","MarshalButton","DvoretzkyButton", "StewardButton",
								"PatriarchButton", "MentorButton", "TiunButton", 
								"ArmyButton", "DvorButton", "TradeButton", "CultButton"};

		for (int i = 0; i < 7; i++) {
			// в цикле перебираем кнопки в форме и пихаем туда морды
			Knop = GameObject.Find (objectname[i]).GetComponent<Button>();
			Img = GameObject.Find (objectname[i]).GetComponent<Image>();
			if (Knop == null || Img == null) {
					Debug.Log(objectname[i] + " не найден!");
					break;
					}
				
			Pers = GetOne (i);	
			if ((Sprite1 = Parent.GetSprite (Pers)) != null)
					Img.sprite = Sprite1;
			}

		for (int j = 0; j<4; j++) {
				Knop = GameObject.Find (objectname[j+7]).GetComponent<Button>();
				TextvOkno (Knop.GetComponentInChildren<Text>(), InfoString[j]);
			}

//		TextvOkno (OfficeText, "Здесь вы видите ваших главных советников. Нажмите на портрет советника, чтобы получить дополнительную информацию.");

			Text t = ArmyScrollView.GetComponentInChildren<Text>();
			TextvOkno (t, "");  // информация об армии
			t = DvorScrollView.GetComponentInChildren<Text>();
			TextvOkno (t, "");  // информация о дворе
			t = TradeScrollView.GetComponentInChildren<Text>();
			TextvOkno (t, "");  // информация о торговле
			t = CultScrollView.GetComponentInChildren<Text>();
			TextvOkno (t, "");  // информация о культуре и религии

	}


}
}
