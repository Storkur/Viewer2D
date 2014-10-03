using UnityEngine;
using System.Collections;

public class ImageControl
{
	private GameObject image;
	private Rect screen;

	public ImageControl(GameObject loadImage)
	{
		image = Object.Instantiate(loadImage, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		ToOriginalSize();
		OrientationChanged();
	}

	public void Resize(float zoomFactor)
	{
		Vector3 scale = new Vector3(1f, 1f, 1f);

		image.transform.localScale += new Vector3(1f, 1f, 0f) * zoomFactor * -0.003f;
		scale.x = Mathf.Max(image.transform.localScale.y, 0.1f); 
		scale.y = Mathf.Max(image.transform.localScale.y, 0.1f);

		image.transform.localScale = scale;

		if (GetSides().width > screen.width)
		{
			SnapToX(image.transform.position);
		}
		if (GetSides().height > screen.height)
		{
			SnapToY(image.transform.position);
		}
		if ((GetSides().width < screen.width) && (GetSides().height < screen.height))
		{
			ToOriginalSize();
		}
	}

	public void Move(Vector3 deltaMove)
	{
		Vector3 newPosition = deltaMove + image.transform.position;
		if (GetSides().width > screen.width)
		{
			SnapToX(newPosition);
		}
		if (GetSides().height > screen.height)
		{
			SnapToY(newPosition);
		}
	}

	public void ToOriginalSize()
	{
		SpriteRenderer spriteRender = image.GetComponent<SpriteRenderer>();

		image.transform.localScale = new Vector3(1, 1, 1);
		float width = spriteRender.sprite.bounds.size.x;
		float height = spriteRender.sprite.bounds.size.y;
		
		var worldScreenHeight = Camera.main.orthographicSize * 2.0;
		var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		float x = (float)worldScreenWidth / width;
		float y = (float)worldScreenHeight / height;
		if (x > y)
			x = y;
		else
			y = x;
		image.transform.localScale = new Vector3(x, y, image.transform.localScale.z);
		image.transform.position = Vector3.zero;
	}

	public void Rotate()
	{
	}

	public void OrientationChanged()
	{
		Vector2 screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
		screen = new Rect(-screenSize.x, -screenSize.y, screenSize.x * 2, screenSize.y * 2);
		//ToOriginalSize();
	}
	
	private Rect GetSides()
	{
		SpriteRenderer sprite = image.GetComponent<SpriteRenderer>();
		Vector2 p1 = sprite.bounds.min;
		float width = sprite.bounds.size.x;	
		float height = sprite.bounds.size.y;	

		return new Rect(p1.x, p1.y, width, height);
	}

	void SnapToX(Vector3 newPosition)
	{
		float max = screen.xMin + GetSides().width / 2;
		float min = screen.xMax - GetSides().width / 2;
		float x = Mathf.Clamp(newPosition.x, min, max);
		image.transform.position = new Vector3(x, image.transform.position.y, newPosition.z);
	}

	void SnapToY(Vector3 newPosition)
	{
		float max = screen.yMin + GetSides().height / 2;
		float min = screen.yMax - GetSides().height / 2;
		float y = Mathf.Clamp(newPosition.y, min, max);
		image.transform.position = new Vector3(image.transform.position.x, y, newPosition.z);
	}

}
