using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3B RID: 3899
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x06005CBA RID: 23738 RVA: 0x001FE5E9 File Offset: 0x001FC7E9
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar()
		{
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x001FE5F1 File Offset: 0x001FC7F1
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x001FE711 File Offset: 0x001FC911
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Altar target"))
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
			foreach (TargetInfo targetInfo in RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, map, this.parent.ideo))
			{
				yield return targetInfo;
			}
			IEnumerator<TargetInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x001FE730 File Offset: 0x001FC930
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			if (target.Thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x001FE77E File Offset: 0x001FC97E
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
