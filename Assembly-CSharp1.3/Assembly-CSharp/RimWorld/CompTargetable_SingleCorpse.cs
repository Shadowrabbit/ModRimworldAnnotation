using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F5 RID: 4597
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x06006EA0 RID: 28320 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006EA1 RID: 28321 RVA: 0x00250AB9 File Offset: 0x0024ECB9
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

		// Token: 0x06006EA2 RID: 28322 RVA: 0x00250AEE File Offset: 0x0024ECEE
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
