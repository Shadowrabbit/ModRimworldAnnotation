using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046B RID: 1131
	public static class ColorExtension
	{
		// Token: 0x0600224A RID: 8778 RVA: 0x000D9200 File Offset: 0x000D7400
		public static Color ToOpaque(this Color c)
		{
			c.a = 1f;
			return c;
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x000D920F File Offset: 0x000D740F
		public static Color ToTransparent(this Color c, float transparency)
		{
			c.a = transparency;
			return c;
		}
	}
}
