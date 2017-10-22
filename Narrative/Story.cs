using UnityEngine;
using System;
using System.Collections.Generic;


namespace JagaJaga
{

// основной объект для выполнения сюжета; здесь задаем условия событий и квестов и пишем диалоги;
// выполняем диалоги и вызов форм через класс дисплей, который зачем-то сделал производным от этого, ну да ладно...
// нужен отдельный класс диалога (сделано)

public struct DialogueInfo {
	public List<string[]> list;
	public int[][] links;
	public int[] choices, decisions, finals;
	public DialogueInfo (List<string[]> lst, int[][] lnk, int[] ch, int[] dec, int[] fin) {
			list = lst; links = lnk; choices = ch; decisions = dec; finals = fin;
		}
}

public enum StoryMode { tutor, audience, stranger, counsil, events }

public class Story : MonoBehaviour {

	public State Strana;
	public Display Output;
//	public Database Info;
	public TextArray Massive;

	private int Weeks, StartingWeek;
	private bool StoryFirstRun;
	private Sequence Sujet; 
	private int ReplikaChoiceFlag;
	private bool SubStoryIsReady;

	private enum StoryVar { Tutor0Showed, Tutor1Showed, Tutor2Showed, Tutor3Showed, Tutor4Showed }
	private const int StoryVarNumber = 5;
	
	private struct sujetvarstruct {
		public bool[] sv;
		public sujetvarstruct (bool flag) {
				sv = new bool[StoryVarNumber];
				for (int i = 0; i < sv.Length; i++)
					sv[i] = flag;
		}
	}
	private sujetvarstruct SVar;

	private enum OfferRewards { nothing, drink, honor, gold, holding, titul }
	private enum Sanctions { nothing, ponos, gold, holding, titul, arrest }
	private enum OfferFlag { No_Offer, Offer_Made, Offer_Accepted, Offer_Refused }

	private struct offerstruct {
		public OfferFlag flag;
		public OfferRewards reward;
		public Person Pers;
		public offerstruct (Person P, OfferFlag f, OfferRewards r) {
			Pers = P;
			flag = f;
			reward = r;
			}
	}
	private offerstruct Offer;

	private struct sanctionstruct {
		public OfferFlag flag;
		public Sanctions shtraf;
		public Person Pers;
		public sanctionstruct (Person P, OfferFlag f, Sanctions s) {
			Pers = P;
			flag = f;
			shtraf = s;
			}
	}
	private sanctionstruct Sanction;

	void Start () {
			Weeks = ReplikaChoiceFlag = 0;	StoryFirstRun = SubStoryIsReady = true;
			StartingWeek = 3;
			Output = GameObject.Find ("Output").GetComponent<Display>();
			Strana = GameObject.Find ("Kentabriya").GetComponent<State>();
//			Database = GameObject.Find ("GameData").GetComponent<Database>();
			Sujet = new Sequence ();
			Sujet.MainStory = this;
			Massive = new TextArray ();
			Offer = new offerstruct (null, OfferFlag.No_Offer, OfferRewards.nothing);
			Sanction = new sanctionstruct (null, OfferFlag.No_Offer, Sanctions.nothing);
			SVar = new sujetvarstruct (false);
		}


	// здесь стартуем; управление передается из 1-го блока флоучарта
	public void main () {
Debug.Log ("Пошла жара!");	
		Output.Init ();
		PrepareStory ();  // инициализируем сюжетку
		ShowMain ();
	//	Output.RunFlow ("ShowMain");
	//  и то и другое работает, можно вызывать отдельные блоки из флоучарта, которые через invoke method
	//  вызывают публичные функции, или делают что-то еще, например, реагируют на кнопки или меняют вью
		}


	private void WeeklyUpdate () {
		Weeks += 1; 
		if (q4(WeeksNumber ()))
			Strana.MonthlyUpdate ();
	// здесь всякие разные апдейты ситуаций, заданий и прочего
		}

	public int WeeksNumber () {
			return Weeks + StartingWeek;
	}

	private bool q4 (int x) {
		if (x % 4 == 0) return true;
		else return false;
	}


	private void ShowMain() {
		ShowForm (FormList.Main_Form, 0); 
	}

	private void ShowCounsil () {
		// там надо нариосвать форму с мордами, и по щелчку на морду вызывать окно перса,
		// например так: Output.PersonInfo (Strana.Government.PrimeMinister.Holder);
		// сейчас морды есть, но не щелкаются
		ShowForm (FormList.Counsil_Form, 0);
	}

