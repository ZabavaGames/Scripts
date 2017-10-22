using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;


namespace JagaJaga
{

public enum FormList { Empty = 0, Court_List, Court_Post_List, Posts_List, Branch_List, 
		Office_Form, Army_Form, Dvor_Form, Trade_Form, Cult_Form, Court_Form, Counsil_Form,
		Spy_Form, Main_Form, Strat_Form, Diplo_Form, Game_Form, Project_Form, Task_Form, 
		Private_Form, Popup, PopInfo, PersonInfo }

public class Display : Story {

	private Flowchart Flow; 
	private View MainView;
	private SayDialog BigWindow, SayWindow, SideWindow;
	private MenuDialog LongList; 
	protected Text StoryText2;

	public Story MainStory;
	private Character Char1, Char2, Char3, Char4, Char5;
//	private Speaker Char2; 
	private Person Empty; 
	private struct CurrentDialog {
		public Dialogue CD;
		public int CD_level;
		public Person CD_AskingPerson;
		public string[] CD_AskText;
		public Action CD_NextAction;
		}
	private CurrentDialog CurDial = new CurrentDialog ();
	private string CurrentTalk;
	private bool SpeakingCharLeft, SpeakingCharRight;
	private bool SayBlockExecution, exitSayWait;  // эти флаги нужны для последовательного выполнения корутин

	private Court CourtBranch;  // временный двор, созданный для частичного списка; 
								// каждый раз создается копия большого двора, возможно, это вредно для памяти? (сбор мусора)
	private int CourtListPos;    // позиция конца списка двора
	
	protected CanvasGroup BackPanel, ListBottomPanel, Zaglushka;
	protected CanvasGroup OfficePanel, CourtPanel, ArmyPanel, Army2Panel, TradePanel, Trade2Panel;
	protected CanvasGroup CultPanel, Cult2Panel, DvorPanel, SpyPanel, Spy2Panel, MainPanel, PrivatePanel;
	protected CanvasGroup CounsilPanel, StratPanel, DiploPanel, GamePanel, ProjectPanel, TaskPanel;
	protected CanvasGroup ArmyScrollView, DvorScrollView, TradeScrollView, CultScrollView;
	protected CanvasGroup PopupPanel, PopInfoPanel;

	private Stack<FormList> FormStack;   // стек для вложенных экранных форм
	private FormList ReturnFromPersonToPlaceFlag;  // куда возвращаться из окна персонажа (см. FormList)
//	protected FormList ReturnToForm;
	
	private OfficeForm OfficeForm;
	private ArmyForm ArmyForm;
	private DvorForm DvorForm;
	private TradeForm TradeForm;
	private CultForm CultForm;
	private CourtForm CourtForm;
	private CounsilForm CounsilForm;
	private SpyForm SpyForm;
	private MainForm MainForm;
	private StratForm StratForm;
	private DiploForm DiploForm;
	private GameForm GameForm;
	private ProjectForm ProjectForm;
	private TaskForm TaskForm;
	private PrivateForm PrivateForm;
	public PersonInfo PersonInfo;
	public Popup Popup;
	public PopInfo PopInfo;


/******************************************************************
				И Н И Ц И А Л И З А Ц И Я
*****************************************************************/

