using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CooldownPrint  {

	private int timer = 0;
	private int interval;
	private string buffer = "";

	public CooldownPrint(int interval)
	{
		this.interval = interval;
	}


	private void Log()
	{
		if(buffer != "")
			Debug.Log(buffer);
		timer = 0;
		buffer = "";
	}

	public void AddToBuffer(string message)
	{
		buffer += message + "\n";
	}

	public void LogBuffer(bool dontWait)
	{
		if (dontWait)
			Log();
		else
		{
			if (timer >= interval)
				Log();
			timer++;
		}
	}

	
}
