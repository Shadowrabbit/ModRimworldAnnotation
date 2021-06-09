using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000841 RID: 2113
	public static class SimpleColorExtension
	{
		// Token: 0x060034D9 RID: 13529 RVA: 0x001553C0 File Offset: 0x001535C0
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
			default:
				throw new ArgumentException();
			}
		}
	}
}
