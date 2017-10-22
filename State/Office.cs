using UnityEngine;

namespace JagaJaga 
{

	public class Office : Bureaucracy {

		public Posts King, PrimeMinister, Voevoda, Kaznachei, Patriarch, Dvoretzky, Okolnichiy, Questor, Mentor;
 		public Posts Guardian, Druzhina, Phantoms, Policemen, Captain, Spymaster, Mystik, Ninja, Sinobi, Orujeinichiy, Dozorny;
		public Posts Tiun, Kluchnik, Stolnik, Postelnichiy, Konuchiy, Lovchiy;
		public Posts Grandmeister, Paramedik, Jurodiviy, Skomoroh, Trademaster, Buildmaster, Miner, Agrarian, Banker, Courier, Tamojnya;
		public Posts Bodyguard, Commander1, Commander2, Commander3, Commander4, Commander5;
		protected Posts[] PostList, SortedPostList;
		protected int PostNumber;
		public string NoFreePlace;		
		public string[] Titles; 
		private Story MainStory;

	void Start () {
			MainStory = GameObject.Find("Story").GetComponent<Story>(); 
			Titles = MainStory.Massive.GetPostsFromFile ();
			NoFreePlace = Titles [Titles.Length-1];
		}
	
		public void Init () {
			PostNumber = 43;
			PostList = new Posts[PostNumber];	
			SortedPostList = new Posts[PostNumber];	
			
			PostList [0] = SortedPostList [0] = King.SetPost (Titles[0], 0, 100, null, null);

			PostList [1] = SortedPostList [1] = Mentor.SetPost (Titles[1], 0, 100, null, King);
			PostList [2] = SortedPostList [2] = PrimeMinister.SetPost (Titles[2], 0, 100, null, King); 
			PostList [3] = SortedPostList [16] = Voevoda.SetPost (Titles[3], 0, 100, null, King);
			PostList [4] = SortedPostList [3] = Kaznachei.SetPost (Titles[4], 0, 100, null, King); 
			PostList [5] = SortedPostList [14] = Patriarch.SetPost (Titles[5], 0, 100, null, King); 
			PostList [6] = SortedPostList [4] = Dvoretzky.SetPost (Titles[7], 0, 100, null, PrimeMinister); 
			PostList [7] = SortedPostList [12] = Tiun.SetPost (Titles[6], 0, 100, null, King); 
			PostList [8] = SortedPostList [5] = Okolnichiy.SetPost (Titles[8], 0, 100, null, PrimeMinister); 
			PostList [9] = SortedPostList [6] = Questor.SetPost (Titles[9], 0, 100, null, PrimeMinister); 
			PostList [10] = SortedPostList [17] = Guardian.SetPost (Titles[10], 0, 100, null, PrimeMinister);
			PostList [11] = SortedPostList [18] = Policemen.SetPost (Titles[11], 0, 100, null, PrimeMinister);
			PostList [12] = SortedPostList [19] = Druzhina.SetPost (Titles[12], 0, 100, null, Voevoda);
			PostList [13] = SortedPostList [20] = Phantoms.SetPost (Titles[13], 0, 100, null, Voevoda); 
			PostList [14] = SortedPostList [21] = Captain.SetPost (Titles[14], 0, 100, null, Voevoda); 
			PostList [15] = SortedPostList [22] = Spymaster.SetPost (Titles[15], 0, 100, null, PrimeMinister); 
			PostList [16] = SortedPostList [23] = Mystik.SetPost (Titles[16], 0, 100, null, Spymaster); 
			PostList [17] = SortedPostList [24] = Ninja.SetPost (Titles[17], 0, 100, null, Spymaster); 
			PostList [18] = SortedPostList [25] = Sinobi.SetPost (Titles[18], 0, 100, null, Spymaster);
			PostList [19] = SortedPostList [26] = Orujeinichiy.SetPost (Titles[19], 0, 100, null, PrimeMinister); 
			PostList [20] = SortedPostList [27] = Dozorny.SetPost (Titles[20], 0, 100, null, Voevoda); 
			PostList [21] = SortedPostList [28] = Kluchnik.SetPost (Titles[21], 0, 100, null, Dvoretzky); 
			PostList [22] = SortedPostList [29] = Stolnik.SetPost (Titles[22], 0, 100, null, Dvoretzky); 
			PostList [23] = SortedPostList [30] = Postelnichiy.SetPost (Titles[23], 0, 100, null, Dvoretzky); 
			PostList [24] = SortedPostList [31] = Konuchiy.SetPost (Titles[24], 0, 100, null, Dvoretzky); 
			PostList [25] = SortedPostList [32] = Lovchiy.SetPost (Titles[25], 0, 100, null, Dvoretzky); 
			PostList [26] = SortedPostList [13] = Grandmeister.SetPost (Titles[26], 0, 100, null, Patriarch);
			PostList [27] = SortedPostList [15] = Paramedik.SetPost (Titles[27], 0, 100, null, Patriarch); 
			PostList [28] = SortedPostList [33] = Jurodiviy.SetPost (Titles[28], 0, 100, null, Patriarch); 
			PostList [29] = SortedPostList [34] = Skomoroh.SetPost (Titles[29], 0, 100, null, PrimeMinister); 
			PostList [30] = SortedPostList [7] = Trademaster.SetPost (Titles[30], 0, 100, null, Kaznachei); 
			PostList [31] = SortedPostList [8] = Buildmaster.SetPost (Titles[31], 0, 100, null, Kaznachei); 
			PostList [32] = SortedPostList [9] = Miner.SetPost (Titles[32], 0, 100, null, Kaznachei); 
			PostList [33] = SortedPostList [10] = Agrarian.SetPost (Titles[33], 0, 100, null, Kaznachei); 
			PostList [34] = SortedPostList [11] = Banker.SetPost (Titles[34], 0, 100, null, Kaznachei); 
			PostList [35] = SortedPostList [35] = Tamojnya.SetPost (Titles[35], 0, 100, null, Kaznachei); 
			PostList [36] = SortedPostList [36] = Courier.SetPost (Titles[36], 0, 100, null, PrimeMinister); 
			PostList [37] = SortedPostList [37] = Bodyguard.SetPost (Titles[37], 0, 100, null, Guardian); 
			PostList [38] = SortedPostList [38] = Commander1.SetPost (Titles[38], 0, 100, null, Voevoda);
			PostList [39] = SortedPostList [39] = Commander2.SetPost (Titles[39], 0, 100, null, Voevoda);
			PostList [40] = SortedPostList [40] = Commander3.SetPost (Titles[40], 0, 100, null, Voevoda); 
			PostList [41] = SortedPostList [41] = Commander4.SetPost (Titles[41], 0, 100, null, Voevoda); 
			PostList [42] = SortedPostList [42] = Commander5.SetPost (Titles[42], 0, 100, null, Voevoda); 
	
			SetCounsillorPosts ();

		}


//		public void AddPost (int i, Posts P) {
//
//		}

