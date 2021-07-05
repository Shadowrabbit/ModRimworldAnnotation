using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF6 RID: 3318
	public class Sketch : IExposable
	{
		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06004D2D RID: 19757 RVA: 0x0019D424 File Offset: 0x0019B624
		public List<SketchEntity> Entities
		{
			get
			{
				return this.entities;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x0019D42C File Offset: 0x0019B62C
		public List<SketchThing> Things
		{
			get
			{
				return this.cachedThings;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06004D2F RID: 19759 RVA: 0x0019D434 File Offset: 0x0019B634
		public List<SketchTerrain> Terrain
		{
			get
			{
				return this.cachedTerrain;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06004D30 RID: 19760 RVA: 0x0019D43C File Offset: 0x0019B63C
		public List<SketchBuildable> Buildables
		{
			get
			{
				return this.cachedBuildables;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0019D444 File Offset: 0x0019B644
		public CellRect OccupiedRect
		{
			get
			{
				if (this.occupiedRectDirty)
				{
					this.cachedOccupiedRect = CellRect.Empty;
					bool flag = false;
					for (int i = 0; i < this.entities.Count; i++)
					{
						if (!flag)
						{
							this.cachedOccupiedRect = this.entities[i].OccupiedRect;
							flag = true;
						}
						else
						{
							CellRect occupiedRect = this.entities[i].OccupiedRect;
							this.cachedOccupiedRect = CellRect.FromLimits(Mathf.Min(this.cachedOccupiedRect.minX, occupiedRect.minX), Mathf.Min(this.cachedOccupiedRect.minZ, occupiedRect.minZ), Mathf.Max(this.cachedOccupiedRect.maxX, occupiedRect.maxX), Mathf.Max(this.cachedOccupiedRect.maxZ, occupiedRect.maxZ));
						}
					}
					this.occupiedRectDirty = false;
				}
				return this.cachedOccupiedRect;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004D32 RID: 19762 RVA: 0x0019D528 File Offset: 0x0019B728
		public IntVec2 OccupiedSize
		{
			get
			{
				return new IntVec2(this.OccupiedRect.Width, this.OccupiedRect.Height);
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004D33 RID: 19763 RVA: 0x0019D558 File Offset: 0x0019B758
		public IntVec3 OccupiedCenter
		{
			get
			{
				return this.OccupiedRect.CenterCell;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004D34 RID: 19764 RVA: 0x0019D573 File Offset: 0x0019B773
		public bool Empty
		{
			get
			{
				return !this.Entities.Any<SketchEntity>();
			}
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0019D584 File Offset: 0x0019B784
		public bool Add(SketchEntity entity, bool wipeIfCollides = true)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (this.entities.Contains(entity))
			{
				return true;
			}
			if (wipeIfCollides)
			{
				this.WipeColliding(entity);
			}
			else if (this.WouldCollide(entity))
			{
				return false;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			SketchTerrain entity2;
			if (sketchTerrain != null && this.terrainAt.TryGetValue(sketchTerrain.pos, out entity2))
			{
				this.Remove(entity2);
			}
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				for (int i = this.cachedBuildables.Count - 1; i >= 0; i--)
				{
					if (sketchBuildable.OccupiedRect.Overlaps(this.cachedBuildables[i].OccupiedRect) && GenSpawn.SpawningWipes(sketchBuildable.Buildable, this.cachedBuildables[i].Buildable))
					{
						this.Remove(this.cachedBuildables[i]);
					}
				}
			}
			this.entities.Add(entity);
			this.AddToCache(entity);
			return true;
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x0019D678 File Offset: 0x0019B878
		public bool AddThing(ThingDef def, IntVec3 pos, Rot4 rot, ThingDef stuff = null, int stackCount = 1, QualityCategory? quality = null, int? hitPoints = null, bool wipeIfCollides = true)
		{
			return this.Add(new SketchThing
			{
				def = def,
				stuff = stuff,
				pos = pos,
				rot = rot,
				stackCount = stackCount,
				quality = quality,
				hitPoints = hitPoints
			}, wipeIfCollides);
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x0019D6CC File Offset: 0x0019B8CC
		public bool AddTerrain(TerrainDef def, IntVec3 pos, bool wipeIfCollides = true)
		{
			return this.Add(new SketchTerrain
			{
				def = def,
				pos = pos
			}, wipeIfCollides);
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x0019D6F5 File Offset: 0x0019B8F5
		public bool Remove(SketchEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (!this.entities.Contains(entity))
			{
				return false;
			}
			this.entities.Remove(entity);
			this.RemoveFromCache(entity);
			return true;
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x0019D724 File Offset: 0x0019B924
		public bool RemoveTerrain(IntVec3 cell)
		{
			SketchTerrain entity;
			return this.terrainAt.TryGetValue(cell, out entity) && this.Remove(entity);
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x0019D74A File Offset: 0x0019B94A
		public void Clear()
		{
			this.entities.Clear();
			this.RecacheAll();
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x0019D760 File Offset: 0x0019B960
		public TerrainDef TerrainAt(IntVec3 pos)
		{
			SketchTerrain sketchTerrain = this.SketchTerrainAt(pos);
			if (sketchTerrain == null)
			{
				return null;
			}
			return sketchTerrain.def;
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x0019D780 File Offset: 0x0019B980
		public SketchTerrain SketchTerrainAt(IntVec3 pos)
		{
			SketchTerrain result;
			if (!this.terrainAt.TryGetValue(pos, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0019D7A0 File Offset: 0x0019B9A0
		public bool AnyTerrainAt(int x, int z)
		{
			return this.AnyTerrainAt(new IntVec3(x, 0, z));
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0019D7B0 File Offset: 0x0019B9B0
		public bool AnyTerrainAt(IntVec3 pos)
		{
			return this.TerrainAt(pos) != null;
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x0019D7BC File Offset: 0x0019B9BC
		public IEnumerable<SketchThing> ThingsAt(IntVec3 pos)
		{
			SketchThing val;
			if (this.thingsAt_single.TryGetValue(pos, out val))
			{
				return Gen.YieldSingle<SketchThing>(val);
			}
			List<SketchThing> result;
			if (this.thingsAt_multiple.TryGetValue(pos, out result))
			{
				return result;
			}
			return Sketch.EmptySketchThingList;
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x0019D7F8 File Offset: 0x0019B9F8
		public void ThingsAt(IntVec3 pos, out SketchThing singleResult, out List<SketchThing> multipleResults)
		{
			SketchThing sketchThing;
			if (this.thingsAt_single.TryGetValue(pos, out sketchThing))
			{
				singleResult = sketchThing;
				multipleResults = null;
				return;
			}
			List<SketchThing> list;
			if (this.thingsAt_multiple.TryGetValue(pos, out list))
			{
				singleResult = null;
				multipleResults = list;
				return;
			}
			singleResult = null;
			multipleResults = null;
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x0019D83C File Offset: 0x0019BA3C
		public SketchThing EdificeAt(IntVec3 pos)
		{
			SketchThing result;
			if (this.edificeAt.TryGetValue(pos, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x0019D85C File Offset: 0x0019BA5C
		public bool WouldCollide(SketchEntity entity)
		{
			if (this.entities.Contains(entity))
			{
				return false;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				return this.WouldCollide(sketchThing.def, sketchThing.pos, sketchThing.rot);
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			return sketchTerrain != null && this.WouldCollide(sketchTerrain.def, sketchTerrain.pos);
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0019D8BC File Offset: 0x0019BABC
		public bool WouldCollide(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 pos2 in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(pos2);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						return true;
					}
				}
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect))
				{
					if (def.race != null)
					{
						if (this.cachedThings[i].def.passability == Traversability.Impassable)
						{
							return true;
						}
					}
					else if (!GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x0019D9BC File Offset: 0x0019BBBC
		public bool WouldCollide(TerrainDef def, IntVec3 pos)
		{
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				return true;
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0019DA4C File Offset: 0x0019BC4C
		public void WipeColliding(SketchEntity entity)
		{
			if (!this.WouldCollide(entity))
			{
				return;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				this.WipeColliding(sketchThing.def, sketchThing.pos, sketchThing.rot);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.WipeColliding(sketchTerrain.def, sketchTerrain.pos);
			}
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0019DAA4 File Offset: 0x0019BCA4
		public void WipeColliding(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			if (!this.WouldCollide(def, pos, rot))
			{
				return;
			}
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 intVec in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(intVec);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						this.RemoveTerrain(intVec);
					}
				}
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect) && !GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0019DB9C File Offset: 0x0019BD9C
		public void WipeColliding(TerrainDef def, IntVec3 pos)
		{
			if (!this.WouldCollide(def, pos))
			{
				return;
			}
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				this.RemoveTerrain(pos);
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0019DC50 File Offset: 0x0019BE50
		public bool IsSpawningBlocked(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				if (this.entities[i].IsSpawningBlocked(this.entities[i].pos + offset, map, null, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0019DCB0 File Offset: 0x0019BEB0
		public bool AnyThingOutOfBounds(Map map, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				SketchThing sketchThing = this.entities[i] as SketchThing;
				if (sketchThing != null)
				{
					if (sketchThing.def.size == IntVec2.One)
					{
						if (!(this.entities[i].pos + offset).InBounds(map))
						{
							return true;
						}
					}
					else
					{
						using (CellRect.Enumerator enumerator = sketchThing.OccupiedRect.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (!(enumerator.Current + offset).InBounds(map))
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0019DD88 File Offset: 0x0019BF88
		public void Spawn(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool clearEdificeWhereFloor = false, List<Thing> spawnedThings = null, bool dormant = false, bool buildRoofsInstantly = false, Func<SketchEntity, IntVec3, bool> canSpawnThing = null, Action<IntVec3, SketchEntity> onFailedToSpawnThing = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			if (clearEdificeWhereFloor)
			{
				for (int i = 0; i < this.cachedTerrain.Count; i++)
				{
					if (this.cachedTerrain[i].def.layerable)
					{
						Building edifice = (this.cachedTerrain[i].pos + offset).GetEdifice(map);
						if (edifice != null)
						{
							edifice.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				IntVec3 intVec = sketchEntity.pos + offset;
				if (((canSpawnThing != null && !canSpawnThing(sketchEntity, intVec)) || !sketchEntity.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, spawnedThings, dormant)) && onFailedToSpawnThing != null)
				{
					onFailedToSpawnThing(intVec, sketchEntity);
				}
			}
			if (spawnedThings != null && spawnMode == Sketch.SpawnMode.TransportPod && !wipeIfCollides)
			{
				bool flag = false;
				for (int j = 0; j < spawnedThings.Count; j++)
				{
					for (int k = j + 1; k < spawnedThings.Count; k++)
					{
						CellRect cellRect = GenAdj.OccupiedRect(spawnedThings[j].Position, spawnedThings[j].Rotation, spawnedThings[j].def.size);
						CellRect other = GenAdj.OccupiedRect(spawnedThings[k].Position, spawnedThings[k].Rotation, spawnedThings[k].def.size);
						if (cellRect.Overlaps(other) && (GenSpawn.SpawningWipes(spawnedThings[j].def, spawnedThings[k].def) || GenSpawn.SpawningWipes(spawnedThings[k].def, spawnedThings[j].def)))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					for (int l = 0; l < spawnedThings.Count; l++)
					{
						ActiveDropPodInfo activeDropPodInfo;
						if ((activeDropPodInfo = (spawnedThings[l].ParentHolder as ActiveDropPodInfo)) != null)
						{
							activeDropPodInfo.spawnWipeMode = null;
						}
					}
				}
			}
			if (buildRoofsInstantly && spawnMode == Sketch.SpawnMode.Normal)
			{
				foreach (IntVec3 a in this.GetSuggestedRoofCells())
				{
					IntVec3 c = a + offset;
					if (c.InBounds(map) && !c.Roofed(map))
					{
						map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
					}
				}
			}
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0019E064 File Offset: 0x0019C264
		public void Merge(Sketch other, bool wipeIfCollides = true)
		{
			foreach (SketchEntity sketchEntity in from x in other.entities
			orderby x.SpawnOrder
			select x)
			{
				this.Add(sketchEntity.DeepCopy(), wipeIfCollides);
			}
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x0019E0DC File Offset: 0x0019C2DC
		public void MergeAt(Sketch other, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool wipeIfCollides = true)
		{
			Sketch sketch = other.DeepCopy();
			sketch.MoveBy(sketch.GetOffset(pos, posType));
			this.Merge(sketch, wipeIfCollides);
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0019E108 File Offset: 0x0019C308
		public void Subtract(Sketch other)
		{
			for (int i = 0; i < this.entities.Count; i++)
			{
				for (int j = 0; j < other.entities.Count; j++)
				{
					if (this.entities[i].SameForSubtracting(other.entities[j]))
					{
						this.Remove(this.entities[i]);
						i--;
						break;
					}
				}
			}
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x0019E179 File Offset: 0x0019C379
		public void MoveBy(IntVec2 by)
		{
			this.MoveBy(by.ToIntVec3);
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x0019E188 File Offset: 0x0019C388
		public void MoveBy(IntVec3 by)
		{
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos += by;
			}
			this.RecacheAll();
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x0019E1EC File Offset: 0x0019C3EC
		public void MoveOccupiedCenterToZero()
		{
			this.MoveBy(new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z));
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x0019E214 File Offset: 0x0019C414
		public Sketch DeepCopy()
		{
			Sketch sketch = new Sketch();
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				sketch.Add(sketchEntity.DeepCopy(), true);
			}
			return sketch;
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x0019E294 File Offset: 0x0019C494
		private void AddToCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Add(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						this.edificeAt[key] = sketchThing;
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing item;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Add(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out item))
					{
						this.thingsAt_single.Remove(key2);
						List<SketchThing> list2 = new List<SketchThing>();
						list2.Add(item);
						list2.Add(sketchThing);
						this.thingsAt_multiple.Add(key2, list2);
					}
					else
					{
						this.thingsAt_single.Add(key2, sketchThing);
					}
				}
				this.cachedThings.Add(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.terrainAt[sketchTerrain.pos] = sketchTerrain;
				this.cachedTerrain.Add(sketchTerrain);
			}
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0019E428 File Offset: 0x0019C628
		private void RemoveFromCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Remove(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						SketchThing sketchThing2;
						if (this.edificeAt.TryGetValue(key, out sketchThing2) && sketchThing2 == sketchThing)
						{
							this.edificeAt.Remove(key);
						}
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing sketchThing3;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Remove(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out sketchThing3) && sketchThing3 == sketchThing)
					{
						this.thingsAt_single.Remove(key2);
					}
				}
				this.cachedThings.Remove(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				SketchTerrain sketchTerrain2;
				if (this.terrainAt.TryGetValue(sketchTerrain.pos, out sketchTerrain2) && sketchTerrain2 == sketchTerrain)
				{
					this.terrainAt.Remove(sketchTerrain.pos);
				}
				this.cachedTerrain.Remove(sketchTerrain);
			}
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0019E5BC File Offset: 0x0019C7BC
		private void Recache(SketchEntity entity)
		{
			this.RemoveFromCache(entity);
			this.AddToCache(entity);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0019E5CC File Offset: 0x0019C7CC
		public void RecacheAll()
		{
			this.terrainAt.Clear();
			this.edificeAt.Clear();
			this.thingsAt_single.Clear();
			this.cachedThings.Clear();
			this.cachedTerrain.Clear();
			this.cachedBuildables.Clear();
			this.occupiedRectDirty = true;
			foreach (KeyValuePair<IntVec3, List<SketchThing>> keyValuePair in this.thingsAt_multiple)
			{
				keyValuePair.Value.Clear();
			}
			foreach (SketchEntity entity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				this.AddToCache(entity);
			}
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0019E6D0 File Offset: 0x0019C8D0
		public bool LineOfSight(IntVec3 start, IntVec3 end, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (!skipFirstCell || !(intVec == start))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0019E748 File Offset: 0x0019C948
		public bool LineOfSight(IntVec3 start, IntVec3 end, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x0019E7CC File Offset: 0x0019C9CC
		public bool CanBeSeenOver(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing == null || sketchThing.def.Fillage != FillCategory.Full;
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x0019E7F7 File Offset: 0x0019C9F7
		public bool Passable(int x, int z)
		{
			return this.Passable(new IntVec3(x, 0, z));
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x0019E808 File Offset: 0x0019CA08
		public bool Passable(IntVec3 pos)
		{
			TerrainDef terrainDef = this.TerrainAt(pos);
			if (terrainDef != null && terrainDef.passability == Traversability.Impassable)
			{
				return false;
			}
			using (IEnumerator<SketchThing> enumerator = this.ThingsAt(pos).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.passability == Traversability.Impassable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x0019E878 File Offset: 0x0019CA78
		public void DrawGhost(IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool placingMode = false, Thing thingToIgnore = null, Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			Map currentMap = Find.CurrentMap;
			bool flag = false;
			using (List<SketchEntity>.Enumerator enumerator = this.Entities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.OccupiedRect.MovedBy(offset).InBounds(currentMap))
					{
						flag = true;
						break;
					}
				}
			}
			foreach (SketchBuildable sketchBuildable in this.Buildables)
			{
				Thing spawnedBlueprintOrFrame = sketchBuildable.GetSpawnedBlueprintOrFrame(sketchBuildable.pos + offset, currentMap);
				SketchThing sketchThing;
				if (spawnedBlueprintOrFrame != null)
				{
					Sketch.tmpSketchThings.Add(spawnedBlueprintOrFrame);
				}
				else if ((sketchThing = (sketchBuildable as SketchThing)) != null)
				{
					Thing sameSpawned = sketchThing.GetSameSpawned(sketchThing.pos + offset, currentMap);
					if (sameSpawned != null)
					{
						Sketch.tmpSketchThings.Add(sameSpawned);
					}
				}
			}
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(1).ClipInsideMap(Find.CurrentMap);
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				if ((placingMode || !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap)) && sketchEntity.OccupiedRect.MovedBy(offset).InBounds(currentMap))
				{
					Color color = (flag || (sketchEntity.IsSpawningBlocked(sketchEntity.pos + offset, currentMap, thingToIgnore, false) && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap)) || (validator != null && !validator(sketchEntity, offset, Sketch.tmpSketchThings, Find.CurrentMap))) ? Sketch.BlockedColor : Sketch.GhostColor;
					if (cellRect.Contains(sketchEntity.pos + offset))
					{
						sketchEntity.DrawGhost(sketchEntity.pos + offset, color);
					}
				}
			}
			Sketch.tmpSketchThings.Clear();
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x0019EAC4 File Offset: 0x0019CCC4
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, int, bool> processor, int maxCellsToProcess = 2147483647, CellRect? bounds = null, IEnumerable<IntVec3> extraRoots = null)
		{
			if (this.floodFillWorking)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			this.floodFillWorking = true;
			if (this.floodFillOpenSet == null)
			{
				this.floodFillOpenSet = new Queue<IntVec3>();
			}
			if (this.floodFillTraversalDistance == null)
			{
				this.floodFillTraversalDistance = new Dictionary<IntVec3, int>();
			}
			this.floodFillTraversalDistance.Clear();
			this.floodFillOpenSet.Clear();
			if (root.IsValid && extraRoots == null && !passCheck(root))
			{
				this.floodFillWorking = false;
				return;
			}
			if (bounds == null)
			{
				bounds = new CellRect?(this.OccupiedRect);
			}
			int area = bounds.Value.Area;
			IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
			int num = cardinalDirectionsAround.Length;
			int num2 = 0;
			if (root.IsValid)
			{
				this.floodFillTraversalDistance.Add(root, 0);
				this.floodFillOpenSet.Enqueue(root);
			}
			if (extraRoots == null)
			{
				goto IL_23C;
			}
			IList<IntVec3> list = extraRoots as IList<IntVec3>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.floodFillTraversalDistance.SetOrAdd(list[i], 0);
					this.floodFillOpenSet.Enqueue(list[i]);
				}
				goto IL_23C;
			}
			using (IEnumerator<IntVec3> enumerator = extraRoots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec = enumerator.Current;
					this.floodFillTraversalDistance.SetOrAdd(intVec, 0);
					this.floodFillOpenSet.Enqueue(intVec);
				}
				goto IL_23C;
			}
			IL_169:
			IntVec3 intVec2 = this.floodFillOpenSet.Dequeue();
			int num3 = this.floodFillTraversalDistance[intVec2];
			if (processor(intVec2, num3))
			{
				goto IL_24D;
			}
			num2++;
			if (num2 == maxCellsToProcess)
			{
				goto IL_24D;
			}
			for (int j = 0; j < num; j++)
			{
				IntVec3 intVec3 = intVec2 + cardinalDirectionsAround[j];
				if (bounds.Value.Contains(intVec3) && !this.floodFillTraversalDistance.ContainsKey(intVec3) && passCheck(intVec3))
				{
					this.floodFillOpenSet.Enqueue(intVec3);
					this.floodFillTraversalDistance.Add(intVec3, num3 + 1);
				}
			}
			if (this.floodFillOpenSet.Count > area)
			{
				Log.Error("Overflow on flood fill (>" + area + " cells). Make sure we're not flooding over the same area after we check it.");
				this.floodFillWorking = false;
				return;
			}
			IL_23C:
			if (this.floodFillOpenSet.Count > 0)
			{
				goto IL_169;
			}
			IL_24D:
			this.floodFillWorking = false;
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x0019ED38 File Offset: 0x0019CF38
		private IEnumerable<IntVec3> GetSuggestedRoofCells()
		{
			if (this.Empty)
			{
				yield break;
			}
			CellRect occupiedRect = this.OccupiedRect;
			Sketch.tmpSuggestedRoofCellsVisited.Clear();
			Sketch.tmpYieldedSuggestedRoofCells.Clear();
			foreach (IntVec3 intVec in this.OccupiedRect)
			{
				if (!Sketch.tmpSuggestedRoofCellsVisited.Contains(intVec) && !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|81_0(intVec))
				{
					Sketch.tmpSuggestedRoofCells.Clear();
					this.FloodFill(intVec, (IntVec3 c) => !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|81_0(c), delegate(IntVec3 c, int dist)
					{
						Sketch.tmpSuggestedRoofCellsVisited.Add(c);
						Sketch.tmpSuggestedRoofCells.Add(c);
						return false;
					}, int.MaxValue, null, null);
					bool flag = false;
					for (int k = 0; k < Sketch.tmpSuggestedRoofCells.Count; k++)
					{
						if (occupiedRect.IsOnEdge(Sketch.tmpSuggestedRoofCells[k]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num;
						for (int i = 0; i < Sketch.tmpSuggestedRoofCells.Count; i = num + 1)
						{
							for (int j = 0; j < 9; j = num + 1)
							{
								IntVec3 intVec2 = Sketch.tmpSuggestedRoofCells[i] + GenAdj.AdjacentCellsAndInside[j];
								if (!Sketch.tmpYieldedSuggestedRoofCells.Contains(intVec2) && occupiedRect.Contains(intVec2) && (j == 8 || this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|81_0(intVec2)))
								{
									Sketch.tmpYieldedSuggestedRoofCells.Add(intVec2);
									yield return intVec2;
								}
								num = j;
							}
							num = i;
						}
					}
				}
			}
			Sketch.tmpSuggestedRoofCellsVisited.Clear();
			Sketch.tmpYieldedSuggestedRoofCells.Clear();
			yield break;
			yield break;
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x0019ED48 File Offset: 0x0019CF48
		private IntVec3 GetOffset(IntVec3 pos, Sketch.SpawnPosType posType)
		{
			IntVec3 intVec;
			switch (posType)
			{
			case Sketch.SpawnPosType.Unchanged:
				intVec = IntVec3.Zero;
				break;
			case Sketch.SpawnPosType.OccupiedCenter:
				intVec = new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z);
				break;
			case Sketch.SpawnPosType.OccupiedBotLeft:
				intVec = new IntVec3(-this.OccupiedRect.minX, 0, -this.OccupiedRect.minZ);
				break;
			default:
				intVec = default(IntVec3);
				break;
			}
			intVec += pos;
			return intVec;
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x0019EDC8 File Offset: 0x0019CFC8
		public void Rotate(Rot4 rot)
		{
			if (rot == this.rotation)
			{
				return;
			}
			int num = rot.AsInt - this.rotation.AsInt;
			if (num < 0)
			{
				num += 4;
			}
			Rot4 rot2 = new Rot4(num);
			this.rotation = rot;
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos = sketchEntity.pos.RotatedBy(rot2);
				SketchThing sketchThing;
				if ((sketchThing = (sketchEntity as SketchThing)) != null)
				{
					int asInt = rot2.AsInt;
					if (sketchThing.def.rotatable)
					{
						RotationDirection rotDir = RotationDirection.None;
						int num2 = 1;
						if (asInt == 1)
						{
							rotDir = RotationDirection.Clockwise;
						}
						else if (asInt == 2)
						{
							rotDir = RotationDirection.Clockwise;
							num2 = 2;
						}
						else if (asInt == 3)
						{
							rotDir = RotationDirection.Counterclockwise;
						}
						for (int i = 0; i < num2; i++)
						{
							sketchThing.rot.Rotate(rotDir);
						}
					}
					else if (asInt == 1 && sketchThing.def.size.z % 2 == 0)
					{
						SketchEntity sketchEntity2 = sketchEntity;
						sketchEntity2.pos.z = sketchEntity2.pos.z - 1;
					}
					else if (rot2.AsInt == 2)
					{
						if (sketchThing.def.size.x % 2 == 0)
						{
							SketchEntity sketchEntity3 = sketchEntity;
							sketchEntity3.pos.x = sketchEntity3.pos.x - 1;
						}
						if (sketchThing.def.size.z % 2 == 0)
						{
							SketchEntity sketchEntity4 = sketchEntity;
							sketchEntity4.pos.z = sketchEntity4.pos.z - 1;
						}
					}
					else if (asInt == 3 && sketchThing.def.size.x % 2 == 0)
					{
						SketchEntity sketchEntity5 = sketchEntity;
						sketchEntity5.pos.x = sketchEntity5.pos.x - 1;
					}
				}
			}
			this.RecacheAll();
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x0019EF98 File Offset: 0x0019D198
		public void ExposeData()
		{
			Scribe_Collections.Look<SketchEntity>(ref this.entities, "entities", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", Rot4.North, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.entities.RemoveAll((SketchEntity x) => x == null) != 0)
				{
					Log.Error("Some sketch entities were null after loading.");
				}
				if (this.entities.RemoveAll((SketchEntity x) => x.LostImportantReferences) != 0)
				{
					Log.Error("Some sketch entities had null defs after loading.");
				}
				this.RecacheAll();
				Sketch.tmpToRemove.Clear();
				for (int i = 0; i < this.cachedThings.Count; i++)
				{
					if (this.cachedThings[i].def.IsDoor)
					{
						for (int j = 0; j < this.cachedThings.Count; j++)
						{
							if (this.cachedThings[j].def == ThingDefOf.Wall && this.cachedThings[j].pos == this.cachedThings[i].pos)
							{
								Sketch.tmpToRemove.Add(this.cachedThings[j]);
							}
						}
					}
				}
				for (int k = 0; k < Sketch.tmpToRemove.Count; k++)
				{
					Log.Error("Sketch has a wall and a door in the same cell. Fixing.");
					this.Remove(Sketch.tmpToRemove[k]);
				}
				Sketch.tmpToRemove.Clear();
			}
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x0019F23C File Offset: 0x0019D43C
		[CompilerGenerated]
		private bool <GetSuggestedRoofCells>g__AnyRoofHolderAt|81_0(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing != null && sketchThing.def.holdsRoof;
		}

		// Token: 0x04002EB3 RID: 11955
		private List<SketchEntity> entities = new List<SketchEntity>();

		// Token: 0x04002EB4 RID: 11956
		private List<SketchThing> cachedThings = new List<SketchThing>();

		// Token: 0x04002EB5 RID: 11957
		private List<SketchTerrain> cachedTerrain = new List<SketchTerrain>();

		// Token: 0x04002EB6 RID: 11958
		private List<SketchBuildable> cachedBuildables = new List<SketchBuildable>();

		// Token: 0x04002EB7 RID: 11959
		private Dictionary<IntVec3, SketchTerrain> terrainAt = new Dictionary<IntVec3, SketchTerrain>();

		// Token: 0x04002EB8 RID: 11960
		private Dictionary<IntVec3, SketchThing> edificeAt = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x04002EB9 RID: 11961
		private Dictionary<IntVec3, SketchThing> thingsAt_single = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x04002EBA RID: 11962
		private Dictionary<IntVec3, List<SketchThing>> thingsAt_multiple = new Dictionary<IntVec3, List<SketchThing>>();

		// Token: 0x04002EBB RID: 11963
		private bool occupiedRectDirty = true;

		// Token: 0x04002EBC RID: 11964
		private CellRect cachedOccupiedRect;

		// Token: 0x04002EBD RID: 11965
		private Rot4 rotation = Rot4.North;

		// Token: 0x04002EBE RID: 11966
		private bool floodFillWorking;

		// Token: 0x04002EBF RID: 11967
		private Queue<IntVec3> floodFillOpenSet;

		// Token: 0x04002EC0 RID: 11968
		private Dictionary<IntVec3, int> floodFillTraversalDistance;

		// Token: 0x04002EC1 RID: 11969
		public const float SpawnOrder_Terrain = 1f;

		// Token: 0x04002EC2 RID: 11970
		public const float SpawnOrder_Thing = 2f;

		// Token: 0x04002EC3 RID: 11971
		private static readonly List<SketchThing> EmptySketchThingList = new List<SketchThing>();

		// Token: 0x04002EC4 RID: 11972
		private static readonly Color GhostColor = new Color(0.7f, 0.7f, 0.7f, 0.35f);

		// Token: 0x04002EC5 RID: 11973
		private static readonly Color BlockedColor = new Color(0.8f, 0.2f, 0.2f, 0.35f);

		// Token: 0x04002EC6 RID: 11974
		private static List<Thing> tmpSketchThings = new List<Thing>();

		// Token: 0x04002EC7 RID: 11975
		private static HashSet<IntVec3> tmpSuggestedRoofCellsVisited = new HashSet<IntVec3>();

		// Token: 0x04002EC8 RID: 11976
		private static List<IntVec3> tmpSuggestedRoofCells = new List<IntVec3>();

		// Token: 0x04002EC9 RID: 11977
		private static HashSet<IntVec3> tmpYieldedSuggestedRoofCells = new HashSet<IntVec3>();

		// Token: 0x04002ECA RID: 11978
		private static List<SketchThing> tmpToRemove = new List<SketchThing>();

		// Token: 0x020021E3 RID: 8675
		public enum SpawnPosType
		{
			// Token: 0x0400815B RID: 33115
			Unchanged,
			// Token: 0x0400815C RID: 33116
			OccupiedCenter,
			// Token: 0x0400815D RID: 33117
			OccupiedBotLeft
		}

		// Token: 0x020021E4 RID: 8676
		public enum SpawnMode
		{
			// Token: 0x0400815F RID: 33119
			Blueprint,
			// Token: 0x04008160 RID: 33120
			Normal,
			// Token: 0x04008161 RID: 33121
			TransportPod
		}
	}
}