	void Start () {

		BackPanel = GameObject.Find ("BackPanel").GetComponent<CanvasGroup>();
		Zaglushka = GameObject.Find ("Глушак").GetComponent<CanvasGroup>();
		OfficePanel = GameObject.Find ("OfficePanel").GetComponent<CanvasGroup>();
		ListBottomPanel = GameObject.Find ("ListNavPanel").GetComponent<CanvasGroup>();
		CourtPanel = GameObject.Find ("CourtPanel").GetComponent<CanvasGroup>();
		ArmyPanel = GameObject.Find ("ArmyPanel").GetComponent<CanvasGroup>();
		Army2Panel = GameObject.Find ("Army2Panel").GetComponent<CanvasGroup>();
		TradePanel = GameObject.Find ("TradePanel").GetComponent<CanvasGroup>();
		CultPanel = GameObject.Find ("CultPanel").GetComponent<CanvasGroup>();
		Trade2Panel = GameObject.Find ("Trade2Panel").GetComponent<CanvasGroup>();
		Cult2Panel = GameObject.Find ("Cult2Panel").GetComponent<CanvasGroup>();
		DvorPanel = GameObject.Find ("DvorPanel").GetComponent<CanvasGroup>();
		SpyPanel = GameObject.Find ("SpyPanel").GetComponent<CanvasGroup>();
		Spy2Panel = GameObject.Find ("Spy2Panel").GetComponent<CanvasGroup>();
		MainPanel = GameObject.Find ("MainPanel").GetComponent<CanvasGroup>();
		CounsilPanel = GameObject.Find ("CounsilPanel").GetComponent<CanvasGroup>();
		StratPanel = GameObject.Find ("StratPanel").GetComponent<CanvasGroup>();
		DiploPanel = GameObject.Find ("DiploPanel").GetComponent<CanvasGroup>();
		GamePanel = GameObject.Find ("GamePanel").GetComponent<CanvasGroup>();
		ProjectPanel = GameObject.Find ("ProjectPanel").GetComponent<CanvasGroup>();
		TaskPanel = GameObject.Find ("TaskPanel").GetComponent<CanvasGroup>();
		PrivatePanel = GameObject.Find ("PrivatePanel").GetComponent<CanvasGroup>();
		PopupPanel = GameObject.Find ("PopupPanel").GetComponent<CanvasGroup>();
		PopInfoPanel = GameObject.Find ("PopInfoPanel").GetComponent<CanvasGroup>();
		
		StoryText2 = GameObject.Find ("StoryText2").GetComponent<Text>();

		OfficeForm = GameObject.Find ("OfficeForm").GetComponent<OfficeForm>();
		ArmyForm = GameObject.Find ("ArmyForm").GetComponent<ArmyForm>();
		DvorForm = GameObject.Find ("DvorForm").GetComponent<DvorForm>();
		TradeForm = GameObject.Find ("TradeForm").GetComponent<TradeForm>();
		CultForm = GameObject.Find ("CultForm").GetComponent<CultForm>();
		CourtForm = GameObject.Find ("CourtForm").GetComponent<CourtForm>();
		CounsilForm = GameObject.Find ("CounsilForm").GetComponent<CounsilForm>();
		SpyForm = GameObject.Find ("SpyForm").GetComponent<SpyForm>();
		MainForm = GameObject.Find ("MainForm").GetComponent<MainForm>();
		StratForm = GameObject.Find ("StratForm").GetComponent<StratForm>();
		DiploForm = GameObject.Find ("DiploForm").GetComponent<DiploForm>();
		GameForm = GameObject.Find ("GameForm").GetComponent<GameForm>();
		ProjectForm = GameObject.Find ("ProjectForm").GetComponent<ProjectForm>();
		TaskForm = GameObject.Find ("TaskForm").GetComponent<TaskForm>();
		PrivateForm = GameObject.Find ("PrivateForm").GetComponent<PrivateForm>();
		Popup = GameObject.Find ("Popup").GetComponent<Popup>();
		PopInfo = GameObject.Find ("PopInfo").GetComponent<PopInfo>();
		PersonInfo = GameObject.Find ("PersonInfo").GetComponent<PersonInfo>();

		MainStory = GameObject.Find("Story").GetComponent<Story>(); 
		Char1 = GameObject.Find("СобеседникМВ").GetComponent<Character>(); 
		Char2 = GameObject.Find("СобеседникМЗ").GetComponent<Character>(); 
		Char3 = GameObject.Find("СобеседникМС").GetComponent<Character>(); 
		Char4 = GameObject.Find("СобеседникЖ").GetComponent<Character>(); 
		Char5 = GameObject.Find("Спецперсона").GetComponent<Character>(); 
		Empty = GameObject.Find("Poleno").GetComponent<Person>(); 
	
		BigWindow = GameObject.Find ("SayDialog2").GetComponent<SayDialog>();	
		SayWindow = GameObject.Find ("SayDialog1").GetComponent<SayDialog>();	
		SideWindow = GameObject.Find ("SayDialog3").GetComponent<SayDialog>();	
		LongList = GameObject.Find ("MenuDialog1").GetComponent<MenuDialog>();	
		Flow = GameObject.Find ("Flowchart").GetComponent<Flowchart>();
//		MainView = GameObject.Find ("View").GetComponent<View>();

		FormStack = new Stack<FormList>();
		ReturnFromPersonToPlaceFlag = FormList.Empty;
		CourtListPos = 0;
		exitSayWait = SayBlockExecution = SpeakingCharLeft = SpeakingCharRight = false;
		CurrentTalk = "";
		}

	// при первом запуске надо убрать заглушку и очистить экран
	public void Init () {
		DisplayClear ();
		ShowZaglushka (false);
	}

	// проверочная фигня
	public void TestRun () {
		Debug.Log ("Тестируем окно перса");			
		StoryText2.text = "Вот это вот я пишу из программы, ла-ла!";
		SpeakerSay (Empty, BigWindow, "А это сообщение я послал через сэйдайлог товарища Фунгуса, вот так вот!");
	}



/******************************************************************
	      		П Е Р С О Н А Ж И   И   Д И А Л О Г И
*******************************************************************/

		private Character SetSpeaker1 (string Name, string Face) { 
			Char1.SetStandardText (Name);	
			return Char1;
			}

		private Character SetSpeaker2 (string Name, Sprite face) { 
			Char2.SetStandardText (Name);			
//			Char2.SetPortrait (SP);
			return Char2;
			}

		private Character SetSpeaker3 (string Name, string Face) { 
			Char3.SetStandardText (Name);	
			return Char3;
			}

		private Character SetSpeaker4 (string Name, string Face) { 
			Char4.SetStandardText (Name);	
			return Char4;
			}

		private Character SetSpeaker5 (string Name, string Face) { 
			Char5.SetStandardText (Name);	
			return Char5;
			}

		public int GetPortraitsCount1 () {
			return Char1.Portraits.Count;
			}

		public int GetPortraitsCount2 () {
			return Char2.Portraits.Count;
			}

		public int GetPortraitsCount3 () {
			return Char3.Portraits.Count;
			}

