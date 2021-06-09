using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BE RID: 6334
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x06008C8F RID: 35983 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008C90 RID: 35984 RVA: 0x0005E394 File Offset: 0x0005C594
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetBuildings = false,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = ((TargetInfo x) => x.Thing is Corpse && base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x06008C91 RID: 35985 RVA: 0x0005E3C9 File Offset: 0x0005C5C9
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
