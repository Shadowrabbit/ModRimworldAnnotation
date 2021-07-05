using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132F RID: 4911
	public class SketchThing : SketchBuildable
	{
		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06006A7A RID: 27258 RVA: 0x00048629 File Offset: 0x00046829
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06006A7B RID: 27259 RVA: 0x00048631 File Offset: 0x00046831
		public override ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06006A7C RID: 27260 RVA: 0x00048639 File Offset: 0x00046839
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.def, this.stuff, this.stackCount);
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06006A7D RID: 27261 RVA: 0x0004855F File Offset: 0x0004675F
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06006A7E RID: 27262 RVA: 0x00048652 File Offset: 0x00046852
		public override CellRect OccupiedRect
		{
			get
			{
				return GenAdj.OccupiedRect(this.pos, this.rot, this.def.size);
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06006A7F RID: 27263 RVA: 0x00048670 File Offset: 0x00046870
		public override float SpawnOrder
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06006A80 RID: 27264 RVA: 0x00048677 File Offset: 0x00046877
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.def.GetStatValueAbstract(StatDefOf.MaxHitPoints, this.stuff ?? GenStuff.DefaultStuffFor(this.def)));
			}
		}

		// Token: 0x06006A81 RID: 27265 RVA: 0x0020D67C File Offset: 0x0020B87C
		public Thing Instantiate()
		{
			Thing thing = ThingMaker.MakeThing(this.def, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
			thing.stackCount = this.stackCount;
			if (this.quality != null)
			{
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(this.quality.Value, ArtGenerationContext.Outsider);
				}
			}
			if (this.hitPoints != null)
			{
				thing.HitPoints = this.hitPoints.Value;
			}
			return thing;
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x000486A3 File Offset: 0x000468A3
		public override void DrawGhost(IntVec3 at, Color color)
		{
			GhostDrawer.DrawGhostThing_NewTmp(at, this.rot, this.def, this.def.graphic, color, AltitudeLayer.Blueprint, null, false);
		}

		// Token: 0x06006A83 RID: 27267 RVA: 0x0020D700 File Offset: 0x0020B900
		public Thing GetSameSpawned(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CellRect lhs = GenAdj.OccupiedRect(at, this.rot, thingList[i].def.Size);
				CellRect lhs2 = GenAdj.OccupiedRect(at, this.rot.Opposite, thingList[i].def.Size);
				CellRect rhs = thingList[i].OccupiedRect();
				if ((lhs == rhs || lhs2 == rhs) && thingList[i].def == this.def && (this.stuff == null || thingList[i].Stuff == this.stuff) && (thingList[i].Rotation == this.rot || thingList[i].Rotation == this.rot.Opposite || !this.def.rotatable))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x000486C7 File Offset: 0x000468C7
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return this.GetSameSpawned(at, map) != null;
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x0020D814 File Offset: 0x0020BA14
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CellRect lhs = GenAdj.OccupiedRect(at, this.rot, thingList[i].def.Size);
				CellRect lhs2 = GenAdj.OccupiedRect(at, this.rot.Opposite, thingList[i].def.Size);
				CellRect rhs = thingList[i].OccupiedRect();
				if ((lhs == rhs || lhs2 == rhs) && thingList[i].def.entityDefToBuild == this.def && (this.stuff == null || ((IConstructible)thingList[i]).EntityToBuildStuff() == this.stuff) && (thingList[i].Rotation == this.rot || thingList[i].Rotation == this.rot.Opposite || !this.def.rotatable))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x0020D934 File Offset: 0x0020BB34
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, this.rot, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, this.rot, map, wipeIfCollides, thingToIgnore, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def)).Accepted;
		}

		// Token: 0x06006A87 RID: 27271 RVA: 0x00048586 File Offset: 0x00046786
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x06006A88 RID: 27272 RVA: 0x000486D4 File Offset: 0x000468D4
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, this.rot, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x0020D9B0 File Offset: 0x0020BBB0
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, List<Thing> spawnedThings = null, bool dormant = false)
		{
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			switch (spawnMode)
			{
			case Sketch.SpawnMode.Blueprint:
				GenConstruct.PlaceBlueprintForBuild(this.def, at, map, this.rot, faction, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
				break;
			case Sketch.SpawnMode.Normal:
			{
				Thing thing = this.Instantiate();
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing);
				}
				if (faction != null)
				{
					thing.SetFactionDirect(faction);
				}
				this.SetDormant(thing, dormant);
				GenSpawn.Spawn(thing, at, map, this.rot, WipeMode.VanishOrMoveAside, false);
				break;
			}
			case Sketch.SpawnMode.TransportPod:
			{
				Thing thing2 = this.Instantiate();
				thing2.Position = at;
				thing2.Rotation = this.rot;
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing2);
				}
				if (faction != null)
				{
					thing2.SetFactionDirect(faction);
				}
				this.SetDormant(thing2, dormant);
				ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
				activeDropPodInfo.innerContainer.TryAdd(thing2, 1, true);
				activeDropPodInfo.openDelay = 60;
				activeDropPodInfo.leaveSlag = false;
				activeDropPodInfo.despawnPodBeforeSpawningThing = true;
				activeDropPodInfo.spawnWipeMode = (wipeIfCollides ? new WipeMode?(WipeMode.VanishOrMoveAside) : null);
				activeDropPodInfo.moveItemsAsideBeforeSpawning = true;
				activeDropPodInfo.setRotation = new Rot4?(this.rot);
				DropPodUtility.MakeDropPodAt(at, map, activeDropPodInfo);
				break;
			}
			default:
				throw new NotImplementedException("Spawn mode " + spawnMode + " not implemented!");
			}
			return true;
		}

		// Token: 0x06006A8A RID: 27274 RVA: 0x0020DB10 File Offset: 0x0020BD10
		private void SetDormant(Thing thing, bool dormant)
		{
			CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				if (dormant)
				{
					compCanBeDormant.ToSleep();
					return;
				}
				compCanBeDormant.WakeUp();
			}
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x0020DB38 File Offset: 0x0020BD38
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchThing sketchThing = other as SketchThing;
			if (sketchThing == null)
			{
				return false;
			}
			if (sketchThing == this)
			{
				return true;
			}
			if (this.def == sketchThing.def && this.stuff == sketchThing.stuff && this.stackCount == sketchThing.stackCount && this.pos == sketchThing.pos && this.rot == sketchThing.rot)
			{
				QualityCategory? qualityCategory = this.quality;
				QualityCategory? qualityCategory2 = sketchThing.quality;
				if (qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null))
				{
					int? num = this.hitPoints;
					int? num2 = sketchThing.hitPoints;
					return num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null);
				}
			}
			return false;
		}

		// Token: 0x06006A8C RID: 27276 RVA: 0x0020DC14 File Offset: 0x0020BE14
		public override SketchEntity DeepCopy()
		{
			SketchThing sketchThing = (SketchThing)base.DeepCopy();
			sketchThing.def = this.def;
			sketchThing.stuff = this.stuff;
			sketchThing.stackCount = this.stackCount;
			sketchThing.rot = this.rot;
			sketchThing.quality = this.quality;
			sketchThing.hitPoints = this.hitPoints;
			return sketchThing;
		}

		// Token: 0x06006A8D RID: 27277 RVA: 0x0020DC74 File Offset: 0x0020BE74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Values.Look<Rot4>(ref this.rot, "rot", default(Rot4), false);
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
			Scribe_Values.Look<int?>(ref this.hitPoints, "hitPoints", null, false);
		}

		// Token: 0x040046DA RID: 18138
		public ThingDef def;

		// Token: 0x040046DB RID: 18139
		public ThingDef stuff;

		// Token: 0x040046DC RID: 18140
		public int stackCount;

		// Token: 0x040046DD RID: 18141
		public Rot4 rot;

		// Token: 0x040046DE RID: 18142
		public QualityCategory? quality;

		// Token: 0x040046DF RID: 18143
		public int? hitPoints;
	}
}
