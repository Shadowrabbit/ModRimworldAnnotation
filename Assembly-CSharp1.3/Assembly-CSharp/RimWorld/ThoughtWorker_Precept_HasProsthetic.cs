using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000950 RID: 2384
	public class ThoughtWorker_Precept_HasProsthetic : ThoughtWorker_Precept
	{
		// Token: 0x06003CFD RID: 15613 RVA: 0x00150D6D File Offset: 0x0014EF6D
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_HasProsthetic.HasProsthetic(p);
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00150D7C File Offset: 0x0014EF7C
		public static bool HasProsthetic(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.countsAsAddedPartOrImplant)
				{
					return true;
				}
			}
			return false;
		}
	}
}
