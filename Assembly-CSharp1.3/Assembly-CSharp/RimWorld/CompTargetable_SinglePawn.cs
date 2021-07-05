using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F6 RID: 4598
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06006EA5 RID: 28325 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006EA6 RID: 28326 RVA: 0x00250B1D File Offset: 0x0024ED1D
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x06006EA7 RID: 28327 RVA: 0x00250B44 File Offset: 0x0024ED44
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
