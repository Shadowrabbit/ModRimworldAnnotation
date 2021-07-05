using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D9 RID: 2521
	public class ThoughtWorker_WearingColor_Favorite : ThoughtWorker_WearingColor
	{
		// Token: 0x06003E60 RID: 15968 RVA: 0x00155138 File Offset: 0x00153338
		protected override Color? Color(Pawn p)
		{
			return p.story.favoriteColor;
		}
	}
}
