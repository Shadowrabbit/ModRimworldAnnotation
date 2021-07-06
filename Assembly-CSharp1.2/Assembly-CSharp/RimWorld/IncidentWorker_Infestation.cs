using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C5 RID: 4549
	public class IncidentWorker_Infestation : IncidentWorker
	{
		// Token: 0x060063E6 RID: 25574 RVA: 0x001F0EE8 File Offset: 0x001EF0E8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && HiveUtility.TotalSpawnedHivesCount(map) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x060063E7 RID: 25575 RVA: 0x001F0F20 File Offset: 0x001EF120
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = InfestationUtility.SpawnTunnels(Mathf.Max(GenMath.RoundRandom(parms.points / 220f), 1), map, false, false, null);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		// Token: 0x040042C8 RID: 17096
		public const float HivePoints = 220f;
	}
}
