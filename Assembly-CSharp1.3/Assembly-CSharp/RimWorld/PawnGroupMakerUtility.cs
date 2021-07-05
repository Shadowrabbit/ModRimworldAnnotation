using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC6 RID: 3526
	public class PawnGroupMakerUtility
	{
		// Token: 0x06005197 RID: 20887 RVA: 0x001B8116 File Offset: 0x001B6316
		public static IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			if (parms.groupKind == null)
			{
				Log.Error("Tried to generate pawns with null pawn group kind def. parms=" + parms);
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms);
				yield break;
			}
			if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no any PawnGroupMakers."
				}));
				yield break;
			}
			PawnGroupMaker pawnGroupMaker;
			if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out pawnGroupMaker))
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no usable PawnGroupMakers for parms ",
					parms
				}));
				yield break;
			}
			foreach (Pawn pawn in pawnGroupMaker.GeneratePawns(parms, warnOnZeroResults))
			{
				yield return pawn;
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x001B812D File Offset: 0x001B632D
		public static IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			if (parms.groupKind == null)
			{
				Log.Error("Tried to generate pawn kinds with null pawn group kind def. parms=" + parms);
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms);
				yield break;
			}
			if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no any PawnGroupMakers."
				}));
				yield break;
			}
			PawnGroupMaker pawnGroupMaker;
			if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out pawnGroupMaker))
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no usable PawnGroupMakers for parms ",
					parms
				}));
				yield break;
			}
			foreach (PawnKindDef pawnKindDef in pawnGroupMaker.GeneratePawnKindsExample(parms))
			{
				yield return pawnKindDef;
			}
			IEnumerator<PawnKindDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x001B8140 File Offset: 0x001B6340
		private static bool TryGetRandomPawnGroupMaker(PawnGroupMakerParms parms, out PawnGroupMaker pawnGroupMaker)
		{
			if (parms.seed != null)
			{
				Rand.PushState(parms.seed.Value);
			}
			bool result = (from gm in parms.faction.def.pawnGroupMakers
			where gm.kindDef == parms.groupKind && gm.CanGenerateFrom(parms)
			select gm).TryRandomElementByWeight((PawnGroupMaker gm) => gm.commonality, out pawnGroupMaker);
			if (parms.seed != null)
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x001B81E4 File Offset: 0x001B63E4
		public static bool PawnGenOptionValid(PawnGenOption o, PawnGroupMakerParms groupParms)
		{
			return (!groupParms.generateFightersOnly || o.kind.isFighter) && (!groupParms.dontUseSingleUseRocketLaunchers || o.kind.weaponTags == null || !o.kind.weaponTags.Contains("GunSingleUse"));
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x001B8238 File Offset: 0x001B6438
		public static IEnumerable<PawnGenOption> ChoosePawnGenOptionsByPoints(float pointsTotal, List<PawnGenOption> options, PawnGroupMakerParms groupParms)
		{
			if (groupParms.seed != null)
			{
				Rand.PushState(groupParms.seed.Value);
			}
			float num = PawnGroupMakerUtility.MaxPawnCost(groupParms.faction, pointsTotal, groupParms.raidStrategy, groupParms.groupKind);
			List<PawnGenOption> list = new List<PawnGenOption>();
			List<PawnGenOption> list2 = new List<PawnGenOption>();
			float num2 = pointsTotal;
			bool flag = false;
			float highestCost = -1f;
			Func<PawnGenOption, float> <>9__0;
			for (;;)
			{
				list.Clear();
				for (int i = 0; i < options.Count; i++)
				{
					PawnGenOption pawnGenOption = options[i];
					if (pawnGenOption.Cost <= num2 && pawnGenOption.Cost <= num && PawnGroupMakerUtility.PawnGenOptionValid(pawnGenOption, groupParms) && (groupParms.raidStrategy == null || groupParms.raidStrategy.Worker.CanUsePawnGenOption(pointsTotal, pawnGenOption, list2)) && (!flag || !pawnGenOption.kind.factionLeader))
					{
						if (pawnGenOption.Cost > highestCost)
						{
							highestCost = pawnGenOption.Cost;
						}
						list.Add(pawnGenOption);
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				Func<PawnGenOption, float> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = ((PawnGenOption gr) => gr.selectionWeight * PawnGroupMakerUtility.PawnWeightFactorByMostExpensivePawnCostFractionCurve.Evaluate(gr.Cost / highestCost)));
				}
				Func<PawnGenOption, float> weightSelector = func;
				PawnGenOption pawnGenOption2 = list.RandomElementByWeight(weightSelector);
				list2.Add(pawnGenOption2);
				num2 -= pawnGenOption2.Cost;
				if (pawnGenOption2.kind.factionLeader)
				{
					flag = true;
				}
			}
			if (list2.Count == 1 && num2 > pointsTotal / 2f)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Used only ",
					pointsTotal - num2,
					" / ",
					pointsTotal,
					" points generating for ",
					groupParms.faction
				}));
			}
			if (groupParms.seed != null)
			{
				Rand.PopState();
			}
			return list2;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001B840C File Offset: 0x001B660C
		public static float MaxPawnCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy, PawnGroupKindDef groupKind)
		{
			float num = faction.def.maxPawnCostPerTotalPointsCurve.Evaluate(totalPoints);
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, faction.def.MinPointsToGeneratePawnGroup(groupKind, null) * 1.2f);
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction, groupKind) * 1.2f);
			}
			return num;
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001B8478 File Offset: 0x001B6678
		public static bool CanGenerateAnyNormalGroup(Faction faction, float points)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return false;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.faction = faction;
			pawnGroupMakerParms.points = points;
			for (int i = 0; i < faction.def.pawnGroupMakers.Count; i++)
			{
				PawnGroupMaker pawnGroupMaker = faction.def.pawnGroupMakers[i];
				if (pawnGroupMaker.kindDef == PawnGroupKindDefOf.Combat && pawnGroupMaker.CanGenerateFrom(pawnGroupMakerParms))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x001B84F0 File Offset: 0x001B66F0
		[DebugOutput]
		public static void PawnGroupsMade()
		{
			Dialog_DebugOptionListLister.ShowSimpleDebugMenu<Faction>(from fac in Find.FactionManager.AllFactions
			where !fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select fac, (Faction fac) => fac.Name + " (" + fac.def.defName + ")", delegate(Faction fac)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(string.Concat(new object[]
				{
					"FACTION: ",
					fac.Name,
					" (",
					fac.def.defName,
					") min=",
					fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null)
				}));
				Action<float> action = delegate(float points)
				{
					if (points < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
					{
						return;
					}
					PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
					pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
					pawnGroupMakerParms.tile = Find.CurrentMap.Tile;
					pawnGroupMakerParms.points = points;
					pawnGroupMakerParms.faction = fac;
					sb.AppendLine(string.Concat(new object[]
					{
						"Group with ",
						pawnGroupMakerParms.points,
						" points (max option cost: ",
						PawnGroupMakerUtility.MaxPawnCost(fac, points, RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat),
						")"
					}));
					float num = 0f;
					foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, false).OrderBy((Pawn pa) => pa.kindDef.combatPower))
					{
						string text;
						if (pawn.equipment.Primary != null)
						{
							text = pawn.equipment.Primary.Label;
						}
						else
						{
							text = "no-equipment";
						}
						Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
						string text2;
						if (apparel != null)
						{
							text2 = apparel.LabelCap;
						}
						else
						{
							text2 = "shirtless";
						}
						sb.AppendLine(string.Concat(new string[]
						{
							"  ",
							pawn.kindDef.combatPower.ToString("F0").PadRight(6),
							pawn.kindDef.defName,
							", ",
							text,
							", ",
							text2
						}));
						num += pawn.kindDef.combatPower;
					}
					sb.AppendLine("         totalCost " + num);
					sb.AppendLine();
				};
				foreach (float obj in DebugActionsUtility.PointsOptions(false))
				{
					action(obj);
				}
				Log.Message(sb.ToString());
			});
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x001B8570 File Offset: 0x001B6770
		public static bool TryGetRandomFactionForCombatPawnGroup(float points, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			return Find.FactionManager.AllFactions.Where(delegate(Faction f)
			{
				if ((allowHidden || !f.Hidden) && !f.temporary && (allowDefeated || !f.defeated) && (allowNonHumanlike || f.def.humanlikeFaction) && (allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null)
				{
					if (f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat) && (validator == null || validator(f)))
					{
						return points >= f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null);
					}
				}
				return false;
			}).ToList<Faction>().TryRandomElementByWeight((Faction f) => f.def.RaidCommonalityFromPoints(points), out faction);
		}

		// Token: 0x04003061 RID: 12385
		private static readonly SimpleCurve PawnWeightFactorByMostExpensivePawnCostFractionCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.3f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			}
		};
	}
}
