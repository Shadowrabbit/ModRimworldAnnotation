using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016EA RID: 5866
	[StaticConstructorOnStartup]
	public class MonumentMarker : Thing
	{
		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x060080E2 RID: 32994 RVA: 0x00262D88 File Offset: 0x00260F88
		public override CellRect? CustomRectForSelector
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return new CellRect?(this.sketch.OccupiedRect.MovedBy(base.Position));
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x060080E3 RID: 32995 RVA: 0x00262DC8 File Offset: 0x00260FC8
		public bool AllDone
		{
			get
			{
				if (!base.Spawned)
				{
					return false;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawned(base.Position + sketchEntity.pos, base.Map))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x060080E4 RID: 32996 RVA: 0x0005694C File Offset: 0x00054B4C
		public IntVec2 Size
		{
			get
			{
				return this.sketch.OccupiedSize;
			}
		}

		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x060080E5 RID: 32997 RVA: 0x00262E4C File Offset: 0x0026104C
		public Thing FirstDisallowedBuilding
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				List<SketchTerrain> terrain = this.sketch.Terrain;
				for (int i = 0; i < terrain.Count; i++)
				{
					MonumentMarker.tmpAllowedBuildings.Clear();
					SketchThing sketchThing;
					List<SketchThing> list;
					this.sketch.ThingsAt(terrain[i].pos, out sketchThing, out list);
					if (sketchThing != null)
					{
						MonumentMarker.tmpAllowedBuildings.Add(sketchThing.def);
					}
					if (list != null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							MonumentMarker.tmpAllowedBuildings.Add(list[j].def);
						}
					}
					List<Thing> thingList = (terrain[i].pos + base.Position).GetThingList(base.Map);
					for (int k = 0; k < thingList.Count; k++)
					{
						if (thingList[k].def.IsBuildingArtificial && !thingList[k].def.IsBlueprint && !thingList[k].def.IsFrame && !MonumentMarker.tmpAllowedBuildings.Contains(thingList[k].def))
						{
							return thingList[k];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x060080E6 RID: 32998 RVA: 0x00056959 File Offset: 0x00054B59
		public bool AnyDisallowedBuilding
		{
			get
			{
				return this.FirstDisallowedBuilding != null;
			}
		}

		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x060080E7 RID: 32999 RVA: 0x00262F8C File Offset: 0x0026118C
		public SketchEntity FirstEntityWithMissingBlueprint
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawnedOrBlueprintOrFrame(base.Position + sketchEntity.pos, base.Map))
					{
						return sketchEntity;
					}
				}
				return null;
			}
		}

		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x060080E8 RID: 33000 RVA: 0x00056964 File Offset: 0x00054B64
		public bool DisallowedBuildingTicksExpired
		{
			get
			{
				return this.ticksSinceDisallowedBuilding >= 60000;
			}
		}

		// Token: 0x060080E9 RID: 33001 RVA: 0x00056976 File Offset: 0x00054B76
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.sketch.Rotate(base.Rotation);
			}
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Monuments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 774341, false);
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060080EA RID: 33002 RVA: 0x00263010 File Offset: 0x00261210
		public override void Tick()
		{
			if (this.IsHashIntervalTick(177))
			{
				bool allDone = this.AllDone;
				if (!this.complete && allDone)
				{
					this.complete = true;
					QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentCompleted", this.Named("SUBJECT"));
				}
				if (this.complete && !allDone)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentDestroyed", this.Named("SUBJECT"));
					if (!base.Destroyed)
					{
						this.Destroy(DestroyMode.Vanish);
					}
					return;
				}
				if (allDone)
				{
					if (this.AnyDisallowedBuilding)
					{
						this.ticksSinceDisallowedBuilding += 177;
						if (this.DisallowedBuildingTicksExpired)
						{
							Messages.Message("MessageMonumentDestroyedBecauseOfDisallowedBuilding".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
							QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentDestroyed", this.Named("SUBJECT"));
							if (!base.Destroyed)
							{
								this.Destroy(DestroyMode.Vanish);
							}
							return;
						}
					}
					else
					{
						this.ticksSinceDisallowedBuilding = 0;
					}
				}
			}
		}

		// Token: 0x060080EB RID: 33003 RVA: 0x00263124 File Offset: 0x00261324
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Sketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksSinceDisallowedBuilding, "ticksSinceDisallowedBuilding", 0, false);
			Scribe_Values.Look<bool>(ref this.complete, "complete", false, false);
		}

		// Token: 0x060080EC RID: 33004 RVA: 0x000569B2 File Offset: 0x00054BB2
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawGhost_NewTmp(drawLoc.ToIntVec3(), false, base.Rotation);
		}

		// Token: 0x060080ED RID: 33005 RVA: 0x000569C7 File Offset: 0x00054BC7
		[Obsolete]
		public void DrawGhost(IntVec3 at, bool placingMode)
		{
			this.DrawGhost_NewTmp(at, placingMode, base.Rotation);
		}

		// Token: 0x060080EE RID: 33006 RVA: 0x00263170 File Offset: 0x00261370
		public void DrawGhost_NewTmp(IntVec3 at, bool placingMode, Rot4 rotation)
		{
			CellRect rect = this.sketch.OccupiedRect.MovedBy(at);
			Blueprint_Install thingToIgnore = this.FindMyBlueprint(rect, Find.CurrentMap);
			this.sketch.Rotate(rotation);
			Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null;
			if (placingMode)
			{
				validator = ((SketchEntity entity, IntVec3 offset, List<Thing> things, Map map) => MonumentMarkerUtility.GetFirstAdjacentBuilding(entity, offset, things, map) == null);
			}
			this.sketch.DrawGhost_NewTmp(at, Sketch.SpawnPosType.Unchanged, placingMode, thingToIgnore, validator);
		}

		// Token: 0x060080EF RID: 33007 RVA: 0x002631E0 File Offset: 0x002613E0
		public Blueprint_Install FindMyBlueprint(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect)
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Blueprint_Install blueprint_Install = thingList[i] as Blueprint_Install;
						if (blueprint_Install != null && blueprint_Install.ThingToInstall == this)
						{
							return blueprint_Install;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060080F0 RID: 33008 RVA: 0x000569D7 File Offset: 0x00054BD7
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!this.AllDone)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelMonumentMarker".Translate(),
					defaultDesc = "CommandCancelMonumentMarkerDesc".Translate(),
					icon = MonumentMarker.CancelCommandTex,
					action = delegate()
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmCancelMonumentMarker".Translate(), delegate
						{
							QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentCancelled", this.Named("SUBJECT"));
							this.RemovePossiblyRelatedBlueprints();
							this.Uninstall();
						}, true, null));
					}
				};
			}
			bool flag = false;
			foreach (SketchEntity sketchEntity in this.sketch.Entities)
			{
				SketchBuildable sketchBuildable = sketchEntity as SketchBuildable;
				if (sketchBuildable != null && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + base.Position, base.Map) && !sketchEntity.IsSpawningBlocked(sketchEntity.pos + base.Position, base.Map, null, false) && BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable, true) != null)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandPlaceBlueprints".Translate(),
					defaultDesc = "CommandPlaceBlueprintsDesc".Translate(),
					icon = MonumentMarker.PlaceBlueprintsCommandTex,
					action = delegate()
					{
						IEnumerable<ThingDef> enumerable = this.AllowedStuffs();
						if (!enumerable.Any<ThingDef>())
						{
							this.PlaceAllBlueprints(null);
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							return;
						}
						if (enumerable.Count<ThingDef>() == 1)
						{
							this.PlaceAllBlueprints(enumerable.First<ThingDef>());
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							return;
						}
						this.ListFloatMenuOptions(enumerable, delegate(ThingDef stuff)
						{
							this.PlaceAllBlueprints(stuff);
						});
					}
				};
			}
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			if (Prefs.DevMode)
			{
				bool flag2 = false;
				foreach (SketchEntity sketchEntity2 in this.sketch.Entities)
				{
					if (!sketchEntity2.IsSameSpawned(sketchEntity2.pos + base.Position, base.Map) && !sketchEntity2.IsSpawningBlocked(sketchEntity2.pos + base.Position, base.Map, null, false))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Build all",
						action = delegate()
						{
							this.DebugBuildAll();
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					};
				}
				if (this.AllDone && this.AnyDisallowedBuilding && !this.DisallowedBuildingTicksExpired)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Disallowed building ticks +6 hours",
						action = delegate()
						{
							this.ticksSinceDisallowedBuilding += 15000;
						}
					};
				}
			}
			MonumentMarker.tmpUniqueBuildableDefs.Clear();
			foreach (SketchEntity sketchEntity3 in this.sketch.Entities)
			{
				SketchBuildable buildable = sketchEntity3 as SketchBuildable;
				if (buildable != null && !sketchEntity3.IsSameSpawnedOrBlueprintOrFrame(sketchEntity3.pos + base.Position, base.Map))
				{
					if (MonumentMarker.tmpUniqueBuildableDefs.Add(new Pair<BuildableDef, ThingDef>(buildable.Buildable, buildable.Stuff)))
					{
						SketchTerrain sketchTerrain;
						if ((sketchTerrain = (buildable as SketchTerrain)) != null && sketchTerrain.treatSimilarAsSame)
						{
							TerrainDef terrain = buildable.Buildable as TerrainDef;
							if (terrain.designatorDropdown != null)
							{
								Designator designator = BuildCopyCommandUtility.FindAllowedDesignatorRoot(buildable.Buildable, true);
								if (designator != null)
								{
									yield return designator;
								}
							}
							else
							{
								IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
								foreach (TerrainDef terrainDef in allDefs)
								{
									if (terrainDef.BuildableByPlayer && terrainDef.designatorDropdown == null)
									{
										bool flag3 = true;
										for (int i = 0; i < terrain.affordances.Count; i++)
										{
											if (!terrainDef.affordances.Contains(terrain.affordances[i]))
											{
												flag3 = false;
												break;
											}
										}
										if (flag3)
										{
											Command command = BuildCopyCommandUtility.BuildCommand(terrainDef, null, terrainDef.label, terrainDef.description, false);
											if (command != null)
											{
												yield return command;
											}
										}
									}
								}
								IEnumerator<TerrainDef> enumerator4 = null;
							}
							terrain = null;
						}
						else
						{
							Command command2 = BuildCopyCommandUtility.BuildCommand(buildable.Buildable, buildable.Stuff, sketchEntity3.LabelCap, buildable.Buildable.description, false);
							if (command2 != null)
							{
								yield return command2;
							}
						}
						Command_Action placeBlueprintsCommand = this.GetPlaceBlueprintsCommand(buildable);
						if (placeBlueprintsCommand != null)
						{
							yield return placeBlueprintsCommand;
						}
					}
					buildable = null;
				}
			}
			List<SketchEntity>.Enumerator enumerator3 = default(List<SketchEntity>.Enumerator);
			MonumentMarker.tmpUniqueBuildableDefs.Clear();
			yield break;
			yield break;
		}

		// Token: 0x060080F1 RID: 33009 RVA: 0x00263270 File Offset: 0x00261470
		private Command_Action GetPlaceBlueprintsCommand(SketchBuildable buildable)
		{
			Action<ThingDef> <>9__1;
			return new Command_Action
			{
				defaultLabel = "CommandPlaceBlueprintsSpecific".Translate(buildable.Label).CapitalizeFirst(),
				defaultDesc = "CommandPlaceBlueprintsSpecificDesc".Translate(buildable.Label).CapitalizeFirst(),
				icon = MonumentMarker.PlaceBlueprintsCommandTex,
				order = 20f,
				action = delegate()
				{
					List<ThingDef> list = this.AllowedStuffsFor(buildable);
					if (!list.Any<ThingDef>())
					{
						this.PlaceBlueprintsSimilarTo(buildable, null);
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					if (list.Count<ThingDef>() == 1)
					{
						this.PlaceBlueprintsSimilarTo(buildable, list.First<ThingDef>());
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					MonumentMarker <>4__this = this;
					IEnumerable<ThingDef> allowedStuff = list;
					Action<ThingDef> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(ThingDef stuff)
						{
							this.PlaceBlueprintsSimilarTo(buildable, stuff);
						});
					}
					<>4__this.ListFloatMenuOptions(allowedStuff, action);
				}
			};
		}

		// Token: 0x060080F2 RID: 33010 RVA: 0x00263318 File Offset: 0x00261518
		private void ListFloatMenuOptions(IEnumerable<ThingDef> allowedStuff, Action<ThingDef> action)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			bool flag = false;
			foreach (ThingDef def in allowedStuff)
			{
				if (base.Map.listerThings.ThingsOfDef(def).Count > 0)
				{
					flag = true;
					break;
				}
			}
			foreach (ThingDef thingDef in allowedStuff)
			{
				if (!flag || base.Map.listerThings.ThingsOfDef(thingDef).Count != 0)
				{
					ThingDef stuffLocal = thingDef;
					list.Add(new FloatMenuOption(stuffLocal.LabelCap, delegate()
					{
						action(stuffLocal);
					}, thingDef, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060080F3 RID: 33011 RVA: 0x00263438 File Offset: 0x00261638
		public void DebugBuildAll()
		{
			this.sketch.Spawn(base.Map, base.Position, Faction.OfPlayer, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, false, null, null);
		}

		// Token: 0x060080F4 RID: 33012 RVA: 0x0026346C File Offset: 0x0026166C
		private void PlaceBlueprintsSimilarTo(SketchBuildable buildable, ThingDef preferredStuffIfNone)
		{
			bool flag = buildable is SketchTerrain;
			foreach (SketchBuildable sketchBuildable in this.sketch.Buildables)
			{
				SketchTerrain sketchTerrain;
				SketchThing sketchThing;
				if ((flag && (sketchTerrain = (sketchBuildable as SketchTerrain)) != null && sketchTerrain.IsSameOrSimilar(buildable.Buildable)) || (!flag && (sketchThing = (sketchBuildable as SketchThing)) != null && buildable.Buildable == sketchThing.def))
				{
					MonumentMarker.tmpBuildables.Add(sketchBuildable);
				}
			}
			foreach (SketchBuildable entity in MonumentMarker.tmpBuildables)
			{
				this.PlaceBlueprint(entity, preferredStuffIfNone);
			}
			MonumentMarker.tmpBuildables.Clear();
		}

		// Token: 0x060080F5 RID: 33013 RVA: 0x00263558 File Offset: 0x00261758
		private void PlaceAllBlueprints(ThingDef preferredStuffIfNone)
		{
			foreach (SketchEntity entity in this.sketch.Entities)
			{
				this.PlaceBlueprint(entity, preferredStuffIfNone);
			}
		}

		// Token: 0x060080F6 RID: 33014 RVA: 0x002635B4 File Offset: 0x002617B4
		private void PlaceBlueprint(SketchEntity entity, ThingDef preferredStuffIfNone)
		{
			SketchBuildable sketchBuildable;
			if ((sketchBuildable = (entity as SketchBuildable)) == null)
			{
				return;
			}
			if (entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + base.Position, base.Map))
			{
				return;
			}
			if (entity.IsSpawningBlocked(entity.pos + base.Position, base.Map, null, false))
			{
				return;
			}
			if (BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable, true) == null)
			{
				return;
			}
			SketchThing sketchThing;
			if ((sketchThing = (entity as SketchThing)) != null && sketchThing.def.MadeFromStuff && sketchThing.stuff == null && preferredStuffIfNone != null && preferredStuffIfNone.stuffProps.CanMake(sketchThing.def))
			{
				sketchThing.stuff = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, null, false);
				sketchThing.stuff = null;
				return;
			}
			SketchTerrain sketchTerrain;
			if ((sketchTerrain = (entity as SketchTerrain)) != null && sketchTerrain.stuffForComparingSimilar == null && preferredStuffIfNone != null)
			{
				sketchTerrain.stuffForComparingSimilar = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, null, false);
				sketchTerrain.stuffForComparingSimilar = null;
				return;
			}
			entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, null, false);
		}

		// Token: 0x060080F7 RID: 33015 RVA: 0x002636FC File Offset: 0x002618FC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Quest quest = Find.QuestManager.QuestsListForReading.FirstOrDefault((Quest q) => q.QuestLookTargets.Contains(this));
			if (quest != null)
			{
				stringBuilder.Append("Quest".Translate() + ": " + quest.name);
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			if (base.Spawned && !this.AllDone)
			{
				MonumentMarker.tmpBuiltParts.Clear();
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					Pair<int, int> value;
					if (!MonumentMarker.tmpBuiltParts.TryGetValue(sketchEntity.LabelCap, out value))
					{
						value = default(Pair<int, int>);
					}
					if (sketchEntity.IsSameSpawned(sketchEntity.pos + base.Position, base.Map))
					{
						value = new Pair<int, int>(value.First + 1, value.Second + 1);
					}
					else
					{
						value = new Pair<int, int>(value.First, value.Second + 1);
					}
					MonumentMarker.tmpBuiltParts[sketchEntity.LabelCap] = value;
				}
				foreach (KeyValuePair<string, Pair<int, int>> keyValuePair in MonumentMarker.tmpBuiltParts)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(string.Concat(new object[]
					{
						keyValuePair.Key.CapitalizeFirst(),
						": ",
						keyValuePair.Value.First,
						" / ",
						keyValuePair.Value.Second
					}));
				}
				MonumentMarker.tmpBuiltParts.Clear();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060080F8 RID: 33016 RVA: 0x00263908 File Offset: 0x00261B08
		private void RemovePossiblyRelatedBlueprints()
		{
			if (!base.Spawned)
			{
				return;
			}
			foreach (SketchBuildable sketchBuildable in this.sketch.Buildables)
			{
				Blueprint blueprint = sketchBuildable.GetSpawnedBlueprintOrFrame(base.Position + sketchBuildable.pos, base.Map) as Blueprint;
				if (blueprint != null)
				{
					blueprint.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x060080F9 RID: 33017 RVA: 0x00263990 File Offset: 0x00261B90
		public bool IsPart(Thing thing)
		{
			MonumentMarker.<>c__DisplayClass41_0 CS$<>8__locals1;
			CS$<>8__locals1.thing = thing;
			CS$<>8__locals1.<>4__this = this;
			if (!base.Spawned)
			{
				return false;
			}
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Contains(CS$<>8__locals1.thing.Position))
			{
				return false;
			}
			SketchThing sketchThing;
			List<SketchThing> list;
			this.sketch.ThingsAt(CS$<>8__locals1.thing.Position - base.Position, out sketchThing, out list);
			if (sketchThing != null && this.<IsPart>g__IsPartInternal|41_0(sketchThing, ref CS$<>8__locals1))
			{
				return true;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<IsPart>g__IsPartInternal|41_0(list[i], ref CS$<>8__locals1))
					{
						return true;
					}
				}
			}
			if (CS$<>8__locals1.thing.def.entityDefToBuild != null)
			{
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(CS$<>8__locals1.thing.Position - base.Position);
				if (sketchTerrain != null && this.<IsPart>g__IsPartInternal|41_0(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060080FA RID: 33018 RVA: 0x00263A90 File Offset: 0x00261C90
		public bool AllowsPlacingBlueprint(BuildableDef buildable, IntVec3 pos, Rot4 rot, ThingDef stuff)
		{
			MonumentMarker.<>c__DisplayClass42_0 CS$<>8__locals1;
			CS$<>8__locals1.buildable = buildable;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.stuff = stuff;
			CS$<>8__locals1.rot = rot;
			if (!base.Spawned)
			{
				return true;
			}
			CS$<>8__locals1.newRect = GenAdj.OccupiedRect(pos, CS$<>8__locals1.rot, CS$<>8__locals1.buildable.Size);
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Overlaps(CS$<>8__locals1.newRect))
			{
				return true;
			}
			CS$<>8__locals1.collided = false;
			foreach (IntVec3 a in CS$<>8__locals1.newRect)
			{
				SketchThing sketchThing;
				List<SketchThing> list;
				this.sketch.ThingsAt(a - base.Position, out sketchThing, out list);
				if (sketchThing != null && this.<AllowsPlacingBlueprint>g__CheckEntity|42_1(sketchThing, ref CS$<>8__locals1))
				{
					return true;
				}
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (this.<AllowsPlacingBlueprint>g__CheckEntity|42_1(list[i], ref CS$<>8__locals1))
						{
							return true;
						}
					}
				}
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(a - base.Position);
				if (sketchTerrain != null && this.<AllowsPlacingBlueprint>g__CheckEntity|42_1(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return !CS$<>8__locals1.collided;
		}

		// Token: 0x060080FB RID: 33019 RVA: 0x00263C00 File Offset: 0x00261E00
		public IEnumerable<ThingDef> AllowedStuffs()
		{
			MonumentMarker.tmpStuffCategories.Clear();
			bool flag = true;
			List<SketchThing> things = this.sketch.Things;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.MadeFromStuff && things[i].stuff == null)
				{
					if (flag)
					{
						flag = false;
						MonumentMarker.tmpStuffCategories.AddRange(things[i].def.stuffCategories);
					}
					else
					{
						bool flag2 = false;
						for (int j = 0; j < things[i].def.stuffCategories.Count; j++)
						{
							if (MonumentMarker.tmpStuffCategories.Contains(things[i].def.stuffCategories[j]))
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							for (int k = MonumentMarker.tmpStuffCategories.Count - 1; k >= 0; k--)
							{
								if (!things[i].def.stuffCategories.Contains(MonumentMarker.tmpStuffCategories[k]))
								{
									MonumentMarker.tmpStuffCategories.RemoveAt(k);
								}
							}
						}
					}
				}
			}
			return GenStuff.AllowedStuffs(MonumentMarker.tmpStuffCategories, TechLevel.Undefined);
		}

		// Token: 0x060080FC RID: 33020 RVA: 0x00263D30 File Offset: 0x00261F30
		public List<ThingDef> AllowedStuffsFor(SketchBuildable buildable)
		{
			if (buildable.Buildable.MadeFromStuff && buildable.Stuff == null)
			{
				return GenStuff.AllowedStuffs(buildable.Buildable.stuffCategories, TechLevel.Undefined).ToList<ThingDef>();
			}
			SketchTerrain sketchTerrain;
			if ((sketchTerrain = (buildable as SketchTerrain)) != null)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
				{
					if (terrainDef.BuildableByPlayer && sketchTerrain.IsSameOrSimilar(terrainDef) && !terrainDef.costList.NullOrEmpty<ThingDefCountClass>())
					{
						list.Add(terrainDef.costList.First<ThingDefCountClass>().thingDef);
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x06008107 RID: 33031 RVA: 0x00263EAC File Offset: 0x002620AC
		[CompilerGenerated]
		private bool <IsPart>g__IsPartInternal|41_0(SketchBuildable b, ref MonumentMarker.<>c__DisplayClass41_0 A_2)
		{
			BuildableDef buildable = b.Buildable;
			SketchThing sketchThing;
			return (A_2.thing.def == buildable || A_2.thing.def.entityDefToBuild == buildable) && (b.GetSpawnedBlueprintOrFrame(b.pos + base.Position, base.Map) == A_2.thing || ((sketchThing = (b as SketchThing)) != null && sketchThing.GetSameSpawned(sketchThing.pos + base.Position, base.Map) == A_2.thing));
		}

		// Token: 0x06008108 RID: 33032 RVA: 0x00263F3C File Offset: 0x0026213C
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__IsSameOrSimilar|42_0(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass42_0 A_2)
		{
			SketchTerrain sketchTerrain;
			return A_2.buildable == entity.Buildable || ((sketchTerrain = (entity as SketchTerrain)) != null && sketchTerrain.IsSameOrSimilar(A_2.buildable));
		}

		// Token: 0x06008109 RID: 33033 RVA: 0x00263F74 File Offset: 0x00262174
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__CheckEntity|42_1(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass42_0 A_2)
		{
			if (entity.IsSameSpawned(entity.pos + base.Position, base.Map))
			{
				return false;
			}
			if (entity.OccupiedRect.MovedBy(base.Position).Overlaps(A_2.newRect))
			{
				A_2.collided = true;
			}
			SketchThing sketchThing = entity as SketchThing;
			return entity.OccupiedRect.MovedBy(base.Position).Equals(A_2.newRect) && this.<AllowsPlacingBlueprint>g__IsSameOrSimilar|42_0(entity, ref A_2) && (A_2.stuff == null || entity.Stuff == null || A_2.stuff == entity.Stuff) && (sketchThing == null || sketchThing.rot == A_2.rot || sketchThing.rot == A_2.rot.Opposite || !sketchThing.def.rotatable);
		}

		// Token: 0x04005381 RID: 21377
		public Sketch sketch = new Sketch();

		// Token: 0x04005382 RID: 21378
		public int ticksSinceDisallowedBuilding;

		// Token: 0x04005383 RID: 21379
		public bool complete;

		// Token: 0x04005384 RID: 21380
		private static readonly Texture2D PlaceBlueprintsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);

		// Token: 0x04005385 RID: 21381
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04005386 RID: 21382
		public const int DestroyAfterTicksSinceDisallowedBuilding = 60000;

		// Token: 0x04005387 RID: 21383
		private const int MonumentCompletedCheckIntervalTicks = 177;

		// Token: 0x04005388 RID: 21384
		private static List<ThingDef> tmpAllowedBuildings = new List<ThingDef>();

		// Token: 0x04005389 RID: 21385
		private static HashSet<Pair<BuildableDef, ThingDef>> tmpUniqueBuildableDefs = new HashSet<Pair<BuildableDef, ThingDef>>();

		// Token: 0x0400538A RID: 21386
		private static List<SketchBuildable> tmpBuildables = new List<SketchBuildable>();

		// Token: 0x0400538B RID: 21387
		private static Dictionary<string, Pair<int, int>> tmpBuiltParts = new Dictionary<string, Pair<int, int>>();

		// Token: 0x0400538C RID: 21388
		private static List<StuffCategoryDef> tmpStuffCategories = new List<StuffCategoryDef>();
	}
}
