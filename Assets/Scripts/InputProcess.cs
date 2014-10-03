using UnityEngine;
using System.Collections;

public class InputProcess : MonoBehaviour
{

	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

	private float deltaMagnitudeDiff;
	private DeviceOrientation deviceOrientation;

	private ImageControl image;

	void Awake()
	{

	}


	void Start()
	{
		image = GetComponent<Manager>().Image;
		deviceOrientation = Input.deviceOrientation;
	}


	void Update()
	{
		if (deviceOrientation != Input.deviceOrientation && Input.deviceOrientation != DeviceOrientation.Unknown)
		{
			image.OrientationChanged();
		}

		// If there are two touches on the device...
		if (Input.touchCount == 2)
		{
			Pinch();
		} 

		if (Input.touchCount == 1)
		{
			Pan();
		} else
		{
			deltaMagnitudeDiff = 1;
		}
	}

	private void Pinch()
	{
		// Store both touches.
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);
		
		// Find the position in the previous frame of each touch.
		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
		
		// Find the magnitude of the vector (the distance) between the touches in each frame.
		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
		
		// Find the difference in the distances between each frame.
		deltaMagnitudeDiff = SmoothInput(deltaMagnitudeDiff, prevTouchDeltaMag - touchDeltaMag); 
		image.Resize(deltaMagnitudeDiff);

	}

	private void Pan()
	{	
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			Touch touch = Input.GetTouch(0);    
			Vector3 oldWorldPos = Camera.main.ScreenToWorldPoint(touch.position - touch.deltaPosition);
			Vector3 newWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
			image.Move(newWorldPos - oldWorldPos);
		}
		// On double tap image will be set at original position and scale
		else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).tapCount == 2)
		{
			image.ToOriginalSize();
		} 
	}

	private float SmoothInput(float prevValue, float newValue)
	{
		float a = 0.5f;
		return newValue * a + prevValue * (1 - a); //Усредняем значения по формуле S[t] = alpha*X[t] + (1-alpha)*S[t-1]
	}

	private Vector3 SmoothInput(Vector3 prevValue, Vector3 newValue)
	{
		float a = 0.3f;
		newValue.x = newValue.x * a + prevValue.x * (1 - a);
		newValue.y = newValue.y * a + prevValue.y * (1 - a);
		newValue.z = newValue.z * a + prevValue.z * (1 - a);

		return newValue;
	}
}
