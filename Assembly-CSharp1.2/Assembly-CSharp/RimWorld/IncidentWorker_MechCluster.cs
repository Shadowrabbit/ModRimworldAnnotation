using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011CE RID: 4558
	public class IncidentWorker_MechCluster : IncidentWorker
	{
		// Token: 0x06006404 RID: 25604 RVA: 0x001F15F8 File Offset: 0x001EF7F8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch_NewTemp(parms.points, map, true, false);
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
