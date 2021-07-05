using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001547 RID: 5447
	public class PlaceWorker_DoorLearnOpeningSpeed : PlaceWorker
	{
		// Token: 0x06008164 RID: 33124 RVA: 0x002DC350 File Offset: 0x002DA550
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			Blueprint_Door blueprint_Door = (Blueprint_Door)loc.GetThingList(map).FirstOrDefault((Thing t) => t is Blueprint_Door);
			if (blueprint_Door != null && blueprint_Door.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.DoorOpenSpeed, blueprint_Door.stuffToUse) < 0.65f)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.DoorOpenSpeed, OpportunityType.Important);
			}
		}
	}
}
