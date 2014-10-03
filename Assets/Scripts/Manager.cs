using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public GameObject imageObject;

	private ImageControl image ;

	public ImageControl Image
	{
		get
		{
			return image; 
		}
	}


	void Awake()
	{
		image = new ImageControl(imageObject);	
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame

	void Update () {
	
	}
}
