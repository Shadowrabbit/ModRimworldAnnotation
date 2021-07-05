using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF9 RID: 3321
	public class SketchTerrain : SketchBuildable
	{
		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06004D7E RID: 19838 RVA: 0x0019F423 File Offset: 0x0019D623
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06004D7F RID: 19839 RVA: 0x0019F42B File Offset: 0x0019D62B
		public override ThingDef Stuff
		{
			get
			{
				return this.stuffForComparingSimilar;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06004D80 RID: 19840 RVA: 0x0019F433 File Offset: 0x0019D633
		public override CellRect OccupiedRect
		{
			get
			{
				return CellRect.SingleCell(this.pos);
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06004D81 RID: 19841 RVA: 0x0001F15E File Offset: 0x0001D35E
		public override float SpawnOrder
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06004D82 RID: 19842 RVA: 0x0019F440 File Offset: 0x0019D640
		public override string Label
		{
			get
			{
				if (this.def.designatorDropdown == null || this.def.designatorDropdown.label.NullOrEmpty() || !this.treatSimilarAsSame)
				{
					return base.Label;
				}
				return this.def.designatorDropdown.label;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06004D83 RID: 19843 RVA: 0x0019F490 File Offset: 0x0019D690
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0019F4A0 File Offset: 0x0019D6A0
		public override void DrawGhost(IntVec3 at, Color color)
		{
			ThingDef blueprintDef = this.def.blueprintDef;
			GraphicDatabase.Get(blueprintDef.graphic.GetType(), blueprintDef.graphic.path, blueprintDef.graphic.Shader, blueprintDef.graphic.drawSize, color, Color.white, blueprintDef.graphicData, null, null).DrawFromDef(at.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), Rot4.North, this.def.blueprintDef, 0f);
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0019F51B File Offset: 0x0019D71B
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return at.InBounds(map) && this.IsSameOrSimilar(at.GetTerrain(map));
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0019F538 File Offset: 0x0019D738
		public bool IsSameOrSimilar(BuildableDef other)
		{
			if (other == null)
			{
				return false;
			}
			if (!this.treatSimilarAsSame)
			{
				return other == this.def;
			}
			if (this.def.designatorDropdown == null && other.designatorDropdown == null && other.BuildableByPlayer)
			{
				return true;
			}
			if (this.def.designatorDropdown == null || other.designatorDropdown == null)
			{
				return other == this.def;
			}
			return other.designatorDropdown == this.def.designatorDropdown;
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0019F5B0 File Offset: 0x0019D7B0
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].Position == at && this.IsSameOrSimilar(thingList[i].def.entityDefToBuild))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0019F618 File Offset: 0x0019D818
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, Rot4.North, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, Rot4.North, map, wipeIfCollides, thingToIgnore, null, null).Accepted;
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0019F67B File Offset: 0x0019D87B
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0019F6A1 File Offset: 0x0019D8A1
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, Rot4.North, null, null);
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0019F6B8 File Offset: 0x0019D8B8
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false)
		{
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			if (spawnMode != Sketch.SpawnMode.Blueprint)
			{
				if (spawnMode != Sketch.SpawnMode.Normal)
				{
					throw new NotImplementedException("Spawn mode " + spawnMode + " not implemented!");
				}
				map.terrainGrid.SetTerrain(at, this.GetDefFromStuff());
			}
			else
			{
				GenConstruct.PlaceBlueprintForBuild(this.GetDefFromStuff(), at, map, Rot4.North, faction, null);
			}
			return true;
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x0019F728 File Offset: 0x0019D928
		private TerrainDef GetDefFromStuff()
		{
			if (this.stuffForComparingSimilar == null)
			{
				return this.def;
			}
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (this.IsSameOrSimilar(terrainDef) && !terrainDef.CostList.NullOrEmpty<ThingDefCountClass>() && terrainDef.CostList[0].thingDef == this.stuffForComparingSimilar)
				{
					return terrainDef;
				}
			}
			return this.def;
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0019F7B8 File Offset: 0x0019D9B8
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchTerrain sketchTerrain = other as SketchTerrain;
			return sketchTerrain != null && (sketchTerrain == this || (this.IsSameOrSimilar(sketchTerrain.Buildable) && this.pos == sketchTerrain.pos));
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0019F7F8 File Offset: 0x0019D9F8
		public override SketchEntity DeepCopy()
		{
			SketchTerrain sketchTerrain = (SketchTerrain)base.DeepCopy();
			sketchTerrain.def = this.def;
			sketchTerrain.stuffForComparingSimilar = this.stuffForComparingSimilar;
			return sketchTerrain;
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0019F81D File Offset: 0x0019DA1D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuffForComparingSimilar, "stuff");
			Scribe_Values.Look<bool>(ref this.treatSimilarAsSame, "treatSimilarAsSame", false, false);
		}

		// Token: 0x04002ECC RID: 11980
		public TerrainDef def;

		// Token: 0x04002ECD RID: 11981
		public ThingDef stuffForComparingSimilar;

		// Token: 0x04002ECE RID: 11982
		public bool treatSimilarAsSame;
	}
}
