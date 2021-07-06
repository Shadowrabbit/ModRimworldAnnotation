using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BC RID: 6332
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x06008C81 RID: 35969 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008C82 RID: 35970 RVA: 0x0005E302 File Offset: 0x0005C502
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x06008C83 RID: 35971 RVA: 0x0005E329 File Offset: 0x0005C529
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			if (this.parent.MapHeld == null)
			{
				yield break;
			}
			TargetingParameters tp = this.GetTargetingParameters();
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (tp.CanTarget(pawn))
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
