using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3A RID: 3898
	public class RitualObligationTargetWorker_AnyRitualSpot : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x06005CB5 RID: 23733 RVA: 0x001FE5E9 File Offset: 0x001FC7E9
		public RitualObligationTargetWorker_AnyRitualSpot()
		{
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x001FE5F1 File Offset: 0x001FC7F1
		public RitualObligationTargetWorker_AnyRitualSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x001FE6CE File Offset: 0x001FC8CE
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Ritual spot target"))
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
			yield break;
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x001FE6DE File Offset: 0x001FC8DE
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			return target.Thing.def == ThingDefOf.RitualSpot;
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x001FE708 File Offset: 0x001FC908
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return ThingDefOf.RitualSpot.LabelCap;
			yield break;
		}
	}
}
