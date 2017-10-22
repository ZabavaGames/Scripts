using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System.Text.RegularExpressions;


namespace JagaJaga {

public enum Dialog_Names { Audience, Counsil, Tutorial0, Tutorial1, Tutorial2, Tutorial3, Tutorial4, 
	PMQuest1, PMQuest2, PMQuest3 }

public class TextArray {

	public string[] DialogsFilenames = {"audience.txt", "counsil.txt", "tutor0.txt", "tutor1.txt", "tutor2.txt",
		 "tutor3.txt", "tutor4.txt", "pmquest1.txt", "pmquest2.txt", "pmquest3.txt"};
	public string PostsFilename = "postsrus.txt";
	public string[] NamesFilenames = {"rusnmm.txt", "rusnmw.txt", "rusfam2.txt"};
	public string PersonFilename = "person.txt";

	// Use this for initialization
	void Start () {
		
	}
	
	// вначале использовал загрузку из файлов, но они непойми где хранятся 
	private string[] GetStringsFromFile (string filename) {
		Encoding Cyrillic = Encoding.GetEncoding(1251); 
		return File.ReadAllLines(Application.dataPath + "/Strings/" + filename, Cyrillic);
	}

	// теперь использую textasset, нюанс в том, что для русского текста должен быть юникод 8
	public string GetStringFromAssets (string filename) {
//		string s = File.ReadAllText (filename);
		TextAsset t = Resources.Load ("Strings/" + SplitFilename (filename)) as TextAsset;
		return t.text;
	}

	public string[] GetStringsFromAssets (string filename) {
//		TextAsset t = Resources.Load (SplitFilename (filename), typeof (TextAsset)) as TextAsset;
		string s = GetStringFromAssets (filename);
		string[] s1 = s.Split (new char[] { '\n' });
		return s1;
	}

	private string SplitFilename (string filename) {
		string s = filename.Split (new char[] {'.'} ) [0];
Debug.Log (s);
		return s;
	}

	public string[] GetPostsFromFile () {
		return GetStringsFromAssets (PostsFilename);
	}

	public string[] GetPersonFromFile () {
		return GetStringsFromAssets (PersonFilename);
	}

	public string[] GetRussianNamesFromFile () {
		return GetStringsFromAssets (NamesFilenames[0]);
	}

	public string[] GetRussianWomenNamesFromFile () {
		return GetStringsFromAssets (NamesFilenames[1]);
	}

	public string[] GetRussianFamiliesFromFile () {
		return GetStringsFromAssets (NamesFilenames[2]);
	}

	private string[] GetDialogStringsFromFile (string filename) {
		return GetStringsFromAssets ("dialogs/" + filename);
	}

	public List<string[]> GetDialogStringsList (Dialog_Names name) {
		string[] textarray = GetDialogStringsFromFile (DialogsFilenames[(int)name]);
		List<string[]> list = ExtractStringsToList (textarray);
		return list;
	}

	public string[] GetDialogStrings (Dialog_Names name) {
		string[] textarray = GetDialogStringsFromFile (DialogsFilenames[(int)name]);
		return ExtractStringsToString (textarray);
	}

	private List<string[]> ExtractStringsToList (string[] textarray) {
		List<string[]> list = new List<string[]> ();
		List<string> replika = new List<string> ();
		string s;  bool end = false;

		for (int i=0; i < textarray.Length; i++) {
			end = false;
			s = textarray[i].Trim ();
			if (!String.IsNullOrEmpty (s) && !isDigit (s[0]))
				replika.Add (CutStr (s));
			else end = true;
			if (i == textarray.Length - 1) end = true;
			if (end)
				if (replika.Count > 0) {
					list.Add (replika.ToArray ());
					replika.Clear ();
					}
			}
		return list;
	}

	private string[] ExtractStringsToString (string[] textarray) {
		List<string> replika = new List<string> ();
		string s;

		for (int i=0; i < textarray.Length; i++) {
			s = textarray[i].Trim ();
			if (!String.IsNullOrEmpty (s) && !isDigit (s[0]))
				replika.Add (CutStr (s));
			}
		return replika.ToArray ();
	}

	private bool isDigit (char c) {
			return char.IsDigit (c);
	/*		if ( c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
					return true;
        	else 	return false;
	*/
	}

	private bool isFinal (char c) {
			if (c == 'F' || c == 'f' || c == 'А' || c == 'Ф' || c == 'К' || c == 'K')
					return true;
        	else 	return false;
	}