		public int GetPortraitsCount4 () {
			return Char4.Portraits.Count;
			}

		public int GetPortraitsCount5 () {
			return Char5.Portraits.Count;
			}


		// вставить в диалог портрет персонажа через ф-ии Фунгуса
		private Sprite ShowSpeakerFace (Person Pers, bool show) {
			Character currentCharacter;
			if ((currentCharacter = GetChar (Pers)) == null) return null;
 			Sprite currentPortrait;
			if ((currentPortrait = FindFaceByNumber (currentCharacter, Pers.Portrait)) == null) return null;
Debug.Log ("Pers is " + Pers + " show " + show);
// перс почему-то два раза подряд вызывается, к тому же отмена накладывается на след. показ

        	var stage = Stage.GetActiveStage();		
        	var portraitOptions = new PortraitOptions(true);

        	portraitOptions.character = currentCharacter;
        	portraitOptions.portrait = currentPortrait;
			portraitOptions.display = show ? DisplayType.Show : DisplayType.Hide;
			string Position = show ? "Left" : "OffscreenLeft";
        	portraitOptions.toPosition = stage.GetPosition (Position);
			portraitOptions.waitUntilFinished = true;
			portraitOptions.fadeDuration = 0f;
			
			if (show && !SpeakingCharLeft)	{
					if (stage.CharactersOnStage.Contains (currentCharacter) == false)
						stage.Show(portraitOptions);
					SpeakingCharLeft = true;
				}
			else if (!show && SpeakingCharLeft)	{
					stage.Hide(portraitOptions);
					SpeakingCharLeft = false;
				}

			return currentPortrait;
		}

		// тоже самое без фунгуса; поместить в рамку для картинки выбранный портрет
		public void InsertFace (Image Img, Person Pers) {
			Sprite currentPortrait;
			if ((currentPortrait = GetSprite (Pers)) != null) {
				Img.sprite = currentPortrait;
			}
		}

		// получить картинку перса из стека картинок; используем 2 другие ф-ии
		public Sprite GetSprite (Person Pers) {
			Sprite spr;
			if (Pers == null || (spr = FindFaceByNumber (GetChar (Pers), Pers.Portrait)) == null)
				spr = NonNameFace ();
			return spr;
		}

		private Character GetChar (Person Pers) {
			Character currentCharacter;
			
			if (Pers == null) return null;
			if (Pers.isSpecial)	// берем обычный набор портретов, или для спецперсон
				currentCharacter = SetSpeaker5 (Pers.Name, null);
			else if (Pers.Sex == Sex.Women) 	// женщины
				currentCharacter = SetSpeaker4 (Pers.Name, null);
			else if (Pers.Age > 59) 			// старики
				currentCharacter = SetSpeaker3 (Pers.Name, null);
			else if (Pers.Age > 44) 			// знать
				currentCharacter = SetSpeaker2 (Pers.Name, null);
			else 								// молодежь )))
				currentCharacter = SetSpeaker1 (Pers.Name, null);

			return currentCharacter;
		}

		// получить картинку перса по номеру портрета	
		private Sprite FindFaceByNumber (Character Char, int number) {
			if (Char != null && number >= 0 && number < Char.Portraits.Count)
				return Char.Portraits[number];
			else return null;
		}

		private Sprite NonNameFace () {
			return FindFaceByNumber (Char5, Data.NoNameFace);
		}


		// показать или спрятать портрет в диалоге фунгуса; используем ShowSpeakerFace
		private void ShowPortrait (Person Pers, SayDialog Okno, bool state) {
			if (Pers == null || Okno == null) 	return;
			if (state) {
		// подготовить диалог, перса и портрет
				Character currentCharacter = GetChar (Pers);
				Okno.SetActive (state);		
				Okno.Clear ();	
//			Okno = currentCharacter.SetSayDialog;
				Okno.SetCharacter (currentCharacter);
				string title = "";
				if (Pers.GetPost() != null)
					title = Pers.GetPost().Titul;
				Okno.SetCharacterName (Pers.Name + ", " + title, Color.blue);
				}
			Sprite currentPortrait = ShowSpeakerFace (Pers, state);
			Okno.SetCharacterImage (currentPortrait);
		}



		private IEnumerator WaitForBlock (Block b) {   // типа заглушка, уже не нужна..
			while (b.IsExecuting())
				yield return null;
		}

		public bool SetSayExecutionState (bool value) {  // установить флаг из флоучарта
			return (SayBlockExecution = value);
		}

		// выполнить действие по завершении диалога
		public override sealed void FinishDialogue (Action onComplete) {
			StartCoroutine (DoFinish (onComplete));
        }

		private IEnumerator DoFinish (Action onComplete) {
			while (SayBlockExecution)
                yield return null;
			if (!SayBlockExecution)
				if (onComplete != null)
    	        	onComplete();
		}

