using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalls : MonoBehaviour {

	public GameObject gameManagerObject;
	private GameManager gameManager;

    private void Start()
    {
		gameManager = gameManagerObject.GetComponent<GameManager>();
	}

    void OnTriggerEnter2D(Collider2D hitInfo) {
		if (hitInfo.name == "Ball")
		{
			string wallName = transform.name;
			gameManager.Score(wallName);
			hitInfo.gameObject.SendMessage ("ResetBall", 1, SendMessageOptions.RequireReceiver);
			//hitInfo.gameObject.SendMessage("GoBall", 2, SendMessageOptions.RequireReceiver);
		}
	}
}
