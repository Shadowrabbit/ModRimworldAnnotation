using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001194 RID: 4500
	public class IncidentWorker_CaravanDemand : IncidentWorker
	{
		// Token: 0x06006330 RID: 25392 RVA: 0x001EE290 File Offset: 0x001EC490
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		// Token: 0x06006331 RID: 25393 RVA: 0x001EE2C4 File Offset: 0x001EC4C4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.points *= IncidentWorker_CaravanDemand.IncidentPointsFactorRange.RandomInRange;
			Caravan caravan = (Caravan)parms.target;
			if (!PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out parms.faction, null, false, false, false, true))
			{
				return false;
			}
			List<ThingCount> demands = this.GenerateDemands(caravan);
			if (demands.NullOrEmpty<ThingCount>())
			{
				return false;
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms, false);
			defaultPawnGroupMakerParms.generateFightersOnly = true;
			defaultPawnGroupMakerParms.dontUseSingleUseRocketLaunchers = true;
			List<Pawn> attackers = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
			if (attackers.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Caravan demand incident couldn't generate any enemies even though min points have been checked. faction=",
					defaultPawnGroupMakerParms.faction,
					"(",
					(defaultPawnGroupMakerParms.faction != null) ? defaultPawnGroupMakerParms.faction.def.ToString() : "null",
					") parms=",
					parms
				}), false);
				return false;
			}
			CameraJumper.TryJumpAndSelect(caravan);
			DiaNode diaNode = new DiaNode(this.GenerateMessageText(parms.faction, attackers.Count, demands, caravan));
			DiaOption diaOption = new DiaOption("CaravanDemand_Give".Translate());
			diaOption.action = delegate()
			{
				this.ActionGive(caravan, demands, attackers);
			};
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			DiaOption diaOption2 = new DiaOption("CaravanDemand_Fight".Translate());
			diaOption2.action = delegate()
			{
				this.ActionFight(caravan, attackers);
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			TaggedString taggedString = "CaravanDemandTitle".Translate(parms.faction.Name);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, parms.faction, true, false, taggedString));
			Find.Archive.Add(new ArchivedDialog(diaNode.text, taggedString, parms.faction));
			return true;
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x001EE4F4 File Offset: 0x001EC6F4
		private List<ThingCount> GenerateDemands(Caravan caravan)
		{
			float num = 1.8f;
			float num2 = Rand.Value * num;
			if (num2 < 0.15f)
			{
				List<ThingCount> list = this.TryGenerateColonistOrPrisonerDemand(caravan);
				if (!list.NullOrEmpty<ThingCount>())
				{
					return list;
				}
			}
			if (num2 < 0.3f)
			{
				List<ThingCount> list2 = this.TryGenerateAnimalsDemand(caravan);
				if (!list2.NullOrEmpty<ThingCount>())
				{
					return list2;
				}
			}
			List<ThingCount> list3 = this.TryGenerateItemsDemand(caravan);
			if (!list3.NullOrEmpty<ThingCount>())
			{
				return list3;
			}
			if (Rand.Bool)
			{
				List<ThingCount> list4 = this.TryGenerateColonistOrPrisonerDemand(caravan);
				if (!list4.NullOrEmpty<ThingCount>())
				{
					return list4;
				}
				List<ThingCount> list5 = this.TryGenerateAnimalsDemand(caravan);
				if (!list5.NullOrEmpty<ThingCount>())
				{
					return list5;
				}
			}
			else
			{
				List<ThingCount> list6 = this.TryGenerateAnimalsDemand(caravan);
				if (!list6.NullOrEmpty<ThingCount>())
				{
					return list6;
				}
				List<ThingCount> list7 = this.TryGenerateColonistOrPrisonerDemand(caravan);
				if (!list7.NullOrEmpty<ThingCount>())
				{
					return list7;
				}
			}
			return null;
		}

		// Token: 0x06006333 RID: 25395 RVA: 0x001EE5B4 File Offset: 0x001EC7B4
		private List<ThingCount> TryGenerateColonistOrPrisonerDemand(Caravan caravan)
		{
			List<Pawn> list = new List<Pawn>();
			int num = 0;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				if (caravan.IsOwner(caravan.pawns[i]))
				{
					num++;
				}
			}
			if (num >= 2)
			{
				for (int j = 0; j < caravan.pawns.Count; j++)
				{
					if (caravan.IsOwner(caravan.pawns[j]))
					{
						list.Add(caravan.pawns[j]);
					}
				}
			}
			for (int k = 0; k < caravan.pawns.Count; k++)
			{
				if (caravan.pawns[k].IsPrisoner)
				{
					list.Add(caravan.pawns[k]);
				}
			}
			if (list.Any<Pawn>())
			{
				return new List<ThingCount>
				{
					new ThingCount(list.RandomElement<Pawn>(), 1)
				};
			}
			return null;
		}

		// Token: 0x06006334 RID: 25396 RVA: 0x001EE69C File Offset: 0x001EC89C
		private List<ThingCount> TryGenerateAnimalsDemand(Caravan caravan)
		{
			int num = 0;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				if (caravan.pawns[i].RaceProps.Animal)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			int count = Rand.RangeInclusive(1, (int)Mathf.Max((float)num * 0.6f, 1f));
			return (from x in (from x in caravan.pawns.InnerListForReading
			where x.RaceProps.Animal
			orderby x.MarketValue descending
			select x).Take(count)
			select new ThingCount(x, 1)).ToList<ThingCount>();
		}

		// Token: 0x06006335 RID: 25397 RVA: 0x001EE780 File Offset: 0x001EC980
		private List<ThingCount> TryGenerateItemsDemand(Caravan caravan)
		{
			List<ThingCount> list = new List<ThingCount>();
			List<Thing> list2 = new List<Thing>();
			list2.AddRange(caravan.PawnsListForReading.SelectMany((Pawn x) => ThingOwnerUtility.GetAllThingsRecursively(x, false)));
			list2.RemoveAll((Thing x) => x.MarketValue * (float)x.stackCount < 50f);
			list2.RemoveAll((Thing x) => x.ParentHolder is Pawn_ApparelTracker && x.MarketValue < 500f);
			float num = list2.Sum((Thing x) => x.MarketValue * (float)x.stackCount);
			float requestedCaravanValue = Mathf.Clamp(IncidentWorker_CaravanDemand.DemandAsPercentageOfCaravan.RandomInRange * num, 300f, 3500f);
			Func<Thing, bool> <>9__4;
			while (requestedCaravanValue > 50f)
			{
				IEnumerable<Thing> source = list2;
				Func<Thing, bool> predicate;
				if ((predicate = <>9__4) == null)
				{
					predicate = (<>9__4 = ((Thing x) => x.MarketValue * (float)x.stackCount <= requestedCaravanValue * 2f));
				}
				Thing thing;
				if (!source.Where(predicate).TryRandomElementByWeight((Thing x) => Mathf.Pow(x.MarketValue / x.GetStatValue(StatDefOf.Mass, true), 2f), out thing))
				{
					return null;
				}
				int num2 = Mathf.Clamp((int)(requestedCaravanValue / thing.MarketValue), 1, thing.stackCount);
				requestedCaravanValue -= thing.MarketValue * (float)num2;
				list.Add(new ThingCount(thing, num2));
				list2.Remove(thing);
			}
			return list;
		}

		// Token: 0x06006336 RID: 25398 RVA: 0x001EE91C File Offset: 0x001ECB1C
		private string GenerateMessageText(Faction enemyFaction, int attackerCount, List<ThingCount> demands, Caravan caravan)
		{
			return "CaravanDemand".Translate(caravan.Name, enemyFaction.Name, attackerCount, GenLabel.ThingsLabel(demands, "  - ", false), enemyFaction.def.pawnsPlural).CapitalizeFirst();
		}

		// Token: 0x06006337 RID: 25399 RVA: 0x001EE980 File Offset: 0x001ECB80
		private void TakeFromCaravan(Caravan caravan, List<ThingCount> demands, Faction enemyFaction)
		{
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < demands.Count; i++)
			{
				ThingCount thingCount = demands[i];
				if (thingCount.Thing is Pawn)
				{
					Pawn pawn = (Pawn)thingCount.Thing;
					caravan.RemovePawn(pawn);
					foreach (Thing thing in ThingOwnerUtility.GetAllThingsRecursively(pawn, false))
					{
						list.Add(thing);
						thing.holdingOwner.Take(thing);
					}
					if (pawn.RaceProps.Humanlike)
					{
						enemyFaction.kidnapped.Kidnap(pawn, null);
					}
					else if (!Find.WorldPawns.Contains(pawn))
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
				else
				{
					thingCount.Thing.SplitOff(thingCount.Count).Destroy(DestroyMode.Vanish);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!list[j].Destroyed)
				{
					CaravanInventoryUtility.GiveThing(caravan, list[j]);
				}
			}
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x001EEAB4 File Offset: 0x001ECCB4
		private void ActionGive(Caravan caravan, List<ThingCount> demands, List<Pawn> attackers)
		{
			this.TakeFromCaravan(caravan, demands, attackers[0].Faction);
			for (int i = 0; i < attackers.Count; i++)
			{
				Find.WorldPawns.PassToWorld(attackers[i], PawnDiscardDecideMode.Decide);
			}
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x001EEAF8 File Offset: 0x001ECCF8
		private void ActionFight(Caravan caravan, List<Pawn> attackers)
		{
			Faction enemyFaction = attackers[0].Faction;
			TaleRecorder.RecordTale(TaleDefOf.CaravanAmbushedByHumanlike, new object[]
			{
				caravan.RandomOwner()
			});
			LongEventHandler.QueueLongEvent(delegate()
			{
				Map map = CaravanIncidentUtility.SetupCaravanAttackMap(caravan, attackers, true);
				LordJob_AssaultColony lordJob_AssaultColony = new LordJob_AssaultColony(enemyFaction, true, false, false, false, true);
				if (lordJob_AssaultColony != null)
				{
					LordMaker.MakeNewLord(enemyFaction, lordJob_AssaultColony, map, attackers);
				}
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				CameraJumper.TryJump(attackers[0]);
			}, "GeneratingMapForNewEncounter", false, null, true);
		}

		// Token: 0x0400425F RID: 16991
		private static readonly FloatRange DemandAsPercentageOfCaravan = new FloatRange(0.05f, 0.2f);

		// Token: 0x04004260 RID: 16992
		private static readonly FloatRange IncidentPointsFactorRange = new FloatRange(1f, 1.7f);

		// Token: 0x04004261 RID: 16993
		private const float DemandAnimalsWeight = 0.15f;

		// Token: 0x04004262 RID: 16994
		private const float DemandColonistOrPrisonerWeight = 0.15f;

		// Token: 0x04004263 RID: 16995
		private const float DemandItemsWeight = 1.5f;

		// Token: 0x04004264 RID: 16996
		private const float MaxDemandedAnimalsPct = 0.6f;

		// Token: 0x04004265 RID: 16997
		private const float MinDemandedMarketValue = 300f;

		// Token: 0x04004266 RID: 16998
		private const float MaxDemandedMarketValue = 3500f;

		// Token: 0x04004267 RID: 16999
		private const float TrashMarketValueThreshold = 50f;

		// Token: 0x04004268 RID: 17000
		private const float IgnoreApparelMarketValueThreshold = 500f;
	}
}
