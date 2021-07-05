using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A5 RID: 2213
	public class LordToil_AssaultColonyBreaching : LordToil
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x001479CC File Offset: 0x00145BCC
		public LordToilData_AssaultColonyBreaching Data
		{
			get
			{
				if (this.data == null)
				{
					this.data = new LordToilData_AssaultColonyBreaching(this.lord);
				}
				return (LordToilData_AssaultColonyBreaching)this.data;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003A8D RID: 14989 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003A8E RID: 14990 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x001477E3 File Offset: 0x001459E3
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x00147A49 File Offset: 0x00145C49
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.Data.Reset();
			this.UpdateAllDuties();
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00147A5C File Offset: 0x00145C5C
		public override void Notify_BuildingSpawnedOnMap(Building b)
		{
			this.Data.breachingGrid.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00147A5C File Offset: 0x00145C5C
		public override void Notify_BuildingDespawnedOnMap(Building b)
		{
			this.Data.breachingGrid.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x00147A6F File Offset: 0x00145C6F
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x00147A8C File Offset: 0x00145C8C
		public override void UpdateAllDuties()
		{
			if (!ModLister.CheckIdeology("Breach raid"))
			{
				return;
			}
			if (!this.lord.ownedPawns.Any<Pawn>())
			{
				return;
			}
			if (!this.Data.breachDest.IsValid)
			{
				this.Data.Reset();
				this.Data.preferMelee = Rand.Chance(0.5f);
				this.Data.breachStart = this.lord.ownedPawns[0].PositionHeld;
				this.Data.breachDest = GenAI.RandomRaidDest(this.Data.breachStart, base.Map);
				int breachRadius = Mathf.RoundToInt(LordToil_AssaultColonyBreaching.BreachRadiusFromNumRaiders.Evaluate((float)this.lord.ownedPawns.Count));
				int walkMargin = Mathf.RoundToInt(LordToil_AssaultColonyBreaching.WalkMarginFromNumRaiders.Evaluate((float)this.lord.ownedPawns.Count));
				this.Data.breachingGrid.CreateBreachPath(this.Data.breachStart, this.Data.breachDest, breachRadius, walkMargin, this.useAvoidGrid);
			}
			this.pawnsRangedDestructive.Clear();
			this.pawnsMeleeDestructive.Clear();
			this.pawnsRangedGeneral.Clear();
			this.pawnSoloAttackers.Clear();
			this.pawnsEscort.Clear();
			this.pawnsLost.Clear();
			this.Data.maxRange = 12f;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.CanReach(this.Data.breachStart, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					this.pawnsLost.Add(pawn);
				}
				else
				{
					Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
					if (verb == null)
					{
						this.pawnsEscort.Add(pawn);
					}
					else if (BreachingUtility.IsSoloAttackVerb(verb))
					{
						this.pawnSoloAttackers.Add(pawn);
					}
					else if (verb.verbProps.ai_IsBuildingDestroyer)
					{
						if (verb.IsMeleeAttack)
						{
							this.pawnsMeleeDestructive.Add(pawn);
						}
						else
						{
							this.pawnsRangedDestructive.Add(pawn);
							this.Data.maxRange = Math.Min(this.Data.maxRange, verb.verbProps.range);
						}
					}
					else if (verb.IsMeleeAttack)
					{
						this.pawnsEscort.Add(pawn);
					}
					else
					{
						this.pawnsRangedGeneral.Add(pawn);
					}
				}
			}
			bool flag = this.pawnsMeleeDestructive.Any<Pawn>();
			bool flag2 = this.pawnsRangedDestructive.Any<Pawn>();
			if (flag && (!flag2 || this.Data.preferMelee))
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsMeleeDestructive, this.pawnSoloAttackers, this.pawnsRangedDestructive, this.pawnsRangedGeneral, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			if (flag2)
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsRangedDestructive, this.pawnSoloAttackers, this.pawnsRangedGeneral, this.pawnsMeleeDestructive, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			if (this.pawnsRangedGeneral.Any<Pawn>())
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsRangedGeneral, this.pawnSoloAttackers, this.pawnsMeleeDestructive, this.pawnsRangedDestructive, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsMeleeDestructive);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsRangedDestructive);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsRangedGeneral);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnSoloAttackers);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsEscort);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x00147E3C File Offset: 0x0014603C
		private static void BalanceAndSetDuties(IntVec3 breachDest, List<Pawn> breachers, List<Pawn> soloAttackers, List<Pawn> escorts1, List<Pawn> escorts2, List<Pawn> escorts3)
		{
			if (!escorts1.Any<Pawn>() && !escorts2.Any<Pawn>() && !escorts3.Any<Pawn>())
			{
				if (soloAttackers.Any<Pawn>())
				{
					escorts3.AddRange(soloAttackers);
					soloAttackers.Clear();
				}
				else if (breachers.Count > 1)
				{
					Pawn item = breachers.First<Pawn>();
					breachers.Remove(item);
					escorts3.Add(item);
				}
			}
			LordToil_AssaultColonyBreaching.SetBreachDuty(breachers, breachDest);
			LordToil_AssaultColonyBreaching.SetSoloAttackDuty(soloAttackers, breachDest);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts1, breachers);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts2, breachers);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts3, breachers);
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x00147EC0 File Offset: 0x001460C0
		private static void SetBackupDuty(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				pawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
			}
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x00147EFC File Offset: 0x001460FC
		private static void SetEscortDuty(List<Pawn> escorts, List<Pawn> targets)
		{
			for (int i = 0; i < escorts.Count; i++)
			{
				Pawn pawn = escorts[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Escort, targets.RandomElement<Pawn>(), BreachingUtility.EscortRadius(pawn));
			}
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x00147F48 File Offset: 0x00146148
		private static void SetBreachDuty(List<Pawn> breachers, IntVec3 breachDest)
		{
			for (int i = 0; i < breachers.Count; i++)
			{
				breachers[i].mindState.duty = new PawnDuty(DutyDefOf.Breaching, breachDest, -1f);
			}
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x00147F8C File Offset: 0x0014618C
		private static void SetSoloAttackDuty(List<Pawn> breachers, IntVec3 breachDest)
		{
			for (int i = 0; i < breachers.Count; i++)
			{
				breachers[i].mindState.duty = new PawnDuty(DutyDefOf.Breaching, breachDest, -1f);
			}
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x00147FD0 File Offset: 0x001461D0
		public void UpdateCurrentBreachTarget()
		{
			if (this.Data.currentTarget != null && this.Data.currentTarget.Destroyed)
			{
				this.Data.currentTarget = null;
			}
			if (this.Data.soloAttacker != null && BreachingUtility.FindVerbToUseForBreaching(this.Data.soloAttacker) == null)
			{
				this.Data.currentTarget = null;
			}
			if (this.Data.currentTarget == null)
			{
				this.Data.currentTarget = this.Data.breachingGrid.FindBuildingToBreach();
				this.Data.soloAttacker = null;
				if (this.pawnSoloAttackers.Any<Pawn>() && BreachingUtility.CanSoloAttackTargetBuilding(this.Data.currentTarget) && Rand.Chance(0.2f))
				{
					Pawn pawn = this.pawnSoloAttackers.RandomElement<Pawn>();
					if (BreachingUtility.FindVerbToUseForBreaching(pawn) != null)
					{
						this.Data.soloAttacker = pawn;
					}
				}
			}
		}

		// Token: 0x04002006 RID: 8198
		private const int UpdateIntervalTicks = 300;

		// Token: 0x04002007 RID: 8199
		private const float PreferMeleeChance = 0.5f;

		// Token: 0x04002008 RID: 8200
		private const float UseSoloAttackOnTargetChance = 0.2f;

		// Token: 0x04002009 RID: 8201
		public const float MaxRangeForShooters = 12f;

		// Token: 0x0400200A RID: 8202
		private static readonly SimpleCurve BreachRadiusFromNumRaiders = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2f),
				true
			}
		};

		// Token: 0x0400200B RID: 8203
		private static readonly SimpleCurve WalkMarginFromNumRaiders = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(60f, 4f),
				true
			}
		};

		// Token: 0x0400200C RID: 8204
		private List<Pawn> pawnsRangedDestructive = new List<Pawn>();

		// Token: 0x0400200D RID: 8205
		private List<Pawn> pawnsMeleeDestructive = new List<Pawn>();

		// Token: 0x0400200E RID: 8206
		private List<Pawn> pawnsRangedGeneral = new List<Pawn>();

		// Token: 0x0400200F RID: 8207
		private List<Pawn> pawnSoloAttackers = new List<Pawn>();

		// Token: 0x04002010 RID: 8208
		private List<Pawn> pawnsEscort = new List<Pawn>();

		// Token: 0x04002011 RID: 8209
		private List<Pawn> pawnsLost = new List<Pawn>();
	}
}
