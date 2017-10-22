using UnityEngine.UI;
using UnityEngine;


namespace JagaJaga {

public class CultForm : Display {

	private Display Parent;
	private Text CultText, CultName;
	private int CultFirstClick, ParamChanging;
	private string[] CultFormStrings;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		CultPanel = GameObject.Find ("CultPanel").GetComponent<CanvasGroup>();
		CultText = GameObject.Find ("CultText").GetComponent<Text>();
//		CultName = GameObject.Find ("CultName").GetComponent<Text>();

		ParamChanging = CultFirstClick = 0;

		CultFormStrings = new string[] {
			"Религия и церковь", "Культура","Проекты", "Наука", "Университет", "Медицина", "Нужды народа",
			"Нажмите на портрет, чтобы получить дополнительную информацию.",
			"Поклонения богам и церковные обряды занимают важнейшее место в жизни простых людей. Церковь руководит процессом поклонения. Не стоит ссориться с патриархами церкви, если вы не хотите навлечь на себя гнев верующих... а может быть, и самого Господа. Главенствующая в стране религия: ",
			"Культурная жизнь вашего народа. Развитие ремесел и искусств, ярмарочные балаганы, театры, галереи, праздничные песнопесения и танцы, словесные поединки и поэтические конкурсы. В текущей версии не реализовано.",
			"Развитие науки, философии, изобретательство. Талантливые одиночки-левши и школы философов. Вы можете выделить деньги на поддержку дерзновенных начинаний, но церковь вряд ли одобрит такой подход.",
			"Ключевое место в системе образования вашей страны занимает Университет. Вы можете учредить его, выделив деньги на постройку здания и привлечение известных лекторов. Университет станет центром мира для образованных людей и ученых, но также приведет к распространению опасных общественных идей.",
			"Для того, чтобы улучшить состояние медицины, вам нужно открыть новые больницы для населения и привлечь к работе знающих лекарей.",
			"До власть предержащих редко доходят мольбы о нуждах и чаяниях простого народа. Благодаря странным людям - юродивым, которые не боятся входить в ваши покои с резким словом на устах, вы можете узнать о том, чем живет народ и как облегчить его участь.",
			""};

	}

	private Person GetOne (int i) {
		var Gov = Strana.Government;
		
		if (i==0) return Gov.Patriarch.GetHolder();
		if (i==1) return Gov.Grandmeister.GetHolder();
		if (i==2) return Gov.Paramedik.GetHolder();
		if (i==3) return Gov.Jurodiviy.GetHolder();
		else return null;
	}


	public bool ShowCultForm (int choice) {
		bool value = true;		Person Pers = null;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowCultPanel (true);
					FillCultForm ();
					GetPopupResult ();   // проверяем, вызывали ли мы попап
					}
					break; 
			case 1:   // патриарх
			case 2:   // вел. магистр
			case 3:   // парамедик
			case 4: {  // юродивый
					Pers = GetOne (choice - 1);
					}
					break;

			case 14: {	// религия
					CallPopInfo (CultFormStrings[8] + Strana.Church.StateReligion);
					}
					break;
			case 15: {	// культура
					CallPopInfo (CultFormStrings[9]);
					}
					break;
			case 16: {	// проекты
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Project_Form, 0);
					}
					break;
			case 17: { 	// наука
					CallPopInfo (CultFormStrings[10]);
					}
					break;
			case 18: {	// университет
					CallPopInfo (CultFormStrings[11]);
					}
					break;
			case 19: {	// медицина
					CallPopInfo (CultFormStrings[12]);
					}
					break;
			case 20: {	// народ
					CallPopInfo (CultFormStrings[13]);
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

		if (choice >= 1 && choice <= 4) {
			if (CultFirstClick != choice) {
				CultFirstClick = choice;
				Parent.PersonInfo.ShowBriefInfo (Pers, CultText);
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

		switch (ParamChanging) {   // флаг указывает на то, какой параметр меняли с помощью попапа
			case 0: break;
			case 14: {	// религия
					}
					break;
			case 15: {	// культура
					}
					break;
			case 17: { 	// наука
					}
					break;
			case 18: {	// университет
					}
					break;
			case 19: {	// медицина
					}
					break;
			case 20: {	// народ
					}
					break;
			default: {
					}
					break;
		}
		ParamChanging = 0;
	}

	// забить офис текущими мордами на местах, вписать инфу
	private void FillCultForm () {
		Button Knop;		Image Img; 		
		Sprite 	Sprite1;	Person Pers;			
		// это просто названия кнопок, переводить не надо
		string[] objectname = {"Архиерей", "Вел. магистр", "Парамедик", "Юродивый", "Религия", "Культура",
			"К. проекты", "Наука", "Образование", "Медицина", "Соцзащита"};

		for (int i = 0; i < 4; i++) {
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

//		TextvOkno (CultText, "Нажмите на портрет, чтобы получить дополнительную информацию.");

		// забиваем кнопки действий текстом и краткими значениями параметров
			for (int j=0; j < 7; j++) {		
				Knop = GameObject.Find (objectname[j+4]).GetComponent<Button>();
				TextvOkno (Knop.GetComponentInChildren<Text>(), CultFormStrings[j]);
				}

	}

}
}
