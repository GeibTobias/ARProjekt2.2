/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

	protected VuMarkBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

	protected virtual void Start()
	{
		mTrackableBehaviour = GetComponent<VuMarkBehaviour>();
		if (mTrackableBehaviour) {
			mTrackableBehaviour.RegisterTrackableEventHandler (this);
			mTrackableBehaviour.RegisterVuMarkTargetAssignedCallback (OnVuMarkAssigned);
		}
	}

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

	void OnVuMarkAssigned() {
		Debug.Log ("VuMark ID: " + mTrackableBehaviour.VuMarkTarget.InstanceId.StringValue);
	}

	protected virtual void OnTrackingFound()
	{

		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<BoxCollider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Enable rendering:
		foreach (var component in rendererComponents) {
			POI poi = component.gameObject.GetComponentInParent<POI>();
			if (poi && int.Parse(poi.trackerID) == int.Parse(mTrackableBehaviour.VuMarkTarget.InstanceId.StringValue)) {
				component.enabled = true;
			}
		}

		// Enable colliders:
		foreach (var component in colliderComponents) {
			POI poi = component.gameObject.GetComponent<POI> ();
			if (int.Parse(poi.trackerID) == int.Parse(mTrackableBehaviour.VuMarkTarget.InstanceId.StringValue)) {
				component.enabled = true;
			}
			Debug.Log(poi.trackerID + " collider enabled: " + component.enabled + " StringValue: " + mTrackableBehaviour.VuMarkTarget.InstanceId.StringValue);
		}

		// Enable canvas':
		foreach (var component in canvasComponents) {
			// Is a canvas in the parent or in this gameobject?
			POI poi = component.gameObject.GetComponent<POI>();
			if (int.Parse(poi.trackerID) == int.Parse(mTrackableBehaviour.VuMarkTarget.InstanceId.StringValue)) {
				component.enabled = true;
			}
		}
	}

    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<BoxCollider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
		foreach (var component in colliderComponents) {
			component.enabled = false;
			component.isTrigger = false;
		}

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PRIVATE_METHODS
}