	// куорутина, которая кидает реплику в диалог и сигналит флагом о завершении
		private IEnumerator VOkno (SayDialog Okno, Person Pers, string s) { 
			exitSayWait = false;
			SayBlockExecution = true;

			Okno.Say (s, true, true, true, false, null, () => { 
				exitSayWait = true; });

			while (!exitSayWait)
                yield return null;
			if (exitSayWait) {
				ShowPortrait (Pers, SayWindow, false); 
				SayBlockExecution = false;
				}
	        exitSayWait = false;
		}

// гавкнуть в сейдайлог, в порядке общей очереди, через корутины
		private IEnumerator SpeakerSay (Person Pers, SayDialog Okno, string s) {
			
			while (SayBlockExecution)
                yield return null;
			if (!SayBlockExecution) {
				ShowPortrait (Pers, SayWindow, true);
				yield return StartCoroutine (VOkno (Okno, Pers, s));
				}
		}

// переопределнная ф-ия для подачи реплики в диалог
		public override sealed void Talk (Person Pers, string text) {
	// первый вариант через функцию, которая запускает SayDialog.Say в кач-ве корутины	
			ShortSpeak (Pers, text);
//			RunFlow ("Pause");

	// вариант 2 - мы передаем управление в блок, который считывает фразу из пременной и выдает ее в диалог
		//	CurrentTalk = text;
		//	SetSayExecutionState (true);
		//	Block g = Flow.FindBlock ("Govorilka");
		//	g.StartExecution ();
		//	или RunFlow (g.BlockName); 

		// StartCoroutine (WaitForBlock (g)); // я хз как еще реализовать заглушку на ожидание выхода из блока
							// наверное надо использовать корутину

// в.3 - попытка сформировать новый блок для выдачи команды
//			Block Speech = Flow.FindBlock ("Temp");
//			var com = Undo.Add as Say;
//			Speech.CommandList.Add ( com );
//			Speech.StartExecution ();
		//	RunFlow ("Temp");

		}


		public override sealed void StartDialogue (Dialogue d, Action onComplete) {
			StartLinearDialogue (d, 0, onComplete);
		}

// запуск диалога; пихаем все строчки Talk сразу, а потом вызываем корутину
// с выбором ответа обрываем исполнение, чтобы продолжить после вызова из менюшки
// делал на скорую, может, нужно переделать, но вроде работает...
	private void StartLinearDialogue (Dialogue d, int startphrase, Action onComplete) {
		Phrase ph;
		string replika;
		string[] choice;
		int i = startphrase, j, link, result;
		Person[] P = { d.Pers1, d.Pers2 };
		Person Pers = P[1];

		while (i < d.count) {
			link = 0;
			if ((ph = d.D_GetPhrase (i)) != null) {
				if (ph.Actor != null) Pers = ph.Actor;
				if ((replika = ph.Ph_GetReplika (MainStory)) != null) {
		//Debug.Log ("это фраза из " + ph.Replika.Count + " реплик");
						result = ph.Ph_GetResult (MainStory);
						Talk (Pers, replika);
						}
				else if ((choice = ph.Ph_GetChoice ()) != null) {
		//Debug.Log ("это вопрос из " + choice.Length + " пунктов");
						AskMain (Pers, choice, i, d, onComplete);
						return; 
						}
				else if ((link = ph.Ph_GetDecision (MainStory)) != 0) {
		//Debug.Log ("это выбор");
						i = link;
						result = ph.Ph_GetResult (MainStory);
						}
				if (ph.IsFinal ()) {
		//Debug.Log ("это конец беседы");
						break;
						}
				}
			if (link == 0) { // если не десижн, то смотрим переход по схеме диалога
				if ((j = d.D_GetLink (i, 0)) > 0)
					i = j;
				else
					i++;
				}
			}
		
	//Debug.Log ("Аудиенция окончена"); // снимаем флаг аудиенции с персонажа
		P[1].Dismiss ();  
		FinishDialogue (onComplete);
	}

// тоже самое в пошаговом режиме, предусматривается возможность обратного запуска из обработчика дисплея
// чтобы не делать рекурсивный Talk
// по сути уже не нужно
	private void StepDialogue (Dialogue d, int startphrase, int choice, Action onComplete) {
		Person[] P = { d.Pers1, d.Pers2 };
		Phrase ph;
		int i = startphrase;
		if (i < d.count) {
			if ((ph = d.D_GetPhrase (i, choice)) != null) {
					if (ph.IsChoice ()) {
						AskMain (q2(i+1) ? P[0] : P[1], ph.Ph_GetChoice (), startphrase, d, onComplete);
						return;
						}
					else
						Talk (q2(i+1) ? P[0] : P[1], ph.Ph_GetReplika (MainStory));
//					if (ph.IsFinal ())
//						break;
				}
			}

		FinishDialogue (onComplete);
	}

// а здесь надо выдать меню вариантов ответа для гл. персонажа и потом передать в Talk
		private void AskMain (Person Pers, string[] text, int startphrase, Dialogue d, Action onComplete) {
			CurDial.CD_AskingPerson = Pers;
			CurDial.CD_AskText = text;
			CurDial.CD = d;
			CurDial.CD_level = startphrase;
			CurDial.CD_NextAction = onComplete;

			StartCoroutine (ReplikMenu (Pers, text));
		}

// делаем меню вариантов ответа на базе диалога MenuDialog из Fungus'a
		private IEnumerator ReplikMenu (Person Pers, string[] text) {
			while (SayBlockExecution)
                yield return null;
			if (!SayBlockExecution) {
				ShowForm (FormList.Branch_List, 0);
				ShowSayMenu (true);
				
				string s;
				for (int i=0; i<text.Length; i++) {
					s = "AskMain"+(i+1);
					LongList.AddOption (text[i], true, Flow.FindBlock(s));  // ищем заранее созданные блоки
					}
	
				SayBlockExecution = true;
			}
		}

