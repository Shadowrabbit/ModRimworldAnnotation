using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C17 RID: 3095
	public class IncidentWorker_ResourcePodCrash : IncidentWorker
	{
		// Token: 0x060048AE RID: 18606 RVA: 0x00180894 File Offset: 0x0017EA94
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map, true);
			DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true, true);
			base.SendStandardLetter("LetterLabelCargoPodCrash".Translate(), "CargoPodCrash".Translate(), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
