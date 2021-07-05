using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005B2 RID: 1458
	internal class Toils_Interact
	{
		// Token: 0x06002AA5 RID: 10917 RVA: 0x001002B4 File Offset: 0x000FE4B4
		public static Toil DestroyThing(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (!thing.Destroyed)
				{
					if (thing.def.category == ThingCategory.Plant && thing.def.plant.IsTree && thing.def.plant.treeLoversCareIfChopped)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.CutTree, actor.Named(HistoryEventArgsNames.Doer)), true);
					}
					thing.Destroy(DestroyMode.Vanish);
				}
			};
			return toil;
		}
	}
}
