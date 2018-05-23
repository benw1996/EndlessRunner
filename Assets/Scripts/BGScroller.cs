using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

    public float scrollSpeed;
    private Vector2 savedOffset;
    private bool scroll = false;

    private Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();

        savedOffset = rend.sharedMaterial.GetTextureOffset("_MainTex");

        GameController.StartDelegate += StartScrolling;
        GameController.PauseDelegate += StopScrolling;
        GameController.UnPauseDelegate += StartScrolling;
        LevelController.GameOverDelegate += StopScrolling;
	}
	
	// Update is called once per frame
	void Update () {
        if (scroll) {
            float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
            Vector2 offset = new Vector2(x, savedOffset.y);
            rend.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
    }

    void OnDisable() {
        rend.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }

    void StartScrolling() {
        scroll = true;
    }

    void StopScrolling() {
        scroll = false;
    }
}
