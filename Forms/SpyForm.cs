using UnityEngine.UI;
using UnityEngine;

namespace JagaJaga {

public class SpyForm : Display {

	private Display Parent;
	private Text SpyText, SpyName;
	private int SpyFirstClick, ParamChanging;
	private string[] SpyFormStrings;
	private bool SpyFirstRun;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		SpyPanel = GameObject.Find ("SpyPanel").GetComponent<CanvasGroup>();
		SpyText = GameObject.Find ("SpyText").GetComponent<Text>();
//		SpyName = GameObject.Find ("SpyName").GetComponent<Text>();

		ParamChanging = SpyFirstClick = 0;
		SpyFirstRun = false;

		SpyFormStrings = new string[] {
			"Тайная служба","Проекты", "Ассассины", "Тайная полиция", "Соглядатаи", "Ночные волки",
			"Нажмите на портрет, чтобы получить дополнительную информацию.",
			"Ваша тайная служба - это невидимая рука, которая делает то, о чем не принято говорить.",
			"В таких делах, где ваши воины не захотели бы пачкать руки, в дело вступают рыцари плаща и кинжала. Наемные убийцы не знают пощады. Вам нужно лишь хорошенько заплатить им, и любая преграда на вашем пути будет устранена.",
			"Тайная полиция не просто следит за порядком. Это мастера-сыщики, которые выискивают среди добропорядочных граждан тех, кто сеет смуту, организует заговоры или действует в интересах иностранных держав.",
			"Ваши соглядатаи - это ваши глаза и уши; мастера наблюдения, способные проникнуть туда, куда невозможно попасть обычному человеку, и принести вам информацию, цену которой определяете только вы.",
			"Ваши витязи способны сокрушить врага в открытом бою, но война - это не только честные поединки один на один, и для победы зачастую все средства хороши. Воины, называемые ночными волками, способны устроить врагу любую диверсию, пока длится ночь, и тем самым помочь вашему войску одержать победу, когда светит солнце.",
			""};

	}

	private Person GetOne (int i) {
		var Gov = Strana.Government;
		
		if (i==0) return Gov.Spymaster.GetHolder();
		if (i==1) return Gov.Ninja.GetHolder();
		if (i==2) return Gov.Sinobi.GetHolder();
		if (i==3) return Gov.Mystik.GetHolder();
		if (i==4) return Gov.Phantoms.GetHolder();
		else return null;
	}

	public bool ShowSpyForm (int choice) {
		bool value = true;		Person Pers = null;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowSpyPanel (true);
					FillSpyForm ();
					GetPopupResult ();   // проверяем, вызывали ли мы попап
					if (!SpyFirstRun) {
						SpyFirstRun = true;
						Parent.StoryRun ((int)StoryMode.tutor, FormList.Spy_Form);
						}
					else
						Parent.StoryRun ((int)StoryMode.events, FormList.Spy_Form);
					}
					break; 
			case 1:   // спаймастер
			case 2:   // ниньзя
			case 3:   // шиноби
			case 4:   // мистик
			case 5: { // призрак
					Pers = GetOne (choice - 1);
					}
					break;
			case 14: {	// с.служба
					CallPopInfo (SpyFormStrings[7]);
					}
					break;
			case 15: {	// проекты
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Project_Form, 0);
					}
					break;
			case 16: {	// убийцы
					CallPopInfo (SpyFormStrings[8]);
					}
					break;
			case 17: { 	// т. полиция
					CallPopInfo (SpyFormStrings[9]);
					}
					break;
			case 18: {	// шпионы
					CallPopInfo (SpyFormStrings[10]);
					}
					break;
			case 19: {	// диверсанты
					CallPopInfo (SpyFormStrings[11]);
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

		if (choice >= 1 && choice <= 5) {
			if (SpyFirstClick != choice) {
				SpyFirstClick = choice;
				Parent.PersonInfo.ShowBriefInfo (Pers, SpyText);
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
			case 14: {	// с.служба
					}
					break;
			case 16: {	// убийцы
					}
					break;
			case 17: { 	// т. полиция
					}
					break;
			case 18: {	// шпионы
					}
					break;
			case 19: {	// диверсанты
					}
					break;
			default: {
					}
					break;
		}
		ParamChanging = 0;
	}

	// забить офис текущими мордами на местах, вписать инфу
	private void FillSpyForm () {
		Button Knop;		Image Img; 		
		Sprite 	Sprite1;	Person Pers;			
		// это просто названия кнопок, переводить не надо
		string[] objectname = {"Шпикмастер", "Ниньзя", "Шиноби", "Мистик", "Призраки", "Тайная служба",
			"С. проекты", "Убийцы", "Тайная полиция", "Шпионы", "Диверсанты"};

		for (int i = 0; i < 5; i++) {
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

//		TextvOkno (SpyText, "Нажмите на портрет, чтобы получить дополнительную информацию.");

		// забиваем кнопки действий текстом и краткими значениями параметров
			for (int j=0; j < 6; j++) {		
				Knop = GameObject.Find (objectname[j+5]).GetComponent<Button>();
				TextvOkno (Knop.GetComponentInChildren<Text>(), SpyFormStrings[j]);
				}

	}


}
}
