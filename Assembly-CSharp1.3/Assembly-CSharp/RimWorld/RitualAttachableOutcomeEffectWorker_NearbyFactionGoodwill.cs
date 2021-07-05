using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F05 RID: 3845
	public class RitualAttachableOutcomeEffectWorker_NearbyFactionGoodwill : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BC6 RID: 23494 RVA: 0x001FB90C File Offset: 0x001F9B0C
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			List<OutcomeChance> outcomeChances = jobRitual.Ritual.outcomeEffect.def.outcomeChances;
			int positivityIndex = outcomeChances.MaxBy((OutcomeChance c) => c.positivityIndex).positivityIndex;
			int positivityIndex2 = (from c in outcomeChances
			where c.positivityIndex >= 0
			select c).MinBy((OutcomeChance c) => c.positivityIndex).positivityIndex;
			int num = RitualAttachableOutcomeEffectWorker_NearbyFactionGoodwill.GoodwillRange.Lerped((float)(outcome.positivityIndex - positivityIndex2) / (float)(positivityIndex - positivityIndex2));
			int tile = jobRitual.Map.Tile;
			float num2 = float.PositiveInfinity;
			Settlement settlement = null;
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				Settlement settlement2;
				if ((settlement2 = (worldObject as Settlement)) != null && worldObject.Faction.CanChangeGoodwillFor(Faction.OfPlayer, num))
				{
					float num3 = Find.WorldGrid.ApproxDistanceInTiles(tile, worldObject.Tile);
					if (num3 < num2)
					{
						num2 = num3;
						settlement = settlement2;
					}
				}
			}
			if (settlement != null && settlement.Faction.TryAffectGoodwillWith(Faction.OfPlayer, num, true, true, HistoryEventDefOf.RitualDone, null))
			{
				letterLookTargets = new LookTargets((letterLookTargets.targets ?? new List<GlobalTargetInfo>()).Concat(Gen.YieldSingle<GlobalTargetInfo>(settlement)));
				extraOutcomeDesc = this.def.letterInfoText.Formatted(num.Named("AMOUNT"), settlement.Faction.Named("FACTION"));
				return;
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x0400357C RID: 13692
		public static readonly IntRange GoodwillRange = new IntRange(10, 20);
	}
}
