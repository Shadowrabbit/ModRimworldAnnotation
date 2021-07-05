using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000952 RID: 2386
	public class ThoughtWorker_Precept_HasNoProsthetic : ThoughtWorker_Precept
	{
		// Token: 0x06003D02 RID: 15618 RVA: 0x00150DCE File Offset: 0x0014EFCE
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return !ThoughtWorker_Precept_HasProsthetic.HasProsthetic(p);
		}
	}
}
