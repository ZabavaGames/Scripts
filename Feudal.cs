using System.Collections;
using UnityEngine;

namespace JagaJaga
{

public class Feudal : Person {
	//	public int[] Holdings;
		public ArrayList Holdings = new ArrayList ();
		

	void Start () {
			Init ("", Sex.Man, 0, 0);
			isFeudal = true;
			DomainSize = 0; MaxDomainSize = 10;
		//	Holdings = new int[MaxDomainSize]; // неэффективное использование памяти :)))
		
		}	

		public void AddHolding (Settlement Hold) {
			Holdings.Add (Hold);
			DomainSize ++;
		}
	
		public void RemoveHolding (Settlement Hold) {
			Holdings.Remove (Hold);
			DomainSize --;
		}
	

		public Settlement GetHolding (int i) {
			return (Settlement)Holdings[i];
		}



	}
}
