using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3C RID: 3900
	public class RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x06005CBF RID: 23743 RVA: 0x001FE5E9 File Offset: 0x001FC7E9
		public RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar()
		{
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x001FE5F1 File Offset: 0x001FC7F1
		public RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x001FE78E File Offset: 0x001FC98E
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Execution target"))
			{
				yield break;
			}
			List<Thing> ritualSpots = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
			int num;
			for (int i = 0; i < ritualSpots.Count; i = num + 1)
			{
				yield return ritualSpots[i];
				num = i;
			}
			List<Thing> ideograms = map.listerThings.ThingsOfDef(ThingDefOf.Ideogram);
			for (int i = 0; i < ideograms.Count; i = num + 1)
			{
				yield return ideograms[i];
				num = i;
			}
			foreach (TargetInfo targetInfo in RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, map, this.parent.ideo))
			{
				yield return targetInfo;
			}
			IEnumerator<TargetInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x001FE7AC File Offset: 0x001FC9AC
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			if (thing.def == ThingDefOf.RitualSpot || thing.def == ThingDefOf.Ideogram)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x001FE809 File Offset: 0x001FCA09
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			foreach (string text in RitualObligationTargetWorker_Altar.GetTargetInfosWorker(this.parent.ideo))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield return ThingDefOf.RitualSpot.LabelCap;
			yield break;
			yield break;
		}
	}
}
