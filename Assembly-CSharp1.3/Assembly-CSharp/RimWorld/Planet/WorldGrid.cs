using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200178E RID: 6030
	public class WorldGrid : IExposable
	{
		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06008B27 RID: 35623 RVA: 0x0031FC44 File Offset: 0x0031DE44
		public int TilesCount
		{
			get
			{
				return this.tileIDToNeighbors_offsets.Count;
			}
		}

		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06008B28 RID: 35624 RVA: 0x0031FC51 File Offset: 0x0031DE51
		public Vector3 NorthPolePos
		{
			get
			{
				return new Vector3(0f, 100f, 0f);
			}
		}

		// Token: 0x170016B0 RID: 5808
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

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x06008B2A RID: 35626 RVA: 0x0031FC82 File Offset: 0x0031DE82
		public bool HasWorldData
		{
			get
			{
				return this.tileBiome != null;
			}
		}

		// Token: 0x06008B2B RID: 35627 RVA: 0x0031FC90 File Offset: 0x0031DE90
		public WorldGrid()
		{
			this.CalculateViewCenterAndAngle();
			PlanetShapeGenerator.Generate(10, out this.verts, out this.tileIDToVerts_offsets, out this.tileIDToNeighbors_offsets, out this.tileIDToNeighbors_values, 100f, this.viewCenter, this.viewAngle);
			this.CalculateAverageTileSize();
		}

		// Token: 0x06008B2C RID: 35628 RVA: 0x0031FCFF File Offset: 0x0031DEFF
		public bool InBounds(int tileID)
		{
			return (ulong)tileID < (ulong)((long)this.TilesCount);
		}

		// Token: 0x06008B2D RID: 35629 RVA: 0x0031FD0C File Offset: 0x0031DF0C
		public Vector2 LongLatOf(int tileID)
		{
			Vector3 tileCenter = this.GetTileCenter(tileID);
			float x = Mathf.Atan2(tileCenter.x, -tileCenter.z) * 57.29578f;
			float y = Mathf.Asin(tileCenter.y / 100f) * 57.29578f;
			return new Vector2(x, y);
		}

		// Token: 0x06008B2E RID: 35630 RVA: 0x0031FD58 File Offset: 0x0031DF58
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

		// Token: 0x06008B2F RID: 35631 RVA: 0x0031FDB4 File Offset: 0x0031DFB4
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

		// Token: 0x06008B30 RID: 35632 RVA: 0x0031FDE4 File Offset: 0x0031DFE4
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

		// Token: 0x06008B31 RID: 35633 RVA: 0x0031FE4C File Offset: 0x0031E04C
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

		// Token: 0x06008B32 RID: 35634 RVA: 0x0031FE99 File Offset: 0x0031E099
		public void GetTileVertices(int tileID, List<Vector3> outVerts)
		{
			PackedListOfLists.GetList<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVerts);
		}

		// Token: 0x06008B33 RID: 35635 RVA: 0x0031FEAE File Offset: 0x0031E0AE
		public void GetTileVerticesIndices(int tileID, List<int> outVertsIndices)
		{
			PackedListOfLists.GetListValuesIndices<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVertsIndices);
		}

		// Token: 0x06008B34 RID: 35636 RVA: 0x0031FEC3 File Offset: 0x0031E0C3
		public void GetTileNeighbors(int tileID, List<int> outNeighbors)
		{
			PackedListOfLists.GetList<int>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID, outNeighbors);
		}

		// Token: 0x06008B35 RID: 35637 RVA: 0x0031FED8 File Offset: 0x0031E0D8
		public int GetTileNeighborCount(int tileID)
		{
			return PackedListOfLists.GetListCount<int>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
		}

		// Token: 0x06008B36 RID: 35638 RVA: 0x0031FEEC File Offset: 0x0031E0EC
		public int GetMaxTileNeighborCountEver(int tileID)
		{
			return PackedListOfLists.GetListCount<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID);
		}

		// Token: 0x06008B37 RID: 35639 RVA: 0x0031FF00 File Offset: 0x0031E100
		public bool IsNeighbor(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.Contains(tile2);
		}

		// Token: 0x06008B38 RID: 35640 RVA: 0x0031FF19 File Offset: 0x0031E119
		public bool IsNeighborOrSame(int tile1, int tile2)
		{
			return tile1 == tile2 || this.IsNeighbor(tile1, tile2);
		}

		// Token: 0x06008B39 RID: 35641 RVA: 0x0031FF29 File Offset: 0x0031E129
		public int GetNeighborId(int tile1, int tile2)
		{
			this.GetTileNeighbors(tile1, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors.IndexOf(tile2);
		}

		// Token: 0x06008B3A RID: 35642 RVA: 0x0031FF42 File Offset: 0x0031E142
		public int GetTileNeighbor(int tileID, int adjacentId)
		{
			this.GetTileNeighbors(tileID, WorldGrid.tmpNeighbors);
			return WorldGrid.tmpNeighbors[adjacentId];
		}

		// Token: 0x06008B3B RID: 35643 RVA: 0x0031FF5C File Offset: 0x0031E15C
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

		// Token: 0x06008B3C RID: 35644 RVA: 0x0031FFD3 File Offset: 0x0031E1D3
		public float TileRadiusToAngle(float radius)
		{
			return this.DistOnSurfaceToAngle(radius * this.averageTileSize);
		}

		// Token: 0x06008B3D RID: 35645 RVA: 0x0031FFE3 File Offset: 0x0031E1E3
		public float DistOnSurfaceToAngle(float dist)
		{
			return dist / 628.31854f * 360f;
		}

		// Token: 0x06008B3E RID: 35646 RVA: 0x0031FFF2 File Offset: 0x0031E1F2
		public float DistanceFromEquatorNormalized(int tile)
		{
			return Mathf.Abs(Find.WorldGrid.GetTileCenter(tile).y / 100f);
		}

		// Token: 0x06008B3F RID: 35647 RVA: 0x0032000F File Offset: 0x0031E20F
		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return sphericalDistance * 100f / this.averageTileSize;
		}

		// Token: 0x06008B40 RID: 35648 RVA: 0x00320020 File Offset: 0x0031E220
		public float ApproxDistanceInTiles(int firstTile, int secondTile)
		{
			Vector3 tileCenter = this.GetTileCenter(firstTile);
			Vector3 tileCenter2 = this.GetTileCenter(secondTile);
			return this.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, tileCenter2.normalized));
		}

		// Token: 0x06008B41 RID: 35649 RVA: 0x00320058 File Offset: 0x0031E258
		public void OverlayRoad(int fromTile, int toTile, RoadDef roadDef)
		{
			if (roadDef == null)
			{
				Log.ErrorOnce("Attempted to remove road with overlayRoad; not supported", 90292249);
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

		// Token: 0x06008B42 RID: 35650 RVA: 0x0032017C File Offset: 0x0031E37C
		public RoadDef GetRoadDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find road information between non-neighboring tiles", 12390444);
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

		// Token: 0x06008B43 RID: 35651 RVA: 0x003201F8 File Offset: 0x0031E3F8
		public void OverlayRiver(int fromTile, int toTile, RiverDef riverDef)
		{
			if (riverDef == null)
			{
				Log.ErrorOnce("Attempted to remove river with overlayRiver; not supported", 90292250);
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

		// Token: 0x06008B44 RID: 35652 RVA: 0x0032031C File Offset: 0x0031E51C
		public RiverDef GetRiverDef(int fromTile, int toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find river information between non-neighboring tiles", 12390444);
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

		// Token: 0x06008B45 RID: 35653 RVA: 0x00320398 File Offset: 0x0031E598
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

		// Token: 0x06008B46 RID: 35654 RVA: 0x0032044C File Offset: 0x0031E64C
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

		// Token: 0x06008B47 RID: 35655 RVA: 0x00320524 File Offset: 0x0031E724
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

		// Token: 0x06008B48 RID: 35656 RVA: 0x00320638 File Offset: 0x0031E838
		public int TilesNumWithinTraversalDistance(int traversalDist)
		{
			if (traversalDist < 0)
			{
				return 0;
			}
			return 3 * traversalDist * (traversalDist + 1) + 1;
		}

		// Token: 0x06008B49 RID: 35657 RVA: 0x00320649 File Offset: 0x0031E849
		public bool IsOnEdge(int tileID)
		{
			return this.InBounds(tileID) && this.GetTileNeighborCount(tileID) < this.GetMaxTileNeighborCountEver(tileID);
		}

		// Token: 0x06008B4A RID: 35658 RVA: 0x00320668 File Offset: 0x0031E868
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

		// Token: 0x06008B4B RID: 35659 RVA: 0x00320724 File Offset: 0x0031E924
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

		// Token: 0x06008B4C RID: 35660 RVA: 0x00320798 File Offset: 0x0031E998
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

		// Token: 0x06008B4D RID: 35661 RVA: 0x00320891 File Offset: 0x0031EA91
		public void StandardizeTileData()
		{
			this.TilesToRawData();
			this.RawDataToTiles();
		}

		// Token: 0x06008B4E RID: 35662 RVA: 0x003208A0 File Offset: 0x0031EAA0
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
								Log.ErrorOnce("Couldn't find valid neighbor for road piece", 81637014);
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
								Log.ErrorOnce("Couldn't find valid neighbor for river piece", 81637014);
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

		// Token: 0x06008B4F RID: 35663 RVA: 0x00320B58 File Offset: 0x0031ED58
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

		// Token: 0x040058B3 RID: 22707
		public List<Tile> tiles = new List<Tile>();

		// Token: 0x040058B4 RID: 22708
		public List<Vector3> verts;

		// Token: 0x040058B5 RID: 22709
		public List<int> tileIDToVerts_offsets;

		// Token: 0x040058B6 RID: 22710
		public List<int> tileIDToNeighbors_offsets;

		// Token: 0x040058B7 RID: 22711
		public List<int> tileIDToNeighbors_values;

		// Token: 0x040058B8 RID: 22712
		public float averageTileSize;

		// Token: 0x040058B9 RID: 22713
		public Vector3 viewCenter;

		// Token: 0x040058BA RID: 22714
		public float viewAngle;

		// Token: 0x040058BB RID: 22715
		private byte[] tileBiome;

		// Token: 0x040058BC RID: 22716
		private byte[] tileElevation;

		// Token: 0x040058BD RID: 22717
		private byte[] tileHilliness;

		// Token: 0x040058BE RID: 22718
		private byte[] tileTemperature;

		// Token: 0x040058BF RID: 22719
		private byte[] tileRainfall;

		// Token: 0x040058C0 RID: 22720
		private byte[] tileSwampiness;

		// Token: 0x040058C1 RID: 22721
		public byte[] tileFeature;

		// Token: 0x040058C2 RID: 22722
		private byte[] tileRoadOrigins;

		// Token: 0x040058C3 RID: 22723
		private byte[] tileRoadAdjacency;

		// Token: 0x040058C4 RID: 22724
		private byte[] tileRoadDef;

		// Token: 0x040058C5 RID: 22725
		private byte[] tileRiverOrigins;

		// Token: 0x040058C6 RID: 22726
		private byte[] tileRiverAdjacency;

		// Token: 0x040058C7 RID: 22727
		private byte[] tileRiverDef;

		// Token: 0x040058C8 RID: 22728
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x040058C9 RID: 22729
		private const int SubdivisionsCount = 10;

		// Token: 0x040058CA RID: 22730
		public const float PlanetRadius = 100f;

		// Token: 0x040058CB RID: 22731
		public const int ElevationOffset = 8192;

		// Token: 0x040058CC RID: 22732
		public const int TemperatureOffset = 300;

		// Token: 0x040058CD RID: 22733
		public const float TemperatureMultiplier = 10f;

		// Token: 0x040058CE RID: 22734
		private int cachedTraversalDistance = -1;

		// Token: 0x040058CF RID: 22735
		private int cachedTraversalDistanceForStart = -1;

		// Token: 0x040058D0 RID: 22736
		private int cachedTraversalDistanceForEnd = -1;
	}
}
