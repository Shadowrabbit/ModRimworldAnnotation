using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1F RID: 3103
	public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
	{
		// Token: 0x060048D9 RID: 18649 RVA: 0x001815BF File Offset: 0x0017F7BF
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.HostileTo(Faction.OfPlayer) && (desperate || (float)GenDate.DaysPassed >= f.def.earliestRaidDays);
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x001815F6 File Offset: 0x0017F7F6
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!base.TryExecuteWorker(parms))
			{
				return false;
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			Find.StoryWatcher.statsRecord.numRaidsEnemy++;
			return true;
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0018162C File Offset: 0x0017F82C
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.faction != null)
			{
				return true;
			}
			float num = parms.points;
			if (num <= 0f)
			{
				num = 999999f;
			}
			return PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, false), true, true, true, true) || PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, true), true, true, true, true);
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x001816B7 File Offset: 0x0017F8B7
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				Log.Error("RaidEnemy is resolving raid points. They should always be set before initiating the incident.");
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x001816E4 File Offset: 0x0017F8E4
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			Map map = (Map)parms.target;
			Predicate<PawnsArrivalModeDef> <>9__2;
			RaidStrategyDef raidStrategy;
			DefDatabase<RaidStrategyDef>.AllDefs.Where(delegate(RaidStrategyDef d)
			{
				if (!d.Worker.CanUseWith(parms, groupKind))
				{
					return false;
				}
				if (parms.raidArrivalMode != null)
				{
					return true;
				}
				if (d.arriveModes != null)
				{
					List<PawnsArrivalModeDef> arriveModes = d.arriveModes;
					Predicate<PawnsArrivalModeDef> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((PawnsArrivalModeDef x) => x.Worker.CanUseWith(parms)));
					}
					return arriveModes.Any(predicate);
				}
				return false;
			}).TryRandomElementByWeight((RaidStrategyDef d) => d.Worker.SelectionWeight(map, parms.points), out raidStrategy);
			parms.raidStrategy = raidStrategy;
			if (parms.raidStrategy == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"No raid stategy found, defaulting to ImmediateAttack. Faction=",
					parms.faction.def.defName,
					", points=",
					parms.points,
					", groupKind=",
					groupKind,
					", parms=",
					parms
				}));
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x001817E9 File Offset: 0x0017F9E9
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy + ": " + parms.faction.Name;
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x0018180C File Offset: 0x0017FA0C
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural, parms.faction.Name.ApplyTag(parms.faction)).CapitalizeFirst();
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextEnemy;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER"));
			}
			return text;
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x001818EF File Offset: 0x0017FAEF
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.ThreatBig;
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x001818F6 File Offset: 0x0017FAF6
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidEnemy".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x00181930 File Offset: 0x0017FB30
		protected override void GenerateRaidLoot(IncidentParms parms, float raidLootPoints, List<Pawn> pawns)
		{
			if (parms.faction.def.raidLootMaker == null || !pawns.Any<Pawn>())
			{
				return;
			}
			raidLootPoints *= Find.Storyteller.difficulty.EffectiveRaidLootPointsFactor;
			float num = parms.faction.def.raidLootValueFromPointsCurve.Evaluate(raidLootPoints);
			if (parms.raidStrategy != null)
			{
				num *= parms.raidStrategy.raidLootValueFactor;
			}
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			parms2.totalMarketValueRange = new FloatRange?(new FloatRange(num, num));
			parms2.makingFaction = parms.faction;
			List<Thing> loot = parms.faction.def.raidLootMaker.root.Generate(parms2);
			new RaidLootDistributor(parms, pawns, loot).DistributeLoot();
		}
	}
}
