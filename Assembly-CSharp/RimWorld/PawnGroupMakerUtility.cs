using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200142F RID: 5167
	public class PawnGroupMakerUtility
	{
		// Token: 0x06006F84 RID: 28548 RVA: 0x0004B654 File Offset: 0x00049854
		public static IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			if (parms.groupKind == null)
			{
				Log.Error("Tried to generate pawns with null pawn group kind def. parms=" + parms, false);
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms, false);
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
				}), false);
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
				}), false);
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

		// Token: 0x06006F85 RID: 28549 RVA: 0x0004B66B File Offset: 0x0004986B
		public static IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			if (parms.groupKind == null)
			{
				Log.Error("Tried to generate pawn kinds with null pawn group kind def. parms=" + parms, false);
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms, false);
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
				}), false);
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
				}), false);
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

		// Token: 0x06006F86 RID: 28550 RVA: 0x0022263C File Offset: 0x0022083C
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

		// Token: 0x06006F87 RID: 28551 RVA: 0x002226E0 File Offset: 0x002208E0
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
					if (pawnGenOption.Cost <= num2 && pawnGenOption.Cost <= num && (!groupParms.generateFightersOnly || pawnGenOption.kind.isFighter) && (groupParms.raidStrategy == null || groupParms.raidStrategy.Worker.CanUsePawnGenOption(pawnGenOption, list2)) && (!groupParms.dontUseSingleUseRocketLaunchers || pawnGenOption.kind.weaponTags == null || !pawnGenOption.kind.weaponTags.Contains("GunSingleUse")) && (!flag || !pawnGenOption.kind.factionLeader))
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
				}), false);
			}
			if (groupParms.seed != null)
			{
				Rand.PopState();
			}
			return list2;
		}

		// Token: 0x06006F88 RID: 28552 RVA: 0x002228F8 File Offset: 0x00220AF8
		public static float MaxPawnCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy, PawnGroupKindDef groupKind)
		{
			float num = faction.def.maxPawnCostPerTotalPointsCurve.Evaluate(totalPoints);
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, faction.def.MinPointsToGeneratePawnGroup(groupKind) * 1.2f);
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction, groupKind) * 1.2f);
			}
			return num;
		}

		// Token: 0x06006F89 RID: 28553 RVA: 0x00222960 File Offset: 0x00220B60
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

		// Token: 0x06006F8A RID: 28554 RVA: 0x002229D8 File Offset: 0x00220BD8
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
					fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat)
				}));
				Action<float> action = delegate(float points)
				{
					if (points < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))
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
				Log.Message(sb.ToString(), false);
			});
		}

		// Token: 0x06006F8B RID: 28555 RVA: 0x00222A58 File Offset: 0x00220C58
		public static bool TryGetRandomFactionForCombatPawnGroup(float points, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			return Find.FactionManager.AllFactions.Where(delegate(Faction f)
			{
				if ((allowHidden || !f.Hidden) && !f.temporary && (allowDefeated || !f.defeated) && (allowNonHumanlike || f.def.humanlikeFaction) && (allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null)
				{
					if (f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat) && (validator == null || validator(f)))
					{
						return points >= f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat);
					}
				}
				return false;
			}).ToList<Faction>().TryRandomElementByWeight((Faction f) => f.def.RaidCommonalityFromPoints(points), out faction);
		}

		// Token: 0x040049B4 RID: 18868
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