		// заполняем посты персонами из двора, кроме первых 2-х - король и ментор
		// также учитываем, что первые 10 - старики, потом знать, потом молодежь
		// на разные должности сажаем по возрасту
		public Court FillOffice (Court Dvor) {
			Person P;
			for (int i=2; i < PostNumber; i++) {
				if (i < 15)  P = Dvor.GetPerson(i-2);
				else  		P = Dvor.GetPerson(i-2+6); // пропускаем непосаженных на места старичков
				P.SetPost (SortedPostList[i].Replace (P, null));
				}
			return Dvor;
		}


		public int GetSize () {
			return PostNumber;
		}

		public Posts[] GetPostList () {
			return PostList;
		}

		public Posts GetPost (int x) {
			if (x >=0 && x < PostNumber)
				return PostList[x];
			return null;
		}

		public int GetPost (Posts post) {
			for (int i=0; i<PostNumber; i++) {
				if (post == GetPost (i)) 	
					return i;
				}
			return -1;
		}

		public Posts GetPost (string s) {
			Posts post;
			for (int i=0; i<PostNumber; i++) {
				if ((post = GetPost (i)) != null && post.Titul == s) 	
					return post;
				}
			return null;
		}


// снятие с должности, возвращает true если успешно
		public bool FireFromOffice (Person Loser) {
			Posts post;
			if ((post = Loser.GetPost()) != null && post.GetHolder() == Loser) {
				if ((post == PrimeMinister && Data.PrimeMinisterCannotBeFired) ||
					(post == Mentor && Data.MentorCannotBeFired)) 
					return false;
				post.Replace (null, null);
				Loser.SetPost (null);
				return true;
				}
			return false;
		}

// назначение на должность, возвращает пост либо нулл, если должность уже есть или пост не найден
		public Posts PlaceToOffice (Person Lucky, string place) {
			Posts post = GetPost (place);
			if (post != null && post.GetHolder() == null && Lucky.GetPost() == null) {
				post.Replace (Lucky, null);
				Lucky.SetPost (post);
				return post;
				}
			return null;
		}

		private void SetCounsillorPosts () {
			for (int i = 2; i < 7; i++) 
				PostList[i].isCounsillor = true;
		}


	}
}
