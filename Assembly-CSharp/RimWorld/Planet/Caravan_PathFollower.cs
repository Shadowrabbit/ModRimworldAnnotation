using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020D5 RID: 8405
	public class Caravan_PathFollower : IExposable
	{
		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x0600B28F RID: 45711 RVA: 0x0007417B File Offset: 0x0007237B
		public int Destination
		{
			get
			{
				return this.destTile;
			}
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x0600B290 RID: 45712 RVA: 0x00074183 File Offset: 0x00072383
		public bool Moving
		{
			get
			{
				return this.moving && this.caravan.Spawned;
			}
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x0600B291 RID: 45713 RVA: 0x0007419A File Offset: 0x0007239A
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.Paused && !this.caravan.CantMove;
			}
		}

		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x0600B292 RID: 45714 RVA: 0x000741BC File Offset: 0x000723BC
		public CaravanArrivalAction ArrivalAction
		{
			get
			{
				if (!this.Moving)
				{
					return null;
				}
				return this.arrivalAction;
			}
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x0600B293 RID: 45715 RVA: 0x000741CE File Offset: 0x000723CE
		// (set) Token: 0x0600B294 RID: 45716 RVA: 0x0033B948 File Offset: 0x00339B48
		public bool Paused
		{
			get
			{
				return this.Moving && this.paused;
			}
			set
			{
				if (value == this.paused)
				{
					return;
				}
				if (!value)
				{
					this.paused = false;
				}
				else if (!this.Moving)
				{
					Log.Error("Tried to pause caravan movement of " + this.caravan.ToStringSafe<Caravan>() + " but it's not moving.", false);
				}
				else
				{
					this.paused = true;
				}
				this.caravan.Notify_DestinationOrPauseStatusChanged();
			}
		}

		// Token: 0x0600B295 RID: 45717 RVA: 0x000741E0 File Offset: 0x000723E0
		public Caravan_PathFollower(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B296 RID: 45718 RVA: 0x0033B9A8 File Offset: 0x00339BA8
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.moving, "moving", true, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", 0, false);
			Scribe_Values.Look<int>(ref this.previousTileForDrawingIfInDoubt, "previousTileForDrawingIfInDoubt", 0, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeft, "nextTileCostLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.nextTileCostTotal, "nextTileCostTotal", 0f, false);
			Scribe_Values.Look<int>(ref this.destTile, "destTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && Current.ProgramState != ProgramState.Entry && this.moving && !this.StartPath(this.destTile, this.arrivalAction, true, false))
			{
				this.StopDead();
			}
		}

		// Token: 0x0600B297 RID: 45719 RVA: 0x0033BA84 File Offset: 0x00339C84
		public bool StartPath(int destTile, CaravanArrivalAction arrivalAction, bool repathImmediately = false, bool resetPauseStatus = true)
		{
			this.caravan.autoJoinable = false;
			if (resetPauseStatus)
			{
				this.paused = false;
			}
			if (arrivalAction != null && !arrivalAction.StillValid(this.caravan, destTile))
			{
				return false;
			}
			if (!this.IsPassable(this.caravan.Tile) && !this.TryRecoverFromUnwalkablePosition())
			{
				return false;
			}
			if (this.moving && this.curPath != null && this.destTile == destTile)
			{
				this.arrivalAction = arrivalAction;
				return true;
			}
			if (!this.caravan.CanReach(destTile))
			{
				this.PatherFailed();
				return false;
			}
			this.destTile = destTile;
			this.arrivalAction = arrivalAction;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
			if (this.nextTile < 0 || !this.IsNextTilePassable())
			{
				this.nextTile = this.caravan.Tile;
				this.nextTileCostLeft = 0f;
				this.previousTileForDrawingIfInDoubt = -1;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return true;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = true;
			if (repathImmediately && this.TrySetNewPath() && this.nextTileCostLeft <= 0f && this.moving)
			{
				this.TryEnterNextPathTile();
			}
			return true;
		}

		// Token: 0x0600B298 RID: 45720 RVA: 0x0033BBBC File Offset: 0x00339DBC
		public void StopDead()
		{
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = null;
			this.moving = false;
			this.paused = false;
			this.nextTile = this.caravan.Tile;
			this.previousTileForDrawingIfInDoubt = -1;
			this.arrivalAction = null;
			this.nextTileCostLeft = 0f;
			this.caravan.Notify_DestinationOrPauseStatusChanged();
		}

		// Token: 0x0600B299 RID: 45721 RVA: 0x0033BC28 File Offset: 0x00339E28
		public void PatherTick()
		{
			if (this.moving && this.arrivalAction != null && !this.arrivalAction.StillValid(this.caravan, this.Destination))
			{
				string failMessage = this.arrivalAction.StillValid(this.caravan, this.Destination).FailMessage;
				Messages.Message("MessageCaravanArrivalActionNoLongerValid".Translate(this.caravan.Name).CapitalizeFirst() + ((failMessage != null) ? (" " + failMessage) : ""), this.caravan, MessageTypeDefOf.NegativeEvent, true);
				this.StopDead();
			}
			if (this.caravan.CantMove || this.paused)
			{
				return;
			}
			if (this.nextTileCostLeft > 0f)
			{
				this.nextTileCostLeft -= this.CostToPayThisTick();
				return;
			}
			if (this.moving)
			{
				this.TryEnterNextPathTile();
			}
		}

		// Token: 0x0600B29A RID: 45722 RVA: 0x00074208 File Offset: 0x00072408
		public void Notify_Teleported_Int()
		{
			this.StopDead();
		}

		// Token: 0x0600B29B RID: 45723 RVA: 0x00072B44 File Offset: 0x00070D44
		private bool IsPassable(int tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x0600B29C RID: 45724 RVA: 0x00074210 File Offset: 0x00072410
		public bool IsNextTilePassable()
		{
			return this.IsPassable(this.nextTile);
		}

		// Token: 0x0600B29D RID: 45725 RVA: 0x0033BD2C File Offset: 0x00339F2C
		private bool TryRecoverFromUnwalkablePosition()
		{
			int num;
			if (GenWorldClosest.TryFindClosestTile(this.caravan.Tile, (int t) => this.IsPassable(t), out num, 2147483647, true))
			{
				Log.Warning(string.Concat(new object[]
				{
					this.caravan,
					" on unwalkable tile ",
					this.caravan.Tile,
					". Teleporting to ",
					num
				}), false);
				this.caravan.Tile = num;
				this.caravan.Notify_Teleported();
				return true;
			}
			Log.Error(string.Concat(new object[]
			{
				this.caravan,
				" on unwalkable tile ",
				this.caravan.Tile,
				". Could not find walkable position nearby. Removed."
			}), false);
			this.caravan.Destroy();
			return false;
		}

		// Token: 0x0600B29E RID: 45726 RVA: 0x0033BE08 File Offset: 0x0033A008
		private void PatherArrived()
		{
			CaravanArrivalAction caravanArrivalAction = this.arrivalAction;
			this.StopDead();
			if (caravanArrivalAction != null && caravanArrivalAction.StillValid(this.caravan, this.caravan.Tile))
			{
				caravanArrivalAction.Arrived(this.caravan);
				return;
			}
			if (this.caravan.IsPlayerControlled && !this.caravan.VisibleToCameraNow())
			{
				Messages.Message("MessageCaravanArrivedAtDestination".Translate(this.caravan.Label), this.caravan, MessageTypeDefOf.TaskCompletion, true);
			}
		}

		// Token: 0x0600B29F RID: 45727 RVA: 0x00074208 File Offset: 0x00072408
		private void PatherFailed()
		{
			this.StopDead();
		}

		// Token: 0x0600B2A0 RID: 45728 RVA: 0x0033BEA0 File Offset: 0x0033A0A0
		private void TryEnterNextPathTile()
		{
			if (!this.IsNextTilePassable())
			{
				this.PatherFailed();
				return;
			}
			this.caravan.Tile = this.nextTile;
			if (this.NeedNewPath() && !this.TrySetNewPath())
			{
				return;
			}
			if (this.AtDestinationPosition())
			{
				this.PatherArrived();
				return;
			}
			if (this.curPath.NodesLeftCount == 0)
			{
				Log.Error(this.caravan + " ran out of path nodes. Force-arriving.", false);
				this.PatherArrived();
				return;
			}
			this.SetupMoveIntoNextTile();
		}

		// Token: 0x0600B2A1 RID: 45729 RVA: 0x0033BF20 File Offset: 0x0033A120
		private void SetupMoveIntoNextTile()
		{
			if (this.curPath.NodesLeftCount < 2)
			{
				Log.Error(string.Concat(new object[]
				{
					this.caravan,
					" at ",
					this.caravan.Tile,
					" ran out of path nodes while pathing to ",
					this.destTile,
					"."
				}), false);
				this.PatherFailed();
				return;
			}
			this.nextTile = this.curPath.ConsumeNextNode();
			this.previousTileForDrawingIfInDoubt = -1;
			if (Find.World.Impassable(this.nextTile))
			{
				Log.Error(string.Concat(new object[]
				{
					this.caravan,
					" entering ",
					this.nextTile,
					" which is unwalkable."
				}), false);
			}
			int num = this.CostToMove(this.caravan.Tile, this.nextTile);
			this.nextTileCostTotal = (float)num;
			this.nextTileCostLeft = (float)num;
		}

		// Token: 0x0600B2A2 RID: 45730 RVA: 0x0033C020 File Offset: 0x0033A220
		private int CostToMove(int start, int end)
		{
			return Caravan_PathFollower.CostToMove(this.caravan, start, end, null);
		}

		// Token: 0x0600B2A3 RID: 45731 RVA: 0x0007421E File Offset: 0x0007241E
		public static int CostToMove(Caravan caravan, int start, int end, int? ticksAbs = null)
		{
			return Caravan_PathFollower.CostToMove(caravan.TicksPerMove, start, end, ticksAbs, false, null, null);
		}

		// Token: 0x0600B2A4 RID: 45732 RVA: 0x0033C044 File Offset: 0x0033A244
		public static int CostToMove(int caravanTicksPerMove, int start, int end, int? ticksAbs = null, bool perceivedStatic = false, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null)
		{
			if (start == end)
			{
				return 0;
			}
			if (explanation != null)
			{
				explanation.Append(caravanTicksPerMoveExplanation);
				explanation.AppendLine();
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float num;
			if (perceivedStatic && explanation == null)
			{
				num = Find.WorldPathGrid.PerceivedMovementDifficultyAt(end);
			}
			else
			{
				num = WorldPathGrid.CalculatedMovementDifficultyAt(end, perceivedStatic, ticksAbs, stringBuilder);
			}
			float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(start, end, stringBuilder);
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.Append("TileMovementDifficulty".Translate() + ":");
				explanation.AppendLine();
				explanation.Append(stringBuilder.ToString().Indented("  "));
				explanation.AppendLine();
				explanation.Append("  = " + (num * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			int num2 = (int)((float)caravanTicksPerMove * num * roadMovementDifficultyMultiplier);
			num2 = Mathf.Clamp(num2, 1, 30000);
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("FinalCaravanMovementSpeed".Translate() + ":");
				int num3 = Mathf.CeilToInt((float)num2 / 1f);
				explanation.AppendLine();
				explanation.Append(string.Concat(new string[]
				{
					"  ",
					(60000f / (float)caravanTicksPerMove).ToString("0.#"),
					" / ",
					(num * roadMovementDifficultyMultiplier).ToString("0.#"),
					" = ",
					(60000f / (float)num3).ToString("0.#"),
					" "
				}) + "TilesPerDay".Translate());
			}
			return num2;
		}

		// Token: 0x0600B2A5 RID: 45733 RVA: 0x0033C214 File Offset: 0x0033A414
		public static bool IsValidFinalPushDestination(int tile)
		{
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (allWorldObjects[i].Tile == tile && !(allWorldObjects[i] is Caravan))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B2A6 RID: 45734 RVA: 0x0033C260 File Offset: 0x0033A460
		private float CostToPayThisTick()
		{
			float num = 1f;
			if (DebugSettings.fastCaravans)
			{
				num = 100f;
			}
			if (num < this.nextTileCostTotal / 30000f)
			{
				num = this.nextTileCostTotal / 30000f;
			}
			return num;
		}

		// Token: 0x0600B2A7 RID: 45735 RVA: 0x0033C2A0 File Offset: 0x0033A4A0
		private bool TrySetNewPath()
		{
			WorldPath worldPath = this.GenerateNewPath();
			if (!worldPath.Found)
			{
				this.PatherFailed();
				return false;
			}
			if (this.curPath != null)
			{
				this.curPath.ReleaseToPool();
			}
			this.curPath = worldPath;
			return true;
		}

		// Token: 0x0600B2A8 RID: 45736 RVA: 0x0033C2E0 File Offset: 0x0033A4E0
		private WorldPath GenerateNewPath()
		{
			int num = (this.moving && this.nextTile >= 0 && this.IsNextTilePassable()) ? this.nextTile : this.caravan.Tile;
			this.lastPathedTargetTile = this.destTile;
			WorldPath worldPath = Find.WorldPathFinder.FindPath(num, this.destTile, this.caravan, null);
			if (worldPath.Found && num != this.caravan.Tile)
			{
				if (worldPath.NodesLeftCount >= 2 && worldPath.Peek(1) == this.caravan.Tile)
				{
					worldPath.ConsumeNextNode();
					if (this.moving)
					{
						this.previousTileForDrawingIfInDoubt = this.nextTile;
						this.nextTile = this.caravan.Tile;
						this.nextTileCostLeft = this.nextTileCostTotal - this.nextTileCostLeft;
					}
				}
				else
				{
					worldPath.AddNodeAtStart(this.caravan.Tile);
				}
			}
			return worldPath;
		}

		// Token: 0x0600B2A9 RID: 45737 RVA: 0x00074231 File Offset: 0x00072431
		private bool AtDestinationPosition()
		{
			return this.caravan.Tile == this.destTile;
		}

		// Token: 0x0600B2AA RID: 45738 RVA: 0x0033C3C8 File Offset: 0x0033A5C8
		private bool NeedNewPath()
		{
			if (!this.moving)
			{
				return false;
			}
			if (this.curPath == null || !this.curPath.Found || this.curPath.NodesLeftCount == 0)
			{
				return true;
			}
			int num = 0;
			while (num < 20 && num < this.curPath.NodesLeftCount)
			{
				int tileID = this.curPath.Peek(num);
				if (Find.World.Impassable(tileID))
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x04007ACB RID: 31435
		private Caravan caravan;

		// Token: 0x04007ACC RID: 31436
		private bool moving;

		// Token: 0x04007ACD RID: 31437
		private bool paused;

		// Token: 0x04007ACE RID: 31438
		public int nextTile = -1;

		// Token: 0x04007ACF RID: 31439
		public int previousTileForDrawingIfInDoubt = -1;

		// Token: 0x04007AD0 RID: 31440
		public float nextTileCostLeft;

		// Token: 0x04007AD1 RID: 31441
		public float nextTileCostTotal = 1f;

		// Token: 0x04007AD2 RID: 31442
		private int destTile;

		// Token: 0x04007AD3 RID: 31443
		private CaravanArrivalAction arrivalAction;

		// Token: 0x04007AD4 RID: 31444
		public WorldPath curPath;

		// Token: 0x04007AD5 RID: 31445
		public int lastPathedTargetTile;

		// Token: 0x04007AD6 RID: 31446
		public const int MaxMoveTicks = 30000;

		// Token: 0x04007AD7 RID: 31447
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x04007AD8 RID: 31448
		private const int MinCostWalk = 50;

		// Token: 0x04007AD9 RID: 31449
		private const int MinCostAmble = 60;

		// Token: 0x04007ADA RID: 31450
		public const float DefaultPathCostToPayPerTick = 1f;

		// Token: 0x04007ADB RID: 31451
		public const int FinalNoRestPushMaxDurationTicks = 10000;
	}
}