	// ф-ия получает ответ из блока меню во флоучарте и перезапускает диалог, используя сохр. данные
		public void GetChoice (int choice) {
			ShowSayMenu (false);
			ShowDialog (true);
			SayBlockExecution = false;
			ShowPreviousForm (0);
	// отображаем в диалоге выбранный вариант ответа
			Talk (CurDial.CD_AskingPerson, CurDial.CD_AskText[choice-1]);
		//	CurDial.CD.D_GetPhrase (?) ЗДЕСЬ НАдо как-то результат вызвать
	// и передаем управление в диалог
			int c = 0;
//			StepDialogue (CurDial.CD, CurDial.CD_level, choice, CurDial.CD_NextAction);
			if ((c = CurDial.CD.D_GetLink (CurDial.CD_level, choice)) < 0) c = CurDial.CD_level + 1;
			StartLinearDialogue (CurDial.CD, c, CurDial.CD_NextAction);
		}


		private bool q2 (int x) {
			if (x % 2 == 0) return true;
			else return false;
		}

		private void ShortSpeak (Person P, string text) {
			StartCoroutine (SpeakerSay (P, SayWindow, text));
		}

		private void LongSpeak (Person P, string text) {
			StartCoroutine (SpeakerSay (P, BigWindow, text));
		}

		private void StorySpeak (string text) {
	//		TextvOkno (StoryText2, text);
			StartCoroutine (SpeakerSay (null, SideWindow, text));
		}

		public string GetCurrentTalk () {   // для вызова из блока говорилка из флоучарта
			return CurrentTalk;
		}

		protected void TextvOkno (Text T, string s) {
			T.text = s;
		}

		public void RunFlow (string block) {
	//		Flow.StopAllBlocks ();			
			Flow.ExecuteBlock (block);
		}

		public void SetScene () {
//			FadeToView f = Command as FadeToView;
//			f.TargetView = MainView;
//			Flow.AddSelectedCommand ( f );

		}

// переопределяем так, чтобы вызвать ф-ию из родителя
		public override sealed void StoryRun (int flag, FormList form) {
			MainStory.StoryRun (flag, form);	
		}




/******************************************************************
					П А Н Е Л И    И   В Ы В О Д
********************************************************************/

		// показать/спрятать панель информации о персонаже
		virtual public void ShowInfoPanel (bool flag) { 
			PersonInfo.ShowInfoPanel (flag);
		}

		// показать/спрятать панель действий с персонажем
		virtual public void ShowActionPanel (bool flag) { 
			PersonInfo.ShowActionPanel (flag);
		}

		// показать/спрятать панель информации о правительстве
		protected void ShowOfficePanel (bool flag) { 
			OfficePanel.gameObject.SetActive (flag);
		}

		protected void ShowCourtPanel (bool flag) { 
			CourtPanel.gameObject.SetActive (flag);
		}

		protected void ShowArmyPanel (bool flag) { 
			ArmyPanel.gameObject.SetActive (flag);
		}

		protected void ShowArmy2Panel (bool flag) { 
			Army2Panel.gameObject.SetActive (flag);
		}

		protected void ShowTradePanel (bool flag) { 
			TradePanel.gameObject.SetActive (flag);
		}

		protected void ShowTrade2Panel (bool flag) { 
			Trade2Panel.gameObject.SetActive (flag);
		}

		protected void ShowCultPanel (bool flag) { 
			CultPanel.gameObject.SetActive (flag);
		}

		protected void ShowCult2Panel (bool flag) { 
			Cult2Panel.gameObject.SetActive (flag);
		}

		protected void ShowDvorPanel (bool flag) { 
			DvorPanel.gameObject.SetActive (flag);
		}

		protected void ShowSpyPanel (bool flag) { 
			SpyPanel.gameObject.SetActive (flag);
		}

		protected void ShowSpy2Panel (bool flag) { 
			Spy2Panel.gameObject.SetActive (flag);
		}

		protected void ShowMainPanel (bool flag) { 
			MainPanel.gameObject.SetActive (flag);
		}

		protected void ShowCounsilPanel (bool flag) { 
			CounsilPanel.gameObject.SetActive (flag);
		}

		protected void ShowStratPanel (bool flag) { 
			StratPanel.gameObject.SetActive (flag);
		}

		protected void ShowDiploPanel (bool flag) { 
			DiploPanel.gameObject.SetActive (flag);
		}

		protected void ShowGamePanel (bool flag) { 
			GamePanel.gameObject.SetActive (flag);
		}

		protected void ShowTaskPanel (bool flag) { 
			TaskPanel.gameObject.SetActive (flag);
		}

		protected void ShowProjectPanel (bool flag) { 
			ProjectPanel.gameObject.SetActive (flag);
		}

		protected void ShowPrivatePanel (bool flag) { 
			PrivatePanel.gameObject.SetActive (flag);
		}

