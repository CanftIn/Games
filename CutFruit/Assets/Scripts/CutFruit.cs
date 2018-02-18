using UnityEngine;
using System.Collections;

public class CutFruit : MonoBehaviour {
	public GameObject KnifeRay;
	public GameObject Fruit1;
	public GameObject Fruit2;
	public GameObject[] Wz;
	public GameObject GoldTip;
	public AudioClip GoldClip;
	public AudioClip FailClip;

	private BoxCollider2D collider;
	private GameObject Script;
	// Use this for initialization
	void Start () {
		collider=GetComponent<BoxCollider2D>();
		Script = GameObject.Find ("script");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			//确定水果被切
			if(collider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))){
				//进行分数管理
				if(name!="cs0(Clone)"){
					if(name=="jpg0(Clone)"){
						GameObject tip=Instantiate(GoldTip) as GameObject;
						tip.transform.position=transform.position;
						Destroy(tip,1);
						AudioSource.PlayClipAtPoint(GoldClip,transform.position);
						Script.SendMessage("SetScore",20);
					}else{
						Script.SendMessage("SetScore",1);
					}
				}else{
					AudioSource.PlayClipAtPoint(FailClip,transform.position);
					Script.SendMessage("SetLife");
				}

				//绘制刀光对象
				GameObject knifeRay=Instantiate(KnifeRay,
				     transform.position,
				     Quaternion.AngleAxis(Random.Range(-90f,90f),Vector3.back)) as GameObject;
				Destroy(knifeRay,1);

				//绘制切开的水果
				GameObject fruit1=Instantiate(Fruit1,transform.position,
				    Quaternion.AngleAxis(Random.Range(-30f,30f),Vector3.back))
				     as GameObject;
				fruit1.GetComponent<Rigidbody2D>().velocity=new Vector2(
					Random.Range(-6f,-2f),
					Random.Range(2f,5f));
				Destroy(fruit1,2);

				GameObject fruit2=Instantiate(Fruit2,transform.position,
				       Quaternion.AngleAxis(Random.Range(-30f,30f),Vector3.back))
						as GameObject;
				fruit2.GetComponent<Rigidbody2D>().velocity=new Vector2(
					Random.Range(2f,6f),
					Random.Range(2f,5f));
				Destroy(fruit2,2);

				//绘制污渍
				GameObject wz=Instantiate(Wz[Random.Range(0,3)]) as GameObject;
				wz.transform.position=transform.position;
				Destroy(wz,1);

				//销毁当前完整的水果
				Destroy(this.gameObject);
			}
		}

		if(transform.position.y<=-5.8f){
			if(name!="cs0(Clone)"){
				AudioSource.PlayClipAtPoint(FailClip,transform.position);
				Script.SendMessage("SetLife");
			}
			
			Destroy(gameObject);
		}
	}
}
