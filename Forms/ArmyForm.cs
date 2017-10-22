using UnityEngine;
using UnityEngine.UI;

namespace JagaJaga {

public class ArmyForm : Display {

	private Display Parent;
	private Text ArmyText, ArmyName;
	private int ArmyFirstClick, ParamChanging;
	private string[] ArmyFormStrings;
	private bool ArmyFirstRun;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		ArmyPanel = GameObject.Find ("ArmyPanel").GetComponent<CanvasGroup>();
		ArmyText = GameObject.Find ("ArmyText").GetComponent<Text>();
//		ArmyName = GameObject.Find ("ArmyName").GetComponent<Text>();

		ParamChanging = ArmyFirstClick = 0;
		ArmyFirstRun = false;

		ArmyFormStrings = new string[] {
			"Стратегия", "Дипломатия", "Тайные дела", "Дружина", "Охрана", "Управа", "Оружейная", 
			"Дозоры", "Околица", "Вербовка", "Наемники", 
			"Нажмите на портрет, чтобы получить дополнительную информацию.",
			"Княжеская дружина - это небольшое войско, преданное лично вам. Она состоит из старшей дружины (отборные воины в лучших доспехах, выступают как тяжелая конница), и младшей дружины (рядовые воины в хороших доспехах, выступают как легкая конница). Дружина получает жалованье в мирное время и часть добычи - в военное. \nЧисленность старшей дружины: ",
			"Численность младшей дружины: ",
			"Дворцовая стража (гвардия) несет охрану дворца и личных покоев правителя. Вы можете усилить охрану, наняв новых стражников. Помните, что им придется платить жалованье. \nСейчас численность стражи: ",
			"Для борьбы с преступностью и охраны порядка в городах и землях нужна управа. Численность стражников в управе должна соответствовать количеству жителей ваших земель, иначе работа управы станет неэффективна. \nТекущее количество стражников: ",
			"Для охраны границ государства нужны засеки - сторожевые посты. На каждом из них есть небольшой гарнизон. Они могут задержать мелкие отряды неприятеля и предупредить о приближении крупных отрядов. Вы можете построить новые засеки. Гарнизон требует ежемесячного содержания. \nСейчас пограничных гарнизонов: ",
			"Для разведки неприятельских войск на подступах к нашим границам пригодятся дозоры - сторожевые разъезды конных воинов. Чем больше дозорных, тем выше вероятность, что противник не сможет застать вас врасплох. Дозорным нужно платить жалованье. \nТекущее количество дозорных: ",
			"В оружейной палате, или арсенале, хранятся оружие и доспехи, которые вы можете выдать любым вновь созванным войскам, у которых нет своего вооружения, тем самым без задержек оснастив свою армию. Вы можете пополнять запасы оружейной палаты, делая заказы у мастеров-оружейников или захватывая трофеи в бою. В данный момент функционал не реализован.",
			"Помимо княжеской дружины, феодального ополчения ваших вассалов и городовых полков, вы можете рассчитывать на регулярную армию. Для создания регулярных полков вам нужно исследовать особый проект. Регулярному войску придется платить жалованье. Вы можете нанимать или распускать регулярные войска, но на их подготовку потребуется время. \nТекущая численность войска: ",
			"Если вам срочно нужны солдаты, то вы можете прибегнуть к услугам наемников-варягов. Вам не придется ждать, пока они соберутся, так как наемные солдаты всегда готовы к бою. Они требуют крайне высокое жалованье и не всегда надежны в бою. Вы можете нанять или распустить их отряды в любой момент. \nТекущая численность наемников: ",
			"",
			""};

	}


	private Person GetOne (int i) {
		var Gov = Strana.Government;
		
		if (i==0) return Gov.Voevoda.GetHolder();
		if (i==1) return Gov.Guardian.GetHolder();
		if (i==2) return Gov.Druzhina.GetHolder();
		if (i==3) return Gov.Bodyguard.GetHolder();
		if (i==4) return Gov.Policemen.GetHolder();
		if (i==5) return Gov.Okolnichiy.GetHolder();
		if (i==6) return Gov.Dozorny.GetHolder();
		if (i==7) return Gov.Orujeinichiy.GetHolder();
		if (i==8) return Gov.Captain.GetHolder();
		if (i==9) return Gov.Commander1.GetHolder();
		if (i==10) return Gov.Commander2.GetHolder();
		if (i==11) return Gov.Commander3.GetHolder();
		else return null;
	}