		public void ShowPopupPanel (bool flag) {
			PopupPanel.gameObject.SetActive (flag);
		}	

		public void ShowPopInfoPanel (bool flag) {
			PopInfoPanel.gameObject.SetActive (flag);
		}	

		public void ShowBackPanel (bool flag) { 
			BackPanel.gameObject.SetActive (flag);
		}

		public void ShowZaglushka (bool flag) { 
			Zaglushka.gameObject.SetActive (flag);
		}

// показать-спрятать менюдайлог + панельку кнопок
// вызывается при заполнение списка
	private void ShowListMenu (bool flag) {	
		ShowSayMenu (flag);
		ListBottomPanel.gameObject.SetActive (flag);
	}

	private void ShowSayMenu (bool flag) {
		LongList.Clear ();
		LongList.HideSayDialog ();
		LongList.SetActive (flag);	
		ListBottomPanel.gameObject.SetActive (false); // всегда выключен для диалога
	}

// показать-спрятать сейдайлог
	private void ShowDialog (bool flag) {
		BigWindow.Clear ();
		BigWindow.Stop ();
		BigWindow.SetActive (flag);
	}

	public void RunFlowchartBlock (string s) {
		
	}


// полная очистка экрана, можно вызывать извне для зачистки (напрямер, из флоучарта)
	public void DisplayClear () {
		ShowBackPanel (true);

		ShowInfoPanel (false);
		ShowActionPanel (false);
		ShowOfficePanel (false);
		ShowArmyPanel (false);
		ShowArmy2Panel (false);
		ShowCounsilPanel (false);
		ShowCourtPanel (false);
		ShowCultPanel (false);
		ShowCult2Panel (false);
		ShowDvorPanel (false);
		ShowTradePanel (false);
		ShowTrade2Panel (false);
		ShowSpyPanel (false);
		ShowSpy2Panel (false);
		ShowMainPanel (false);
		ShowStratPanel (false);
		ShowDiploPanel (false);
		ShowGamePanel (false);
		ShowProjectPanel (false);
		ShowTaskPanel (false);
		ShowPrivatePanel (false);
		ShowPopupPanel (false);
		ShowPopInfoPanel (false);

		ShowListMenu (false);
		ShowDialog (false);
	}




/***********************************************************************
				ПОКАЗ ПЕРСОН И ДЕЙСТВИЯ С НИМИ
***********************************************************************/


	// эта функция нужна для внешнего вызова из флоучарта
	public override sealed Person CallShowPerson (int x, bool ok) {
			return ShowPerson (null, x, ok);
	}

	// перегруженная функция, которая показывает персону
	public override Person ShowPerson (Person Pers, int value, bool ok) {
			return PersonInfo.ShowPerson (Pers, value, ok);
	}


	// функция которая дает понять, в какой список возвращаться из окна персонажа
	public int GetPersonFlag () {
		return (int)ReturnFromPersonToPlaceFlag;
	}

	public void ClearPersonFlag () {
		ReturnFromPersonToPlaceFlag = FormList.Empty;
	}




/***************************************************************************
                             Э К Р А Н Н Ы Е   Ф О Р М Ы
******************************************************************************/

// выводим полный список персон с помощью меню, а также по клику отдельного чела, вызывается из меню (списка)
	public override sealed bool CourtList (int choice) {
			var Dvor = Strana.Dvor;

			return PersonList (choice, ReturnFromPersonToPlaceFlag, Dvor);
		}

// дублер списка придворных, только отображаются владельцы должностей
	public override sealed bool CourtPostList (int choice) {
			var Dvor = Strana.Dvor;

			return PersonList (choice, ReturnFromPersonToPlaceFlag, Dvor);
		}

// отображаются только должности (реверс списка владельцев)
	public override sealed bool PostsList (int choice) {
			var Dvor = Strana.Dvor;

			return PersonList (choice, ReturnFromPersonToPlaceFlag, Dvor);
		}

// тоже самое, частичный список по вышестоящему титулу
	public override sealed bool BranchList (int choice, string titul) {
			var Dvor = Strana.Dvor;
			if (ReturnFromPersonToPlaceFlag == FormList.Empty)  {
				ReturnFromPersonToPlaceFlag = FormList.Branch_List;		
				CourtListPos = 0;
				if (titul != null) CourtBranch = CreateBranch (Dvor, titul);	
				else CourtBranch = Dvor;
				}
			return PersonList (choice, ReturnFromPersonToPlaceFlag, CourtBranch);
		}