	private void ShowPrivate() {
		ShowForm (FormList.Private_Form, 0); 
	}


//     ПЕРЕОПРЕДЕЛЕННЫЕ ФУНКЦИИ
//
// вызывает список персон при дворе, который можно листать и просматривать отдельные морды
	public virtual bool CourtList (int choice) {
			return Output.CourtList (choice);  // хотя можно и без значения обойтись
	}

	// показать список всех людей на должностях
	public virtual bool CourtPostList (int choice) {
			return Output.CourtPostList (choice);
	}

	// показать список должностей
	public virtual bool PostsList (int choice) {
			return Output.PostsList (choice);
	}

	// показать все должности в подчинении определенного поста
	public virtual bool BranchList (int choice, string titul) {
			return Output.BranchList (choice, titul);
	}
	
	// показать отдельного персонажа
	public virtual Person ShowPerson (Person P, int choice, bool value) {
			return Output.ShowPerson (P, choice, value);
	}

	public virtual Person CallShowPerson (int choice, bool value) {
			return Output.CallShowPerson (choice, value);
	}

	public virtual bool ShowForm (FormList form, int choice) {
// запарился, короче, вызываем общую форму
			return Output.ShowForm (form, choice);
	}

	public virtual bool ShowActiveForm (int choice) {
			return Output.ShowActiveForm (choice);
	}

	public virtual int GetActiveForm () {
			return Output.GetActiveForm ();
	}



/***********************************************************************
					С Ю Ж Е Т
***********************************************************************/


	public virtual void StoryRun (int flag, FormList form) {  // вызывается из формы через переопр. ф-ию
		StoryMode mode = (StoryMode)flag;
// всякие проверки, потом вызываем рассказчика сюжета с подборкой эпизодов
		PrepareStory ();

		if (mode == StoryMode.tutor && PrepareTutorials (form))   // прогоняем туториалку
			Storyteller ( mode, () => { ShowForm (form, 0); });
		// это вызывается из формы ShowCounsilForm
		if (mode == StoryMode.counsil) {  // нужно промотать время
			PrepareCounsil ();
//			Storyteller ( mode, () => { WeeklyUpdate (); main (); });
			Storyteller ( mode, () => { WeeklyUpdate (); ShowPrivate (); }); // оттуда попадаем в мейн
			}
		// оттуда же
		else if (mode == StoryMode.stranger || mode == StoryMode.audience) {
			PrepareAudiences ();
			Storyteller ( mode, () => { ShowCounsil (); });
			}
		// это может быть вызвано из ShowMainForm, но не только
		else if (mode == StoryMode.events && PrepareEvents (form)) {  
			Storyteller ( mode, () => { ShowForm (form, 0); }); 
			}
// потом всякие действия по перемотке времени - апдейт страны и т.д.
// чистим и обновляем историю		
	}


// здесь формируем список диалогов для текущей сессии и запускаем цепочку исполнения
	private Action Storyteller (StoryMode mode, Action onComplete) {
			SubStory sub1;	
			Dialogue dial1; 
			List<Dialogue> dialist = new List<Dialogue>();
	// в цикле перебираем сюжетные линии; пока всего одна + отдельные для аудиенций и совета
			for (int i = 0; i < Sujet.count; i++) {
				if ((sub1 = Sujet.Seq_GetOne (i)) != null) {
	Debug.Log ("нашли " +sub1.SubName + " " + sub1.index); 
				// для подсюжета смотрим следующий диалог, повествование линейное, в будущем надо сделать
				// дерево диалогов в подсюжете; диалогам надо добавлять стартовые условия
					if (GetSubStoryCategory(sub1) == mode && (dial1 = sub1.Sub_NextDialogue ()) != null) {
						dialist.Add (dial1);  // добавляем диалог в текущий набор рассказчика
	Debug.Log ("добавили " +sub1.SubName + " " + sub1.index);  // из каждой сюжетки взяли один диалог
						}
					}
				}
			Sujet.Seq_ClearFinished ();
			Output.SetScene (); // пока пустая
		// чтобы не зациклилась, стопарим, если нет диалогов
			if (dialist.Count > 0)
				// запускаем выполнение всех выбранных диалогов
				return DialogueRoutine (dialist, onComplete);  
			else return onComplete;
	}

// рекурсивная ф-ия запуска цепочки диалогов с финишем одного за другим (как стек)
	private Action DialogueRoutine (List<Dialogue> dialist, Action onComplete) {
		Dialogue d;
		if (dialist.Count > 0) {
				if ((d = dialist[0]) != null) {
					dialist.RemoveAt(0);
					DialogueRoutine (dialist, () => { StartDialogue (d, onComplete); });
				}
		}
		else 
			FinishDialogue (onComplete); // первый в цепочке стартует без задержки

		return onComplete;
	}

// все, что касается показа диалога, кроме этой ф-ии, передать в дисплей
	public virtual void StartDialogue (Dialogue d, Action onComplete) {
			Output.StartDialogue (d, onComplete);
	}

