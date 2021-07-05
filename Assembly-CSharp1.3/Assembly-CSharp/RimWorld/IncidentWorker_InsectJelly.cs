using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0D RID: 3085
	public class IncidentWorker_InsectJelly : IncidentWorker
	{
		// Token: 0x06004885 RID: 18565 RVA: 0x0017FA18 File Offset: 0x0017DC18
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x0017FA4C File Offset: 0x0017DC4C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = InfestationUtility.SpawnJellyTunnels(Rand.Range(2, 3), (int)parms.points / 8, map);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04002C64 RID: 11364
		public const int JellyPointsCost = 8;
	}
}
