using UnityEngine;
using System.Collections;

public class BtnClick : MonoBehaviour {
	public Sprite Sound1;
	public Sprite Sound2;

	private bool isPlayBg=false;

	void OnClick(){
		if (name == "play") {
			Application.LoadLevel ("play");
		} else if (name == "credits") {
			Application.OpenURL ("http://www.baidu.com");
		} else if (name == "more") {
			Application.OpenURL ("http://www.baidu.com");
		} else if (name == "sound") {
			if (isPlayBg) {//当前正在播放音乐，点击后停播
				GetComponent<SpriteRenderer> ().sprite = Sound2;
				Camera.main.GetComponent<AudioSource> ().Stop ();
				isPlayBg = false;
			} else {
				GetComponent<SpriteRenderer> ().sprite = Sound1;
				Camera.main.GetComponent<AudioSource> ().Play ();
				isPlayBg = true;
			}
		} else if (name == "main") {
			Application.LoadLevel("start");
		}
	}
}
