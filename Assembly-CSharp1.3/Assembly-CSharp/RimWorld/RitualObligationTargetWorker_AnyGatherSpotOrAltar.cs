using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F39 RID: 3897
	public class RitualObligationTargetWorker_AnyGatherSpotOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x06005CB0 RID: 23728 RVA: 0x001FE5E9 File Offset: 0x001FC7E9
		public RitualObligationTargetWorker_AnyGatherSpotOrAltar()
		{
		}

		// Token: 0x06005CB1 RID: 23729 RVA: 0x001FE5F1 File Offset: 0x001FC7F1
		public RitualObligationTargetWorker_AnyGatherSpotOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CB2 RID: 23730 RVA: 0x001FE5FA File Offset: 0x001FC7FA
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Altar target"))
			{
				yield break;
			}
			List<Thing> partySpot = map.listerThings.ThingsOfDef(ThingDefOf.PartySpot);
			int num;
			for (int i = 0; i < partySpot.Count; i = num + 1)
			{
				yield return partySpot[i];
				num = i;
			}
			List<Thing> ritualSpots = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
			for (int i = 0; i < ritualSpots.Count; i = num + 1)
			{
				yield return ritualSpots[i];
				num = i;
			}
			for (int i = 0; i < map.gatherSpotLister.activeSpots.Count; i = num + 1)
			{
				yield return map.gatherSpotLister.activeSpots[i].parent;
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

		// Token: 0x06005CB3 RID: 23731 RVA: 0x001FE618 File Offset: 0x001FC818
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			if (this.def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
			{
				return false;
			}
			if (thing.def == ThingDefOf.PartySpot)
			{
				return true;
			}
			if (thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
			if (compGatherSpot != null && compGatherSpot.Active)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x001FE6BE File Offset: 0x001FC8BE
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetGatherSpotInfo".Translate();
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
