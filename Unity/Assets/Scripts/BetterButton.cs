using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class BetterButton : Button {
	// Event delegate triggered on mouse or touch down.
	[SerializeField]
	ButtonDownEvent _onDown = new ButtonDownEvent();
	ButtonUpEvent _onUp = new ButtonUpEvent();

	protected BetterButton() { }

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);

		if (eventData.button != PointerEventData.InputButton.Left)
			return;

		_onDown.Invoke();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);

		if (eventData.button != PointerEventData.InputButton.Left)
			return;

		_onUp.Invoke();
	}

	public ButtonDownEvent onDown
	{
		get { return _onDown; }
		set { _onDown = value; }
	}

	public ButtonUpEvent onUp
	{
		get { return _onUp; }
		set { _onUp = value; }
	}

	[Serializable]
	public class ButtonDownEvent : UnityEvent { }
	public class ButtonUpEvent : UnityEvent { }
}