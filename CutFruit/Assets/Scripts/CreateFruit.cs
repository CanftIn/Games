using UnityEngine;
using System.Collections;

public class CreateFruit : MonoBehaviour {
	public GameObject[] Fruits;

	private int fruitCount;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("Create", 1f, 0.5f);
		fruitCount = Fruits.Length;
	}
	void Create(){
		float y = -5.75f;
		float x= Random.Range (-6.5f, 6.5f);
		GameObject fruit= Instantiate(Fruits[Random.Range(0,fruitCount)]) as GameObject;
		fruit.transform.position = new Vector3 (x, y, 0);
			//在-6.5f至-2f，向右抛；-2f至2f，向不抛；2f至6.5f向左抛
			float xSpeed = 0f;
			if (x <= -3f) {
				xSpeed=Random.Range(2f,4f);
			} else if (x < 3f) {
				xSpeed=Random.Range(-1f,1f);
			} else {
				xSpeed=Random.Range(-4f,-2f);
			}
		fruit.GetComponent<Rigidbody2D> ().velocity = new Vector2 (xSpeed,14f);
		}
}
