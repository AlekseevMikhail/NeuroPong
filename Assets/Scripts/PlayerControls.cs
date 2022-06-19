using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO.Ports;

public class PlayerControls : MonoBehaviour {
	SerialPort serialPort = new SerialPort("COM6", 115200);
	public KeyCode moveUp = KeyCode.W;
	public KeyCode moveDown = KeyCode.S;
	public float speed = 30.0f;
	public float boundY = 2.25f;
	private Rigidbody2D rb2d;
	private Vector3 target;
	private GameObject theBall;
	private BallControl ballScript;
	public float value1 = 0;
	public float value2 = 0;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		target = transform.position;
		theBall = GameObject.FindGameObjectWithTag("Ball");
		ballScript = (BallControl)theBall.GetComponent(typeof(BallControl));
		serialPort.Open();
		serialPort.ReadTimeout = 3;
	}
	
	// Update is called once per frame
	void Update () {

		bool userInteraction = false;

		//OPenBCI controls using 1 and 2 channel
		var vel = rb2d.velocity;
        if (serialPort.IsOpen) {
            try {
                string[] data=serialPort.ReadLine().Split(",");
				value1 = float.Parse(data[1]);
				value2 = float.Parse(data[2]);
				serialPort.DiscardInBuffer();
				serialPort.DiscardOutBuffer();
								
            } catch (System.Exception e) {
				Debug.Log(e);
			}
        }

        if (value1 > 230) {
			vel.y = speed;
			userInteraction = true;
        } else if (value2 > 230) {
			vel.y = -speed;
			userInteraction = true;
        }
		rb2d.velocity = vel;

		bool isOverGameObject = EventSystem.current.IsPointerOverGameObject();
		//Mouse/Touch Controls
		if (Input.GetMouseButton(0) && !isOverGameObject) {
			//Debug.Log(isOverGameObject);
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Maintain paddle X position and change Y position only (up/down)
			target.x = transform.position.x;
			target.z = transform.position.z;
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			userInteraction = true;

		}
		
		//Constrain to window size
		var pos = transform.position;
		if (pos.y > boundY) {
			pos.y = boundY;
		} else if (pos.y < -boundY) {
			pos.y = -boundY;
		}
		transform.position = pos;

		bool startGame = ballScript.BallIsStopped() && userInteraction;
		if (startGame)
		{
			theBall.SendMessage("GoBall", 0.5f, SendMessageOptions.RequireReceiver);
		}
	}
}
