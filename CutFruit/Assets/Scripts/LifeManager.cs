using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {
	public SpriteRenderer[] Lifes;

	private int lifeCount;
	// Use this for initialization
	void Start () {
		lifeCount = Lifes.Length;
	}

	void SetLife(){
		Lifes [--lifeCount].color = Color.red;
		if (lifeCount <= 0) {
			Application.LoadLevel("over");
		}
	}

}
