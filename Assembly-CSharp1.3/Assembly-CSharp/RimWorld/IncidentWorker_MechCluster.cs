using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C10 RID: 3088
	public class IncidentWorker_MechCluster : IncidentWorker
	{
		// Token: 0x06004891 RID: 18577 RVA: 0x0017FF5E File Offset: 0x0017E15E
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && Faction.OfMechanoids != null;
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x0017FF74 File Offset: 0x0017E174
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(parms.points, map, true, false);
			IntVec3 center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0.5f);
			if (!center.IsValid)
			{
				return false;
			}
			IEnumerable<Thing> targets = from t in MechClusterUtility.SpawnCluster(center, map, sketch, true, true, parms.questTag)
			where t.def != ThingDefOf.Wall && t.def != ThingDefOf.Barricade
			select t;
			base.SendStandardLetter(parms, new LookTargets(targets), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
