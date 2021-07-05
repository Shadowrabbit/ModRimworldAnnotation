using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200103A RID: 4154
	public class Building_StylingStation : Building
	{
		// Token: 0x0600621A RID: 25114 RVA: 0x00214B34 File Offset: 0x00212D34
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (ModLister.IdeologyInstalled)
			{
				if (!selPawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return new FloatMenuOption("CannotUseReason".Translate("NoPath".Translate().CapitalizeFirst()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return new FloatMenuOption("ChangeStyle".Translate().CapitalizeFirst(), delegate()
					{
						selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OpenStylingStationDialog, this), new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				Job job;
				if (JobGiver_OptimizeApparel.TryCreateRecolorJob(selPawn, out job, true))
				{
					yield return new FloatMenuOption("RecolorApparel".Translate().CapitalizeFirst(), delegate()
					{
						Job job2;
						JobGiver_OptimizeApparel.TryCreateRecolorJob(selPawn, out job2, false);
						selPawn.jobs.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			yield break;
			yield break;
		}
	}
}
