// Modified from: http://wiki.unity3d.com/index.php/TextureScale#TextureScale.cs
// Only works on ARGB32, RGB24 and Alpha8 textures that are marked readable

using UnityEngine;


public class TextureScale
{
	private static Color[] texColors;
	private static Color[] newColors;
	private static int w;
	private static float ratioX;
	private static float ratioY;
	private static int w2;

	public static void Scale(ref Texture2D tex, int newWidth, int newHeight)
	{
		texColors = tex.GetPixels();
		newColors = new Color[newWidth * newHeight];
		ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
		ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
		w = tex.width;
		w2 = newWidth;

		BilinearScale(0, newHeight);

		tex.Reinitialize(newWidth, newHeight);
		tex.SetPixels(newColors);
		tex.Apply();
	}

	private static void BilinearScale(int start, int end)
	{
		for (var y = start; y < end; y++)
		{
			int yFloor = (int)Mathf.Floor(y * ratioY);
			var y1 = yFloor * w;
			var y2 = (yFloor + 1) * w;
			var yw = y * w2;

			for (var x = 0; x < w2; x++)
			{
				int xFloor = (int)Mathf.Floor(x * ratioX);
				var xLerp = x * ratioX - xFloor;
				newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
													   ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
													   y * ratioY - yFloor);
			}
		}
	}

	private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value,
						  c1.g + (c2.g - c1.g) * value,
						  c1.b + (c2.b - c1.b) * value,
						  c1.a + (c2.a - c1.a) * value);
	}

	public static void ScaleGPU(ref Texture2D texture, int newWidth, int newHeight){
		RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		RenderTexture.active = rt;
		Graphics.Blit(texture, rt);
		texture.Reinitialize(newWidth, newHeight, texture.format, false);
		texture.filterMode = FilterMode.Bilinear;
		texture.ReadPixels(new Rect(0.0f, 0.0f, newWidth, newHeight), 0, 0);
      	texture.Apply();
		RenderTexture.ReleaseTemporary(rt);
	}
}