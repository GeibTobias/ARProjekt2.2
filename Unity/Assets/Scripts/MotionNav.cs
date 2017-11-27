using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public enum ButtonPress
{
	up, down, left, right, plus, minus
}

public class MotionNav : MonoBehaviour {

    public GameObject buttons;
	public BetterButton upButton;
	public BetterButton downButton;
	public BetterButton leftButton;
	public BetterButton rightButton;
	public BetterButton plusButton;
	public BetterButton minusButton;
	private bool upButtonPressed;
	private bool downButtonPressed;
	private bool leftButtonPressed;
	private bool rightButtonPressed;
	private bool plusButtonPressed;
	private bool minusButtonPressed;
	private IEnumerator upCouroutine;
	private IEnumerator downCouroutine;
	private IEnumerator leftCouroutine;
	private IEnumerator rightCouroutine;
	private IEnumerator plusCouroutine;
	private IEnumerator minusCouroutine;
	private RestConsumer restConsumer;

	// Use this for initialization
	void Start () {
		restConsumer = GameObject.Find("RestConsumer").GetComponent<RestConsumer>();
		upButton.onDown.AddListener(onUpButtonPressed);
		upButton.onUp.AddListener(onUpButtonReleased);
		downButton.onDown.AddListener(onDownButtonPressed);
		downButton.onUp.AddListener(onDownButtonReleased);
		leftButton.onDown.AddListener(onLeftButtonPressed);
		leftButton.onUp.AddListener(onLeftButtonReleased);
		rightButton.onDown.AddListener(onRightButtonPressed);
		rightButton.onUp.AddListener(onRightButtonReleased);
		plusButton.onDown.AddListener(onPlusButtonPressed);
		plusButton.onUp.AddListener(onPlusButtonReleased);
		minusButton.onDown.AddListener(onMinusButtonPressed);
		minusButton.onUp.AddListener(onMinusButtonReleased);

		upCouroutine = sendCouroutine (ButtonPress.up);
		downCouroutine = sendCouroutine (ButtonPress.down);
		leftCouroutine = sendCouroutine (ButtonPress.left);
		rightCouroutine = sendCouroutine (ButtonPress.right);
		plusCouroutine = sendCouroutine (ButtonPress.plus);
		minusCouroutine = sendCouroutine (ButtonPress.minus);
	}

	// Update is called once per frame
	void Update () {
	}

    public void toggleVisibility()
    {
        buttons.SetActive(!buttons.activeSelf);
    }

	public void onUpButtonPressed() {
		onButtonPressed (ButtonPress.up);
	}
	public void onDownButtonPressed() {
		onButtonPressed (ButtonPress.down);
	}
	public void onLeftButtonPressed() {
		onButtonPressed (ButtonPress.left);
	}
	public void onRightButtonPressed() {
		onButtonPressed (ButtonPress.right);
	}
	public void onMinusButtonPressed() {
		onButtonPressed (ButtonPress.minus);
	}
	public void onPlusButtonPressed() {
		onButtonPressed (ButtonPress.plus);
	}
	public void onUpButtonReleased() {
		onButtonReleased (ButtonPress.up);
	}
	public void onDownButtonReleased() {
		onButtonReleased (ButtonPress.down);
	}
	public void onLeftButtonReleased() {
		onButtonReleased (ButtonPress.left);
	}
	public void onRightButtonReleased() {
		onButtonReleased (ButtonPress.right);
	}
	public void onMinusButtonReleased() {
		onButtonReleased (ButtonPress.minus);
	}
	public void onPlusButtonReleased() {
		onButtonReleased (ButtonPress.plus);
	}

	public void onButtonPressed(ButtonPress btnPr) {
		switch (btnPr) {
		case ButtonPress.up:
			upButtonPressed = true;
			StartCoroutine (upCouroutine);
			break;
		case ButtonPress.down:
			downButtonPressed = true;
			StartCoroutine (downCouroutine);
			break;
		case ButtonPress.left:
			leftButtonPressed = true;
			StartCoroutine (leftCouroutine);
			break;
		case ButtonPress.right:
			rightButtonPressed = true;
			StartCoroutine (rightCouroutine);
			break;
		case ButtonPress.plus:
			plusButtonPressed = true;
			StartCoroutine (plusCouroutine);
			break;
		case ButtonPress.minus:
			minusButtonPressed = true;
			StartCoroutine (minusCouroutine);
			break;
		default:
			break;
		}
	}

	public void onButtonReleased(ButtonPress btnPr) {
		switch (btnPr) {
		case ButtonPress.up:
			upButtonPressed = false;
			StopCoroutine (upCouroutine);
			break;
		case ButtonPress.down:
			downButtonPressed = false;
			StopCoroutine (downCouroutine);
			break;
		case ButtonPress.left:
			leftButtonPressed = false;
			StopCoroutine (leftCouroutine);
			break;
		case ButtonPress.right:
			rightButtonPressed = false;
			StopCoroutine (rightCouroutine);
			break;
		case ButtonPress.plus:
			plusButtonPressed = false;
			StopCoroutine (plusCouroutine);
			break;
		case ButtonPress.minus:
			minusButtonPressed = false;
			StopCoroutine (minusCouroutine);
			break;
		default:
			break;
		}
	}

	IEnumerator sendCouroutine(ButtonPress btnPr) {

		switch (btnPr) {
		case ButtonPress.up:
			while (upButtonPressed) {
				sendEventToServer (ButtonPress.up);
				yield return new WaitForSeconds (1);
			}
			break;
		case ButtonPress.down:
			while (downButtonPressed) {
				sendEventToServer (ButtonPress.down);
				yield return new WaitForSeconds (1);
			}
			break;
		case ButtonPress.left:
			while (leftButtonPressed) {
				sendEventToServer (ButtonPress.left);
				yield return new WaitForSeconds (1);
			}
			break;
		case ButtonPress.right:
			while (rightButtonPressed) {
				sendEventToServer (ButtonPress.right);
				yield return new WaitForSeconds (1);
			}
			break;
		case ButtonPress.plus:
			while (plusButtonPressed) {
				sendEventToServer (ButtonPress.plus);
				yield return new WaitForSeconds (1);
			}
			break;
		case ButtonPress.minus:
			while (minusButtonPressed) {
				sendEventToServer (ButtonPress.minus);
				yield return new WaitForSeconds (1);
			}
			break;
		default:
			break;
		}
	}

	void sendEventToServer(ButtonPress btnPr) {
		switch (btnPr) {
		case ButtonPress.up:
			StartCoroutine(restConsumer.setMapSettings (0, -100));
			Debug.Log ("up");
			break;
		case ButtonPress.down:
			StartCoroutine(restConsumer.setMapSettings (0, 100));
			break;
		case ButtonPress.left:
                StartCoroutine(restConsumer.setMapSettings (-100, 0));
			break;
		case ButtonPress.right:
            StartCoroutine(restConsumer.setMapSettings (100, 0));
			break;
		case ButtonPress.plus:
            StartCoroutine(restConsumer.incrementZoom ());
			break;
		case ButtonPress.minus:
            StartCoroutine(restConsumer.decrementZoom ());
			break;
		default:
			break;
		}
	}
}
