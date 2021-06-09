using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020015AE RID: 5550
	public class VisitorGiftForPlayerUtility
	{
		// Token: 0x0600787D RID: 30845 RVA: 0x00051292 File Offset: 0x0004F492
		public static float ChanceToLeaveGift(Faction faction, Map map)
		{
			if (faction.IsPlayer)
			{
				return 0f;
			}
			return 0.25f * VisitorGiftForPlayerUtility.PlayerWealthChanceFactor(map) * VisitorGiftForPlayerUtility.FactionRelationsChanceFactor(faction);
		}

		// Token: 0x0600787E RID: 30846 RVA: 0x0024A414 File Offset: 0x00248614
		public static List<Thing> GenerateGifts(Faction faction, Map map)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(DiplomacyTuning.VisitorGiftTotalMarketValueRangeBase * DiplomacyTuning.VisitorGiftTotalMarketValueFactorFromPlayerWealthCurve.Evaluate(map.wealthWatcher.WealthTotal));
			return ThingSetMakerDefOf.VisitorGift.root.Generate(parms);
		}

		// Token: 0x0600787F RID: 30847 RVA: 0x000512B5 File Offset: 0x0004F4B5
		private static float PlayerWealthChanceFactor(Map map)
		{
			return DiplomacyTuning.VisitorGiftChanceFactorFromPlayerWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x000512CC File Offset: 0x0004F4CC
		private static float FactionRelationsChanceFactor(Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				return 0f;
			}
			return DiplomacyTuning.VisitorGiftChanceFactorFromGoodwillCurve.Evaluate((float)faction.PlayerGoodwill);
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x0024A464 File Offset: 0x00248664
		public static void GiveGift(List<Pawn> possibleGivers, Faction faction)
		{
			if (possibleGivers.NullOrEmpty<Pawn>())
			{
				return;
			}
			Pawn pawn = null;
			for (int i = 0; i < possibleGivers.Count; i++)
			{
				if (possibleGivers[i].RaceProps.Humanlike && possibleGivers[i].Faction == faction)
				{
					pawn = possibleGivers[i];
					break;
				}
			}
			if (pawn == null)
			{
				for (int j = 0; j < possibleGivers.Count; j++)
				{
					if (possibleGivers[j].Faction == faction)
					{
						pawn = possibleGivers[j];
						break;
					}
				}
			}
			if (pawn == null)
			{
				pawn = possibleGivers[0];
			}
			List<Thing> list = VisitorGiftForPlayerUtility.GenerateGifts(faction, pawn.Map);
			TargetInfo target = TargetInfo.Invalid;
			for (int k = 0; k < list.Count; k++)
			{
				if (GenPlace.TryPlaceThing(list[k], pawn.Position, pawn.Map, ThingPlaceMode.Near, null, null, default(Rot4)))
				{
					target = list[k];
				}
				else
				{
					list[k].Destroy(DestroyMode.Vanish);
				}
			}
			if (target.IsValid)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelVisitorsGaveGift".Translate(pawn.Faction.Name), "LetterVisitorsGaveGift".Translate(pawn.Faction.def.pawnsPlural, (from g in list
				select g.LabelCap).ToLineList("   -", false), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), LetterDefOf.PositiveEvent, target, faction, null, null, null);
			}
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x0024A614 File Offset: 0x00248814
		[DebugOutput]
		private static void VisitorGiftChance()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Current wealth factor (wealth=" + Find.CurrentMap.wealthWatcher.WealthTotal.ToString("F0") + "): ");
			stringBuilder.AppendLine(VisitorGiftForPlayerUtility.PlayerWealthChanceFactor(Find.CurrentMap).ToStringPercent());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Chance per faction:");
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && !faction.Hidden)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						faction.Name,
						" (",
						faction.PlayerGoodwill.ToStringWithSign(),
						", ",
						faction.PlayerRelationKind.GetLabel(),
						")"
					}));
					stringBuilder.Append(": " + VisitorGiftForPlayerUtility.ChanceToLeaveGift(faction, Find.CurrentMap).ToStringPercent());
					stringBuilder.AppendLine(" (rels factor: " + VisitorGiftForPlayerUtility.FactionRelationsChanceFactor(faction).ToStringPercent() + ")");
				}
			}
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				Dictionary<IIncidentTarget, int> dictionary;
				int[] array;
				List<Pair<IncidentDef, IncidentParms>> list;
				int num2;
				StorytellerUtility.DebugGetFutureIncidents(60, true, out dictionary, out array, out list, out num2, null, null, null, null);
				for (int j = 0; j < list.Count; j++)
				{
					if ((list[j].First == IncidentDefOf.VisitorGroup || list[j].First == IncidentDefOf.TraderCaravanArrival) && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(list[j].Second.faction ?? Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined), Find.CurrentMap)))
					{
						num++;
					}
				}
			}
			float num3 = (float)num / 6f;
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Calculated number of gifts received on average within the next 1 year");
			stringBuilder.AppendLine("(assuming current wealth and faction relations)");
			stringBuilder.Append("  = " + num3.ToString("0.##"));
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}
