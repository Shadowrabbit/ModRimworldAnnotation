﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000606 RID: 1542
	public class Pawn_PathFollower : IExposable
	{
		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x00107735 File Offset: 0x00105935
		public LocalTargetInfo Destination
		{
			get
			{
				return this.destination;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002C43 RID: 11331 RVA: 0x0010773D File Offset: 0x0010593D
		public bool Moving
		{
			get
			{
				return this.moving;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x00107745 File Offset: 0x00105945
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.WillCollideWithPawnOnNextPathCell();
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x0010775C File Offset: 0x0010595C
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

		// Token: 0x06002C46 RID: 11334 RVA: 0x0010781C File Offset: 0x00105A1C
		public Pawn_PathFollower(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x00107870 File Offset: 0x00105A70
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

		// Token: 0x06002C48 RID: 11336 RVA: 0x00107928 File Offset: 0x00105B28
		public void StartPath(LocalTargetInfo dest, PathEndMode peMode)
		{
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(this.pawn, dest.ToTargetInfo(this.pawn.Map), ref peMode);
			if (dest.HasThing && dest.ThingDestroyed)
			{
				Log.Error(this.pawn + " pathing to destroyed thing " + dest.Thing);
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
			if (!this.pawn.Map.reachability.CanReach(this.pawn.Position, dest, peMode, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)))
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
				Log.Error(this.pawn.LabelCap + " tried to path while downed. This should never happen. curJob=" + this.pawn.CurJob.ToStringSafe<Job>());
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

		// Token: 0x06002C49 RID: 11337 RVA: 0x00107B45 File Offset: 0x00105D45
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

		// Token: 0x06002C4A RID: 11338 RVA: 0x00107B7C File Offset: 0x00105D7C
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

		// Token: 0x06002C4B RID: 11339 RVA: 0x00107D57 File Offset: 0x00105F57
		public void TryResumePathingAfterLoading()
		{
			if (this.moving)
			{
				this.StartPath(this.destination, this.peMode);
			}
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x00107D73 File Offset: 0x00105F73
		public void Notify_Teleported_Int()
		{
			this.StopDead();
			this.ResetToCurrentPosition();
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x00107D81 File Offset: 0x00105F81
		public void ResetToCurrentPosition()
		{
			this.nextCell = this.pawn.Position;
			this.nextCellCostLeft = 0f;
			this.nextCellCostTotal = 1f;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x00107DAC File Offset: 0x00105FAC
		private bool PawnCanOccupy(IntVec3 c)
		{
			if (!c.WalkableBy(this.pawn.Map, this.pawn))
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

		// Token: 0x06002C4F RID: 11343 RVA: 0x00107E0C File Offset: 0x0010600C
		public Building BuildingBlockingNextPathCell()
		{
			Building edifice = this.nextCell.GetEdifice(this.pawn.Map);
			if (edifice != null && edifice.BlocksPawn(this.pawn))
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x00107E44 File Offset: 0x00106044
		public bool WillCollideWithPawnOnNextPathCell()
		{
			return this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x00107E52 File Offset: 0x00106052
		private bool IsNextCellWalkable()
		{
			return this.nextCell.WalkableBy(this.pawn.Map, this.pawn) && !this.WillCollideWithPawnAt(this.nextCell);
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x00107E85 File Offset: 0x00106085
		private bool WillCollideWithPawnAt(IntVec3 c)
		{
			return PawnUtility.ShouldCollideWithPawns(this.pawn) && PawnUtility.AnyPawnBlockingPathAt(c, this.pawn, false, false, false);
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x00107EA8 File Offset: 0x001060A8
		public Building_Door NextCellDoorToWaitForOrManuallyOpen()
		{
			Building_Door building_Door = this.pawn.Map.thingGrid.ThingAt<Building_Door>(this.nextCell);
			if (building_Door != null && building_Door.SlowsPawns && (!building_Door.Open || building_Door.TicksTillFullyOpened > 0) && building_Door.PawnCanOpen(this.pawn))
			{
				return building_Door;
			}
			return null;
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x00107F00 File Offset: 0x00106100
		private Pawn RopeeWithStretchedRopeAtNextPathCell()
		{
			List<Pawn> ropees = this.pawn.roping.Ropees;
			for (int i = 0; i < ropees.Count; i++)
			{
				Pawn pawn = ropees[i];
				if (!pawn.Position.InHorDistOf(this.nextCell, 8f))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x00107F55 File Offset: 0x00106155
		public void PatherDraw()
		{
			if (DebugViewSettings.drawPaths && this.curPath != null && Find.Selector.IsSelected(this.pawn))
			{
				this.curPath.DrawPath(this.pawn);
			}
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x00107F89 File Offset: 0x00106189
		public bool MovedRecently(int ticks)
		{
			return Find.TickManager.TicksGame - this.lastMovedTick <= ticks;
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x00107FA4 File Offset: 0x001061A4
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
						}));
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
				}));
			}
			return flag;
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x001080BB File Offset: 0x001062BB
		private void PatherArrived()
		{
			this.StopDead();
			if (this.pawn.jobs.curJob != null)
			{
				this.pawn.jobs.curDriver.Notify_PatherArrived();
			}
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x001080EA File Offset: 0x001062EA
		private void PatherFailed()
		{
			this.StopDead();
			this.pawn.jobs.curDriver.Notify_PatherFailed();
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x00108108 File Offset: 0x00106308
		private void TryEnterNextPathCell()
		{
			Building building = this.BuildingBlockingNextPathCell();
			if (building != null)
			{
				Building_Door building_Door = building as Building_Door;
				if (building_Door == null || !building_Door.FreePassage)
				{
					if ((this.pawn.CurJob != null && this.pawn.CurJob.canBashDoors) || this.pawn.HostileTo(building))
					{
						this.MakeBashBlockerJob(building);
						return;
					}
					this.PatherFailed();
					return;
				}
				else if (building.def.IsFence && this.pawn.def.race.FenceBlocked)
				{
					if (this.pawn.CurJob != null && this.pawn.CurJob.canBashFences)
					{
						this.MakeBashBlockerJob(building);
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
			Pawn pawn = this.RopeeWithStretchedRopeAtNextPathCell();
			if (pawn != null)
			{
				Stance_Cooldown stance_Cooldown2 = new Stance_Cooldown(60, pawn, null);
				stance_Cooldown2.neverAimWeapon = true;
				this.pawn.stances.SetStance(stance_Cooldown2);
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

		// Token: 0x06002C5B RID: 11355 RVA: 0x00108394 File Offset: 0x00106594
		private void MakeBashBlockerJob(Building blocker)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, blocker);
			job.expiryInterval = 300;
			this.pawn.jobs.StartJob(job, JobCondition.Incompletable, null, false, true, null, null, false, false);
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x001083E0 File Offset: 0x001065E0
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
				}));
				this.PatherFailed();
				return;
			}
			this.nextCell = this.curPath.ConsumeNextNode();
			if (!this.nextCell.WalkableBy(this.pawn.Map, this.pawn))
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" entering ",
					this.nextCell,
					" which is unwalkable."
				}));
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

		// Token: 0x06002C5D RID: 11357 RVA: 0x00108503 File Offset: 0x00106703
		private int CostToMoveIntoCell(IntVec3 c)
		{
			return Pawn_PathFollower.CostToMoveIntoCell(this.pawn, c);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x00108514 File Offset: 0x00106714
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
			num += pawn.Map.pathing.For(pawn).pathGrid.CalculatedCostAt(c, false, pawn.Position);
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

		// Token: 0x06002C5F RID: 11359 RVA: 0x00108644 File Offset: 0x00106844
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

		// Token: 0x06002C60 RID: 11360 RVA: 0x00108690 File Offset: 0x00106890
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

		// Token: 0x06002C61 RID: 11361 RVA: 0x00108778 File Offset: 0x00106978
		private PawnPath GenerateNewPath()
		{
			this.lastPathedTargetPosition = this.destination.Cell;
			return this.pawn.Map.pathFinder.FindPath(this.pawn.Position, this.destination, this.pawn, this.peMode, null);
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x001087C9 File Offset: 0x001069C9
		private bool AtDestinationPosition()
		{
			return this.pawn.CanReachImmediate(this.destination, this.peMode);
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x001087E4 File Offset: 0x001069E4
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
			PathingContext pc = this.pawn.Map.pathing.For(this.pawn);
			bool canBashFences = this.pawn.CurJob != null && this.pawn.CurJob.canBashFences;
			IntVec3 intVec = IntVec3.Invalid;
			int num3 = 0;
			while (num3 < 20 && num3 < this.curPath.NodesLeftCount)
			{
				IntVec3 intVec2 = this.curPath.Peek(num3);
				if (!intVec2.WalkableBy(this.pawn.Map, this.pawn))
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
				if (num3 != 0 && intVec2.AdjacentToDiagonal(intVec) && (PathFinder.BlocksDiagonalMovement(intVec2.x, intVec.z, pc, canBashFences) || PathFinder.BlocksDiagonalMovement(intVec.x, intVec2.z, pc, canBashFences)))
				{
					return true;
				}
				intVec = intVec2;
				num3++;
			}
			return false;
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x00108B3E File Offset: 0x00106D3E
		private bool BestPathHadPawnsInTheWayRecently()
		{
			return this.foundPathWhichCollidesWithPawns + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x00108B58 File Offset: 0x00106D58
		private bool BestPathHadDangerRecently()
		{
			return this.foundPathWithDanger + 240 > Find.TickManager.TicksGame;
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x00108B72 File Offset: 0x00106D72
		private bool FailedToFindCloseUnoccupiedCellRecently()
		{
			return this.failedToFindCloseUnoccupiedCellTicks + 100 > Find.TickManager.TicksGame;
		}

		// Token: 0x04001AF3 RID: 6899
		protected Pawn pawn;

		// Token: 0x04001AF4 RID: 6900
		private bool moving;

		// Token: 0x04001AF5 RID: 6901
		public IntVec3 nextCell;

		// Token: 0x04001AF6 RID: 6902
		private IntVec3 lastCell;

		// Token: 0x04001AF7 RID: 6903
		public float nextCellCostLeft;

		// Token: 0x04001AF8 RID: 6904
		public float nextCellCostTotal = 1f;

		// Token: 0x04001AF9 RID: 6905
		private int cellsUntilClamor;

		// Token: 0x04001AFA RID: 6906
		private int lastMovedTick = -999999;

		// Token: 0x04001AFB RID: 6907
		private LocalTargetInfo destination;

		// Token: 0x04001AFC RID: 6908
		private PathEndMode peMode;

		// Token: 0x04001AFD RID: 6909
		public PawnPath curPath;

		// Token: 0x04001AFE RID: 6910
		public IntVec3 lastPathedTargetPosition;

		// Token: 0x04001AFF RID: 6911
		private int foundPathWhichCollidesWithPawns = -999999;

		// Token: 0x04001B00 RID: 6912
		private int foundPathWithDanger = -999999;

		// Token: 0x04001B01 RID: 6913
		private int failedToFindCloseUnoccupiedCellTicks = -999999;

		// Token: 0x04001B02 RID: 6914
		private const int MaxMoveTicks = 450;

		// Token: 0x04001B03 RID: 6915
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x04001B04 RID: 6916
		private const float SnowReductionFromWalking = 0.001f;

		// Token: 0x04001B05 RID: 6917
		private const int ClamorCellsInterval = 12;

		// Token: 0x04001B06 RID: 6918
		private const int MinCostWalk = 50;

		// Token: 0x04001B07 RID: 6919
		private const int MinCostAmble = 60;

		// Token: 0x04001B08 RID: 6920
		private const float StaggerMoveSpeedFactor = 0.17f;

		// Token: 0x04001B09 RID: 6921
		private const int CheckForMovingCollidingPawnsIfCloserToTargetThanX = 30;

		// Token: 0x04001B0A RID: 6922
		private const int AttackBlockingHostilePawnAfterTicks = 180;

		// Token: 0x04001B0B RID: 6923
		private const int WaitForRopeeTicks = 60;

		// Token: 0x04001B0C RID: 6924
		private const float RopeLength = 8f;
	}
}
