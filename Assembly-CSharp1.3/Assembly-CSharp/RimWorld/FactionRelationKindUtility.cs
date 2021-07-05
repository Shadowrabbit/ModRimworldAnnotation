using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBE RID: 3774
	public static class FactionRelationKindUtility
	{
		// Token: 0x0600590F RID: 22799 RVA: 0x001E6034 File Offset: 0x001E4234
		public static string GetLabel(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return "HostileLower".Translate();
			case FactionRelationKind.Neutral:
				return "NeutralLower".Translate();
			case FactionRelationKind.Ally:
				return "AllyLower".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06005910 RID: 22800 RVA: 0x001E608C File Offset: 0x001E428C
		public static string GetLabelCap(this FactionRelationKind kind)
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

		// Token: 0x06005911 RID: 22801 RVA: 0x001E60E2 File Offset: 0x001E42E2
		public static Color GetColor(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return ColorLibrary.RedReadable;
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
