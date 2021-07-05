using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DA RID: 1242
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x06002581 RID: 9601 RVA: 0x000E9DAC File Offset: 0x000E7FAC
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000E9DBE File Offset: 0x000E7FBE
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x04001777 RID: 6007
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04001778 RID: 6008
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}