	private bool isDecision (char c) {
			if (c == 'D' || c == 'd' || c == 'В' || c == 'Д' || c == 'Р')
					return true;
        	else 	return false;
	}

	private bool isChoice (char c) {
			if (c == 'C' || c == 'c' || c == 'С' || c == 'с' || c == 'Ч' || c == 'Ц')
					return true;
        	else 	return false;
	}

	private string CutStr (string s) {
		string s1;
		int i = 0;
		if (s[i] == '(') {
				do i++; while (s[i] != ')');
				s1 = s.Substring (i+1);
	//			for (int j=i; j < s.Length; j++)
	//				s1[j-i]=s[j];
				return s1;
			}
		else 	return s;
	}


	public DialogueInfo GetDialogueInfo (Dialog_Names name) {

		string[] textarray = GetDialogStringsFromFile (DialogsFilenames[(int)name]);
		List<string[]> list = ExtractStringsToList (textarray); 
		List<int> choices = new List<int>(), decisions = new List<int>(), finals = new List<int>();
		int x;
		string s;
Debug.Log ("здесь чтоли падает?");

		for (int i = 0; i < textarray.Length; i++) {
				s = textarray[i].TrimStart ();
				if (!String.IsNullOrEmpty (s) && isDigit (s[0])) {
					x = GetNumberFromString (s);
Debug.Log(x);
					if (!String.IsNullOrEmpty (s = CutStr (s, x))) {
						if (isFinal(s[0]))
							finals.Add (x);
						if (isDecision(s[0]))
							decisions.Add (x);
						if (isChoice(s[0]))
							choices.Add (x);
						}
					}
				}
//		Debug.Log(finals[0] + " " + choices[0] + " " + choices[1] + " " +decisions[0]);

Debug.Log ("вроде нет");
		int[][] links = GetLinksFromTA (textarray);

		DialogueInfo d = new DialogueInfo (list, links, choices.ToArray (), decisions.ToArray (), finals.ToArray ());
		return d;
	}

  
	private int[][] GetLinksFromTA  (string[] textarray) {
			List<int[]> links = new List<int[]>();
			int[] raw = { 0, 0, 0 };
			int x1, x2, x3;
			string s;

			for (int i = 0; i < textarray.Length; i++) {
				s = textarray[i].TrimStart ();
				x1 = x2 = x3 = 0;
				if (!String.IsNullOrEmpty (s) && s.Length > 3) {
					if (isDigit (s[0])) {
						x1 = GetNumberFromString (s);
						s = CutStr (s, x1);
						x2 = 0;
						if (!String.IsNullOrEmpty (s) && s[0] == '-' && s[1] == '>')
							x3 = GetNumberFromString (s);
						}
					else if (s[0] == '(') {
						x1 = GetNumberFromString (s);
						s = CutStr (s.Substring (1), x1);
						if (s[0] == '.') {
							x2 = GetNumberFromString (s);
							s = CutStr (s.Substring (1), x2);
							if (s[0] == '-' && s[1] == '>')
								x3 = GetNumberFromString (s);
							}
						}
					if (x3 != 0) {
						raw = new int[] { x1, x2, x3 };
						links.Add (raw);
						}
					}
				}

			return links.ToArray ();
	}

	private int GetNumberFromString (string s) {
			char[] mas = s.ToCharArray();
			string str = "";
			int j = 0;
			for (int i=0; i < mas.Length; i++) 
				if (isDigit(mas[i])) {
					while (i+j < mas.Length && isDigit(mas[i+j])) {
						str += mas[i+j];
						j++;
						}
					break;
					}
			return Convert.ToInt32 (str);
	}

/*	private int GetNumberFromString (string s) {
		int[] x; int i = 0, v = 0;
//		string pattern = @"\d+";
		Regex regex = new Regex(@"^\s*(?<n>\d+)\s+"); 
		Match m = regex.Match(s);
		if (m.Success) {
//Debug.Log ("" + m + " - " + m.Index);
			v = Int32.Parse(m.Groups["n"].Value); 
		//	x[i++] = v;
		//	m = m.NextMatch(); 
			}
		return v;
	}
*/

	private string CutStr (string s, int x) {
			int ind = 0;
			if (x < 10) ind = 1;
			else if (x < 100) ind = 2;
			else if (x < 1000) ind = 3;
			return s.Substring (ind);
	}


}
}
