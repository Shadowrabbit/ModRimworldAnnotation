using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AC RID: 2476
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06003DE6 RID: 15846 RVA: 0x00153B70 File Offset: 0x00151D70
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ModsConfig.IdeologyActive)
			{
				return false;
			}
			return p.apparel.PsychologicallyNude;
		}
	}
}
