using UnityEngine.UI;
using UnityEngine;

namespace JagaJaga {

public class DvorForm : Display {

	private Display Parent;
	private Text DvorText, DvorName;
	private int DvorFirstClick, ParamChanging;
	private string[] DvorFormStrings;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		DvorPanel = GameObject.Find ("DvorPanel").GetComponent<CanvasGroup>();
		DvorText = GameObject.Find ("DvorText").GetComponent<Text>();
//		DvorName = GameObject.Find ("DvorName").GetComponent<Text>();

		ParamChanging = DvorFirstClick = 0;

		DvorFormStrings = new string[] {
			"Дворец", "Охрана", "Личные покои",	"Охота", "Увеселения",
			"Нажмите на портрет, чтобы получить дополнительную информацию.",
			"Содержание и украшение княжеского дворца, а также многочисленных слуг - обременительные, но необходимые траты для вашей казны. О правителе судят по его дворцу. Роскошь необходима, если вы хотите произвести впечатление на ваших вассалов и иностранных послов.",
/* дубль */	"Дворцовая стража (гвардия) несет охрану дворца и личных покоев правителя. Вы можете усилить охрану, наняв новых стражников. Помните, что им придется платить жалованье. \nСейчас численность стражи: ",
			"Ваши личные покои вы можете украсить так, как сочтете нужным. Немногие увидят их, но все ваши гости оценят то, как вы живете. Роскошь понравится одним, аскетизм и простота - другим. Ну а для того, кто в них живет, главное - это удобство.",
			"Охота - это не просто развлечение, это практически ритуальное действо для знатных людей вашей страны. На охоте можно и себя показать, и с людьми поговорить без условностей этикета, и просто отдохнуть. Сосновый бор, свежий воздух, разъяренный вепрь - что может быть лучше?",
			"Помимо охоты, вам доступно множество других увеселений. И даже если вы сами не расположены смотреть на пляски скоморохов, огненные забавы или ручных медведей, возможно, вашему двору это понравится.",
			""};

	}

	private Person GetOne (int i) {
		var Gov = Strana.Government;
		
		if (i==0) return Gov.Dvoretzky.GetHolder();
		if (i==1) return Gov.Guardian.GetHolder();
		if (i==2) return Gov.Postelnichiy.GetHolder();
		if (i==3) return Gov.Kluchnik.GetHolder();
		if (i==4) return Gov.Stolnik.GetHolder();
		if (i==5) return Gov.Bodyguard.GetHolder();
		if (i==6) return Gov.Konuchiy.GetHolder();
		if (i==7) return Gov.Lovchiy.GetHolder();
		if (i==8) return Gov.Skomoroh.GetHolder();
		else return null;
	}

	public bool ShowDvorForm (int choice) {
		bool value = true;		Person Pers = null;
		var Voisko = Strana.Voisko;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowDvorPanel (true);
					FillDvorForm ();
					GetPopupResult ();   // проверяем, вызывали ли мы попап
					}
					break; 
			case 1:   // дворецкий
			case 2:   // нач. охраны
			case 3:   // постельничий
			case 4:   // ключник
			case 5:   // стольник
			case 6:   // телохран
			case 7:  // конюший
			case 8:  // ловчий
			case 9: { // скоморох
					Pers = GetOne (choice - 1);
					}
					break;
			case 14: {	// дворец
					CallPopInfo (DvorFormStrings[6]);
					}
					break;
			case 15: {	// охрана
					CallPopup (choice, DvorFormStrings[7] + Voisko.PalaceGuard, Voisko.PalaceGuard, 10);
					}
					break;
			case 16: {	// покои
					CallPopInfo (DvorFormStrings[8]);
					}
					break;
			case 17: { 	// охота
					CallPopInfo (DvorFormStrings[9]);
					}
					break;
			case 18: {	// увеселения
					CallPopInfo (DvorFormStrings[10]);
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

		if (choice >= 1 && choice <= 9) {
			if (DvorFirstClick != choice) {
				DvorFirstClick = choice;
				Parent.PersonInfo.ShowBriefInfo (Pers, DvorText);
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


		return value;
	}	

		private void CallPopup (int choice, string s, int x, int y) {
			ParamChanging = choice;
			Parent.Popup.SetWindow (s, x, y);
		}

		private void CallPopInfo (string s) {
			Parent.PopInfo.SetWindow (s);
		}

	private void GetPopupResult () {   // определяем, надо ли забрать из попапа результат операции
		int result = Parent.Popup.GetResult ();  
		var Voisko = Strana.Voisko;

		switch (ParamChanging) {   // флаг указывает на то, какой параметр меняли с помощью попапа
			case 0: break;
			case 14: {	// дворец
					}
					break;
			case 15: {	// охрана
					Voisko.PalaceGuard = result;
					}
					break;
			case 16: {	// покои
					}
					break;
			case 17: { 	// охота
					}
					break;
			case 18: {	// увеселения
					}
					break;
			default: {
					}
					break;
		}
		ParamChanging = 0;
	}

	// забить офис текущими мордами на местах, вписать инфу
	private void FillDvorForm () {
		Button Knop;		Image Img; 		
		Sprite 	Sprite1;	Person Pers;			
		// это просто названия кнопок, переводить не надо
		string[] objectname = {"Дворецкий", "Нач. охраны1", "Постельничий", "Ключник", "Стольник",
				"Телохранитель1", "Конюший", "Ловчий", "Скоморох", "Дворец", "Охрана", "Личные покои",
				"Охота", "Увеселения"};

		for (int i = 0; i < 9; i++) {
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

//		TextvOkno (DvorText, "Нажмите на портрет, чтобы получить дополнительную информацию.");

		// забиваем кнопки действий текстом и краткими значениями параметров
			for (int j=0; j < 5; j++) {		
				Knop = GameObject.Find (objectname[j+9]).GetComponent<Button>();
				TextvOkno (Knop.GetComponentInChildren<Text>(), DvorFormStrings[j]);
				}

	}


}
}
