using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001327 RID: 4903
	public class Sketch : IExposable
	{
		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06006A02 RID: 27138 RVA: 0x00048348 File Offset: 0x00046548
		public List<SketchEntity> Entities
		{
			get
			{
				return this.entities;
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06006A03 RID: 27139 RVA: 0x00048350 File Offset: 0x00046550
		public List<SketchThing> Things
		{
			get
			{
				return this.cachedThings;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x00048358 File Offset: 0x00046558
		public List<SketchTerrain> Terrain
		{
			get
			{
				return this.cachedTerrain;
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06006A05 RID: 27141 RVA: 0x00048360 File Offset: 0x00046560
		public List<SketchBuildable> Buildables
		{
			get
			{
				return this.cachedBuildables;
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06006A06 RID: 27142 RVA: 0x0020B178 File Offset: 0x00209378
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

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06006A07 RID: 27143 RVA: 0x0020B25C File Offset: 0x0020945C
		public IntVec2 OccupiedSize
		{
			get
			{
				return new IntVec2(this.OccupiedRect.Width, this.OccupiedRect.Height);
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06006A08 RID: 27144 RVA: 0x0020B28C File Offset: 0x0020948C
		public IntVec3 OccupiedCenter
		{
			get
			{
				return this.OccupiedRect.CenterCell;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06006A09 RID: 27145 RVA: 0x00048368 File Offset: 0x00046568
		public bool Empty
		{
			get
			{
				return !this.Entities.Any<SketchEntity>();
			}
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x0020B2A8 File Offset: 0x002094A8
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

		// Token: 0x06006A0B RID: 27147 RVA: 0x0020B39C File Offset: 0x0020959C
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

		// Token: 0x06006A0C RID: 27148 RVA: 0x0020B3F0 File Offset: 0x002095F0
		public bool AddTerrain(TerrainDef def, IntVec3 pos, bool wipeIfCollides = true)
		{
			return this.Add(new SketchTerrain
			{
				def = def,
				pos = pos
			}, wipeIfCollides);
		}

		// Token: 0x06006A0D RID: 27149 RVA: 0x00048378 File Offset: 0x00046578
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

		// Token: 0x06006A0E RID: 27150 RVA: 0x0020B41C File Offset: 0x0020961C
		public bool RemoveTerrain(IntVec3 cell)
		{
			SketchTerrain entity;
			return this.terrainAt.TryGetValue(cell, out entity) && this.Remove(entity);
		}

		// Token: 0x06006A0F RID: 27151 RVA: 0x000483A4 File Offset: 0x000465A4
		public void Clear()
		{
			this.entities.Clear();
			this.RecacheAll();
		}

		// Token: 0x06006A10 RID: 27152 RVA: 0x0020B444 File Offset: 0x00209644
		public TerrainDef TerrainAt(IntVec3 pos)
		{
			SketchTerrain sketchTerrain = this.SketchTerrainAt(pos);
			if (sketchTerrain == null)
			{
				return null;
			}
			return sketchTerrain.def;
		}

		// Token: 0x06006A11 RID: 27153 RVA: 0x0020B464 File Offset: 0x00209664
		public SketchTerrain SketchTerrainAt(IntVec3 pos)
		{
			SketchTerrain result;
			if (!this.terrainAt.TryGetValue(pos, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06006A12 RID: 27154 RVA: 0x000483B7 File Offset: 0x000465B7
		public bool AnyTerrainAt(int x, int z)
		{
			return this.AnyTerrainAt(new IntVec3(x, 0, z));
		}

		// Token: 0x06006A13 RID: 27155 RVA: 0x000483C7 File Offset: 0x000465C7
		public bool AnyTerrainAt(IntVec3 pos)
		{
			return this.TerrainAt(pos) != null;
		}

		// Token: 0x06006A14 RID: 27156 RVA: 0x0020B484 File Offset: 0x00209684
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

		// Token: 0x06006A15 RID: 27157 RVA: 0x0020B4C0 File Offset: 0x002096C0
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

		// Token: 0x06006A16 RID: 27158 RVA: 0x0020B504 File Offset: 0x00209704
		public SketchThing EdificeAt(IntVec3 pos)
		{
			SketchThing result;
			if (this.edificeAt.TryGetValue(pos, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06006A17 RID: 27159 RVA: 0x0020B524 File Offset: 0x00209724
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

		// Token: 0x06006A18 RID: 27160 RVA: 0x0020B584 File Offset: 0x00209784
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

		// Token: 0x06006A19 RID: 27161 RVA: 0x0020B684 File Offset: 0x00209884
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

		// Token: 0x06006A1A RID: 27162 RVA: 0x0020B714 File Offset: 0x00209914
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

		// Token: 0x06006A1B RID: 27163 RVA: 0x0020B76C File Offset: 0x0020996C
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

		// Token: 0x06006A1C RID: 27164 RVA: 0x0020B864 File Offset: 0x00209A64
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

		// Token: 0x06006A1D RID: 27165 RVA: 0x0020B918 File Offset: 0x00209B18
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

		// Token: 0x06006A1E RID: 27166 RVA: 0x0020B978 File Offset: 0x00209B78
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

		// Token: 0x06006A1F RID: 27167 RVA: 0x0020BA50 File Offset: 0x00209C50
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

		// Token: 0x06006A20 RID: 27168 RVA: 0x0020BD2C File Offset: 0x00209F2C
		public void Merge(Sketch other, bool wipeIfCollides = true)
		{
			foreach (SketchEntity sketchEntity in from x in other.entities
			orderby x.SpawnOrder
			select x)
			{
				this.Add(sketchEntity.DeepCopy(), wipeIfCollides);
			}
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x0020BDA4 File Offset: 0x00209FA4
		public void MergeAt(Sketch other, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool wipeIfCollides = true)
		{
			Sketch sketch = other.DeepCopy();
			sketch.MoveBy(sketch.GetOffset(pos, posType));
			this.Merge(sketch, wipeIfCollides);
		}

		// Token: 0x06006A22 RID: 27170 RVA: 0x0020BDD0 File Offset: 0x00209FD0
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

		// Token: 0x06006A23 RID: 27171 RVA: 0x000483D3 File Offset: 0x000465D3
		public void MoveBy(IntVec2 by)
		{
			this.MoveBy(by.ToIntVec3);
		}

		// Token: 0x06006A24 RID: 27172 RVA: 0x0020BE44 File Offset: 0x0020A044
		public void MoveBy(IntVec3 by)
		{
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos += by;
			}
			this.RecacheAll();
		}

		// Token: 0x06006A25 RID: 27173 RVA: 0x000483E2 File Offset: 0x000465E2
		public void MoveOccupiedCenterToZero()
		{
			this.MoveBy(new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z));
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x0020BEA8 File Offset: 0x0020A0A8
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

		// Token: 0x06006A27 RID: 27175 RVA: 0x0020BF28 File Offset: 0x0020A128
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

		// Token: 0x06006A28 RID: 27176 RVA: 0x0020C0BC File Offset: 0x0020A2BC
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

		// Token: 0x06006A29 RID: 27177 RVA: 0x00048408 File Offset: 0x00046608
		private void Recache(SketchEntity entity)
		{
			this.RemoveFromCache(entity);
			this.AddToCache(entity);
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x0020C250 File Offset: 0x0020A450
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

		// Token: 0x06006A2B RID: 27179 RVA: 0x0020C354 File Offset: 0x0020A554
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

		// Token: 0x06006A2C RID: 27180 RVA: 0x0020C3CC File Offset: 0x0020A5CC
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

		// Token: 0x06006A2D RID: 27181 RVA: 0x0020C450 File Offset: 0x0020A650
		public bool CanBeSeenOver(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing == null || sketchThing.def.Fillage != FillCategory.Full;
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x00048418 File Offset: 0x00046618
		public bool Passable(int x, int z)
		{
			return this.Passable(new IntVec3(x, 0, z));
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x0020C47C File Offset: 0x0020A67C
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

		// Token: 0x06006A30 RID: 27184 RVA: 0x00048428 File Offset: 0x00046628
		[Obsolete]
		public void DrawGhost(IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool placingMode = false, Thing thingToIgnore = null)
		{
			this.DrawGhost_NewTmp(pos, posType, placingMode, thingToIgnore, null);
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x0020C4EC File Offset: 0x0020A6EC
		public void DrawGhost_NewTmp(IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool placingMode = false, Thing thingToIgnore = null, Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null)
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

		// Token: 0x06006A32 RID: 27186 RVA: 0x0020C738 File Offset: 0x0020A938
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, int, bool> processor, int maxCellsToProcess = 2147483647, CellRect? bounds = null, IEnumerable<IntVec3> extraRoots = null)
		{
			if (this.floodFillWorking)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.", false);
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
				goto IL_23E;
			}
			IList<IntVec3> list = extraRoots as IList<IntVec3>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.floodFillTraversalDistance.SetOrAdd(list[i], 0);
					this.floodFillOpenSet.Enqueue(list[i]);
				}
				goto IL_23E;
			}
			using (IEnumerator<IntVec3> enumerator = extraRoots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec = enumerator.Current;
					this.floodFillTraversalDistance.SetOrAdd(intVec, 0);
					this.floodFillOpenSet.Enqueue(intVec);
				}
				goto IL_23E;
			}
			IL_16A:
			IntVec3 intVec2 = this.floodFillOpenSet.Dequeue();
			int num3 = this.floodFillTraversalDistance[intVec2];
			if (processor(intVec2, num3))
			{
				goto IL_24F;
			}
			num2++;
			if (num2 == maxCellsToProcess)
			{
				goto IL_24F;
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
				Log.Error("Overflow on flood fill (>" + area + " cells). Make sure we're not flooding over the same area after we check it.", false);
				this.floodFillWorking = false;
				return;
			}
			IL_23E:
			if (this.floodFillOpenSet.Count > 0)
			{
				goto IL_16A;
			}
			IL_24F:
			this.floodFillWorking = false;
		}

		// Token: 0x06006A33 RID: 27187 RVA: 0x00048436 File Offset: 0x00046636
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
				if (!Sketch.tmpSuggestedRoofCellsVisited.Contains(intVec) && !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|82_0(intVec))
				{
					Sketch.tmpSuggestedRoofCells.Clear();
					this.FloodFill(intVec, (IntVec3 c) => !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|82_0(c), delegate(IntVec3 c, int dist)
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
								if (!Sketch.tmpYieldedSuggestedRoofCells.Contains(intVec2) && occupiedRect.Contains(intVec2) && (j == 8 || this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|82_0(intVec2)))
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

		// Token: 0x06006A34 RID: 27188 RVA: 0x0020C9AC File Offset: 0x0020ABAC
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

		// Token: 0x06006A35 RID: 27189 RVA: 0x0020CA2C File Offset: 0x0020AC2C
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

		// Token: 0x06006A36 RID: 27190 RVA: 0x0020CBFC File Offset: 0x0020ADFC
		public void ExposeData()
		{
			Scribe_Collections.Look<SketchEntity>(ref this.entities, "entities", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", Rot4.North, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.entities.RemoveAll((SketchEntity x) => x == null) != 0)
				{
					Log.Error("Some sketch entities were null after loading.", false);
				}
				if (this.entities.RemoveAll((SketchEntity x) => x.LostImportantReferences) != 0)
				{
					Log.Error("Some sketch entities had null defs after loading.", false);
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
					Log.Error("Sketch has a wall and a door in the same cell. Fixing.", false);
					this.Remove(Sketch.tmpToRemove[k]);
				}
				Sketch.tmpToRemove.Clear();
			}
		}

		// Token: 0x06006A39 RID: 27193 RVA: 0x0020CEA4 File Offset: 0x0020B0A4
		[CompilerGenerated]
		private bool <GetSuggestedRoofCells>g__AnyRoofHolderAt|82_0(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing != null && sketchThing.def.holdsRoof;
		}

		// Token: 0x040046A6 RID: 18086
		private List<SketchEntity> entities = new List<SketchEntity>();

		// Token: 0x040046A7 RID: 18087
		private List<SketchThing> cachedThings = new List<SketchThing>();

		// Token: 0x040046A8 RID: 18088
		private List<SketchTerrain> cachedTerrain = new List<SketchTerrain>();

		// Token: 0x040046A9 RID: 18089
		private List<SketchBuildable> cachedBuildables = new List<SketchBuildable>();

		// Token: 0x040046AA RID: 18090
		private Dictionary<IntVec3, SketchTerrain> terrainAt = new Dictionary<IntVec3, SketchTerrain>();

		// Token: 0x040046AB RID: 18091
		private Dictionary<IntVec3, SketchThing> edificeAt = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x040046AC RID: 18092
		private Dictionary<IntVec3, SketchThing> thingsAt_single = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x040046AD RID: 18093
		private Dictionary<IntVec3, List<SketchThing>> thingsAt_multiple = new Dictionary<IntVec3, List<SketchThing>>();

		// Token: 0x040046AE RID: 18094
		private bool occupiedRectDirty = true;

		// Token: 0x040046AF RID: 18095
		private CellRect cachedOccupiedRect;

		// Token: 0x040046B0 RID: 18096
		private Rot4 rotation = Rot4.North;

		// Token: 0x040046B1 RID: 18097
		private bool floodFillWorking;

		// Token: 0x040046B2 RID: 18098
		private Queue<IntVec3> floodFillOpenSet;

		// Token: 0x040046B3 RID: 18099
		private Dictionary<IntVec3, int> floodFillTraversalDistance;

		// Token: 0x040046B4 RID: 18100
		public const float SpawnOrder_Terrain = 1f;

		// Token: 0x040046B5 RID: 18101
		public const float SpawnOrder_Thing = 2f;

		// Token: 0x040046B6 RID: 18102
		private static readonly List<SketchThing> EmptySketchThingList = new List<SketchThing>();

		// Token: 0x040046B7 RID: 18103
		private static readonly Color GhostColor = new Color(0.7f, 0.7f, 0.7f, 0.35f);

		// Token: 0x040046B8 RID: 18104
		private static readonly Color BlockedColor = new Color(0.8f, 0.2f, 0.2f, 0.35f);

		// Token: 0x040046B9 RID: 18105
		private static List<Thing> tmpSketchThings = new List<Thing>();

		// Token: 0x040046BA RID: 18106
		private static HashSet<IntVec3> tmpSuggestedRoofCellsVisited = new HashSet<IntVec3>();

		// Token: 0x040046BB RID: 18107
		private static List<IntVec3> tmpSuggestedRoofCells = new List<IntVec3>();

		// Token: 0x040046BC RID: 18108
		private static HashSet<IntVec3> tmpYieldedSuggestedRoofCells = new HashSet<IntVec3>();

		// Token: 0x040046BD RID: 18109
		private static List<SketchThing> tmpToRemove = new List<SketchThing>();

		// Token: 0x02001328 RID: 4904
		public enum SpawnPosType
		{
			// Token: 0x040046BF RID: 18111
			Unchanged,
			// Token: 0x040046C0 RID: 18112
			OccupiedCenter,
			// Token: 0x040046C1 RID: 18113
			OccupiedBotLeft
		}

		// Token: 0x02001329 RID: 4905
		public enum SpawnMode
		{
			// Token: 0x040046C3 RID: 18115
			Blueprint,
			// Token: 0x040046C4 RID: 18116
			Normal,
			// Token: 0x040046C5 RID: 18117
			TransportPod
		}
	}
}
