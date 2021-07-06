using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DB RID: 4571
	public class IncidentWorker_ResourcePodCrash : IncidentWorker
	{
		// Token: 0x0600642F RID: 25647 RVA: 0x001F1EE4 File Offset: 0x001F00E4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true, true);
			base.SendStandardLetter("LetterLabelCargoPodCrash".Translate(), "CargoPodCrash".Translate(), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