	public virtual void Talk (Person Pers, string text) {
			Output.Talk (Pers, text);
	}

	public virtual void FinishDialogue (Action onComplete) {
			Output.FinishDialogue (onComplete);
	}

	private StoryMode GetSubStoryCategory (SubStory sub) {
			return (StoryMode)sub.category;
	}

	public bool GetSubStoryFlag () {
		return SubStoryIsReady;
	}


	// здесь создаем каркас сюжета, закладываем новые сторилайны с ветками диалогов и проч.
	// возможно, стоит вынести в отдельный класс
	// диалоги имеет смысл хранить в файлах и динамически считывать, чтобы экономить память
	private void PrepareStory () {
		if (StoryFirstRun) {
			// инициализируем сюжетку, здесь надо считать и записать в подсюжеты все предопределенные квесты, 
			// например MainQuest
			PrepareMainQuest ();
			StoryFirstRun = false;
			}
		else {
	//		TestStory ();  // пока в стадии теста
	//		PrepareAudiences ();  
	//		PrepareCounsil ();
		}
	}


	private void TestStory () {
	// пока тестовый разговор 1
			Dialogue d = new Dialogue ();	
		
			Person MainHero = Strana.Dvor.GetPerson (0);
			Person Boltun = Strana.Dvor.GetPerson (33); // случайно
			string[] s1 = {
				"Кто здесь?",
				"",
				"Ау!",
				"Это я, Болтун!",
				"Болтун, как я рад тебя видеть!",
				"А уж я-то!",
				};
			Dialogue[] d1 = { d.Prep1SDialog (MainHero, Boltun, s1, null) };
			Sujet.PrepSubStory ("test1", (int)StoryMode.audience, d1, true);

	// пока тестовый разговор 2
			MainHero = Strana.Dvor.GetPerson (0);
			Boltun = Strana.Dvor.GetPerson (35); // случайно
			string[] s2 = {
				"Мы ехали, ехали, и наконец приехали!",
				"Ура товарищу рекурсиву!",
				"Гип-гип-ура!"
				};
			Dialogue[] d2 = { d.Prep1SDialog (MainHero, Boltun, s2, null) };
			Sujet.PrepSubStory ("test2", (int)StoryMode.audience, d2, true);

	// пока тестовый разговор 3
			MainHero = Strana.Dvor.GetPerson (0);
			Boltun = Strana.Dvor.GetPerson (36); // случайно
			string[] s3 = {
				"А вот и еще один разговорчик!",
				"Смело товарищи в ногу!",
				"Трам-пам-пам!"
				};
			Dialogue[] d3 = { d.Prep1SDialog (MainHero, Boltun, s3, null) };
			Sujet.PrepSubStory ("test3", (int)StoryMode.audience, d3, true);

	}


// каким образом мы создаем историю?
// сначала формируем список реплик на основе строковых массивов из TextArray (переменная Massive)
// их мы будем считывать из файла, в зависимости от версии языка... возможно (уже сделал :-))
// потом формируем матрицы переходов от реплики к реплике - "маршрут" диалога, а также 2 списка -
// номера вопросников и номера концовок
// с помощью всего этого инициализируем диалог, или последовательность диалогов
// потом отдельно вписываем в реплики связанные концовки (изменение отношений, арест, повышение и т.д.)
// таким же образом можно прописать условия выбора реплики, если оно отличается от стандартного 
// сформировав таким образом цепочку диалогов, добавляем сабстори, из которых складывается массив сюжета

