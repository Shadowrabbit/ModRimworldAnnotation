using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F37 RID: 3895
	public class RitualObligationTargetWorker_AnyGatherSpot : RitualObligationTargetFilter
	{
		// Token: 0x06005CA2 RID: 23714 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_AnyGatherSpot()
		{
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_AnyGatherSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x001FE461 File Offset: 0x001FC661
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			List<Thing> partySpot = map.listerThings.ThingsOfDef(ThingDefOf.PartySpot);
			int num;
			for (int i = 0; i < partySpot.Count; i = num + 1)
			{
				yield return partySpot[i];
				num = i;
			}
			for (int i = 0; i < map.gatherSpotLister.activeSpots.Count; i = num + 1)
			{
				yield return map.gatherSpotLister.activeSpots[i].parent;
				num = i;
			}
			yield break;
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x001FE474 File Offset: 0x001FC674
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
			CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
			if (compGatherSpot != null && compGatherSpot.Active)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06005CA6 RID: 23718 RVA: 0x001FE4F5 File Offset: 0x001FC6F5
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetGatherSpotInfo".Translate();
			yield break;
		}
	}
}
