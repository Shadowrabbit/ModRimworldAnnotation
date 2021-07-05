using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3E RID: 3902
	public class RitualObligationTargetWorker_IdeoBuildingOrRitualSpot : RitualObligationTargetFilter
	{
		// Token: 0x06005CC8 RID: 23752 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot()
		{
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CCA RID: 23754 RVA: 0x001FE899 File Offset: 0x001FCA99
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			yield break;
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x001FE8A4 File Offset: 0x001FCAA4
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Ideo building target"))
			{
				return false;
			}
			Building building = target.Thing as Building;
			if (building == null || building.Faction == null || !building.Faction.IsPlayer)
			{
				return false;
			}
			if (building.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			for (int i = 0; i < this.parent.ideo.PreceptsListForReading.Count; i++)
			{
				Precept_Building precept_Building = this.parent.ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null && building.def == precept_Building.ThingDef)
				{
					return true;
				}
			}
			if (building.TryGetComp<CompGatherSpot>() != null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x001FE96D File Offset: 0x001FCB6D
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			int num;
			for (int i = 0; i < this.parent.ideo.PreceptsListForReading.Count; i = num + 1)
			{
				Precept_Building precept_Building = this.parent.ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null)
				{
					yield return precept_Building.LabelCap;
				}
				num = i;
			}
			yield return ThingDefOf.RitualSpot.LabelCap;
			yield return "RitualTargetGatherSpotInfo".Translate();
			yield break;
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x00002688 File Offset: 0x00000888
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			return null;
		}
	}
}
