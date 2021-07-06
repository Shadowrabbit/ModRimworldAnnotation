using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DA7 RID: 3495
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		// Token: 0x06004FAA RID: 20394 RVA: 0x00037FBE File Offset: 0x000361BE
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return pawn;
			yield break;
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06004FAB RID: 20395 RVA: 0x00036CF4 File Offset: 0x00034EF4
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x001B514C File Offset: 0x001B334C
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
