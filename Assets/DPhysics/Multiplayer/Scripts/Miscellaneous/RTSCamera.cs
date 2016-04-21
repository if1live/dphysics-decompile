using UnityEngine;
using System.Collections;

/// <summary>
/// A simple script for the camera that lets it move around in a bird's eye fashion
/// </summary>
public class RTSCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	float CamHeight = 30;
	float Speed = 20;
	//Updates the camera's position
	void LateUpdate () {
		Vector3 NewPosition = transform.position;
		NewPosition.y = CamHeight;
		float change = Speed * Time.deltaTime;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			NewPosition.x -= change;
		}
		else if (Input.GetKey (KeyCode.RightArrow))
		{
			NewPosition.x += change;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			NewPosition.z += change;
		}
		else if (Input.GetKey (KeyCode.DownArrow))
		{
			NewPosition.z -= change;
		}
		transform.position = NewPosition;
	}
	void OnGUI ()
	{
		GUILayout.BeginArea (new Rect(Screen.width - 50, 0, 50,300));
		GUILayout.Label ("Camera Speed");
		Speed = GUILayout.HorizontalSlider(Speed,5,40);
		GUILayout.Label ("CameraHeight");
		CamHeight = GUILayout.VerticalSlider(CamHeight,100,5);
		GUILayout.Label("Arrow keys to move camera");
		GUILayout.EndArea ();

	}
}
