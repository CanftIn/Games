using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	public GameObject DObj;
	public Sprite[] Digits;

	private int score;
	// Use this for initialization
	void Start () {
		score = 0;

		SetScore (0);
	}

	void SetScore(int score1){
		score += score1;

		//先清除原来的分数
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("score");
		for (int i = 0; i < objs.Length; i++) {
			Destroy(objs[i]);
		}

		//创新新的分数
		string s1 = score.ToString ();
		int index = 0;
		foreach (var item in s1) {
			Sprite dSprite= Digits[int.Parse( item.ToString())];
			GameObject img=Instantiate(DObj) as GameObject;
			img.GetComponent<SpriteRenderer>().sprite=dSprite;
			img.transform.position=new Vector3(-5f+index,4.4f,0);
			index++;
		}
	}
}
