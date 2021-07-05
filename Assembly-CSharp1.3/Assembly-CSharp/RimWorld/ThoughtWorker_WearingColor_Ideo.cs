using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DA RID: 2522
	public class ThoughtWorker_WearingColor_Ideo : ThoughtWorker_WearingColor
	{
		// Token: 0x06003E62 RID: 15970 RVA: 0x00155150 File Offset: 0x00153350
		protected override Color? Color(Pawn p)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return null;
			}
			return new Color?(ideo.ApparelColor);
		}
	}
}
