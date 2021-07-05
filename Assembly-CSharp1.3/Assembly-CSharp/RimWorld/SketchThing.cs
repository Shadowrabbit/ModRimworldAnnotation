using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFA RID: 3322
	public class SketchThing : SketchBuildable
	{
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06004D91 RID: 19857 RVA: 0x0019F85F File Offset: 0x0019DA5F
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06004D92 RID: 19858 RVA: 0x0019F867 File Offset: 0x0019DA67
		public override ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06004D93 RID: 19859 RVA: 0x0019F86F File Offset: 0x0019DA6F
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.def, this.stuff, this.stackCount);
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06004D94 RID: 19860 RVA: 0x0019F490 File Offset: 0x0019D690
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06004D95 RID: 19861 RVA: 0x0019F888 File Offset: 0x0019DA88
		public override CellRect OccupiedRect
		{
			get
			{
				return GenAdj.OccupiedRect(this.pos, this.rot, this.def.size);
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06004D96 RID: 19862 RVA: 0x0019F8A6 File Offset: 0x0019DAA6
		public override float SpawnOrder
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06004D97 RID: 19863 RVA: 0x0019F8AD File Offset: 0x0019DAAD
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.def.GetStatValueAbstract(StatDefOf.MaxHitPoints, this.stuff ?? GenStuff.DefaultStuffFor(this.def)));
			}
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x0019F8DC File Offset: 0x0019DADC
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

		// Token: 0x06004D99 RID: 19865 RVA: 0x0019F960 File Offset: 0x0019DB60
		public override void DrawGhost(IntVec3 at, Color color)
		{
			GhostDrawer.DrawGhostThing(at, this.rot, this.def, this.def.graphic, color, AltitudeLayer.Blueprint, null, false, null);
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x0019F990 File Offset: 0x0019DB90
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

		// Token: 0x06004D9B RID: 19867 RVA: 0x0019FAA2 File Offset: 0x0019DCA2
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return this.GetSameSpawned(at, map) != null;
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0019FAB0 File Offset: 0x0019DCB0
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

		// Token: 0x06004D9D RID: 19869 RVA: 0x0019FBD0 File Offset: 0x0019DDD0
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, this.rot, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, this.rot, map, wipeIfCollides, thingToIgnore, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def)).Accepted;
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x0019F67B File Offset: 0x0019D87B
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x0019FC49 File Offset: 0x0019DE49
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, this.rot, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x0019FC74 File Offset: 0x0019DE74
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

		// Token: 0x06004DA1 RID: 19873 RVA: 0x0019FDD4 File Offset: 0x0019DFD4
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

		// Token: 0x06004DA2 RID: 19874 RVA: 0x0019FDFC File Offset: 0x0019DFFC
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

		// Token: 0x06004DA3 RID: 19875 RVA: 0x0019FED8 File Offset: 0x0019E0D8
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

		// Token: 0x06004DA4 RID: 19876 RVA: 0x0019FF38 File Offset: 0x0019E138
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

		// Token: 0x04002ECF RID: 11983
		public ThingDef def;

		// Token: 0x04002ED0 RID: 11984
		public ThingDef stuff;

		// Token: 0x04002ED1 RID: 11985
		public int stackCount;

		// Token: 0x04002ED2 RID: 11986
		public Rot4 rot;

		// Token: 0x04002ED3 RID: 11987
		public QualityCategory? quality;

		// Token: 0x04002ED4 RID: 11988
		public int? hitPoints;
	}
}