	// универсальная ф-ия показа экранной формы, проверяет стек форм при показе
	public override sealed bool ShowForm (FormList form, int choice) {
		bool value = false;

		if (form == FormList.Empty) 
			if (FormStack.Count > 0)	form = FormStack.Pop ();
			else form = FormList.Main_Form;
		else if (FormStack.Count == 0 || (FormStack.Count > 0 && FormStack.Peek () != form))
			FormStack.Push (form);
		if (ReturnFromPersonToPlaceFlag	== FormList.Empty) {
			ReturnFromPersonToPlaceFlag = form;
			CourtListPos = 0;
			}
Debug.Log(form + " " + FormStack.Count);	

		switch (form) {
			case FormList.Branch_List: value = BranchList (choice, null);
				break;
			case FormList.Court_List: value = CourtList (choice);
				break;
			case FormList.Posts_List: value = PostsList (choice);
				break;
			case FormList.Court_Post_List: value = CourtPostList (choice);
				break;

			case FormList.Army_Form: value = ShowArmy (choice);
				break;
			case FormList.Counsil_Form: value = ShowCounsil (choice);
				break;
			case FormList.Court_Form: value = ShowCourt (choice);
				break;
			case FormList.Cult_Form: value = ShowCult (choice);
				break;
			case FormList.Diplo_Form: value = ShowDiplo (choice);
				break;
			case FormList.Dvor_Form: value = ShowDvor (choice);
				break;
			case FormList.Game_Form: value = ShowGame (choice);
				break;
			case FormList.Main_Form: value = ShowMain (choice);
				break;
			case FormList.Office_Form: value = ShowOffice (choice);
				break;
			case FormList.Project_Form: value = ShowProject (choice);
				break;
			case FormList.Spy_Form: value = ShowSpy (choice);
				break;
			case FormList.Strat_Form: value = ShowStrat (choice);
				break;
			case FormList.Task_Form: value = ShowTask (choice);
				break;
			case FormList.Trade_Form: value = ShowTrade (choice);
				break;
			case FormList.Private_Form: value = ShowPrivate (choice);
				break;
			case FormList.Popup: value = ShowPopup (choice);
				break;
			case FormList.PopInfo: value = ShowPopInfo (choice);
				break;

			default: value = ShowMain (choice);
				break;
		}

		if (!value) {    // осторожно, рекурсия!
				value = ShowPreviousForm (0);
				}
		return value;
	}

	// получить номер текущий формы из стека
	public override sealed int GetActiveForm () {
			int value;
			if (FormStack.Count > 0)	
				value = (int)FormStack.Peek ();
			else 						
				value = (int)FormList.Empty;
			return value;
	}

	// отобразить текущую экранную форму
	public override sealed bool ShowActiveForm (int choice) {
			return ShowForm ((FormList)GetActiveForm (), choice);
	}

	// отобразить предыдущую форму
	private bool ShowPreviousForm (int choice) {

			FormList form = FormList.Empty;	
			int count = FormStack.Count;

			if (count > 0)	{
				FormStack.Pop (); // сбрасываем текущую	и отображаем предыдущую, если есть	
				if (count > 1)
					form = FormStack.Pop ();  
				}
			
			return ShowForm (form, choice);
	}


	// показать правительство (экранная форма) и так далее
		private bool ShowOffice (int choice) {
			return OfficeForm.ShowOfficeForm (choice);
		}

		private bool ShowArmy (int choice) {
			return ArmyForm.ShowArmyForm (choice);
		}

		private bool ShowDvor (int choice) {
			return DvorForm.ShowDvorForm (choice);
		}

		private bool ShowTrade (int choice) {
			return TradeForm.ShowTradeForm (choice);
		}

		private bool ShowCult (int choice) {
			return CultForm.ShowCultForm (choice);
		}

		private bool ShowSpy (int choice) {
			return SpyForm.ShowSpyForm (choice);
		}

		private bool ShowCourt (int choice) {
			return CourtForm.ShowCourtForm (choice);
		}

		private bool ShowMain (int choice) {
			return MainForm.ShowMainForm (choice);
		}

		private bool ShowCounsil (int choice) {
			return CounsilForm.ShowCounsilForm (choice);
		}

		private bool ShowStrat (int choice) {
			return StratForm.ShowStratForm (choice);
		}

		private bool ShowDiplo (int choice) {
			return DiploForm.ShowDiploForm (choice);
		}

		private bool ShowGame (int choice) {
			return GameForm.ShowGameForm (choice);
		}

		private bool ShowTask (int choice) {
			return TaskForm.ShowTaskForm (choice);
		}

		private bool ShowProject (int choice) {
			return ProjectForm.ShowProjectForm (choice);
		}

		private bool ShowPrivate (int choice) {
			return PrivateForm.ShowPrivateForm (choice);
		}

		private bool ShowPopup (int choice) {
			return Popup.ShowPopupForm (choice);
		}

		private bool ShowPopInfo (int choice) {
			return PopInfo.ShowPopInfoForm (choice);
		}


/*
				С П И С К И   П Е Р С О Н
*/

// список пресон, перемотка и показ отдельной персоны
	private bool PersonList (int choice, FormList form, Court Dvor) {
		bool value = true;
		Person P = null;
		int count = pos (CourtListPos - 10) + choice - 1;

		DisplayClear ();

		// если choice = 0 то стартуем заполнение, иначе показываем инфо
		switch (choice) {	
			case 0:	{  // начало или возврат из просмотра
					CourtListPos = FillLongList (CourtListPos - 10, form, Dvor);
					}
					break;
			case 11: {  // мотаем назад
					CourtListPos = FillLongList (CourtListPos - 20, form, Dvor);
					}
					break;
			case 12: { // мотаем вперед
					CourtListPos = FillLongList (CourtListPos, form, Dvor);
					}
					break;
			case 13: { // menuUp; выход из списка
					ClearPersonFlag ();					
					value = false;
					}
					break;
/*			case 14:	{   
						}
						break;
			case 15:	{  
						}
						break;*/
			case 16: {		// шахматная партия
					ShowForm (FormList.Game_Form, 0);
					}
					break;
			default:	{   // показываем  инфу по отдельному лицу из списка
						P = Dvor.GetPerson(count);
						ShowPerson (P, 0, false);
						}
						break;
			}	
			return value;   // уже необязательно
		}


