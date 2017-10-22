using UnityEngine;

namespace JagaJaga
{

public static class Data {

	public static int GameDate = 1000;

	public static string MainHeroName = "Евгений";
	public static string MentorName = "Василий";
	public static int MainHeroFace = 0;
	public static int MentorFace = 1;
	public static int NoNameFace = 2;

	public static bool PrimeMinisterCannotBeFired = true;
	public static bool MentorCannotBeFired = true;

	private static Story MainStory = GameObject.Find("Story").GetComponent<Story>(); 
	public static string[] PersonParamString = MainStory.Massive.GetPersonFromFile ();

}
}
