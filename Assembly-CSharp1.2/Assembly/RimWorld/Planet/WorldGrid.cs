using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020B0 RID: 8368
	public class WorldGrid : IExposable
	{
		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x0600B143 RID: 45379 RVA: 0x00073206 File Offset: 0x00071406
		public int TilesCount
		{
			get
			{
				return this.tileIDToNeighbors_offsets.Count;
			}
		}

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x0600B144 RID: 45380 RVA: 0x00073213 File Offset: 0x00071413
		public Vector3 NorthPolePos
		{
			get
			{
				return new Vector3(0f, 100f, 0f);
			}
		}

		// Token: 0x17001A2E RID: 6702
		public Tile this[int tileID]
		{
			get
			{
				if ((ulong)tileID >= (ulong)((long)this.TilesCount))
				{
					return null;
				}
				return this.tiles[tileID];
			}
		}

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x0600B146 RID: 45382 RVA: 0x00073244 File Offset: 0x00071444
		public bool HasWorldData
		{
			get
			{
				return this.tileBiome != null;
			}
		}

		// Token: 0x0600B147 RID: 45383 RVA: 0x003374C0 File Offset: 0x003356C0
		public WorldGrid()
		{
			this.CalculateViewCenterAndAngle();
			PlanetShapeGenerator.Generate(10, out this.verts, out this.tileIDToVerts_offsets, out this.tileIDToNeighbors_offsets, out this.tileIDToNeighbors_values, 100f, this.viewCenter, this.viewAngle);
			this.CalculateAverageTileSize();
		}

		// Token: 0x0600B148 RID: 45384 RVA: 0x0007324F File Offset: 0x0007144F
		public bool InBounds(int tileID)
		{
			return (ulong)tileID < (ulong)((long)this.TilesCount);
		}

		// Token: 0x0600B149 RID: 45385 RVA: 0x00337530 File Offset: 0x00335730
		public Vector2 LongLatOf(int tileID)
		{
			Vector3 tileCenter = this.GetTileCenter(tileID);
			float x = Mathf.Atan2(tileCenter.x, -tileCenter.z) * 57.29578f;
			float y = Mathf.Asin(tileCenter.y / 100f) * 57.29578f;
			return new Vector2(x, y);
		}

		// Token: 0x0600B14A RID: 45386 RVA: 0x0033757C File Offset: 0x0033577C
		public float GetHeadingFromTo(Vector3 from, Vector3 to)
		{
			if (from == to)
			{
				return 0f;
			}
			Vector3 northPolePos = this.NorthPolePos;
			Vector3 from2;
			Vector3 rhs;
			WorldRendererUtility.GetTangentialVectorFacing(from, northPolePos, out from2, out rhs);
			Vector3 vector;
			Vector3 vector2;
			WorldRendererUtility.GetTangentialVectorFacing(from, to, out vector, out vector2);
			float num = Vector3.Angle(from2, vector);
			if (Vector3.Dot(vector, rhs) < 0f)
			{
				num = 360f - num;
			}
			return num;
		}

		// Token: 0x0600B14B RID: 45387 RVA: 0x003375D8 File Offset: 0x003357D8
		public float GetHeadingFromTo(int fromTileID, int toTileID)
		{
			if (fromTileID == toTileID)
			{
				return 0f;
			}
			Vector3 tileCenter = this.GetTileCenter(fromTileID);
			Vector3 tileCenter2 = this.GetTileCenter(toTileID);
			return this.GetHeadingFromTo(tileCenter, tileCenter2);
		}

		// Token: 0x0600B14C RID: 45388 RVA: 0x00337608 File Offset: 0x00335808
		public Direction8Way GetDirection8WayFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (headingFromTo >= 337.5f || headingFromTo < 22.5f)
			{
				return Direction8Way.North;
			}
			if (headingFromTo < 67.5f)
			{
				return Direction8Way.NorthEast;
			}
			if (headingFromTo < 112.5f)
			{
				return Direction8Way.East;
			}
			if (headingFromTo < 157.5f)
			{
				return Direction8Way.SouthEast;
			}
			if (headingFromTo < 202.5f)
			{
				return Direction8Way.South;
			}
			if (headingFromTo < 247.5f)
			{
				return Direction8Way.SouthWest;
			}
			if (headingFromTo < 292.5f)
			{
				return Direction8Way.West;
			}
			return Direction8Way.NorthWest;
		}

		// Token: 0x0600B14D RID: 45389 RVA: 0x00337670 File Offset: 0x00335870
		public Rot4 GetRotFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (headingFromTo >= 315f || headingFromTo < 45f)
			{
				return Rot4.North;
			}
			if (headingFromTo < 135f)
			{
				return Rot4.East;
			}
			if (headingFromTo < 225f)
			{
				return Rot4.South;
			}
			return Rot4.West;
		}

		// Token: 0x0600B14E RID: 45390 RVA: 0x0007325C File Offset: 0x0007145C
		public void GetTileVertices(int tileID, List<Vector3> outVerts)
		{
			PackedListOfLists.GetList<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVerts);
		}

		// Token: 0x0600B14F RID: 45391 RVA: 0x00073271 File Offset: 0x00071471
		public void GetTileVerticesIndices(int tileID, List<int> outVertsIndices)
		{
			PackedListOfLists.GetListValuesIndices<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVertsIndices);
		}

		// Token: 0x0600B150 RID: 45392 RVA: 0x00073286 File Offset: 0x00071486
		public void GetTileNeighbors(int tileID, List<int> outNeighbors)
		{
			PackedListOfLists.GetList<int>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID, outNeighbors);
		}

		// Token: 0x0600B151 RID: 45393 RVA: 0x0007329B File Offset: 0x0007149B
		public int GetTileNeighborCount(int tileID)
		{
			return PackedListOfLists.GetListCount<int>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
		}

		// Token: 0x0600B152 RID: 45394 RVA: 0x000732AF File Offset: 0x000714AF
		public int GetMaxTileNeighborCountEver(int tileID)
		{
			return PackedListOfLists.GetListCount<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID);
		}

		// Token: 0x0600B153 RID: 45395 RVA: 0x000732C3 File Offset: 0x000714C3
		public bool IsNeighbor(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.Contains(tile2);
		}

		// Token: 0x0600B154 RID: 45396 RVA: 0x000732DC File Offset: 0x000714DC
		public bool IsNeighborOrSame(int tile1, int tile2)
		{
			return tile1 == tile2 || this.IsNeighbor(tile1, tile2);
		}

		// Token: 0x0600B155 RID: 45397 RVA: 0x000732EC File Offset: 0x000714EC
		public int GetNeighborId(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.IndexOf(tile2);
		}

		// Token: 0x0600B156 RID: 45398 RVA: 0x00073305 File Offset: 0x00071505
		public int GetTileNeighbor(int tileID, int adjacentId)
		{
			this.GetTileNeighbors(tileID, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors[adjacentId];
		}

		// Token: 0x0600B157 RID: 45399 RVA: 0x003376C0 File Offset: 0x003358C0
		public Vector3 GetTileCenter(int tileID)
		{
			int num = (tileID + 1 < this.tileIDToVerts_offsets.Count) ? this.tileIDToVerts_offsets[tileID + 1] : this.verts.Count;
			Vector3 a = Vector3.zero;
			int num2 = 0;
			for (int i = this.tileIDToVerts_offsets[tileID]; i < num; i++)
			{
				a += this.verts[i];
				num2++;
			}
			return a / (float)num2;
		}

		// Token: 0x0600B158 RID: 45400 RVA: 0x0007331E File Offset: 0x0007151E
		public float TileRadiusToAngle(float radius)
		{
			return this.DistOnSurfaceToAngle(radius * this.averageTileSize);
		}

		// Token: 0x0600B159 RID: 45401 RVA: 0x0007332E File Offset: 0x0007152E
		public float DistOnSurfaceToAngle(float dist)
		{
			return dist / 628.31854f * 360f;
		}

		// Token: 0x0600B15A RID: 45402 RVA: 0x0007333D File Offset: 0x0007153D
		public float DistanceFromEquatorNormalized(int tile)
		{
			return Mathf.Abs(Find.WorldGrid.GetTileCenter(tile).y / 100f);
		}

		// Token: 0x0600B15B RID: 45403 RVA: 0x0007335A File Offset: 0x0007155A
		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return sphericalDistance * 100f / this.averageTileSize;
		}

		// Token: 0x0600B15C RID: 45404 RVA: 0x00337738 File Offset: 0x00335938
		public float ApproxDistanceInTiles(int firstTile, int secondTile)
		{
			Vector3 tileCenter = this.GetTileCenter(firstTile);
			Vector3 tileCenter2 = this.GetTileCenter(secondTile);
			return this.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, tileCenter2.normalized));
		}

		// Token: 0x0600B15D RID: 45405 RVA: 0x00337770 File Offset: 0x00335970
		public void OverlayRoad(int fromTile, int toTile, RoadDef roadDef)
		{
			if (roadDef == null)
			{
				Log.ErrorOnce("Attempted to remove road with overlayRoad; not supported", 90292249, false);
				return;
			}
			RoadDef roadDef2 = this.GetRoadDef(fromTile, toTile, false);
			if (roadDef2 == roadDef)
			{
				return;
			}
			Tile tile = this[fromTile];
			Tile tile2 = this[toTile];
			if (roadDef2 != null)
			{
				if (roadDef2.priority >= roadDef.priority)
				{
					return;
				}
				tile.potentialRoads.RemoveAll((Tile.RoadLink rl) => rl.neighbor == toTile);
				tile2.potentialRoads.RemoveAll((Tile.RoadLink rl) => rl.neighbor == fromTile);
			}
			if (tile.potentialRoads == null)
			{
				tile.potentialRoads = new List<Tile.RoadLink>();
			}
			if (tile2.potentialRoads == null)
			{
				tile2.potentialRoads = new List<Tile.RoadLink>();
			}
			tile.potentialRoads.Add(new Tile.RoadLink
			{
				neighbor = toTile,
				road = roadDef
			});
			tile2.potentialRoads.Add(new Tile.RoadLink
			{
				neighbor = fromTile,
				road = roadDef
			});
		}

		// Token: 0x0600B15E RID: 45406 RVA: 0x00337894 File Offset: 0x00335A94
		public RoadDef GetRoadDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find road information between non-neighboring tiles", 12390444, false);
				return null;
			}
			Tile tile = this.tiles[fromTile];
			List<Tile.RoadLink> list = visibleOnly ? tile.Roads : tile.potentialRoads;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].neighbor == toTile)
				{
					return list[i].road;
				}
			}
			return null;
		}

		// Token: 0x0600B15F RID: 45407 RVA: 0x00337910 File Offset: 0x00335B10
		public void OverlayRiver(int fromTile, int toTile, RiverDef riverDef)
		{
			if (riverDef == null)
			{
				Log.ErrorOnce("Attempted to remove river with overlayRiver; not supported", 90292250, false);
				return;
			}
			RiverDef riverDef2 = this.GetRiverDef(fromTile, toTile, false);
			if (riverDef2 == riverDef)
			{
				return;
			}
			Tile tile = this[fromTile];
			Tile tile2 = this[toTile];
			if (riverDef2 != null)
			{
				if (riverDef2.degradeThreshold >= riverDef.degradeThreshold)
				{
					return;
				}
				tile.potentialRivers.RemoveAll((Tile.RiverLink rl) => rl.neighbor == toTile);
				tile2.potentialRivers.RemoveAll((Tile.RiverLink rl) => rl.neighbor == fromTile);
			}
			if (tile.potentialRivers == null)
			{
				tile.potentialRivers = new List<Tile.RiverLink>();
			}
			if (tile2.potentialRivers == null)
			{
				tile2.potentialRivers = new List<Tile.RiverLink>();
			}
			tile.potentialRivers.Add(new Tile.RiverLink
			{
				neighbor = toTile,
				river = riverDef
			});
			tile2.potentialRivers.Add(new Tile.RiverLink
			{
				neighbor = fromTile,
				river = riverDef
			});
		}

		// Token: 0x0600B160 RID: 45408 RVA: 0x00337A34 File Offset: 0x00335C34
		public RiverDef GetRiverDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find river information between non-neighboring tiles", 12390444, false);
				return null;
			}
			Tile tile = this.tiles[fromTile];
			List<Tile.RiverLink> list = visibleOnly ? tile.Rivers : tile.potentialRivers;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].neighbor == toTile)
				{
					return list[i].river;
				}
			}
			return null;
		}

		// Token: 0x0600B161 RID: 45409 RVA: 0x00337AB0 File Offset: 0x00335CB0
		public float GetRoadMovementDifficultyMultiplier(int fromTile, int toTile, StringBuilder explanation = null)
		{
			List<Tile.RoadLink> roads = this.tiles[fromTile].Roads;
			if (roads == null)
			{
				return 1f;
			}
			if (toTile == -1)
			{
				toTile = this.FindMostReasonableAdjacentTileForDisplayedPathCost(fromTile);
			}
			for (int i = 0; i < roads.Count; i++)
			{
				if (roads[i].neighbor == toTile)
				{
					float movementCostMultiplier = roads[i].road.movementCostMultiplier;
					if (explanation != null)
					{
						if (explanation.Length > 0)
						{
							explanation.AppendLine();
						}
						explanation.Append(roads[i].road.LabelCap + ": " + movementCostMultiplier.ToStringPercent());
					}
					return movementCostMultiplier;
				}
			}
			return 1f;
		}

		// Token: 0x0600B162 RID: 45410 RVA: 0x00337B64 File Offset: 0x00335D64
		public int FindMostReasonableAdjacentTileForDisplayedPathCost(int fromTile)
		{
			Tile tile = this.tiles[fromTile];
			float num = 1f;
			int num2 = -1;
			List<Tile.RoadLink> roads = tile.Roads;
			if (roads != null)
			{
				for (int i = 0; i < roads.Count; i++)
				{
					float movementCostMultiplier = roads[i].road.movementCostMultiplier;
					if (movementCostMultiplier < num && !Find.World.Impassable(roads[i].neighbor))
					{
						num = movementCostMultiplier;
						num2 = roads[i].neighbor;
					}
				}
			}
			if (num2 != -1)
			{
				return num2;
			}
			WorldGrid.tmpNeighbors.Clear();
			this.GetTileNeighbors(fromTile, WorldGrid.tmpNeighbors);
			for (int j = 0; j < WorldGrid.tmpNeighbors.Count; j++)
			{
				if (!Find.World.Impassable(WorldGrid.tmpNeighbors[j]))
				{
					return WorldGrid.tmpNeighbors[j];
				}
			}
			return fromTile;
		}

		// Token: 0x0600B163 RID: 45411 RVA: 0x00337C3C File Offset: 0x00335E3C
		public int TraversalDistanceBetween(int start, int end, bool passImpassable = true, int maxDist = 2147483647)
		{
			if (start == end)
			{
				return 0;
			}
			if (start < 0 || end < 0)
			{
				return int.MaxValue;
			}
			if (((this.cachedTraversalDistanceForStart == start && this.cachedTraversalDistanceForEnd == end) & passImpassable) && maxDist == 2147483647)
			{
				return this.cachedTraversalDistance;
			}
			if (!passImpassable && !Find.WorldReachability.CanReach(start, end))
			{
				return int.MaxValue;
			}
			int finalDist = int.MaxValue;
			int maxTilesToProcess = (maxDist == int.MaxValue) ? int.MaxValue : this.TilesNumWithinTraversalDistance(maxDist + 1);
			Find.WorldFloodFiller.FloodFill(start, (int x) => passImpassable || !Find.World.Impassable(x), delegate(int tile, int dist)
			{
				if (tile == end)
				{
					finalDist = dist;
					return true;
				}
				return false;
			}, maxTilesToProcess, null);
			if (passImpassable && maxDist == 2147483647)
			{
				this.cachedTraversalDistance = finalDist;
				this.cachedTraversalDistanceForStart = start;
				this.cachedTraversalDistanceForEnd = end;
			}
			return finalDist;
		}

		// Token: 0x0600B164 RID: 45412 RVA: 0x0007336A File Offset: 0x0007156A
		public int TilesNumWithinTraversalDistance(int traversalDist)
		{
			if (traversalDist < 0)
			{
				return 0;
			}
			return 3 * traversalDist * (traversalDist + 1) + 1;
		}

		// Token: 0x0600B165 RID: 45413 RVA: 0x0007337B File Offset: 0x0007157B
		public bool IsOnEdge(int tileID)
		{
			return this.InBounds(tileID) && this.GetTileNeighborCount(tileID) < this.GetMaxTileNeighborCountEver(tileID);
		}

		// Token: 0x0600B166 RID: 45414 RVA: 0x00337D50 File Offset: 0x00335F50
		private void CalculateAverageTileSize()
		{
			int tilesCount = this.TilesCount;
			double num = 0.0;
			int num2 = 0;
			for (int i = 0; i < tilesCount; i++)
			{
				Vector3 tileCenter = this.GetTileCenter(i);
				int num3 = (i + 1 < this.tileIDToNeighbors_offsets.Count) ? this.tileIDToNeighbors_offsets[i + 1] : this.tileIDToNeighbors_values.Count;
				for (int j = this.tileIDToNeighbors_offsets[i]; j < num3; j++)
				{
					int tileID = this.tileIDToNeighbors_values[j];
					Vector3 tileCenter2 = this.GetTileCenter(tileID);
					num += (double)Vector3.Distance(tileCenter, tileCenter2);
					num2++;
				}
			}
			this.averageTileSize = (float)(num / (double)num2);
		}

		// Token: 0x0600B167 RID: 45415 RVA: 0x00337E0C File Offset: 0x0033600C
		private void CalculateViewCenterAndAngle()
		{
			this.viewAngle = Find.World.PlanetCoverage * 180f;
			this.viewCenter = Vector3.back;
			float angle = 45f;
			if (this.viewAngle > 45f)
			{
				angle = Mathf.Max(90f - this.viewAngle, 0f);
			}
			this.viewCenter = Quaternion.AngleAxis(angle, Vector3.right) * this.viewCenter;
		}

		// Token: 0x0600B168 RID: 45416 RVA: 0x00337E80 File Offset: 0x00336080
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.TilesToRawData();
			}
			DataExposeUtility.ByteArray(ref this.tileBiome, "tileBiome");
			DataExposeUtility.ByteArray(ref this.tileElevation, "tileElevation");
			DataExposeUtility.ByteArray(ref this.tileHilliness, "tileHilliness");
			DataExposeUtility.ByteArray(ref this.tileTemperature, "tileTemperature");
			DataExposeUtility.ByteArray(ref this.tileRainfall, "tileRainfall");
			DataExposeUtility.ByteArray(ref this.tileSwampiness, "tileSwampiness");
			DataExposeUtility.ByteArray(ref this.tileFeature, "tileFeature");
			DataExposeUtility.ByteArray(ref this.tileRoadOrigins, "tileRoadOrigins");
			DataExposeUtility.ByteArray(ref this.tileRoadAdjacency, "tileRoadAdjacency");
			DataExposeUtility.ByteArray(ref this.tileRoadDef, "tileRoadDef");
			DataExposeUtility.ByteArray(ref this.tileRiverOrigins, "tileRiverOrigins");
			DataExposeUtility.ByteArray(ref this.tileRiverAdjacency, "tileRiverAdjacency");
			DataExposeUtility.ByteArray(ref this.tileRiverDef, "tileRiverDef");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.RawDataToTiles();
			}
		}

		// Token: 0x0600B169 RID: 45417 RVA: 0x00073398 File Offset: 0x00071598
		public void StandardizeTileData()
		{
			this.TilesToRawData();
			this.RawDataToTiles();
		}

		// Token: 0x0600B16A RID: 45418 RVA: 0x00337F7C File Offset: 0x0033617C
		private void TilesToRawData()
		{
			this.tileBiome = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => this.tiles[i].biome.shortHash);
			this.tileElevation = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((this.tiles[i].WaterCovered ? this.tiles[i].elevation : Mathf.Max(this.tiles[i].elevation, 1f)) + 8192f), 0, 65535));
			this.tileHilliness = DataSerializeUtility.SerializeByte(this.TilesCount, (int i) => (byte)this.tiles[i].hilliness);
			this.tileTemperature = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((this.tiles[i].temperature + 300f) * 10f), 0, 65535));
			this.tileRainfall = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].rainfall), 0, 65535));
			this.tileSwampiness = DataSerializeUtility.SerializeByte(this.TilesCount, (int i) => (byte)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].swampiness * 255f), 0, 255));
			this.tileFeature = DataSerializeUtility.SerializeUshort(this.TilesCount, delegate(int i)
			{
				if (this.tiles[i].feature != null)
				{
					return (ushort)this.tiles[i].feature.uniqueID;
				}
				return ushort.MaxValue;
			});
			List<int> list = new List<int>();
			List<byte> list2 = new List<byte>();
			List<ushort> list3 = new List<ushort>();
			for (int m = 0; m < this.TilesCount; m++)
			{
				List<Tile.RoadLink> potentialRoads = this.tiles[m].potentialRoads;
				if (potentialRoads != null)
				{
					for (int j = 0; j < potentialRoads.Count; j++)
					{
						Tile.RoadLink roadLink = potentialRoads[j];
						if (roadLink.neighbor >= m)
						{
							byte b = (byte)this.GetNeighborId(m, roadLink.neighbor);
							if (b < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for road piece", 81637014, false);
							}
							else
							{
								list.Add(m);
								list2.Add(b);
								list3.Add(roadLink.road.shortHash);
							}
						}
					}
				}
			}
			this.tileRoadOrigins = DataSerializeUtility.SerializeInt(list.ToArray());
			this.tileRoadAdjacency = DataSerializeUtility.SerializeByte(list2.ToArray());
			this.tileRoadDef = DataSerializeUtility.SerializeUshort(list3.ToArray());
			List<int> list4 = new List<int>();
			List<byte> list5 = new List<byte>();
			List<ushort> list6 = new List<ushort>();
			for (int k = 0; k < this.TilesCount; k++)
			{
				List<Tile.RiverLink> potentialRivers = this.tiles[k].potentialRivers;
				if (potentialRivers != null)
				{
					for (int l = 0; l < potentialRivers.Count; l++)
					{
						Tile.RiverLink riverLink = potentialRivers[l];
						if (riverLink.neighbor >= k)
						{
							byte b2 = (byte)this.GetNeighborId(k, riverLink.neighbor);
							if (b2 < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for river piece", 81637014, false);
							}
							else
							{
								list4.Add(k);
								list5.Add(b2);
								list6.Add(riverLink.river.shortHash);
							}
						}
					}
				}
			}
			this.tileRiverOrigins = DataSerializeUtility.SerializeInt(list4.ToArray());
			this.tileRiverAdjacency = DataSerializeUtility.SerializeByte(list5.ToArray());
			this.tileRiverDef = DataSerializeUtility.SerializeUshort(list6.ToArray());
		}

		// Token: 0x0600B16B RID: 45419 RVA: 0x00338234 File Offset: 0x00336434
		private void RawDataToTiles()
		{
			if (this.tiles.Count != this.TilesCount)
			{
				this.tiles.Clear();
				for (int m = 0; m < this.TilesCount; m++)
				{
					this.tiles.Add(new Tile());
				}
			}
			else
			{
				for (int j = 0; j < this.TilesCount; j++)
				{
					this.tiles[j].potentialRoads = null;
					this.tiles[j].potentialRivers = null;
				}
			}
			DataSerializeUtility.LoadUshort(this.tileBiome, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].biome = (DefDatabase<BiomeDef>.GetByShortHash(data) ?? BiomeDefOf.TemperateForest);
			});
			DataSerializeUtility.LoadUshort(this.tileElevation, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].elevation = (float)(data - 8192);
			});
			DataSerializeUtility.LoadByte(this.tileHilliness, this.TilesCount, delegate(int i, byte data)
			{
				this.tiles[i].hilliness = (Hilliness)data;
			});
			DataSerializeUtility.LoadUshort(this.tileTemperature, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].temperature = (float)data / 10f - 300f;
			});
			DataSerializeUtility.LoadUshort(this.tileRainfall, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].rainfall = (float)data;
			});
			DataSerializeUtility.LoadByte(this.tileSwampiness, this.TilesCount, delegate(int i, byte data)
			{
				this.tiles[i].swampiness = (float)data / 255f;
			});
			int[] array = DataSerializeUtility.DeserializeInt(this.tileRoadOrigins);
			byte[] array2 = DataSerializeUtility.DeserializeByte(this.tileRoadAdjacency);
			ushort[] array3 = DataSerializeUtility.DeserializeUshort(this.tileRoadDef);
			for (int k = 0; k < array.Length; k++)
			{
				int num = array[k];
				int tileNeighbor = this.GetTileNeighbor(num, (int)array2[k]);
				RoadDef byShortHash = DefDatabase<RoadDef>.GetByShortHash(array3[k]);
				if (byShortHash != null)
				{
					if (this.tiles[num].potentialRoads == null)
					{
						this.tiles[num].potentialRoads = new List<Tile.RoadLink>();
					}
					if (this.tiles[tileNeighbor].potentialRoads == null)
					{
						this.tiles[tileNeighbor].potentialRoads = new List<Tile.RoadLink>();
					}
					this.tiles[num].potentialRoads.Add(new Tile.RoadLink
					{
						neighbor = tileNeighbor,
						road = byShortHash
					});
					this.tiles[tileNeighbor].potentialRoads.Add(new Tile.RoadLink
					{
						neighbor = num,
						road = byShortHash
					});
				}
			}
			int[] array4 = DataSerializeUtility.DeserializeInt(this.tileRiverOrigins);
			byte[] array5 = DataSerializeUtility.DeserializeByte(this.tileRiverAdjacency);
			ushort[] array6 = DataSerializeUtility.DeserializeUshort(this.tileRiverDef);
			for (int l = 0; l < array4.Length; l++)
			{
				int num2 = array4[l];
				int tileNeighbor2 = this.GetTileNeighbor(num2, (int)array5[l]);
				RiverDef byShortHash2 = DefDatabase<RiverDef>.GetByShortHash(array6[l]);
				if (byShortHash2 != null)
				{
					if (this.tiles[num2].potentialRivers == null)
					{
						this.tiles[num2].potentialRivers = new List<Tile.RiverLink>();
					}
					if (this.tiles[tileNeighbor2].potentialRivers == null)
					{
						this.tiles[tileNeighbor2].potentialRivers = new List<Tile.RiverLink>();
					}
					this.tiles[num2].potentialRivers.Add(new Tile.RiverLink
					{
						neighbor = tileNeighbor2,
						river = byShortHash2
					});
					this.tiles[tileNeighbor2].potentialRivers.Add(new Tile.RiverLink
					{
						neighbor = num2,
						river = byShortHash2
					});
				}
			}
		}

		// Token: 0x04007A27 RID: 31271
		public List<Tile> tiles = new List<Tile>();

		// Token: 0x04007A28 RID: 31272
		public List<Vector3> verts;

		// Token: 0x04007A29 RID: 31273
		public List<int> tileIDToVerts_offsets;

		// Token: 0x04007A2A RID: 31274
		public List<int> tileIDToNeighbors_offsets;

		// Token: 0x04007A2B RID: 31275
		public List<int> tileIDToNeighbors_values;

		// Token: 0x04007A2C RID: 31276
		public float averageTileSize;

		// Token: 0x04007A2D RID: 31277
		public Vector3 viewCenter;

		// Token: 0x04007A2E RID: 31278
		public float viewAngle;

		// Token: 0x04007A2F RID: 31279
		private byte[] tileBiome;

		// Token: 0x04007A30 RID: 31280
		private byte[] tileElevation;

		// Token: 0x04007A31 RID: 31281
		private byte[] tileHilliness;

		// Token: 0x04007A32 RID: 31282
		private byte[] tileTemperature;

		// Token: 0x04007A33 RID: 31283
		private byte[] tileRainfall;

		// Token: 0x04007A34 RID: 31284
		private byte[] tileSwampiness;

		// Token: 0x04007A35 RID: 31285
		public byte[] tileFeature;

		// Token: 0x04007A36 RID: 31286
		private byte[] tileRoadOrigins;

		// Token: 0x04007A37 RID: 31287
		private byte[] tileRoadAdjacency;

		// Token: 0x04007A38 RID: 31288
		private byte[] tileRoadDef;

		// Token: 0x04007A39 RID: 31289
		private byte[] tileRiverOrigins;

		// Token: 0x04007A3A RID: 31290
		private byte[] tileRiverAdjacency;

		// Token: 0x04007A3B RID: 31291
		private byte[] tileRiverDef;

		// Token: 0x04007A3C RID: 31292
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04007A3D RID: 31293
		private const int SubdivisionsCount = 10;

		// Token: 0x04007A3E RID: 31294
		public const float PlanetRadius = 100f;

		// Token: 0x04007A3F RID: 31295
		public const int ElevationOffset = 8192;

		// Token: 0x04007A40 RID: 31296
		public const int TemperatureOffset = 300;

		// Token: 0x04007A41 RID: 31297
		public const float TemperatureMultiplier = 10f;

		// Token: 0x04007A42 RID: 31298
		private int cachedTraversalDistance = -1;

		// Token: 0x04007A43 RID: 31299
		private int cachedTraversalDistanceForStart = -1;

		// Token: 0x04007A44 RID: 31300
		private int cachedTraversalDistanceForEnd = -1;
	}
}
