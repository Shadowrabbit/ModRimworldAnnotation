using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A3 RID: 5539
	public static class FactionRelationKindUtility
	{
		// Token: 0x0600784B RID: 30795 RVA: 0x0024972C File Offset: 0x0024792C
		public static string GetLabel(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return "Hostile".Translate();
			case FactionRelationKind.Neutral:
				return "Neutral".Translate();
			case FactionRelationKind.Ally:
				return "Ally".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600784C RID: 30796 RVA: 0x00050FBD File Offset: 0x0004F1BD
		public static Color GetColor(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return ColoredText.RedReadable;
			case FactionRelationKind.Neutral:
				return new Color(0f, 0.75f, 1f);
			case FactionRelationKind.Ally:
				return Color.green;
			default:
				return Color.white;
			}
		}
	}
}
