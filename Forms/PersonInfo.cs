using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JagaJaga {

public class PersonInfo : Display {

	private Display Parent;
	private CanvasGroup PersonInfoPanel, PersonActionPanel;
	private Text InfoText, InfoName, InfoText2, InfoName2;
	private Button ConfirmAction, RefuseAction;
	private Image PImage, PImage2;
	private Dropdown FreePosts; 
	private List<string> DDListOpt = new List<string>();

	private string[][] PersonActions, SpyActionsOnPerson, PrisonerActions;
	private string[] PersonInfoShorts, ActionsDescription, ActionsResults;

	public Person CurrentPerson;

	private bool SwitchPersonAction, DropListActive; // показывать инфу или действия для персонажа


	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();	

		PersonInfoPanel = GameObject.Find ("PersonInfoPanel").GetComponent<CanvasGroup>();
		PersonActionPanel = GameObject.Find ("PersonActionPanel").GetComponent<CanvasGroup>();

		ConfirmAction = GameObject.Find ("ConfirmAction").GetComponent<Button>();
		RefuseAction = GameObject.Find ("RefuseAction").GetComponent<Button>();
		FreePosts = GameObject.Find ("FreePostsDropdown").GetComponent<Dropdown>();

		PImage = GameObject.Find ("PImage").GetComponent<Image>();
		PImage2 = GameObject.Find ("PImage2").GetComponent<Image>();

		InfoText = GameObject.Find ("InfoText").GetComponent<Text>();
		InfoName = GameObject.Find ("InfoName").GetComponent<Text>();
		InfoText2 = GameObject.Find ("InfoText2").GetComponent<Text>();
		InfoName2 = GameObject.Find ("InfoName2").GetComponent<Text>();

		SwitchPersonAction = DropListActive = false;

		PersonActions = new string[][] {
			new string[] {"Пригласить на аудиенцию", "Попросить об услуге", "Предложить помощь", "Угрожать"},
			new string[] {"Объявить благодарность", "Наградить деньгами", "Пожаловать титул", "Назначить советником"}, 
			new string[] {"Удалить из совета", "Лишить титула", "Призвать к ответу в суде", "Взять под стражу"},
			new string[] {"Получить информацию", "Скомпрометировать", "Шантажировать", "Ударить в спину"},
			new string[] {"","","",""}
			};

		SpyActionsOnPerson = new string[][] {
			new string[] {"Установить слежку", "Подослать шпиона", "Подкупить слуг или друзей", "Провести тайное расследование"},
			new string[] {"Распространять слухи", "Подстроить конфузию", "Объявить еретиком", ""},
			new string[] {"Угрожать расправой", "Поджечь усадьбу", "Похитить родича", ""},
			new string[] {"Нападение на родственников", "Подставить/Обвинить в преступлении", "Устроить похищение", "Заказать убийство"}
			};

		PrisonerActions = new string[][] {
			new string[] {"Удалить из совета", "Лишить титула", "Призвать к ответу в суде", "Конфисковать имущество"},			
			new string[] {"","","",""},
			new string[] {"Допрос с пристрастием", "Подвергнуть пыткам", "Привести родственников", "Заточить в склеп"},
			new string[] {"Освободить", "Потребовать выкуп", "Отправить в изгнание", "Казнить"}
			};

		ActionsDescription = new string[] {
				"Персонаж должен явиться к вам для беседы. Он может отказаться, если настроен к вам враждебно.", 
				"Если у вас хорошие отношения с этим человеком, вы можете попросить у него помощи или совета.", 
				"Вы можете благородно помочь тому, кто оказался в затруднительной ситуации. Но помните, не всякий готов принять чужую помощь.",
				"Этот человек чем-то прогневал вас? Вы можете его... предупредить. Это может ухудшить ваши отношения, но, по крайней мере, заставит его опасаться вашего гнева.",
	
				"Объявите благодарность перед всем двором тому, кто снискал ваше расположение. Пусть другие завидуют ему!\nОтношения + 5", 
				"Преданную службу принято вознаграждать золотом. Не скупитесь! Чем щедрее ваша рука, тех охотнее вам служат.\nЗолото - 20, Отношения + 20", 
				"Титулы и звания, а также прилагающиеся к ним земли и доход - вот лучшая награда для ваших соратников!\nПожалован титул, Владения - 1, Отношения + 30", 
				"Если вы уверены в способностях этого человека, вы можете вознаградить его должностью при вашем дворе. Это честь для него и подспорье для вас.\nВыбрать должность, Отношения + 10", 
	
				"Советник не оправдал ваших ожиданий? Он жаден, глуп, некомпетентен или просто говорит неприятные вещи? Всегда можно заменить неугодного человека кем-то другим.\nОтозвать должность, Отношения - 15", 
				"Ваш вассал совершил преступление, опорочил вас или поднял мятеж? Лишите его всех земель! Это худшее наказание для феодала.\nВладения +1, Отношения - 50", 
				"Если вам известно о преступлениях этого человека, то вы можете отдать его под суд. Но учтите - суд может и не вынести устраивающее вас решение.\nНазначен суд, Отношения - 10", 
				"Вы уверены, что перед вами - преступник? Пусть ваша стража арестует его, и вы сами станете ему судьей! Но учтите - несправедливая кара повлияет на общественное мнение.\nАрестован, Отношения - 30, Общ. мнение - 10",
	
				"Вам нужно лучше узнать, что за человек перед вами? Годится ли он для государственной должности, не замышляет ли измену, и как часто исполняет супружеский долг?\nТайные дела, Сбор информации", 
				"Влияние этого человека слишком велико и угрожает вашим позициям? Тогда пятнышко дегтя на его репутации не помешает. Пусть ваши люди начнут распространять грязные слухи о нем.\nТайные дела, Компрометация", 
				"Если этот человек неудобен для вас, вы можете его шантажировать. Но не забывайте - не все поддаются на угрозы. К каждому человеку нужен свой подход. \nТайные дела, Шантаж", 
				"Ваш соперник слишком силен и влиятелен, чтобы с ним можно было расправиться в открытую? Нанесите удар тайно. Это жестокий, но эффективный прием.\nТайные дела, Нападение",
			
				"Советник не оправдал ваших ожиданий? Он жаден, глуп, некомпетентен или просто говорит неприятные вещи? Всегда можно заменить неугодного человека кем-то другим.\nОтозвать должность, Отношения - 15", 	
				"Ваш вассал совершил преступление, опорочил вас или поднял мятеж? Лишите его всех земель! Это худшее наказание для феодала.\nВладения +1, Отношения - 50", 
				"Если вам известно о преступлениях этого человека, то вы можете отдать его под суд. Но учтите - суд может и не вынести устраивающее вас решение.\nНазначен суд, Отношения - 10", 
				"Богаства этого человека принадлежат ему не по праву. Отобрать все земли, деньги и титулы!\nВладения +1, Золото +30, Отношения - 60",			
				"",
				"",
				"",
				"",
				"Настало время кое о чем спросить вашего заключенного. Пусть расскажет все, пока не стало хуже!\nДействия: допрос, Отношения - 10", 
				"Заключенный ведет себя вызывающе, либо скрывает важную информацию? Посмотрим, как он запоет на дыбе! Эй, палач!\nДействия: пытки, Отношения - 80", 
				"Возможно, свидание с семьей повлияет на этого человека.", 
				"Бросить его в волчью яму! Пусть он сгниет во тьме и одиночестве!",
	
				"Кажется, вы слегка погорячились. Этот человек свободен.", 
				"Все имеет свою цену, в том числе и свобода. Заплати выкуп, и ты больше не пленник.", 
				"Этот человек будет лишен всех своих владений и отправлен в изгнание. Под страхом смерти ему будет запрещено возвращаться.", 
				"Его жизнь была достаточно долгой. Пришло время с ней попрощаться. Казнить, нельзя помиловать."
				};

		ActionsResults = new string[] {
				"Вы назначили встречу.",
				"Вы попросили об услуге.",
				"Вы оказали помощь.",
				"Вы послали предупреждение.",
				"Вы объявили благодарность.\nОтношения + 5",
				"Выплачена награда.\nЗолото - 20, Отношения + 10",
				"Вы подарили феодальное владение.\nЗемли - 1, Отношения + 30",
				"Назначен новый советник на вакантную должность.\nОтношения + 10",
				"Вы уволили советника.\nОтношения - 15",
				"Вы отобрали владение, принадлежащее этому человеку.\nЗемли + 1, Отношения - 50",
				"В отношение этого человека назначен суд.\nОтношения - 10",
				"Вы арестовали этого человека. Он ожидает вашего решения в тюрьме.\nОтношения - 30",
				"Ваша тайная полиция приступила к работе.",
				"Ваша тайная полиция приступила к работе.",
				"Ваша тайная полиция приступила к работе.",
				"Ваша тайная полиция приступила к работе.",
				"Вы уволили советника.\nОтношения - 15",
				"Вы отобрали владение, принадлежащее этому человеку.\nЗемли + 1, Отношения - 50",
				"В отношение этого человека назначен суд.\nОтношения - 10",
				"Вы конфисковали все имущество этого человека.\nОтношения - 60",
				"Произведен допрос заключенного.\nОтношения - 10",
				"Заключенного пытают!\nОтношения - 80",
				"К заключенному привели семью.",
				"Заключенного бросили в волчью яму.",
				"Вы освободили заключенного.",
				"Вы требуете выкуп.",
				"Этот человек отправлен в изгнание.",
				"Заключенный был казнен.",
				"",
/* 30 */		"Это действие невозможно выполнить!",
/* 31 */		"Этот человек уже был награжден в этом месяце.",
/* 32 */		"Этот человек уже занимает должность в совете; нужно освободить советника от должности, прежде чем дать ему другое назначение.",
				"Этот человек отказался от встречи с вами!",
				""
			};

		PersonInfoShorts = new string[] {
			"Текущая должность: ",
			"нет",
			"Возраст: ",
			"Влияние: ",
			"Отношение: ",
			"Политич. взгляды: ",
			"Происхождение: ",
			"Вера: ",
			"Культура: ",
			"Навыки: ",
			"Особенности: ",
			"Эта должность вакантна; нажмите еще раз, чтобы назначить на нее кого-либо.",
			"Нажмите на портрет еще раз для подробной информации."
			};


	}


// активировать инфопанель и показать инфу о персонаже
		private void ShowPersonInfo (Person Pers, string s) {
			ShowInfoPanel (true);
			
//			ShowSpeakerFace (Pers, true);
			Parent.InsertFace (PImage, Pers);
			TextvOkno(InfoText, s);
			TextvOkno(InfoName, Pers.Name);
		}

	// тоже самое - спрятать
		private void HidePersonInfo (Person Pers) {
//			ShowSpeakerFace (Pers, false);
			TextvOkno(InfoText, "");
			TextvOkno(InfoName, "");

//			ShowInfoPanel (false);
		}

		override public void ShowInfoPanel (bool flag) { 
			PersonInfoPanel.gameObject.SetActive (flag);
			if (!flag)
				HidePersonInfo (null);
		}

		// показать/спрятать панель действий с персонажем
		override public void ShowActionPanel (bool flag) { 
			PersonActionPanel.gameObject.SetActive (flag);
			if (flag)
				ClearButtonsText ();
			if (ConfirmAction.IsActive()) {    // кнопки да-нет всегда отключены, пока нет выбора
				ConfirmAction.gameObject.SetActive (false);
				RefuseAction.gameObject.SetActive (false);
				}
			FreePosts.gameObject.SetActive (false);  // скроллист тоже отключен
			ClearPost ();
		}

	// чистое окно со списком действий для персонажа
	private void ShowPersonAction (Person Pers) {
			Parent.DisplayClear ();
			ShowActionPanel (true);
			Parent.InsertFace (PImage2, Pers);
			TextvOkno (InfoName2, Pers.Name);
		}

	// перегруженная функция, которая показывает персону
	public override sealed Person ShowPerson (Person Pers, int value, bool ok) {
			if (Pers != null) CurrentPerson = Pers;  // запоминаем последнее лицо
			if (CurrentPerson != null) {	// работаем с последним лицом (в т.ч. если аргумент нулл)
				if (!SwitchPersonAction) 	
					PersonInfoP (CurrentPerson);
				else 						
					PersonAction (CurrentPerson, value, ok);
				}
			else Debug.Log ("Никого нет дома!");

			return CurrentPerson;
	}


	// окно с информацией о персонаже; в будущем надо показывать только revealed инфу
	private void PersonInfoP (Person Pers) {
		if (Pers == null) return;
		string s1 = "";
	//	s1 = Pers.Name;
		s1 += "\n" + PersonInfoShorts[0];
		if (Pers.GetPost () != null) 	
			s1 += Pers.GetPost ().Titul;
		else s1 += PersonInfoShorts[1];
		s1 += "\n\n" + PersonInfoShorts[2] + Pers.Age.ToString() + "\n" + PersonInfoShorts[3] + 
			Pers.Influence + "\n" + PersonInfoShorts[4] + Pers.ShowRelation () + "\n\n" + PersonInfoShorts[5] + 
			Pers.PoliticalView;
		s1 += "\n" + PersonInfoShorts[6] + Pers.SocialGroup + "\n" + PersonInfoShorts[7] + 
			Pers.Religion + "\n" + PersonInfoShorts[8] + Pers.Culture;
		s1 += "\n\n" + PersonInfoShorts[9] + Pers.StatsToString() + "\n" + PersonInfoShorts[10] + 
			Pers.Traits.Trait1;
//		SpeakerSay (Pers, BigWindow, s1); // вывод в сейдайлог
		ShowPersonInfo (Pers, s1);     // другой вариант вывода того же самого
		}

	// тоже самое, но кратко для небольшого текстового окна
	public void ShowBriefInfo (Person Pers, Text text) {
		string s1 = "";
		if (Pers == null) s1 = PersonInfoShorts[11];
		else {
			s1 = Pers.Name + ",  ";
			if (Pers.GetPost () != null) 	s1 += Pers.GetPost ().Titul;
			s1 += "\n" + PersonInfoShorts[3] + Pers.Influence + ",  " + PersonInfoShorts[4] + 
				Pers.ShowRelation () + ",  " + PersonInfoShorts[5] + Pers.PoliticalView + "\n" + PersonInfoShorts[12];
			}
		TextvOkno (text, s1);
	}


// выбираем какие действия совершить с персонажем
	private void PersonAction (Person Pers, int choice, bool ok) {
	//		bool ActionFlag = false;
			if (Pers.isPrisoner) 	
				PrisonerAction (Pers, choice, ok);
	
	/* первые 4 чойса - кнопки, в ответ на них надо отрисовать вторую колоку кнопок (текст)
	выборы с 5 по 20 - действия, на них надо менять параметры Персоны и проч. */

			switch (choice) {
				case 0: {  // пуск по умолчанию, надо показать панельку 
						ShowPersonAction (Pers);  
						}
						break;
				case 1: 
				case 2:
				case 3:
				case 4:	// пишем текст на кнопках, в зав-ти от выбора 1-4
						{
						ChangeButtonsText (PersonActions, choice); 
						}
						break;
 				case 5: { // "Пригласить на аудиенцию",
						if (ok) { 
							// StartDialog (Pers);
							//PersonAction (Pers, 0, false);
							if (Pers.Invite ())	ReportDecision (choice-4);
							else ReportDecision (33);
						}
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 6: { // "Попросить об услуге",
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 7: { // "Предложить помощь"
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 8: { // "Угрожать"}
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 9: { // "Объявить благодарность"
						if (ok && !Pers.isRewarded) { 
							Pers.RelationPositive += 5;
							Pers.isRewarded = true;
							ReportDecision (choice-4);
						}
						else if (ok && Pers.isRewarded)
							ReportDecision (31);
						else 
							PrepareDecision (true, choice-4);
						}
						break;
				case 10: { // "Наградить деньгами"
						if (ok && !Pers.isRewarded && Strana.Treasure >= 20) { 
							Strana.Treasure -= 20;
							Pers.RelationPositive += 10;
							Pers.PersonalWealth += 20;
							Pers.isRewarded = true;
							ReportDecision (choice-4);
						}
						else if (ok && Pers.isRewarded)
							ReportDecision (31);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 11: { // "Пожаловать титул (и земли)"
						if (ok && !Pers.isRewarded && Strana.Ruler.GrantHoldings (Pers)) { 
							Pers.RelationPositive += 30;
							Pers.isRewarded = true;
							ReportDecision (choice-4);
						}
						else if (ok && Pers.isRewarded)
							ReportDecision (31);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 12: { // "Назначить советником"
	// NB! падает ексепшн если повторно дать должность уволенному чудаку
						// вначале проверяем, что у него нет должности
						if (ok && Pers.GetPost () != null)
							ReportDecision (32);
						// шаг 1 - активировать дроплист, оставить кнопки
						else if (ok && !DropListActive) {  
							DropListActive = true;
							PrepareDecision (true, choice-4); // NB! здесь это нужно
							ChoosePost ();
						}
						// шаг 2 - выбрать и назначить советника, спрятать кнопки
						else if (ok && DropListActive) {  
							string s = GetDropListPost ();
							if (Strana.Government.PlaceToOffice (Pers, s) != null) {
								Pers.RelationPositive += 10;
								ReportDecision (choice-4);
								}
							else 	// нельзя назначить на пустую должность; 
								ReportDecision (30);
							DropListActive = false;
							}
						else {
							DropListActive = false;
							PrepareDecision (true, choice-4);
							}
						}
						break;
				case 13: { // "Удалить из совета"
						if (ok && Strana.Government.FireFromOffice (Pers)) { 
							Pers.RelationNegative -= 15;
							ReportDecision (choice-4);
						}
						else if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 14: { // Лишить титула"
						if (ok && Pers.GrantHoldings (Strana.Ruler)) { 
							Pers.RelationNegative -= 50;
							ReportDecision (choice-4);
						}
						else if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 15: { // "Призвать к ответу в суде"
						// установить флаг суда
						if (ok) { 
							Pers.RelationNegative -= 10;
							ReportDecision (choice-4);
						}
						else if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 16: { // "Взять под стражу"
			//			if (ok) { 
						//if (arest == success ) 
			//				Pers.isPrisoner = true;
			//				ReportDecision (choice-4);
			//				}
						if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 17: { // "Получить информацию"
					//	if (ok)
					//		ReportDecision (choice-4);
						if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 18: { // "Скомпрометировать"
					//	if (ok)  
					//		ReportDecision (choice-4);
						if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 19: { // "Шантажировать"
					//	if (ok)
					//		ReportDecision (choice-4);
						if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;
				case 20: { // "Ударить в спину"
					//	if (ok)
					//		ReportDecision (choice-4);
						if (ok)
							ReportDecision (30);
						else
							PrepareDecision (true, choice-4);
						}
						break;				
				default: {
						ShowPersonAction (Pers);
						}
						break;
				}

		}

	private void PrisonerAction (Person Pers, int choice, bool ok) {
			if (!Pers.isPrisoner) 	PersonAction (Pers, choice, ok);
			
			switch (choice) {
				case 0: {
						ShowPrisonerAction (Pers);
						}
						break;
				case 1: 
				case 2:
				case 3:
				case 4:	{
						ChangeButtonsText (PrisonerActions, choice);
						}
						break;
				case 5: { // "Удалить из совета"
						if (ok) { 
							Strana.Government.FireFromOffice (Pers);
							Pers.RelationNegative -= 15;
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 6: { // "Лишить титула"
						if (ok && Pers.GrantHoldings (Strana.Ruler)) { 
							Pers.RelationNegative -= 50;
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 7: { // "Призвать к ответу в суде"
						// установить флаг суда
						if (ok) { 
							Pers.RelationNegative -= 10;
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 8: { // "Конфисковать имущество"
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 9: {
						}
						break;
				case 10: {
						}
						break;
				case 11: {
						}
						break;
				case 12: {
						}
						break;
				case 13: { // Допрос с пристрастием
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 14: { // Подвергнуть пыткам"
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 15: { // "Привести родственников"
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 16: { // "Заточить в склеп"
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 17: { // "Освободить"
						if (ok) { 
							Pers.isPrisoner = false;
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 18: { // "Потребовать выкуп"
						if (ok) { 
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 19: { // "Отправить в изгнание"
						if (ok) { 
							Strana.Dvor.RemovePerson (Pers);
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;
				case 20: { // "Казнить"	
						if (ok) { 
							Strana.Dvor.RemovePerson (Pers);
							ReportDecision (choice-4);
						}
						else
							PrepareDecision (true, choice-4 + 16);
						}
						break;	
				default: {
						ShowPrisonerAction (Pers);
						}
						break;
				}
		}


	// переименовать кнопки для пленника
	// NB! падает в эксепшн
	private void ShowPrisonerAction (Person Pers) {
			ShowPersonAction (Pers);

			Button Btn;		Text T;
			for (int i=1; i<=5; i++) {
				Btn = GameObject.Find ("ChoiceButton"+i.ToString()).GetComponent<Button>();
				T = Btn.GetComponentInChildren<Text>();
				TextvOkno (T, PrisonerActions[0][i-1]);
			}
	}

	// проверяем флаг переключения из режима просмотра в режим действий
	public bool GetActionFlag () {
		return SwitchPersonAction;
	}

	public void SetActionFlag (bool value) {
		SwitchPersonAction = value;
	}

	public void ClearActionFlag () {
		SwitchPersonAction = false;
	}


	// прописываем опции для действий персонажа в окне персоны
		private void ChangeButtonsText (string[][] s, int str) {
			Button Btn;		Text T;
			string ChoiceButton;
			for (int i=5; i<=8; i++) {
				ChoiceButton = "ChoiceButton" + i.ToString();
				Btn = GameObject.Find (ChoiceButton).GetComponent<Button>();
				T = Btn.GetComponentInChildren<Text>();
				TextvOkno (T, s[str-1][i-5]);
			}
		}

		private void ClearButtonsText () {
	//		ChangeButtonsText (PersonActions,5);
		}

		// показать кнопки да и нет (вначале скрыты), дать описание действия
		private void PrepareDecision (bool show, int str) {
			ConfirmAction.gameObject.SetActive (show);
			RefuseAction.gameObject.SetActive (show);
		
			if (show && str == 8 && DropListActive)  { // для назначения советником
				TextvOkno (InfoText2, "");
				FreePosts.gameObject.SetActive (true);
				}
			else 	{
				FreePosts.gameObject.SetActive (false); // в остальных случаях
				TextvOkno (InfoText2, ActionsDescription[str-1]);
				}
		}

		// отобразить результат действия, спрятать кнопки
		private void ReportDecision (int str) {
			ConfirmAction.gameObject.SetActive (false);
			RefuseAction.gameObject.SetActive (false);
			FreePosts.gameObject.SetActive (false);

			TextvOkno (InfoText2, ActionsResults[str-1]);
		}

	
		// показать список пустующих постов для выбора из дроплиста
		private void ChoosePost () {
			Posts P;	int count = 0;
			Office Gov = Strana.Government;
			
			ClearPost ();

			for (int i=0; i < Gov.GetSize (); i++) {
				if ((P = Gov.GetPost (i)) != null && P.GetHolder () == null) {
					DDListOpt.Add (P.Titul);
					count ++;
					}
				}
			if (count == 0) 	DDListOpt.Add (Gov.NoFreePlace);
			FreePosts.AddOptions (DDListOpt);
			FreePosts.value = 0;
			//FreePosts.Show ();
			FreePosts.gameObject.SetActive (true);
		}

		private void ClearPost () {
			DDListOpt.RemoveRange (0, DDListOpt.Count);
			FreePosts.ClearOptions ();
		}

		// получить текущий выбранный пост из дроплиста
		private string GetDropListPost () {
			Debug.Log ("value " + FreePosts.value);
			return FreePosts.options[FreePosts.value].text;
		}

	
}
}