	// здесь заполняем список персонами по 10шт. на стр., проверяем края на допустимость
	// если уперлись в край, отсчитываем 10ку до края
		private int FillLongList (int start, FormList form, Court Spisok) {
			Person P; 		Posts post;
			var Gov = Strana.Government;
			string s, blockname, titul;
			int startpos, position = pos (start);
			int ListPos = 0;
			
			if (Spisok != null) {
				if (form == FormList.Court_List) ListPos = Spisok.CourtSize;
				else ListPos = min (Spisok.CourtSize, Gov.GetSize());
				if (ListPos - position > 10) ListPos = position + 10;
				else if (ListPos > 10) position = ListPos - 10;
				else position = 0;
				}

			startpos = position;

			ShowListMenu (true);

			while (position < ListPos) {
				P = null; 	titul = "";
				
				if (form != FormList.Posts_List )
					// сначала перебираем персоны, если важен титул, то ищем ту, что с титулом 
					for (int i = position; i < Spisok.CourtSize; i++) {
						if ((P = Spisok.GetPerson(i)) != null) {
							if (P.GetPost () != null )
								titul = P.GetPost ().Titul;
							else if (form != FormList.Court_Post_List) 
								titul = "";
							else {
								P = null;
								titul = "";
								continue;
								}
							}
						break;
						}
				else 
					// для списка постов просматриваем список постов, а не персон
				//	for (int j = position; j < Gov.GetSize (); j++) {
					if (position < Gov.GetSize ()) {
						if ((post = Gov.GetPost (position)) != null && (P = post.GetHolder()) != null)
							titul = post.Titul;
						else	
							P = null;
					//	break;
						}

				if (P == null) break;
				// дальше выбираем, какие поля отображать в колонках списка 
				// NB! колонки съезжают по ширине из-за того, что все символы разные, нужен спец. шрифт?
				if (form == FormList.Court_List || form == FormList.Branch_List)
					s = FormatString(P.Name, 30) + FormatString(P.Age.ToString(), 4) + FormatString(P.Prestige.ToString(), 5) + FormatString(P.GetRelation ().ToString(), 5) + FormatString(titul, 20);
				else if (form == FormList.Court_Post_List) {
					s = FormatString(P.Name, 30) + FormatString(titul, 25);
					if (P.GetPost().GetChief() != null)
						s += FormatString(P.GetPost().GetChief().Titul, 20);
					}
				else if (form == FormList.Posts_List) {
					s = FormatString(titul, 25) + FormatString(P.Name, 30);
					if (P.GetPost().GetChief() != null)
						s += FormatString(P.GetPost().GetChief().Titul, 20);
					}
				else s = "";
					
				// сформированные строки - это строки меню, а кнопки привязаны к блокам флоучарта по имени блоков
				blockname = "MenuBlock" + (position - startpos +1);
				LongList.AddOption (s, true, Flow.FindBlock(blockname));
				position ++;
				}
			// возвращает позицию последнего видимого элемента +1
			return ListPos;
		}

		int min (int x, int y) {
			return x<y ? x : y;
		}

		int pos (int x) {  // positive
			return x>0 ? x : 0;
		}

		// форматируем строку пробелами, но это фигня, ширина символов все равно разная...
		string FormatString (string s, int l) {
			string s1 = s;
			if (s.Length < l)
				for (int i=s.Length; i<l; i++) 
					s1 += " ";
			return s1;
		}


	// отбираем из общего двора всех подчиненных конкретного лица в мини-список
	private Court CreateBranch (Court Dvor, string Titul) {
		Person P; 		Posts Post;
		ArrayList People = new ArrayList ();
		int CourtSize = 0;	
		Court Branch = Instantiate (Dvor);

		for (int i=0; i	< Dvor.CourtSize; i++) {
				P = Dvor.GetPerson (i);
				if (P != null && (Post = P.GetPost ()) != null)
					if (Post.GetChief() != null)
						if (string.Compare (Post.GetChief().Titul, Titul) == 0) {
							People.Add (P); 
							CourtSize ++;
							}
				}
		Branch.People = People;
		Branch.CourtSize = CourtSize;
		
		return Branch;
		}

// сортировка... не работает (
	private Court SortByTitul (Court Dvor) {
		Person Pers;
		Posts Post;
		var Office = Strana.Government;
		int j, rank = 0;

		for (int i = 0; i < Office.GetSize (); i++) {
			for (j = rank; j < Dvor.CourtSize; j++) {
				if ((Pers = Dvor.GetPerson (j)) != null && (Post = Pers.GetPost ()) != null) {
					if (Office.GetPost (i).GetHolder () == Pers) {
						Dvor.SwapPersons (rank, j);
						rank++;
						break;
						}
					}
				}
			}
		return Dvor;
	}


}
}