	private void PrepareAudiences () {
		Person Guest;
		for (int i=0; i < Strana.Dvor.CourtSize; i++) {
			Guest = Strana.Dvor.GetPerson (i);
			if (Guest.isInvited) 
				PrepareAudience (Guest);
			}
	}

	private void PrepareAudience (Person Guest) {	
		Dialogue Audience1 = new Dialogue ();
		DialogueInfo dInfo = new DialogueInfo ();
	// считываем диалог из соответствующего файла
		List<string[]> list = Massive.GetDialogStringsList (Dialog_Names.Audience); 
	// готовим матриwу переходов внутри диалога
	// сделал первым, потом стал забивать эту инфу прямо в диалог и считывать уже по-другому, см. counsil
		int[][] links = { new int[] {1,1,2}, new int[] {1,2,3}, new int[] {1,3,4}, new int[] {1,4,4}, 
						  new int[] {1,5,33}, new int[] {1,6,32}, new int[] {1,7,7}, new int[] {1,8,8},
 						  new int[] {9,1,47}, new int[] {9,2,34}, new int[] {9,3,35}, new int[] {9,4,36},
 						  new int[] {13,0,14}, new int[] {9,5,38}, new int[] {9,6,39}, new int[] {9,7,40}, 
 						  new int[] {9,8,27}, new int[] {18,1,41}, new int[] {18,2,42}, new int[] {18,3,43},
 						  new int[] {18,4,44}, new int[] {18,5,45}, new int[] {18,6,46}, new int[] {18,7,27},
 						  new int[] {22,1,23}, new int[] {22,2,23}, new int[] {22,3,27}, new int[] {24,1,48},
						  new int[] {24,2,49}, new int[] {24,3,50}, new int[] {24,5,51}, new int[] {24,6,27},
						  new int[] {27,1,1}, new int[] {27,2,28}, new int[] {2,0,9}, new int[] {3,0,18}, 
						  new int[] {4,0,27}, new int[] {5,0,27}, new int[] {6,0,27}, new int[] {7,0,24}, 
						  new int[] {10,0,9}, new int[] {11,0,9}, new int[] {12,0,9}, new int[] {14,0,9}, 
						  new int[] {15,0,9}, new int[] {16,0,9}, new int[] {17,0,9}, new int[] {19,0,27}, 
						  new int[] {20,0,27}, new int[] {23,0,27}, new int[] {25,0,24}, new int[] {26,0,24},
						  new int[] {21,1,52}, new int[] {21,2,53}, new int[] {21,3,54}, new int[] {21,4,55}, 
						  new int[] {21,5,31}, new int[] {29,0,24}, new int[] {30,0,24}, new int[] {31,0,27}, 
						  new int[] {32,0,21}, new int[] {24,4,56}
 						};
		int[] choices = new int[] { 1, 9, 18, 21, 22, 24, 27 }, 
			decisions = new int[] { 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56 }, 
			   finals = new int[] { 8, 28 };
		
//		Audience1.PrepNSDialog (Strana.Ruler, Guest, list, () => { DefaultReplikaChoice (Guest); }, links, choices, decisions, finals);
		dInfo.list = list; dInfo.links = links; dInfo.choices = choices; dInfo.decisions = decisions; dInfo.finals = finals;
		Audience1.PrepNSDialog (Strana.Ruler, Guest, dInfo, () => { DefaultReplikaChoice (Guest); });

		SetAct (Audience1, 8, null, () => { Debug.Log("Эtо конееееец!!!"); } );
		SetAct (Audience1, 28, null, () => { Debug.Log("Эtо конееееец!!!"); } );
		{
		SetAct (Audience1, 33, () => { ReplikaChoiceNeed (Guest, 5, 6); }, null );
		SetAct (Audience1, 34, () => { ReplikaChoiceNeutralMood (Guest, 11, 10); }, null );
		SetAct (Audience1, 35, () => { ReplikaChoiceNeutralMood (Guest, 12, 10); }, null );
		SetAct (Audience1, 36, () => { ReplikaChoiceNeutralMood (Guest, 13, 10); }, null );
		SetAct (Audience1, 37, () => { ReplikaChoiceNeutralMood (Guest, 14, 10); }, null );
		// намеренно поставил для вопроса об отношениях нет = да, чтобы потом сработал сам вопрос
		SetAct (Audience1, 38, () => { ReplikaChoiceNeutralMood (Guest, 15, 15); }, null );
		SetAct (Audience1, 39, () => { ReplikaChoiceNeutralMood (Guest, 16, 10); }, null );
		SetAct (Audience1, 40, () => { ReplikaChoiceNeutralMood (Guest, 17, 10); }, null );
		SetAct (Audience1, 41, () => { ReplikaChoiceNeutralMood (Guest, 22, 20); }, () => { MadeOffer (Guest, OfferRewards.drink); } );
		SetAct (Audience1, 42, () => { ReplikaChoiceNeutralMood (Guest, 19, 20); }, () => { MadeOffer (Guest, OfferRewards.honor); } );
		SetAct (Audience1, 43, () => { ReplikaChoiceNeutralMood (Guest, 19, 20); }, () => { MadeOffer (Guest, OfferRewards.gold); } );
		SetAct (Audience1, 44, () => { ReplikaChoiceNeutralMood (Guest, 19, 20); }, () => { MadeOffer (Guest, OfferRewards.holding); } );
		SetAct (Audience1, 45, () => { ReplikaChoiceNeutralMood (Guest, 19, 20); }, () => { MadeOffer (Guest, OfferRewards.titul); });
		SetAct (Audience1, 46, () => { ReplikaChoiceNeutralMood (Guest, 19, 20); }, null );
		// для вопроса о происхождении пока нет ответа, так что да = нет
		SetAct (Audience1, 47, () => { ReplikaChoiceGoodMood (Guest, 10, 10); }, null );
		SetAct (Audience1, 48, () => { ReplikaChoiceNeutralMood (Guest, 25, 26); }, () => { LaySanctions (Guest, Sanctions.ponos); } );
		SetAct (Audience1, 49, () => { ReplikaChoiceNeutralMood (Guest, 25, 26); }, () => { LaySanctions (Guest, Sanctions.gold); } );
		SetAct (Audience1, 50, () => { ReplikaChoiceNeutralMood (Guest, 25, 26); }, () => { LaySanctions (Guest, Sanctions.holding); } );
		SetAct (Audience1, 51, () => { ReplikaChoiceNeutralMood (Guest, 25, 26); }, () => { LaySanctions (Guest, Sanctions.arrest); } );
		SetAct (Audience1, 56, () => { ReplikaChoiceNeutralMood (Guest, 25, 26); }, () => { LaySanctions (Guest, Sanctions.titul); } );
		SetAct (Audience1, 52, () => { ReplikaChoiceGoodMood (Guest, 29, 30); }, null );
		SetAct (Audience1, 53, () => { ReplikaChoiceGoodMood (Guest, 29, 30); }, null );
		SetAct (Audience1, 54, () => { ReplikaChoiceGoodMood (Guest, 29, 30); }, null );
		SetAct (Audience1, 55, () => { ReplikaChoiceGoodMood (Guest, 29, 30); }, null );

		SetAct (Audience1, 11, () => { ReplikaChoiceReligion (Guest); }, null );
		SetAct (Audience1, 12, () => { ReplikaChoicePolitic (Guest); }, null );
		SetAct (Audience1, 13, () => { ReplikaChoiceDost (Guest); }, () => { ReplikaRevealStat (Guest); } );
		SetAct (Audience1, 14, () => { ReplikaChoiceNedost (Guest); }, () => { ReplikaRevealStat (Guest); } );
		SetAct (Audience1, 15, () => { ReplikaChoiceRelation (Guest); }, null );
		SetAct (Audience1, 16, () => { ReplikaChoicePosition (Guest); }, null );
		SetAct (Audience1, 17, () => { ReplikaChoiceCounsil (Guest); }, null );
		SetAct (Audience1, 19, null, () => { AcceptOffer (Guest); } );
		SetAct (Audience1, 20, null, () => { RefuseOffer (Guest); } );
		SetAct (Audience1, 25, null, () => { AcceptSanction (Guest); } );
		SetAct (Audience1, 26, null, () => { RefuseSanction (Guest); } );
		}

		Dialogue[] Audience = { Audience1 };
		Sujet.PrepSubStory ("Audience of " + Guest.Name, (int)StoryMode.audience, Audience, true);
	}


