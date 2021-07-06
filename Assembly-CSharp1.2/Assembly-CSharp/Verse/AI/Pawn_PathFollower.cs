﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A58 RID: 2648
	public class Pawn_PathFollower : IExposable
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x0002F2C7 File Offset: 0x0002D4C7
		public LocalTargetInfo Destination
		{
			get
			{
				return this.destination;
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06003ED9 RID: 16089 RVA: 0x0002F2CF File Offset: 0x0002D4CF
		public bool Moving
		{
			get
			{
				return this.moving;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06003EDA RID: 16090 RVA: 0x0002F2D7 File Offset: 0x0002D4D7
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.WillCollideWithPawnOnNextPathCell();
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x0017A81C File Offset: 0x00178A1C
		public IntVec3 LastPassableCellInPath
		{
			get
			{
				if (!this.Moving || this.curPath == null)
				{
					return IntVec3.Invalid;
				}
				if (!this.Destination.Cell.Impassable(this.pawn.Map))
				{
					return this.Destination.Cell;
				}
				List<IntVec3> nodesReversed = this.curPath.NodesReversed;
				for (int i = 0; i < nodesReversed.Count; i++)
				{
					if (!nodesReversed[i].Impassable(this.pawn.Map))
					{
						return nodesReversed[i];
					}
				}
				if (!this.pawn.Position.Impassable(this.pawn.Map))
				{
					return this.pawn.Position;
				}
				return IntVec3.Invalid;
			}
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0017A8DC File Offset: 0x00178ADC
		public Pawn_PathFollower(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0017A930 File Offset: 0x00178B30
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<IntVec3>(ref this.nextCell, "nextCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.nextCellCostLeft, "nextCellCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextCellCostTotal, "nextCellCostInitial", 0f, false);
			Scribe_Values.Look<PathEndMode>(ref this.peMode, "peMode", PathEndMode.None, false);
			Scribe_Values.Look<int>(ref this.cellsUntilClamor, "cellsUntilClamor", 0, false);
			Scribe_Values.Look<int>(ref this.lastMovedTick, "lastMovedTick", -999999, false);
			if (this.moving)
			{
				Scribe_TargetInfo.Look(ref this.destination, "destination");
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0017A9E8 File Offset: 0x00178BE8
		public void StartPath(LocalTargetInfo dest, PathEndMode peMode)
		{
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(this.pawn, dest.ToTargetInfo(this.pawn.Map), ref peMode);
			if (dest.HasThing && dest.ThingDestroyed)
			{
				Log.Error(this.pawn + " pathing to destroyed thing " + dest.Thing, false);
				this.PatherFailed();
				return;
			}
			if (!this.PawnCanOccupy(this.pawn.Position) && !this.TryRecoverFromUnwalkablePosition(true))
			{
				return;
			}
			if (this.moving && this.curPath != null && this.destination == dest && this.peMode == peMode)
			{
				return;
			}
			if (!this.pawn.Map.reachability.CanReach(this.pawn.Position, dest, peMode, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				this.PatherFailed();
				return;
			}
			this.peMode = peMode;
			this.destination = dest;
			if (!this.IsNextCellWalkable() || this.NextCellDoorToWaitForOrManuallyOpen() != null || this.nextCellCostLeft == this.nextCellCostTotal)
			{
				this.ResetToCurrentPosition();
			}
			PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation = this.pawn.Map.pawnDestinationReservationManager.MostRecentReservationFor(this.pawn);
			if (pawnDestinationReservation != null && ((this.destination.HasThing && pawnDestinationReservation.target != this.destination.Cell) || (pawnDestinationReservation.job != this.pawn.CurJob && pawnDestinationReservation.target != this.destination.Cell)))
			{
				this.pawn.Map.pawnDestinationReservationManager.ObsoleteAllClaimedBy(this.pawn);
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.pawn.Downed)
			{
				Log.Error(this.pawn.LabelCap + " tried to path while downed. This should never happen. curJob=" + this.pawn.CurJob.ToStringSafe<Job>(), false);
				this.PatherFailed();
				return;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = true;
			this.pawn.jobs.posture = PawnPosture.Standing;
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0002F2EC File Offset: 0x0002D4EC
		public void StopDead()
		{
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = false;
			this.nextCell = this.pawn.Position;
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0017AC08 File Offset: 0x00178E08
		public void PatherTick()
		{
			if (this.WillCollideWithPawnAt(this.pawn.Position))
			{
				if (!this.FailedToFindCloseUnoccupiedCellRecently())
				{
					IntVec3 intVec;
					if (CellFinder.TryFindBestPawnStandCell(this.pawn, out intVec, true) && intVec != this.pawn.Position)
					{
						this.pawn.Position = intVec;
						this.ResetToCurrentPosition();
						if (this.moving && this.TrySetNewPath())
						{
							this.TryEnterNextPathCell();
							return;
						}
					}
					else
					{
						this.failedToFindCloseUnoccupiedCellTicks = Find.TickManager.TicksGame;
					}
				}
				return;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return;
			}
			if (this.moving && this.WillCollideWithPawnOnNextPathCell())
			{
				this.nextCellCostLeft = this.nextCellCostTotal;
				if (((this.curPath != null && this.curPath.NodesLeftCount < 30) || PawnUtility.AnyPawnBlockingPathAt(this.nextCell, this.pawn, false, true, false)) && !this.BestPathHadPawnsInTheWayRecently() && this.TrySetNewPath())
				{
					this.ResetToCurrentPosition();
					this.TryEnterNextPathCell();
					return;
				}
				if (Find.TickManager.TicksGame - this.lastMovedTick >= 180)
				{
					Pawn pawn = PawnUtility.PawnBlockingPathAt(this.nextCell, this.pawn, false, false, false);
					if (pawn != null && this.pawn.HostileTo(pawn) && this.pawn.TryGetAttackVerb(pawn, false) != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn);
						job.maxNumMeleeAttacks = 1;
						job.expiryInterval = 300;
						this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false);
						return;
					}
				}
				return;
			}
			else
			{
				this.lastMovedTick = Find.TickManager.TicksGame;
				if (this.nextCellCostLeft > 0f)
				{
					this.nextCellCostLeft -= this.CostToPayThisTick();
					return;
				}
				if (this.moving)
				{
					this.TryEnterNextPathCell();
				}
				return;
			}
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0002F320 File Offset: 0x0002D520
		public void TryResumePathingAfterLoading()
		{
			if (this.moving)
			{
				this.StartPath(this.destination, this.peMode);
			}
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0002F33C File Offset: 0x0002D53C
		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x0002F34A File Offset: 0x0002D54A
		public void ResetToCurrentPosition()
		{
			this.nextCell = this.pawn.Position;
			this.nextCellCostLeft = 0f;
			this.nextCellCostTotal = 1f;
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0017ADE4 File Offset: 0x00178FE4
		private bool PawnCanOccupy(IntVec3 c)
		{
			if (!c.Walkable(this.pawn.Map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(this.pawn.Map);
			if (edifice != null)
			{
				Building_Door building_Door = edifice as Building_Door;
				if (building_Door != null && !building_Door.PawnCanOpen(this.pawn) && !building_Door.Open)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0017AE40 File Offset: 0x00179040
		public Building BuildingBlockingNextPathCell()
		{
			Building edifice = this.nextCell.GetEdifice(this.pawn.Map);
			if (edifice != null && edifice.BlocksPawn(this.pawn))
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0002F373 File Offset: 0x0002D573
		public bool WillCollideWithPawnOnNextPathCell()
		{
			return this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0002F381 File Offset: 0x0002D581
		private bool IsNextCellWalkable()
		{
			return this.nextCell.Walkable(this.pawn.Map) && !this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x0002F3AE File Offset: 0x0002D5AE
		private bool WillCollideWithPawnAt(IntVec3 c)
		{
			return PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false, false);
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x0017AE78 File Offset: 0x00179078
		public Building_Door NextCellDoorToWaitForOrManuallyOpen()
		{
			Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
			if (building_Door != null && building_Door.SlowsPawns && (!building_Door.Open || building_Door.TicksTillFullyOpened > 0) && building_Door.PawnCanOpen(this.pawn))
			{
				return building_Door;
			}
			return null;
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x0002F3CE File Offset: 0x0002D5CE
		public void PatherDraw()
		{
			if (DebugViewSettings.drawPaths && this.curPath != null && Find.Selector.IsSelected(this.pawn))
			{
				this.curPath.DrawPath(this.pawn);
			}
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x0002F402 File Offset: 0x0002D602
		public bool MovedRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastMovedTick <= ticks;
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0017AED0 File Offset: 0x001790D0
		public bool TryRecoverFromUnwalkablePosition(bool error = true)
		{
			bool flag = false;
			int i = 0;
			while (i < GenRadial.RadialPattern.Length)
			{
				IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[i];
				if (this.PawnCanOccupy(intVec))
				{
					if (intVec == this.pawn.Position)
					{
						return true;
					}
					if (error)
					{
						Log.Warning(string.Concat(new object[]
						{
							this.pawn,
							" on unwalkable cell ",
							this.pawn.Position,
							". Teleporting to ",
							intVec
						}), false);
					}
					this.pawn.Position = intVec;
					this.pawn.Notify_Teleported(true, false);
					flag = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (!flag)
			{
				this.pawn.Destroy(DestroyMode.Vanish);
				Log.Error(string.Concat(new object[]
				{
					this.pawn.ToStringSafe<Pawn>(),
					" on unwalkable cell ",
					this.pawn.Position,
					". Could not find walkable position nearby. Destroyed."
				}), false);
			}
			return flag;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0002F41B File Offset: 0x0002D61B
		private void PatherArrived()
		{
			this.StopDead();
			if (this.pawn.jobs.curJob != null)
			{
				this.pawn.jobs.curDriver.Notify_PatherArrived();
			}
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x0002F44A File Offset: 0x0002D64A
		private void PatherFailed()
		{
			this.StopDead();
			this.pawn.jobs.curDriver.Notify_PatherFailed();
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x0017AFEC File Offset: 0x001791EC
		private void TryEnterNextPathCell()
		{
			Building building = this.BuildingBlockingNextPathCell();
			if (building != null)
			{
				Building_Door building_Door = building as Building_Door;
				if (building_Door == null || !building_Door.FreePassage)
				{
					if ((this.pawn.CurJob != null && this.pawn.CurJob.canBash) || this.pawn.HostileTo(building))
					{
						Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, building);
						job.expiryInterval = 300;
						this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false);
						return;
					}
					this.PatherFailed();
					return;
				}
			}
			Building_Door building_Door2 = this.NextCellDoorToWaitForOrManuallyOpen();
			if (building_Door2 != null)
			{
				if (!building_Door2.Open)
				{
					building_Door2.StartManualOpenBy(this.pawn);
				}
				Stance_Cooldown stance_Cooldown = new Stance_Cooldown(building_Door2.TicksTillFullyOpened, building_Door2, null);
				stance_Cooldown.neverAimWeapon = true;
				this.pawn.stances.SetStance(stance_Cooldown);
				building_Door2.CheckFriendlyTouched(this.pawn);
				return;
			}
			this.lastCell = this.pawn.Position;
			this.pawn.Position = this.nextCell;
			if (this.pawn.RaceProps.Humanlike)
			{
				this.cellsUntilClamor--;
				if (this.cellsUntilClamor <= 0)
				{
					GenClamor.DoClamor(this.pawn, 7f, ClamorDefOf.Movement);
					this.cellsUntilClamor = 12;
				}
			}
			this.pawn.filth.Notify_EnteredNewCell();
			if (this.pawn.BodySize > 0.9f)
			{
				this.pawn.Map.snowGrid.AddDepth(this.pawn.Position, -0.001f);
			}
			Building_Door building_Door3 = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.lastCell);
			if (building_Door3 != null && !this.pawn.HostileTo(building_Door3))
			{
				building_Door3.CheckFriendlyTouched(this.pawn);
				if (!building_Door3.BlockedOpenMomentary && !building_Door3.HoldOpen && building_Door3.SlowsPawns && building_Door3.PawnCanOpen(this.pawn))
				{
					building_Door3.StartManualCloseBy(this.pawn);
					return;
				}
			}
			if (this.NeedNewPath() && !this.TrySetNewPath())
			{
				return;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			this.SetupMoveIntoNextCell();
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x0017B228 File Offset: 0x00179428
		private void SetupMoveIntoNextCell()
		{
			if (this.curPath.NodesLeftCount <= 1)
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" at ",
					this.pawn.Position,
					" ran out of path nodes while pathing to ",
					this.destination,
					"."
				}), false);
				this.PatherFailed();
				return;
			}
			this.nextCell = this.curPath.ConsumeNextNode();
			if (!this.nextCell.Walkable(this.pawn.Map))
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" entering ",
					this.nextCell,
					" which is unwalkable."
				}), false);
			}
			int num = this.CostToMoveIntoCell(this.nextCell);
			this.nextCellCostTotal = (float)num;
			this.nextCellCostLeft = (float)num;
			Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
			if (building_Door != null)
			{
				building_Door.Notify_PawnApproaching(this.pawn, num);
			}
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0002F467 File Offset: 0x0002D667
		private int CostToMoveIntoCell(IntVec3 c)
		{
			return Pawn_PathFollower.CostToMoveIntoCell(this.pawn, c);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0017B348 File Offset: 0x00179548
		private static int CostToMoveIntoCell(Pawn pawn, IntVec3 c)
		{
			int num;
			if (c.x == pawn.Position.x || c.z == pawn.Position.z)
			{
				num = pawn.TicksPerMoveCardinal;
			}
			else
			{
				num = pawn.TicksPerMoveDiagonal;
			}
			num += pawn.Map.pathGrid.CalculatedCostAt(c, false, pawn.Position);
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice != null)
			{
				num += (int)edifice.PathWalkCostFor(pawn);
			}
			if (num > 450)
			{
				num = 450;
			}
			if (pawn.CurJob != null)
			{
				Pawn locomotionUrgencySameAs = pawn.jobs.curDriver.locomotionUrgencySameAs;
				if (locomotionUrgencySameAs != null && locomotionUrgencySameAs != pawn && locomotionUrgencySameAs.Spawned)
				{
					int num2 = Pawn_PathFollower.CostToMoveIntoCell(locomotionUrgencySameAs, c);
					if (num < num2)
					{
						num = num2;
					}
				}
				else
				{
					switch (pawn.jobs.curJob.locomotionUrgency)
					{
					case LocomotionUrgency.Amble:
						num *= 3;
						if (num < 60)
						{
							num = 60;
						}
						break;
					case LocomotionUrgency.Walk:
						num *= 2;
						if (num < 50)
						{
							num = 50;
						}
						break;
					case LocomotionUrgency.Jog:
						num = num;
						break;
					case LocomotionUrgency.Sprint:
						num = Mathf.RoundToInt((float)num * 0.75f);
						break;
					}
				}
			}
			return Mathf.Max(num, 1);
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0017B46C File Offset: 0x0017966C
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (this.pawn.stances.Staggered)
			{
				num *= 0.17f;
			}
			if (num < this.nextCellCostTotal / 450f)
			{
				num = this.nextCellCostTotal / 450f;
			}
			return num;
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0017B4B8 File Offset: 0x001796B8
		private bool TrySetNewPath()
		{
			PawnPath pawnPath = this.GenerateNewPath();
			if (!pawnPath.Found)
			{
				this.PatherFailed();
				return false;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = pawnPath;
			int num = 0;
			while (num < 20 && num < this.curPath.NodesLeftCount)
			{
				IntVec3 c = this.curPath.Peek(num);
				if (PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false, false))
				{
					this.foundPathWhichCollidesWithPawns = Find.TickManager.TicksGame;
				}
				if (PawnUtility.KnownDangerAt(c, this.pawn.Map, this.pawn))
				{
					this.foundPathWithDanger = Find.TickManager.TicksGame;
				}
				if (this.foundPathWhichCollidesWithPawns == Find.TickManager.TicksGame && this.foundPathWithDanger == Find.TickManager.TicksGame)
				{
					break;
				}
				num++;
			}
			return true;
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x0017B5A0 File Offset: 0x001797A0
		private PawnPath GenerateNewPath()
		{
			this.lastPathedTargetPosition = this.destination.Cell;
			return this.pawn.Map.pathFinder.FindPath(this.pawn.Position, this.destination, this.pawn, this.peMode);
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x0002F475 File Offset: 0x0002D675
		private bool AtDestinationPosition()
		{
			return this.pawn.CanReachImmediate(this.destination, this.peMode);
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x0017B5F0 File Offset: 0x001797F0
		private bool NeedNewPath()
		{
			if (!this.destination.IsValid || this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			if (this.destination.HasThing && this.destination.Thing.Map != this.pawn.Map)
			{
				return true;
			}
			if ((this.pawn.Position.InHorDistOf(this.curPath.LastNode, 15f) || this.pawn.Position.InHorDistOf(this.destination.Cell, 15f)) && !ReachabilityImmediate.CanReachImmediate(this.curPath.LastNode, this.destination, this.pawn.Map, this.peMode, this.pawn))
			{
				return true;
			}
			if (this.curPath.UsedRegionHeuristics && this.curPath.NodesConsumedCount >= 75)
			{
				return true;
			}
			if (this.lastPathedTargetPosition != this.destination.Cell)
			{
				float num = (float)(this.pawn.Position - this.destination.Cell).LengthHorizontalSquared;
				float num2;
				if (num > 900f)
				{
					num2 = 10f;
				}
				else if (num > 289f)
				{
					num2 = 5f;
				}
				else if (num > 100f)
				{
					num2 = 3f;
				}
				else if (num > 49f)
				{
					num2 = 2f;
				}
				else
				{
					num2 = 0.5f;
				}
				if ((float)(this.lastPathedTargetPosition - this.destination.Cell).LengthHorizontalSquared > num2 * num2)
				{
					return true;
				}
			}
			bool flag = PawnUtility.ShouldCollideWithPawns(this.pawn);
			bool flag2 = this.curPath.NodesLeftCount < 30;
			IntVec3 intVec = IntVec3.Invalid;
			int num3 = 0;
			while (num3 < 20 && num3 < this.curPath.NodesLeftCount)
			{
				IntVec3 intVec2 = this.curPath.Peek(num3);
				if (!intVec2.Walkable(this.pawn.Map))
				{
					return true;
				}
				if (flag && !this.BestPathHadPawnsInTheWayRecently() && (PawnUtility.AnyPawnBlockingPathAt(intVec2, this.pawn, false, true, false) || (flag2 && PawnUtility.AnyPawnBlockingPathAt(intVec2, this.pawn, false, false, false))))
				{
					return true;
				}
				if (!this.BestPathHadDangerRecently() && PawnUtility.KnownDangerAt(intVec2, this.pawn.Map, this.pawn))
				{
					return true;
				}
				Building_Door building_Door = intVec2.GetEdifice(this.pawn.Map) as Building_Door;
				if (building_Door != null)
				{
					if (!building_Door.CanPhysicallyPass(this.pawn) && !this.pawn.HostileTo(building_Door))
					{
						return true;
					}
					if (building_Door.IsForbiddenToPass(this.pawn))
					{
						return true;
					}
				}
				if (num3 != 0 && intVec2.AdjacentToDiagonal(intVec) && (PathFinder.BlocksDiagonalMovement(intVec2.x, intVec.z, this.pawn.Map) || PathFinder.BlocksDiagonalMovement(intVec.x, intVec2.z, this.pawn.Map)))
				{
					return true;
				}
				intVec = intVec2;
				num3++;
			}
			return false;
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x0002F48E File Offset: 0x0002D68E
		private bool BestPathHadPawnsInTheWayRecently()
		{
			return this.foundPathWhichCollidesWithPawns + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0002F4A8 File Offset: 0x0002D6A8
		private bool BestPathHadDangerRecently()
		{
			return this.foundPathWithDanger + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0002F4C2 File Offset: 0x0002D6C2
		private bool FailedToFindCloseUnoccupiedCellRecently()
		{
			return this.failedToFindCloseUnoccupiedCellTicks + 100 > Find.TickManager.TicksGame;
		}

		// Token: 0x04002B4C RID: 11084
		protected Pawn pawn;

		// Token: 0x04002B4D RID: 11085
		private bool moving;

		// Token: 0x04002B4E RID: 11086
		public IntVec3 nextCell;

		// Token: 0x04002B4F RID: 11087
		private IntVec3 lastCell;

		// Token: 0x04002B50 RID: 11088
		public float nextCellCostLeft;

		// Token: 0x04002B51 RID: 11089
		public float nextCellCostTotal = 1f;

		// Token: 0x04002B52 RID: 11090
		private int cellsUntilClamor;

		// Token: 0x04002B53 RID: 11091
		private int lastMovedTick = -999999;

		// Token: 0x04002B54 RID: 11092
		private LocalTargetInfo destination;

		// Token: 0x04002B55 RID: 11093
		private PathEndMode peMode;

		// Token: 0x04002B56 RID: 11094
		public PawnPath curPath;

		// Token: 0x04002B57 RID: 11095
		public IntVec3 lastPathedTargetPosition;

		// Token: 0x04002B58 RID: 11096
		private int foundPathWhichCollidesWithPawns = -999999;

		// Token: 0x04002B59 RID: 11097
		private int foundPathWithDanger = -999999;

		// Token: 0x04002B5A RID: 11098
		private int failedToFindCloseUnoccupiedCellTicks = -999999;

		// Token: 0x04002B5B RID: 11099
		private const int MaxMoveTicks = 450;

		// Token: 0x04002B5C RID: 11100
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x04002B5D RID: 11101
		private const float SnowReductionFromWalking = 0.001f;

		// Token: 0x04002B5E RID: 11102
		private const int ClamorCellsInterval = 12;

		// Token: 0x04002B5F RID: 11103
		private const int MinCostWalk = 50;

		// Token: 0x04002B60 RID: 11104
		private const int MinCostAmble = 60;

		// Token: 0x04002B61 RID: 11105
		private const float StaggerMoveSpeedFactor = 0.17f;

		// Token: 0x04002B62 RID: 11106
		private const int CheckForMovingCollidingPawnsIfCloserToTargetThanX = 30;

		// Token: 0x04002B63 RID: 11107
		private const int AttackBlockingHostilePawnAfterTicks = 180;
	}
}
