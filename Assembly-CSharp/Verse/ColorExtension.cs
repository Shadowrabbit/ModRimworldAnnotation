using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007C5 RID: 1989
	public static class ColorExtension
	{
		// Token: 0x06003211 RID: 12817 RVA: 0x00027522 File Offset: 0x00025722
		public static Color ToOpaque(this Color c)
		{
			c.a = 1f;
			return c;
		}
	}
}
