using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F4 RID: 4596
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06006E9B RID: 28315 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x00250A6B File Offset: 0x0024EC6B
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x06006E9D RID: 28317 RVA: 0x00250A92 File Offset: 0x0024EC92
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			if (this.parent.MapHeld == null)
			{
				yield break;
			}
			TargetingParameters tp = this.GetTargetingParameters();
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (tp.CanTarget(pawn, null))
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}
	}
}
