using UnityEngine.UI;
using UnityEngine;

namespace JagaJaga {

public class TradeForm : Display {

	private Display Parent;
	private Text TradeText, TradeName;
	private int TradeFirstClick, ParamChanging;
	private string[] TradeFormStrings;
	private bool TradeFirstRun;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		TradePanel = GameObject.Find ("TradePanel").GetComponent<CanvasGroup>();
		TradeText = GameObject.Find ("TradeText").GetComponent<Text>();
//		TradeName = GameObject.Find ("TradeName").GetComponent<Text>();

		ParamChanging = TradeFirstClick = 0;
		TradeFirstRun = false;

		TradeFormStrings = new string[] {"Нажмите на портрет, чтобы получить дополнительную информацию.",
			"Укажите величину налоговой ставки в процентах. Помните, что высокие налоги приводят к обнищанию и недовольству жителей. Текущая ставка: ",
			"Размер вашей казны: ", 
			"Ваш ежемесячный доход из всех источников: ", 
			"Ваши расходы в прошлом месяце: ", 
			"Вы можете стимулировать развитие торговли, выделив деньги на открытие торговых рядов. Создание 10 новых лавок обойдется в 50 золотых. Сейчас количество торговцев в вашей стране: ",
			"Пошлины на торговлю с иностранными купцами. Более низкие пошлины стимулируют импорт и экспорт. Текущий размер пошлины, в процентах: ",
			"Выделение денег на застройку и ремонт городских зданий. Расширение и обустройство города привлечет в него новых жителей. Стоимость постройки одного квартала 100 золотых. Сколько кварталов построить?",
			"Развитие ресурсодобывающей отрасли пока не реализовано.",
			"Выделение денег на развитие сельского хозяйства увеличит производство продуктов. Стоимость постройки одной деревни 100 золотых. Сколько деревень построить?",
			"Налоги и подати", "Казна", "Проекты", 
			"Торговые дела", "Пошлины", "Строительство", "Шахты", "Земледелие",
			"Сальдо за месяц: ",
			""};

	}

	private Person GetOne (int i) {
		var Gov = Strana.Government;
		
		if (i==0) return Gov.Kaznachei.GetHolder();
		if (i==1) return Gov.Banker.GetHolder();
		if (i==2) return Gov.Trademaster.GetHolder();
		if (i==3) return Gov.Tamojnya.GetHolder();
		if (i==4) return Gov.Buildmaster.GetHolder();
		if (i==5) return Gov.Miner.GetHolder();
		if (i==6) return Gov.Agrarian.GetHolder();
		else return null;
	}

