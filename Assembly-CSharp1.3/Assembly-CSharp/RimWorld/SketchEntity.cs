using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF8 RID: 3320
	public abstract class SketchEntity : IExposable
	{
		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06004D6D RID: 19821
		public abstract string Label { get; }

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06004D6E RID: 19822
		public abstract string LabelCap { get; }

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06004D6F RID: 19823
		public abstract CellRect OccupiedRect { get; }

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06004D70 RID: 19824
		public abstract float SpawnOrder { get; }

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06004D71 RID: 19825 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool LostImportantReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004D72 RID: 19826
		public abstract void DrawGhost(IntVec3 at, Color color);

		// Token: 0x06004D73 RID: 19827
		public abstract bool IsSameSpawned(IntVec3 at, Map map);

		// Token: 0x06004D74 RID: 19828
		public abstract bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x06004D75 RID: 19829
		public abstract bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06004D76 RID: 19830
		public abstract bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06004D77 RID: 19831
		public abstract bool CanBuildOnTerrain(IntVec3 at, Map map);

		// Token: 0x06004D78 RID: 19832
		public abstract bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false);

		// Token: 0x06004D79 RID: 19833
		public abstract bool SameForSubtracting(SketchEntity other);

		// Token: 0x06004D7A RID: 19834 RVA: 0x0019F37C File Offset: 0x0019D57C
		public bool SpawnNear(IntVec3 near, Map map, float radius, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false, Func<SketchEntity, IntVec3, bool> validator = null)
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

		// Token: 0x06004D7B RID: 19835 RVA: 0x0019F3DD File Offset: 0x0019D5DD
		public virtual SketchEntity DeepCopy()
		{
			SketchEntity sketchEntity = (SketchEntity)Activator.CreateInstance(base.GetType());
			sketchEntity.pos = this.pos;
			return sketchEntity;
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0019F3FC File Offset: 0x0019D5FC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
		}

		// Token: 0x04002ECB RID: 11979
		public IntVec3 pos;
	}
}
