using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B3 RID: 1203
	public static class SimpleColorExtension
	{
		// Token: 0x0600249B RID: 9371 RVA: 0x000E3988 File Offset: 0x000E1B88
		public static Color ToUnityColor(this SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return Color.white;
			case SimpleColor.Red:
				return Color.red;
			case SimpleColor.Green:
				return Color.green;
			case SimpleColor.Blue:
				return Color.blue;
			case SimpleColor.Magenta:
				return Color.magenta;
			case SimpleColor.Yellow:
				return Color.yellow;
			case SimpleColor.Cyan:
				return Color.cyan;
			case SimpleColor.Orange:
				return ColorLibrary.Orange;
			default:
				throw new ArgumentException();
			}
		}
	}
}