	private void PrepareCounsil () {
		Dialogue Counsil1 = new Dialogue ();	
		var Pers = Strana.Government.PrimeMinister.GetHolder ();

		DialogueInfo dInfo = Massive.GetDialogueInfo (Dialog_Names.Counsil);
//			for (int i = 0; i < d.links.Length; i++)
//				Debug.Log(d.links[i][0] + " " + d.links[i][1] + " " + d.links[i][2]);
		Counsil1.PrepNSDialog (Strana.Ruler, Pers, dInfo, () => { DefaultReplikaChoice (Pers); });

		SetAct (Counsil1, 8, null, () => { Debug.Log("Эtо конееееец!!!"); } );
		SetAct (Counsil1, 14, () => { ReplikaChoiceState (3, 4, 5); }, null );

		Dialogue[] Counsil = { Counsil1 };	
		Sujet.PrepSubStory ("Counsil" + WeeksNumber (), (int)StoryMode.counsil, Counsil, true);
	}

	// делаем как бы главный квест, заряжаем два диалога и стартовое условие
	private void PrepareMainQuest () {
		Dialogue quest1 = new Dialogue (), quest2 = new Dialogue (), quest3 = new Dialogue ();	
		var Pers1 = Strana.Government.PrimeMinister.GetHolder ();
		var Pers2 = Strana.Government.Voevoda.GetHolder ();
		var Pers3 = Strana.Government.Spymaster.GetHolder ();

		DialogueInfo dInfo = Massive.GetDialogueInfo (Dialog_Names.PMQuest1);
		quest1.PrepNSDialog (Strana.Ruler, Pers1, dInfo, () => { DefaultReplikaChoice (Pers1); });

		dInfo = Massive.GetDialogueInfo (Dialog_Names.PMQuest2);
		quest2.PrepNSDialog (Strana.Ruler, Pers2, dInfo, () => { DefaultReplikaChoice (Pers2); });
		dInfo = Massive.GetDialogueInfo (Dialog_Names.PMQuest3);
		quest3.PrepNSDialog (Strana.Ruler, Pers3, dInfo, () => { DefaultReplikaChoice (Pers3); });

		Dialogue[] PMQuest = { quest1, quest2, quest3 };	
		SubStory sub = Sujet.PrepSubStory ("PMQuest", (int)StoryMode.events, PMQuest, false);
		sub.StageCondition[0] = SetCond ( () => { SubStoryIsReady = (WeeksNumber () == StartingWeek + 1) ? true: false; });
		sub.StageCondition[1] = SetCond ( () => { SubStoryIsReady = (WeeksNumber () == StartingWeek + 2) ? true: false; });
		sub.StageCondition[2] = SetCond ( () => { SubStoryIsReady = (WeeksNumber () == StartingWeek + 3) ? true: false; });

	}


