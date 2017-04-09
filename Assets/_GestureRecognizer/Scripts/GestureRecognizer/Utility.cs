using UnityEngine;
using System.Collections;


namespace GestureRecognizer {
  
    public class Utility {
		

		public static bool IsTouchDevice() {
			return Application.platform == RuntimePlatform.Android || 
				Application.platform == RuntimePlatform.IPhonePlayer;
		}					
    }
}

