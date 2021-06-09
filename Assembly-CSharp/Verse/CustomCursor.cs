using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000885 RID: 2181
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x06003613 RID: 13843 RVA: 0x00029EB7 File Offset: 0x000280B7
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x00029EC9 File Offset: 0x000280C9
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x040025B3 RID: 9651
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x040025B4 RID: 9652
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}