	private bool PrepareTutorials (FormList form) {
		bool f = true;
		var Ment = Strana.Government.Mentor.GetHolder ();

		if (form == FormList.Main_Form) {
			f = SVar.sv[(int)StoryVar.Tutor0Showed];			
			if (!f && Ment != null) {
				PrepareTutorial (0, Ment);
				SVar.sv[(int)StoryVar.Tutor0Showed] = true;
				}
			}
		if (form == FormList.Counsil_Form) {
			f = SVar.sv[(int)StoryVar.Tutor1Showed];			
			if (!f && Ment != null) {
				PrepareTutorial (1, Ment);
				SVar.sv[(int)StoryVar.Tutor1Showed] = true;
				}
			}

		if (form == FormList.Trade_Form) {
			f = SVar.sv[(int)StoryVar.Tutor2Showed];			
			var Kazn = Strana.Government.Kaznachei.GetHolder ();
	
			if (!f && Kazn != null) {
				PrepareTutorial (2, Kazn);
				SVar.sv[(int)StoryVar.Tutor2Showed] = true;
				}
			}
		if (form == FormList.Army_Form) {
			f = SVar.sv[(int)StoryVar.Tutor3Showed];			
			var Voed = Strana.Government.Voevoda.GetHolder ();

			if (!f && Voed != null) {
				PrepareTutorial (3, Voed);
				SVar.sv[(int)StoryVar.Tutor3Showed] = true;
				}
			}
		if (form == FormList.Spy_Form) {
			f = SVar.sv[(int)StoryVar.Tutor4Showed];	
			var Spy = Strana.Government.Spymaster.GetHolder ();
		
			if (!f && Spy != null) {
				PrepareTutorial (4, Spy);
				SVar.sv[(int)StoryVar.Tutor4Showed] = true;
				}
			}
		return !f;
	}