	public bool ShowTradeForm (int choice) {
		bool value = true;		
		Person Pers = null;
		var Kazna = Strana.Kazna;
		var Capital = Strana.Capital;

		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowTradePanel (true);
					FillTradeForm ();
					GetPopupResult ();   // проверяем, вызывали ли мы попап
				// прогоняем туториал либо сюжетные ивенты
					if (!TradeFirstRun) {
						TradeFirstRun = true;
						Parent.StoryRun ((int)StoryMode.tutor, FormList.Trade_Form);
						}
					else
						Parent.StoryRun ((int)StoryMode.events, FormList.Trade_Form);
					}
					break; 
			case 1:   // казначей
			case 2:   // банкир
			case 3:   // торговец
			case 4:   // таможня
			case 5:   // строитель
			case 6:   // шахтер
			case 7: { // аграрий
					Pers = GetOne (choice - 1);
					}
					break;
			case 14: {	// налоги
					var tax = Kazna.TaxLevel;
					CallPopup (choice, TradeFormStrings[1] + tax, tax, 5);
					}
					break;
			case 15: {	// казна
					CallPopInfo (TradeFormStrings[2] + Kazna.Treasure + "\n" + TradeFormStrings[3] + 
						(int)Kazna.LastMonthIncome + "\n" + TradeFormStrings[4] + (int)Kazna.LastMonthExpense +
						 "\n" + TradeFormStrings[18] + (int)Kazna.MonthlySaldo);
					}
					break;
			case 16: {	// проекты
					Parent.ClearPersonFlag ();
					Parent.ShowForm (FormList.Project_Form, 0);
					}
					break;
			case 17: { 	// торговля
					int t = (int)Capital.Traders;
					CallPopup (choice, TradeFormStrings[5] + t, 0, 10);
					}
					break;
			case 18: {	// пошлины
					var tar = Kazna.ImportTariff;
					CallPopup (choice, TradeFormStrings[6] + tar, tar, 5);
					}
					break;
			case 19: {	// строительство
					CallPopup (choice, TradeFormStrings[7], 0, 1);
					}
					break;
			case 20: {	// шахты
					CallPopInfo (TradeFormStrings[8]);
					}
					break;
			case 21: {	// земледелие
					CallPopup (choice, TradeFormStrings[9], 0, 1);
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

		if (choice >= 1 && choice <= 7) {
			if (TradeFirstClick != choice) {
				TradeFirstClick = choice;
				Parent.PersonInfo.ShowBriefInfo (Pers, TradeText);
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
		var Kazna = Strana.Kazna;
		var Capital = Strana.Capital;
		int result = Parent.Popup.GetResult ();  

		switch (ParamChanging) {   // флаг указывает на то, какой параметр меняли с помощью попапа
			case 0: break;
			case 14: {   // налоговая ставка
					Kazna.TaxLevel = result; 
					}
					break;
			case 17: {   // добавляем торговцев
					Capital.Traders += result;
					Kazna.Expense ( result*5 );  // здесь нет проверки на перерасход стредств
					}
					break;
			case 18: {   // пошлины
					Kazna.ImportTariff = result;
					}
					break;
			case 19: {   // застройка
					Kazna.Expense ( result*100 );	// здесь нет проверки на перерасход стредств
					Capital.BuildNewDistrict ( result );
					}
					break;
			case 20: {   // шахты
					}
					break;
			case 21: {   // деревни
					Kazna.Expense ( result*100 );	// здесь нет проверки на перерасход стредств
					Strana.Subjects.Provinces[0].BuildNewVillage (result);
					}
					break;
			default: {
					}
					break;
		}
		ParamChanging = 0;
	}


	// забить офис текущими мордами на местах, вписать инфу
	private void FillTradeForm () {
		Button Knop;		Image Img; 		
		Sprite 	Sprite1;	Person Pers;	
		var Kazna = Strana.Kazna;
		var Capital = Strana.Capital;
		
		// это просто названия кнопок, _переводить_ не надо!!!
		string[] objectname = {"Казначей", "Банкир", "Торговец", "Таможня", "Строитель",
			"Шахтер", "Аграрий", "Налоги", "Казна", "Э. проекты", "Торговля", "Пошлины", 
			"Строительство", "Шахты", "Земледелие"};
	
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

	//	TextvOkno (TradeText, TradeFormStrings[0]);
	// забиваем кнопки действий текстом и краткими значениями параметров
		Knop = GameObject.Find (objectname[7]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[10] + "\n" + Kazna.TaxLevel + "%");
		Knop = GameObject.Find (objectname[8]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[11] + "\n" + Kazna.Treasure);
		Knop = GameObject.Find (objectname[9]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[12]);
		Knop = GameObject.Find (objectname[10]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[13] + "\n" + (int)Capital.Traders);
		Knop = GameObject.Find (objectname[11]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[14] + "\n" + Kazna.ImportTariff);
		Knop = GameObject.Find (objectname[12]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[15]);
		Knop = GameObject.Find (objectname[13]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[16]);
		Knop = GameObject.Find (objectname[14]).GetComponent<Button>();
		TextvOkno (Knop.GetComponentInChildren<Text>(), TradeFormStrings[17]);

	}


}
}
