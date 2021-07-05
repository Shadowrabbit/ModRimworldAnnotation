using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0B RID: 3083
	public class IncidentWorker_Infestation : IncidentWorker
	{
		// Token: 0x0600487F RID: 18559 RVA: 0x0017F720 File Offset: 0x0017D920
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && HiveUtility.TotalSpawnedHivesCount(map) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x0017F760 File Offset: 0x0017D960
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = InfestationUtility.SpawnTunnels(Mathf.Max(GenMath.RoundRandom(parms.points / 220f), 1), map, false, parms.infestationLocOverride != null, null, parms.infestationLocOverride, null);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		// Token: 0x04002C63 RID: 11363
		public const float HivePoints = 220f;
	}
}
