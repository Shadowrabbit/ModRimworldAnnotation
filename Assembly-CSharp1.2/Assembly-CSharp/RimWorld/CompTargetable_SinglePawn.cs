using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C0 RID: 6336
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x06008C9C RID: 35996 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008C9D RID: 35997 RVA: 0x0005E422 File Offset: 0x0005C622
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x06008C9E RID: 35998 RVA: 0x0005E449 File Offset: 0x0005C649
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
