using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000FAD RID: 4013
	public class RitualTargetFilter_Altar : RitualTargetFilter
	{
		// Token: 0x06005EC3 RID: 24259 RVA: 0x00206E74 File Offset: 0x00205074
		public RitualTargetFilter_Altar()
		{
		}

		// Token: 0x06005EC4 RID: 24260 RVA: 0x00206E7C File Offset: 0x0020507C
		public RitualTargetFilter_Altar(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x00207220 File Offset: 0x00205420
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilityDisabledNoAltarOrRitualsSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x0020725C File Offset: 0x0020545C
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Pawn pawn = initiator.Thing as Pawn;
			if (pawn == null)
			{
				return null;
			}
			IEnumerable<Precept_Building> enumerable = from b in pawn.Ideo.cachedPossibleBuildings
			where b.ThingDef.isAltar
			select b;
			Thing thing = null;
			float num = 99999f;
			foreach (Precept_Building precept_Building in enumerable)
			{
				foreach (Building building in initiator.Thing.Map.listerBuildings.AllBuildingsColonistOfDef(precept_Building.ThingDef))
				{
					if (building.def.isAltar && pawn.CanReach(building, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						int lengthHorizontalSquared = (pawn.Position - building.Position).LengthHorizontalSquared;
						if ((float)lengthHorizontalSquared < num)
						{
							thing = building;
							num = (float)lengthHorizontalSquared;
						}
					}
				}
			}
			if (thing == null && this.def.fallbackToRitualSpot)
			{
				foreach (Thing thing2 in pawn.Map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot))
				{
					if (pawn.CanReach(thing2, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						int lengthHorizontalSquared2 = (pawn.Position - thing2.Position).LengthHorizontalSquared;
						if ((float)lengthHorizontalSquared2 < num)
						{
							thing = thing2;
							num = (float)lengthHorizontalSquared2;
						}
					}
				}
			}
			return thing;
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x00207440 File Offset: 0x00205640
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			yield return "RitualTargetGatherAltarOrRitualSpotInfo".Translate();
			yield break;
		}
	}
}
