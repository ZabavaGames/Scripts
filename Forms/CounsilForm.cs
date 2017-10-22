using System.Collections;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

namespace JagaJaga {

public class CounsilForm : Display {

	private	enum StateForm { tutor, audience, stranger, counsil, events, init, clear }

	private Display Parent;
	private Text ConsText;
	private ScrollRect CounsilFaceView;
	private HorizontalLayoutGroup CounsilContent;
	public Button FaceButton, prefabB, StartCons;
	private StateForm StateFlag;
	private string[] CounsilStrings;
	private bool CounsilFirstRun;

	void Start () {
		Parent = GameObject.Find ("Output").GetComponent<Display>();		
		CounsilPanel = GameObject.Find ("CounsilPanel").GetComponent<CanvasGroup>();
		ConsText = GameObject.Find ("ConsText").GetComponent<Text>();
		CounsilFaceView  = GameObject.Find ("CounsilFaceView").GetComponent<ScrollRect>();
		CounsilContent = GameObject.Find ("CounsilContent").GetComponent<HorizontalLayoutGroup>();
//		FaceButton  = GameObject.Find ("FaceButton").GetComponent<Button>();
		StartCons = GameObject.Find ("StartCons").GetComponent<Button>();
		CounsilFirstRun = false;
		CounsilStrings = new string[] {
			"Здесь вы проводите заседания совета, решаете государственные дела и даете аудиенции. В данный момент ожидают приема: ",
			" человек. ",
			"Вас ожидают: ",
			"никого. ",
			"Нажмите 'Далее' для того, чтобы начать беседу. ",
			"(После заседания совета вы сможете вернуться к аудиенциям только через неделю!) ",
			""};

	}


	public bool ShowCounsilForm (int choice) {
		bool value = true;
			
		switch (choice) {
			case 0: {  // отрисовка
					Parent.DisplayClear ();
					ShowCounsilPanel (true);
					FillCounsilForm (StateForm.init);
					if (!CounsilFirstRun) {
						CounsilFirstRun = true;
						Parent.StoryRun ((int)StateForm.tutor, FormList.Counsil_Form);
						}
					else
						Parent.StoryRun ((int)StoryMode.events, FormList.Counsil_Form);
					}
					break; 
			case 3: {	// начинаем
					FillCounsilForm (StateForm.clear);
					Parent.StoryRun ((int)StateFlag, FormList.Counsil_Form);
					}
					break;
			case 4: {	// вызов аудиенций
					FillCounsilForm (StateForm.audience);
					}
					break;
			case 5: {	// вызов совета
					FillCounsilForm (StateForm.counsil);
					}
					break;
			case 6: {	// вызов послов
					FillCounsilForm (StateForm.stranger);
					}
					break;
			case 10: {
					FillCounsilForm (StateForm.clear);
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


	private void FillCounsilForm (StateForm state) {
		Button Knop;		Image Img; 		
		Sprite 	Sprite1;	Person Pers;			
		// это просто названия кнопок, переводить не надо
//		string[] objectname = {""};
		ArrayList PList;
		string PersList = "", comma = "; ";
		int PCount;		bool activate;

		if (state == StateForm.init) {
			CounsilFaceView.gameObject.SetActive (false);
			StartCons.gameObject.SetActive (false);
			//PList = FormPersonList (state);
			PCount = GetWaitingNumber ();
			TextvOkno (ConsText, CounsilStrings[0] + PCount + CounsilStrings[1]);
			}
		else if (state == StateForm.clear) {
			FindAndDestroyAllFaceButtons ();
			}
		else {
			StateFlag = state;
			CounsilFaceView.gameObject.SetActive (true);
			PList = FormPersonList (state);
			FindAndDestroyAllFaceButtons ();

			for (int i = 0; i < PList.Count; i++) {
				// в цикле создаем кнопки, прикрепляем к ним лица и пихаем в полосу
					Knop = Instantiate (prefabB);
					Knop.gameObject.name = "FaceButton" + i;
					Img = Knop.gameObject.GetComponent<Image>();

					if ((Pers = (Person)PList[i]) != null && (Sprite1 = Parent.GetSprite (Pers)) != null) {
						Img.sprite = Sprite1; 
						Img.transform.localScale = new Vector3(.5f, .5f, .5f);  // resize
						// вставляем в полосу скролвью, объекты сами подгоняются под нее
						Knop.gameObject.transform.SetParent (CounsilContent.gameObject.transform);
						}
					if (i >= PList.Count - 1) comma = ". ";
					PersList += Pers.Name + comma;
				}

			if (PList.Count == 0) {
				PersList = CounsilStrings[3];
				activate = false;
				} 
			else {
				PersList += CounsilStrings[4];
				activate = true;
			}
			
			if (state == StateForm.counsil) 
				PersList += CounsilStrings[5];
			TextvOkno (ConsText, CounsilStrings[2] + PersList);
			StartCons.gameObject.SetActive (activate);
				
		}
	}

	private ArrayList FormPersonList (StateForm state) {
		ArrayList People = new ArrayList ();
		Person Pers; 	Posts Post;
		var Dvor = Strana.Dvor; 
		var Gov = Strana.Government;

		if (state == StateForm.counsil) {
	// FormListForCounsil 
				for (int i = 0; i < Gov.GetSize (); i++) {
					Post = Gov.GetPost (i);
					if (Post.isCounsillor && Post.GetHolder () != null) 
						People.Add (Post.GetHolder ());
				}
		}
		else if (state == StateForm.audience) {
	// FormListForAudience 
				for (int i = 0; i < Dvor.CourtSize; i++) {
					Pers = Dvor.GetPerson (i);
					if (Pers.isInvited) 
						People.Add (Pers);
				}
		}
		else if (state == StateForm.stranger) {
	// FormListForStrangers
		// ...
		}
		return People;
	}

	private int GetWaitingNumber () {
		var Dvor = Strana.Dvor; 
		int number = 0;

		for (int i = 0; i < Dvor.CourtSize; i++)
			if (Dvor.GetPerson (i).isInvited) 
				number++;
// а также посчитать strangers
		return number;
	}


	private void FindAndDestroyAllFaceButtons () {
		GameObject[] fb;
		if ((fb = GameObject.FindGameObjectsWithTag ("FaceButton")) != null) 
				for (int i = 0; i < fb.Length; i ++) 
					Destroy (fb[i]);
		}


}
}
