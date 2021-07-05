using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000866 RID: 2150
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		// Token: 0x060038BB RID: 14523 RVA: 0x0013DCF7 File Offset: 0x0013BEF7
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return pawn;
			yield break;
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060038BC RID: 14524 RVA: 0x00136B6A File Offset: 0x00134D6A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x0013DD08 File Offset: 0x0013BF08
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool flag = pawn == t && pawn.playerSettings != null && base.HasJobOnThing(pawn, t, forced);
			if (flag && !pawn.playerSettings.selfTend)
			{
				JobFailReason.Is("SelfTendDisabled".Translate(), null);
			}
			return flag && pawn.playerSettings.selfTend;
		}
	}
}
