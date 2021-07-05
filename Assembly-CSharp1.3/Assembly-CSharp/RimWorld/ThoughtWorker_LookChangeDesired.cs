using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D5 RID: 2517
	public class ThoughtWorker_LookChangeDesired : ThoughtWorker
	{
		// Token: 0x06003E57 RID: 15959 RVA: 0x00154FAD File Offset: 0x001531AD
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.style == null || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return p.style.LookChangeDesired;
		}
	}
}
