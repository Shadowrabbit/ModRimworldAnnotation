using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017B3 RID: 6067
	public class Caravan_PathFollower : IExposable
	{
		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x06008CA8 RID: 36008 RVA: 0x003281A8 File Offset: 0x003263A8
		public int Destination
		{
			get
			{
				return this.destTile;
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x06008CA9 RID: 36009 RVA: 0x003281B0 File Offset: 0x003263B0
		public bool Moving
		{
			get
			{
				return this.moving && this.caravan.Spawned;
			}
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x06008CAA RID: 36010 RVA: 0x003281C7 File Offset: 0x003263C7
		public bool MovingNow
		{
			get
			{
				return this.Moving && !this.Paused && !this.caravan.CantMove;
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x06008CAB RID: 36011 RVA: 0x003281E9 File Offset: 0x003263E9
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

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x06008CAC RID: 36012 RVA: 0x003281FB File Offset: 0x003263FB
		// (set) Token: 0x06008CAD RID: 36013 RVA: 0x00328210 File Offset: 0x00326410
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
					Log.Error("Tried to pause caravan movement of " + this.caravan.ToStringSafe<Caravan>() + " but it's not moving.");
				}
				else
				{
					this.paused = true;
				}
				this.caravan.Notify_DestinationOrPauseStatusChanged();
			}
		}

		// Token: 0x06008CAE RID: 36014 RVA: 0x0032826E File Offset: 0x0032646E
		public Caravan_PathFollower(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008CAF RID: 36015 RVA: 0x00328298 File Offset: 0x00326498
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

		// Token: 0x06008CB0 RID: 36016 RVA: 0x00328374 File Offset: 0x00326574
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

		// Token: 0x06008CB1 RID: 36017 RVA: 0x003284AC File Offset: 0x003266AC
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

		// Token: 0x06008CB2 RID: 36018 RVA: 0x00328518 File Offset: 0x00326718
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

		// Token: 0x06008CB3 RID: 36019 RVA: 0x0032861B File Offset: 0x0032681B
		public void Notify_Teleported_Int()
		{
			this.StopDead();
		}

		// Token: 0x06008CB4 RID: 36020 RVA: 0x00328623 File Offset: 0x00326823
		private bool IsPassable(int tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x06008CB5 RID: 36021 RVA: 0x00328633 File Offset: 0x00326833
		public bool IsNextTilePassable()
		{
			return this.IsPassable(this.nextTile);
		}

		// Token: 0x06008CB6 RID: 36022 RVA: 0x00328644 File Offset: 0x00326844
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
				}));
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
			}));
			this.caravan.Destroy();
			return false;
		}

		// Token: 0x06008CB7 RID: 36023 RVA: 0x00328720 File Offset: 0x00326920
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

		// Token: 0x06008CB8 RID: 36024 RVA: 0x0032861B File Offset: 0x0032681B
		private void PatherFailed()
		{
			this.StopDead();
		}

		// Token: 0x06008CB9 RID: 36025 RVA: 0x003287B8 File Offset: 0x003269B8
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
				Log.Error(this.caravan + " ran out of path nodes. Force-arriving.");
				this.PatherArrived();
				return;
			}
			this.SetupMoveIntoNextTile();
		}

		// Token: 0x06008CBA RID: 36026 RVA: 0x00328834 File Offset: 0x00326A34
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
				}));
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
				}));
			}
			int num = this.CostToMove(this.caravan.Tile, this.nextTile);
			this.nextTileCostTotal = (float)num;
			this.nextTileCostLeft = (float)num;
		}

		// Token: 0x06008CBB RID: 36027 RVA: 0x00328934 File Offset: 0x00326B34
		private int CostToMove(int start, int end)
		{
			return Caravan_PathFollower.CostToMove(this.caravan, start, end, null);
		}

		// Token: 0x06008CBC RID: 36028 RVA: 0x00328957 File Offset: 0x00326B57
		public static int CostToMove(Caravan caravan, int start, int end, int? ticksAbs = null)
		{
			return Caravan_PathFollower.CostToMove(caravan.TicksPerMove, start, end, ticksAbs, false, null, null, caravan.ImmobilizedByMass);
		}

		// Token: 0x06008CBD RID: 36029 RVA: 0x00328970 File Offset: 0x00326B70
		public static int CostToMove(int caravanTicksPerMove, int start, int end, int? ticksAbs = null, bool perceivedStatic = false, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null, bool immobile = false)
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
			if (explanation != null && !immobile)
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
				if (immobile)
				{
					explanation.Append("EncumberedCaravanTilesPerDayTip".Translate());
				}
				else
				{
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
			}
			return num2;
		}

		// Token: 0x06008CBE RID: 36030 RVA: 0x00328B64 File Offset: 0x00326D64
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

		// Token: 0x06008CBF RID: 36031 RVA: 0x00328BB0 File Offset: 0x00326DB0
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

		// Token: 0x06008CC0 RID: 36032 RVA: 0x00328BF0 File Offset: 0x00326DF0
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

		// Token: 0x06008CC1 RID: 36033 RVA: 0x00328C30 File Offset: 0x00326E30
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

		// Token: 0x06008CC2 RID: 36034 RVA: 0x00328D15 File Offset: 0x00326F15
		private bool AtDestinationPosition()
		{
			return this.caravan.Tile == this.destTile;
		}

		// Token: 0x06008CC3 RID: 36035 RVA: 0x00328D2C File Offset: 0x00326F2C
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

		// Token: 0x04005936 RID: 22838
		private Caravan caravan;

		// Token: 0x04005937 RID: 22839
		private bool moving;

		// Token: 0x04005938 RID: 22840
		private bool paused;

		// Token: 0x04005939 RID: 22841
		public int nextTile = -1;

		// Token: 0x0400593A RID: 22842
		public int previousTileForDrawingIfInDoubt = -1;

		// Token: 0x0400593B RID: 22843
		public float nextTileCostLeft;

		// Token: 0x0400593C RID: 22844
		public float nextTileCostTotal = 1f;

		// Token: 0x0400593D RID: 22845
		private int destTile;

		// Token: 0x0400593E RID: 22846
		private CaravanArrivalAction arrivalAction;

		// Token: 0x0400593F RID: 22847
		public WorldPath curPath;

		// Token: 0x04005940 RID: 22848
		public int lastPathedTargetTile;

		// Token: 0x04005941 RID: 22849
		public const int MaxMoveTicks = 30000;

		// Token: 0x04005942 RID: 22850
		private const int MaxCheckAheadNodes = 20;

		// Token: 0x04005943 RID: 22851
		private const int MinCostWalk = 50;

		// Token: 0x04005944 RID: 22852
		private const int MinCostAmble = 60;

		// Token: 0x04005945 RID: 22853
		public const float DefaultPathCostToPayPerTick = 1f;

		// Token: 0x04005946 RID: 22854
		public const int FinalNoRestPushMaxDurationTicks = 10000;
	}
}
