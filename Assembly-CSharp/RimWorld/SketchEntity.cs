using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132D RID: 4909
	public abstract class SketchEntity : IExposable
	{
		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06006A55 RID: 27221
		public abstract string Label { get; }

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06006A56 RID: 27222
		public abstract string LabelCap { get; }

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06006A57 RID: 27223
		public abstract CellRect OccupiedRect { get; }

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06006A58 RID: 27224
		public abstract float SpawnOrder { get; }

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06006A59 RID: 27225 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool LostImportantReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006A5A RID: 27226
		public abstract void DrawGhost(IntVec3 at, Color color);

		// Token: 0x06006A5B RID: 27227
		public abstract bool IsSameSpawned(IntVec3 at, Map map);

		// Token: 0x06006A5C RID: 27228
		public abstract bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x06006A5D RID: 27229
		public abstract bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06006A5E RID: 27230
		public abstract bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06006A5F RID: 27231
		public abstract bool CanBuildOnTerrain(IntVec3 at, Map map);

		// Token: 0x06006A60 RID: 27232
		public abstract bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false);

		// Token: 0x06006A61 RID: 27233
		public abstract bool SameForSubtracting(SketchEntity other);

		// Token: 0x06006A62 RID: 27234 RVA: 0x0020D27C File Offset: 0x0020B47C
		[Obsolete("Only used for mod compatibility")]
		public bool SpawnNear(IntVec3 near, Map map, float radius, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false)
		{
			return this.SpawnNear_NewTmp(near, map, radius, faction, spawnMode, wipeIfCollides, spawnedThings, dormant, null);
		}

		// Token: 0x06006A63 RID: 27235 RVA: 0x0020D2A0 File Offset: 0x0020B4A0
		public bool SpawnNear_NewTmp(IntVec3 near, Map map, float radius, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false, Func<SketchEntity, IntVec3, bool> validator = null)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = near + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && (validator == null || validator(this, intVec)) && this.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, spawnedThings, dormant))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006A64 RID: 27236 RVA: 0x00048524 File Offset: 0x00046724
		public virtual SketchEntity DeepCopy()
		{
			SketchEntity sketchEntity = (SketchEntity)Activator.CreateInstance(base.GetType());
			sketchEntity.pos = this.pos;
			return sketchEntity;
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x0020D304 File Offset: 0x0020B504
		public virtual void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
		}

		// Token: 0x040046D6 RID: 18134
		public IntVec3 pos;
	}
}