	private void PrepareTutorial (int x, Person Pers) {
		Dialogue Tutor1 = new Dialogue ();	
	//	var fname = (x==1) ? Dialog_Names.Tutorial1 : Dialog_Names.Tutorial0;
		var fname = Dialog_Names.Tutorial0 + x;
		
	//	string[] str = Massive.GetDialogStrings (fname);
	//	Tutor1.PrepMonolog (Strana.Ruler, Pers, str, null);
		DialogueInfo dInfo = Massive.GetDialogueInfo (fname);
		Tutor1.PrepNSDialog (Strana.Ruler, Pers, dInfo, () => { DefaultReplikaChoice (Pers); });

		Dialogue[] Tutor = { Tutor1 };	
		Sujet.PrepSubStory ("Tutor" + x.ToString (), (int)StoryMode.tutor, Tutor, true);
	}

	private bool PrepareEvents (FormList form) {
			bool f = true;
			return f;  // пока вообще не нужно
	}


	private void SetAct (Dialogue d, int pN, Action a1, Action a2) {
		d.D_GetPhrase (pN).Ph_InitAct (a1, a2);
	}

	private Action SetCond (Action cond) {
		return cond;
	}


	private void MadeOffer (Person Pers, OfferRewards reward) {
		Offer.flag = OfferFlag.Offer_Made;
		Offer.Pers = Pers;
		Offer.reward = reward;
	}

	private void LaySanctions (Person Pers, Sanctions shtraf) {
		Sanction.flag = OfferFlag.Offer_Made;
		Sanction.Pers = Pers;
		Sanction.shtraf = shtraf;
	}

	private void AcceptOffer (Person Pers) {
		if (Offer.flag == OfferFlag.Offer_Made && Offer.Pers == Pers && !Pers.isRewarded) {
				Offer.flag = OfferFlag.Offer_Accepted;
				if (Offer.reward == OfferRewards.titul) 
					Pers.RelationPositive += 15;
				if (Offer.reward == OfferRewards.honor) 
					Pers.RelationPositive += 10;
				if (Offer.reward == OfferRewards.holding && Strana.Ruler.GrantHoldings (Pers)) {
					Pers.RelationPositive += 30;
					}
				if (Offer.reward == OfferRewards.gold && Strana.Treasure >= 20) {
					Pers.RelationPositive += 10;
					Pers.PersonalWealth += 20;
					Strana.Treasure -= 20;
					}
				if (Offer.reward == OfferRewards.drink) 
					Pers.RelationPositive += 5;
				Pers.isRewarded = true;
				}
	}

	private void AcceptSanction (Person Pers) {
		if (Sanction.flag == OfferFlag.Offer_Made && Sanction.Pers == Pers) {
				Sanction.flag = OfferFlag.Offer_Accepted;
				if (Sanction.shtraf == Sanctions.titul) 
					Pers.RelationNegative += -15;
				if (Sanction.shtraf == Sanctions.ponos) 
					Pers.RelationNegative += -10;
				if (Sanction.shtraf == Sanctions.holding && Pers.GrantHoldings (Strana.Ruler)) {
					Pers.RelationNegative += -30;
					}
				if (Sanction.shtraf == Sanctions.gold && Pers.PersonalWealth >= 20) {
					Pers.RelationNegative += -10;
					Pers.PersonalWealth += -20;
					Strana.Treasure += 20;
					}
				if (Sanction.shtraf == Sanctions.arrest) {
					Pers.RelationNegative += -10;
					Pers.isPrisoner = true;
					}
				}
	}


	private void RefuseOffer (Person Pers) {
		if (Offer.flag == OfferFlag.Offer_Made && Offer.Pers == Pers && !Pers.isRewarded) {
			Offer.flag = OfferFlag.Offer_Refused;
		//	Pers.RelationNegative += -5;
		}
	}

	private void RefuseSanction (Person Pers) {
		if (Sanction.flag == OfferFlag.Offer_Made && Sanction.Pers == Pers) {
			Sanction.flag = OfferFlag.Offer_Refused;
		//	Pers.RelationNegative += -5;
		}
	}



/******************************************************************************
						У С Л О В И Я   В Ы Б О Р А   Р Е П Л И К
*******************************************************************************/

	public int GetReplikaFlag () {
		return ReplikaChoiceFlag;
	}

