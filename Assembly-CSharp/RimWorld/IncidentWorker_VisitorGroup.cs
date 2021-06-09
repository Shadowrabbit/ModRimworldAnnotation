using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001204 RID: 4612
	public class IncidentWorker_VisitorGroup : IncidentWorker_NeutralGroup
	{
		// Token: 0x060064DF RID: 25823 RVA: 0x001F46EC File Offset: 0x001F28EC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!base.TryResolveParms(parms))
			{
				return false;
			}
			List<Pawn> list = base.SpawnPawns(parms);
			if (list.Count == 0)
			{
				return false;
			}
			IntVec3 chillSpot;
			RCellFinder.TryFindRandomSpotJustOutsideColony(list[0], out chillSpot);
			LordJob_VisitColony lordJob = new LordJob_VisitColony(parms.faction, chillSpot);
			LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
			bool flag = false;
			if (Rand.Value < 0.75f)
			{
				flag = this.TryConvertOnePawnToSmallTrader(list, parms.faction, map);
			}
			Pawn pawn = list.Find((Pawn x) => parms.faction.leader == x);
			TaggedString baseLetterLabel;
			TaggedString baseLetterText;
			if (list.Count == 1)
			{
				TaggedString value = flag ? ("\n\n" + "SingleVisitorArrivesTraderInfo".Translate(list[0].Named("PAWN")).AdjustedFor(list[0], "PAWN", true)) : "";
				TaggedString value2 = (pawn != null) ? ("\n\n" + "SingleVisitorArrivesLeaderInfo".Translate(list[0].Named("PAWN")).AdjustedFor(list[0], "PAWN", true)) : "";
				baseLetterLabel = "LetterLabelSingleVisitorArrives".Translate();
				baseLetterText = "SingleVisitorArrives".Translate(list[0].story.Title, parms.faction.NameColored, list[0].Name.ToStringFull, value, value2, list[0].Named("PAWN")).AdjustedFor(list[0], "PAWN", true);
			}
			else
			{
				TaggedString value3 = flag ? ("\n\n" + "GroupVisitorsArriveTraderInfo".Translate()) : TaggedString.Empty;
				TaggedString value4 = (pawn != null) ? ("\n\n" + "GroupVisitorsArriveLeaderInfo".Translate(pawn.LabelShort, pawn)) : TaggedString.Empty;
				baseLetterLabel = "LetterLabelGroupVisitorsArrive".Translate();
				baseLetterText = "GroupVisitorsArrive".Translate(parms.faction.NameColored, value3, value4);
			}
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref baseLetterLabel, ref baseLetterText, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, LetterDefOf.NeutralEvent, parms, list[0], Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x000451A5 File Offset: 0x000433A5
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			if (parms.points >= 0f)
			{
				return;
			}
			parms.points = Rand.ByCurve(IncidentWorker_VisitorGroup.PointsCurve);
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x001F49CC File Offset: 0x001F2BCC
		private bool TryConvertOnePawnToSmallTrader(List<Pawn> pawns, Faction faction, Map map)
		{
			if (faction.def.visitorTraderKinds.NullOrEmpty<TraderKindDef>())
			{
				return false;
			}
			Pawn pawn = pawns.RandomElement<Pawn>();
			Lord lord = pawn.GetLord();
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			TraderKindDef traderKindDef = faction.def.visitorTraderKinds.RandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality);
			pawn.trader.traderKind = traderKindDef;
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = traderKindDef;
			parms.tile = new int?(map.Tile);
			parms.makingFaction = faction;
			foreach (Thing thing in ThingSetMakerDefOf.TraderStock.root.Generate(parms))
			{
				Pawn pawn2 = thing as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.Faction != pawn.Faction)
					{
						pawn2.SetFaction(pawn.Faction, null);
					}
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(pawn.Position, map, 5, null);
					GenSpawn.Spawn(pawn2, loc, map, WipeMode.Vanish);
					lord.AddPawn(pawn2);
				}
				else if (!pawn.inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			PawnInventoryGenerator.GiveRandomFood(pawn);
			return true;
		}

		// Token: 0x04004329 RID: 17193
		private const float TraderChance = 0.75f;

		// Token: 0x0400432A RID: 17194
		private static readonly SimpleCurve PointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(45f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(200f, 0.25f),
				true
			},
			{
				new CurvePoint(300f, 0.1f),
				true
			},
			{
				new CurvePoint(500f, 0f),
				true
			}
		};
	}
}
