using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F41 RID: 3905
	public class RitualObligationTargetWorker_CampfireParty : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x06005CD5 RID: 23765 RVA: 0x001FE9E5 File Offset: 0x001FCBE5
		public RitualObligationTargetWorker_CampfireParty()
		{
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x001FE9ED File Offset: 0x001FCBED
		public RitualObligationTargetWorker_CampfireParty(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x001FEB28 File Offset: 0x001FCD28
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Campfire party"))
			{
				return false;
			}
			if (!base.CanUseTargetInternal(target, obligation).canUse)
			{
				return false;
			}
			Thing thing = target.Thing;
			CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
			if (compRefuelable != null && !compRefuelable.HasFuel)
			{
				return "RitualTargetCampfireNoFuel".Translate();
			}
			List<Thing> forCell = target.Map.listerBuldingOfDefInProximity.GetForCell(target.Cell, (float)this.def.maxDrumDistance, ThingDefOf.Drum, null);
			bool flag = false;
			using (List<Thing>.Enumerator enumerator = forCell.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoom(RegionType.Set_All) == thing.GetRoom(RegionType.Set_All))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return "RitualTargetNoDrum".Translate();
			}
			return true;
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x001FEC1C File Offset: 0x001FCE1C
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetCampfirePartyInfo".Translate();
			yield break;
		}
	}
}