	// алгоритм выбора реплики в зав-ти от характера и настроения субъекта
	// передается как условие в класс phrase, нужен флаг ReplikaChoiceFlag
	public void DefaultReplikaChoice (Person Pers) {
			var pr = Pers.GetRelation ();
			var pk = Pers.Kharakter;
			int flag = 0;
			if (pk == Kharakter.Gentle) {
				if (pr > 0)	flag = 0;
				else 		flag = 1;
				}
			if (pk == Kharakter.Average) {
				if (pr > 50)		flag = 0;
				else if (pr > -50)	flag = 1;
				else 				flag = 2;
				}
			if (pk == Kharakter.Rude) {
				if (pr > 0)	flag = 1;
				else 		flag = 2;
				}
			ReplikaChoiceFlag = flag;
	}

	// алгоритм выбора выриантов ответа в зависимости от состояния дел в стране (для диалога в совете)
	public void ReplikaChoiceState (int x, int y, int z) {
			int flag = 0;
			var tres = Strana.Treasure;
			var publ = Strana.Capital.Disorder;
			var army = Strana.Voisko.GetArmySize ();
			int coef1 = 0, coef2 = 0, coef3 = 0;

			if (tres > 1000) 		coef1 = 1;
			else if (tres < 0) 		coef1 = -1;
			if (publ < 0.1f) 		coef2 = 1;
			else if (publ > 0.5f) 	coef2 = -1;
			if (army > 500) 		coef3 = 1;
			else if (army < 100) 	coef3 = -1;
			
			float median = (coef1 + coef2 + coef3) / 3f;
			if (median > 0.5f) 			flag = x;
			else if (median < -0.5f)  	flag = z;
			else 						flag = y;
			ReplikaChoiceFlag = flag;
Debug.Log (tres + " " + army + " " + publ + " = " + median + " ответ " + flag);
	}

	public void ReplikaChoiceNeed (Person Pers, int x, int y) {
			ReplikaChoiceFlag = Pers.isInTrouble ? x : y;
	}

	public void ReplikaChoiceGoodMood (Person Pers, int x, int y) {
			ReplikaChoiceFlag = Pers.GetRelation () >= 0 ? x : y;
	}

	public void ReplikaChoiceNeutralMood (Person Pers, int x, int y) {
			ReplikaChoiceFlag = Pers.GetRelation () >= -50 ? x : y;
	}

	public void ReplikaChoiceReligion (Person Pers) {
			ReplikaChoiceFlag = (int)Pers.Religion;
	}

	public void ReplikaChoicePolitic (Person Pers) {
			ReplikaChoiceFlag = (int)Pers.PoliticalView;
	}

	public void ReplikaChoiceDost (Person Pers) {
			int[] stat = Pers.Stats.GetStat ();
			int max = 0, maxi = 0; 
			for (int i = 0; i < stat.Length; i++) {
				if (stat[i] > max) {
					max = stat[i];
					maxi = i;
					}
				}
			ReplikaChoiceFlag = maxi;
	}

	public void ReplikaRevealStat (Person Pers) {
			int i = ReplikaChoiceFlag;
			Pers.Stats.isRevealed[i] = true;
	}

	public void ReplikaChoiceNedost (Person Pers) {
			int[] stat = Pers.Stats.GetStat ();
			int min = 10, mini = 0; 
			for (int i = 0; i < stat.Length; i++) {
				if (stat[i] < min) {
					min = stat[i];
					mini = i;
					}
				}
			ReplikaChoiceFlag = mini;
	}

	public void ReplikaChoiceRelation (Person Pers) {
			int flag = 0, relation = Pers.GetRelation ();
			if (relation > 50) flag = 0;
			else if (relation >= 0) flag = 1;
			else {
				if (Pers.Stats.CheckStat (PersonParameter.Intrigue) >= 7) flag = 4;
				else if (Pers.Stats.CheckStat (PersonParameter.Diplomacy) >= 7) flag = 5;
				else if (relation > -50) flag = 2;
				else flag = 3;
				}
			if (flag < 4) 
				Pers.RelationView = true;
			ReplikaChoiceFlag = flag;
	}

	public void ReplikaChoicePosition (Person Pers) {
			ReplikaChoiceFlag = Pers.GetAmbition ();
	}

	public void ReplikaChoiceCounsil (Person Pers) {
			if (Pers.isCounsillor) ReplikaChoiceFlag = 3;
			else ReplikaChoiceFlag = Pers.GetAmbition ();
	}



}
}