	public bool ShowArmyForm (int choice) {
		bool value = true;		
		Person Pers = null;
		var Voisko = Strana.Voisko;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowArmyPanel (true);
					FillArmyForm ();
					GetPopupResult ();   // проверяем, вызывали ли мы попап
				// прогоняем туториал либо сюжетные ивенты
					if (!ArmyFirstRun) {
						ArmyFirstRun = true;
						Parent.StoryRun ((int)StoryMode.tutor, FormList.Army_Form);
						}
					else
						Parent.StoryRun ((int)StoryMode.events, FormList.Army_Form);
					}
					break; 
			case 1: {   // стратегия
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Strat_Form, 0);
					}
					break;
			case 2: {   // дипломатия
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Diplo_Form, 0);
					}
					break;
			case 3: {   // тайные дела
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Spy_Form, 0);
					}
					break;
			case 4: {	// дружина
					CallPopInfo (ArmyFormStrings[12] + Voisko.DruzhinaStar + "\n" + ArmyFormStrings[13] + Voisko.DruzhinaMlad);
					}
					break;
			case 5: {	// охрана
					CallPopup (choice, ArmyFormStrings[14] + Voisko.PalaceGuard, Voisko.PalaceGuard, 10);
					}
					break;
			case 6: {	// полиция
					CallPopup (choice, ArmyFormStrings[15] + Voisko.CityGuard, Voisko.CityGuard, 10);
					}
					break;
			case 7: { 	// граница
					CallPopup (choice, ArmyFormStrings[16] + Voisko.Garnisons, 0, 1);
					}
					break;
			case 8: {	// разведка
					CallPopup (choice, ArmyFormStrings[17] + Voisko.Dozory, Voisko.Dozory, 10);
					}
					break;
			case 9: {	// арсенал
					CallPopInfo (ArmyFormStrings[18]);
					}
					break;
			case 13: {	// стрельцы
					CallPopup (choice, ArmyFormStrings[19] + Voisko.Strelzy, Voisko.Strelzy, 50);
					}
					break;
			case 14: {	// наемники
					CallPopup (choice, ArmyFormStrings[20] + Voisko.Mercenary, Voisko.Mercenary, 50);
					}
					break;
			case 15: 	// воевода (маршал)
			case 16:  	// нач. охраны
			case 17: 	// ст. дружины
			case 18: 	// телохранитель
			case 19: 	// нач. полиции
			case 20: 	// окольничий
			case 21: 	// дозорный
			case 22: 	// оружничий
			case 23: 	// капитан наемников
			case 24: 	// командир1
			case 25: 	// командир2
			case 26: {	// командир3
					Pers = GetOne (choice - 15);
					}
					break;
			case 10: {	// выход
					Parent.ClearPersonFlag ();
					Parent.DisplayClear ();
					return (value = false);
					}
			case 11: {	// вперед

					}
					break;
			case 12: {	// назад

					}
					break;
			default: {
					}
					break;
			}

		if (choice >= 15 && choice <= 26) {
			if (ArmyFirstClick != choice) {
				ArmyFirstClick = choice;
				Parent.PersonInfo.ShowBriefInfo (Pers, ArmyText);
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
		else if ((choice >= 4 && choice <= 9) || choice >=13 && choice <=14) {
				if (ArmyFirstClick != choice) {
					ArmyFirstClick = choice;
					TextvOkno (ArmyText, ArmyFormStrings[11]);
				}
//				else CallExternForm (form);
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
		var Kazna = Strana.Kazna;
		var Capital = Strana.Capital;
		var Voisko = Strana.Voisko;
		int result = Parent.Popup.GetResult ();  

		switch (ParamChanging) {   // флаг указывает на то, какой параметр меняли с помощью попапа
			case 0: break;
			case 4: {	// дружина
					}
					break;
			case 5: {	// охрана
					Voisko.PalaceGuard = result;
					}
					break;
			case 6: {	// полиция
					Voisko.CityGuard = result;
					Capital.Guards = (int)result;
					}
					break;
			case 7: { 	// граница
					Kazna.Expense ( result * 50 );
					Voisko.Garnisons += result;
					}
					break;
			case 8: {	// разведка
					Voisko.Dozory = result;
					}
					break;
			case 9: {	// арсенал
					}
					break;
			case 13: {	// стрельцы
					Voisko.Strelzy = result;
					}
					break;
			case 14: {	// наемники
					Voisko.Mercenary = result;
					}
					break;
			default: {
					}
					break;
		}
		ParamChanging = 0;
	}



	// забить офис текущими мордами на местах, вписать инфу
	private void FillArmyForm () {
		Button Knop;		Image Img; 		
		Sprite 	Sprite1;	Person Pers;			
		// это просто названия кнопок, переводить не надо
		string[] objectname = {"Воевода", "Нач. охраны", "Ст. дружины", "Телохранитель", "Нач. полиции",
				"Окольничий", "Дозорный", "Оружничий", "Капитан", "Командир1", "Командир2", "Командир3",
				"Стратегия", "Дипломатия", "Шпионаж", "Дружина", "Охрана", "Полиция", "Арсенал", 
				"Разведка", "Граница", "Стрельцы", "Наемники"};

		for (int i = 0; i < 12; i++) {
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

//		TextvOkno (ArmyText, "Здесь вы видите ваших воинов. Нажмите на портрет, чтобы получить дополнительную информацию.");
	// забиваем кнопки действий текстом и краткими значениями параметров
		Knop = GameObject.Find (objectname[12]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[0]);
		Knop = GameObject.Find (objectname[13]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[1]);
		Knop = GameObject.Find (objectname[14]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[2]);
		Knop = GameObject.Find (objectname[15]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[3] + "\n" + Strana.Voisko.Druzhina);
		Knop = GameObject.Find (objectname[16]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[4] + "\n" + Strana.Voisko.PalaceGuard);
		Knop = GameObject.Find (objectname[17]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[5] + "\n" + Strana.Voisko.CityGuard);
		Knop = GameObject.Find (objectname[18]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[6]);
		Knop = GameObject.Find (objectname[19]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[7] + "\n" + Strana.Voisko.Dozory);
		Knop = GameObject.Find (objectname[20]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[8] + "\n" + Strana.Voisko.Garnisons);
		Knop = GameObject.Find (objectname[21]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[9] + "\n" + Strana.Voisko.Strelzy);
		Knop = GameObject.Find (objectname[22]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), ArmyFormStrings[10] + "\n" + Strana.Voisko.Mercenary);


	}


}
}
