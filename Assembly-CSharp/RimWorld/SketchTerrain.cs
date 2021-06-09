using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132E RID: 4910
	public class SketchTerrain : SketchBuildable
	{
		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06006A67 RID: 27239 RVA: 0x00048542 File Offset: 0x00046742
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06006A68 RID: 27240 RVA: 0x0004854A File Offset: 0x0004674A
		public override ThingDef Stuff
		{
			get
			{
				return this.stuffForComparingSimilar;
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06006A69 RID: 27241 RVA: 0x00048552 File Offset: 0x00046752
		public override CellRect OccupiedRect
		{
			get
			{
				return CellRect.SingleCell(this.pos);
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06006A6A RID: 27242 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public override float SpawnOrder
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06006A6B RID: 27243 RVA: 0x0020D32C File Offset: 0x0020B52C
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

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06006A6C RID: 27244 RVA: 0x0004855F File Offset: 0x0004675F
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x06006A6D RID: 27245 RVA: 0x0020D37C File Offset: 0x0020B57C
		public override void DrawGhost(IntVec3 at, Color color)
		{
			ThingDef blueprintDef = this.def.blueprintDef;
			GraphicDatabase.Get(blueprintDef.graphic.GetType(), blueprintDef.graphic.path, blueprintDef.graphic.Shader, blueprintDef.graphic.drawSize, color, Color.white, blueprintDef.graphicData, null).DrawFromDef(at.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), Rot4.North, this.def.blueprintDef, 0f);
		}

		// Token: 0x06006A6E RID: 27246 RVA: 0x0004856C File Offset: 0x0004676C
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return at.InBounds(map) && this.IsSameOrSimilar(at.GetTerrain(map));
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x0020D3F8 File Offset: 0x0020B5F8
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

		// Token: 0x06006A70 RID: 27248 RVA: 0x0020D470 File Offset: 0x0020B670
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

		// Token: 0x06006A71 RID: 27249 RVA: 0x0020D4D8 File Offset: 0x0020B6D8
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, Rot4.North, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, Rot4.North, map, wipeIfCollides, thingToIgnore, null, null).Accepted;
		}

		// Token: 0x06006A72 RID: 27250 RVA: 0x00048586 File Offset: 0x00046786
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x06006A73 RID: 27251 RVA: 0x000485AC File Offset: 0x000467AC
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, Rot4.North, null, null);
		}

		// Token: 0x06006A74 RID: 27252 RVA: 0x0020D53C File Offset: 0x0020B73C
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

		// Token: 0x06006A75 RID: 27253 RVA: 0x0020D5AC File Offset: 0x0020B7AC
		private TerrainDef GetDefFromStuff()
		{
			if (this.stuffForComparingSimilar == null)
			{
				return this.def;
			}
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (this.IsSameOrSimilar(terrainDef) && !terrainDef.costList.NullOrEmpty<ThingDefCountClass>() && terrainDef.costList[0].thingDef == this.stuffForComparingSimilar)
				{
					return terrainDef;
				}
			}
			return this.def;
		}

		// Token: 0x06006A76 RID: 27254 RVA: 0x0020D63C File Offset: 0x0020B83C
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchTerrain sketchTerrain = other as SketchTerrain;
			return sketchTerrain != null && (sketchTerrain == this || (this.IsSameOrSimilar(sketchTerrain.Buildable) && this.pos == sketchTerrain.pos));
		}

		// Token: 0x06006A77 RID: 27255 RVA: 0x000485C2 File Offset: 0x000467C2
		public override SketchEntity DeepCopy()
		{
			SketchTerrain sketchTerrain = (SketchTerrain)base.DeepCopy();
			sketchTerrain.def = this.def;
			sketchTerrain.stuffForComparingSimilar = this.stuffForComparingSimilar;
			return sketchTerrain;
		}

		// Token: 0x06006A78 RID: 27256 RVA: 0x000485E7 File Offset: 0x000467E7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuffForComparingSimilar, "stuff");
			Scribe_Values.Look<bool>(ref this.treatSimilarAsSame, "treatSimilarAsSame", false, false);
		}

		// Token: 0x040046D7 RID: 18135
		public TerrainDef def;

		// Token: 0x040046D8 RID: 18136
		public ThingDef stuffForComparingSimilar;

		// Token: 0x040046D9 RID: 18137
		public bool treatSimilarAsSame;
	}
}
