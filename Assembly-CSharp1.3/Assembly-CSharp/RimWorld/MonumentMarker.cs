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
	// Token: 0x02001092 RID: 4242
	[StaticConstructorOnStartup]
	public class MonumentMarker : Thing
	{
		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x06006510 RID: 25872 RVA: 0x00221BA8 File Offset: 0x0021FDA8
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

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06006511 RID: 25873 RVA: 0x00221BE8 File Offset: 0x0021FDE8
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

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06006512 RID: 25874 RVA: 0x00221C6C File Offset: 0x0021FE6C
		public IntVec2 Size
		{
			get
			{
				return this.sketch.OccupiedSize;
			}
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06006513 RID: 25875 RVA: 0x00221C7C File Offset: 0x0021FE7C
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

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06006514 RID: 25876 RVA: 0x00221DBB File Offset: 0x0021FFBB
		public bool AnyDisallowedBuilding
		{
			get
			{
				return this.FirstDisallowedBuilding != null;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06006515 RID: 25877 RVA: 0x00221DC8 File Offset: 0x0021FFC8
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

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06006516 RID: 25878 RVA: 0x00221E4C File Offset: 0x0022004C
		public bool DisallowedBuildingTicksExpired
		{
			get
			{
				return this.ticksSinceDisallowedBuilding >= 60000;
			}
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x00221E5E File Offset: 0x0022005E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.sketch.Rotate(base.Rotation);
			}
			if (!ModLister.CheckRoyalty("Monument"))
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x00221E90 File Offset: 0x00220090
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

		// Token: 0x06006519 RID: 25881 RVA: 0x00221FA4 File Offset: 0x002201A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Sketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksSinceDisallowedBuilding, "ticksSinceDisallowedBuilding", 0, false);
			Scribe_Values.Look<bool>(ref this.complete, "complete", false, false);
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x00221FF0 File Offset: 0x002201F0
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawGhost(drawLoc.ToIntVec3(), false, base.Rotation);
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x00222008 File Offset: 0x00220208
		public void DrawGhost(IntVec3 at, bool placingMode, Rot4 rotation)
		{
			CellRect rect = this.sketch.OccupiedRect.MovedBy(at);
			Blueprint_Install thingToIgnore = this.FindMyBlueprint(rect, Find.CurrentMap);
			this.sketch.Rotate(rotation);
			Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null;
			if (placingMode)
			{
				validator = ((SketchEntity entity, IntVec3 offset, List<Thing> things, Map map) => MonumentMarkerUtility.GetFirstAdjacentBuilding(entity, offset, things, map) == null);
			}
			this.sketch.DrawGhost(at, Sketch.SpawnPosType.Unchanged, placingMode, thingToIgnore, validator);
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x00222078 File Offset: 0x00220278
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

		// Token: 0x0600651D RID: 25885 RVA: 0x00222108 File Offset: 0x00220308
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

		// Token: 0x0600651E RID: 25886 RVA: 0x00222118 File Offset: 0x00220318
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

		// Token: 0x0600651F RID: 25887 RVA: 0x002221C0 File Offset: 0x002203C0
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
					}, thingDef, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x002222E4 File Offset: 0x002204E4
		public void DebugBuildAll()
		{
			this.sketch.Spawn(base.Map, base.Position, Faction.OfPlayer, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, false, null, null);
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x00222318 File Offset: 0x00220518
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

		// Token: 0x06006522 RID: 25890 RVA: 0x00222404 File Offset: 0x00220604
		private void PlaceAllBlueprints(ThingDef preferredStuffIfNone)
		{
			foreach (SketchEntity entity in this.sketch.Entities)
			{
				this.PlaceBlueprint(entity, preferredStuffIfNone);
			}
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x00222460 File Offset: 0x00220660
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

		// Token: 0x06006524 RID: 25892 RVA: 0x002225A8 File Offset: 0x002207A8
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

		// Token: 0x06006525 RID: 25893 RVA: 0x002227B4 File Offset: 0x002209B4
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

		// Token: 0x06006526 RID: 25894 RVA: 0x0022283C File Offset: 0x00220A3C
		public bool IsPart(Thing thing)
		{
			MonumentMarker.<>c__DisplayClass40_0 CS$<>8__locals1;
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
			if (sketchThing != null && this.<IsPart>g__IsPartInternal|40_0(sketchThing, ref CS$<>8__locals1))
			{
				return true;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<IsPart>g__IsPartInternal|40_0(list[i], ref CS$<>8__locals1))
					{
						return true;
					}
				}
			}
			if (CS$<>8__locals1.thing.def.entityDefToBuild != null)
			{
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(CS$<>8__locals1.thing.Position - base.Position);
				if (sketchTerrain != null && this.<IsPart>g__IsPartInternal|40_0(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006527 RID: 25895 RVA: 0x0022293C File Offset: 0x00220B3C
		public bool AllowsPlacingBlueprint(BuildableDef buildable, IntVec3 pos, Rot4 rot, ThingDef stuff)
		{
			MonumentMarker.<>c__DisplayClass41_0 CS$<>8__locals1;
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
				if (sketchThing != null && this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(sketchThing, ref CS$<>8__locals1))
				{
					return true;
				}
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(list[i], ref CS$<>8__locals1))
						{
							return true;
						}
					}
				}
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(a - base.Position);
				if (sketchTerrain != null && this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return !CS$<>8__locals1.collided;
		}

		// Token: 0x06006528 RID: 25896 RVA: 0x00222AAC File Offset: 0x00220CAC
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

		// Token: 0x06006529 RID: 25897 RVA: 0x00222BDC File Offset: 0x00220DDC
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
					if (terrainDef.BuildableByPlayer && sketchTerrain.IsSameOrSimilar(terrainDef) && !terrainDef.CostList.NullOrEmpty<ThingDefCountClass>())
					{
						list.Add(terrainDef.CostList.First<ThingDefCountClass>().thingDef);
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x00222E04 File Offset: 0x00221004
		[CompilerGenerated]
		private bool <IsPart>g__IsPartInternal|40_0(SketchBuildable b, ref MonumentMarker.<>c__DisplayClass40_0 A_2)
		{
			BuildableDef buildable = b.Buildable;
			SketchThing sketchThing;
			return (A_2.thing.def == buildable || A_2.thing.def.entityDefToBuild == buildable) && (b.GetSpawnedBlueprintOrFrame(b.pos + base.Position, base.Map) == A_2.thing || ((sketchThing = (b as SketchThing)) != null && sketchThing.GetSameSpawned(sketchThing.pos + base.Position, base.Map) == A_2.thing));
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x00222E94 File Offset: 0x00221094
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__IsSameOrSimilar|41_0(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass41_0 A_2)
		{
			SketchTerrain sketchTerrain;
			return A_2.buildable == entity.Buildable || ((sketchTerrain = (entity as SketchTerrain)) != null && sketchTerrain.IsSameOrSimilar(A_2.buildable));
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x00222ECC File Offset: 0x002210CC
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__CheckEntity|41_1(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass41_0 A_2)
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
			return entity.OccupiedRect.MovedBy(base.Position).Equals(A_2.newRect) && this.<AllowsPlacingBlueprint>g__IsSameOrSimilar|41_0(entity, ref A_2) && (A_2.stuff == null || entity.Stuff == null || A_2.stuff == entity.Stuff) && (sketchThing == null || sketchThing.rot == A_2.rot || sketchThing.rot == A_2.rot.Opposite || !sketchThing.def.rotatable);
		}

		// Token: 0x040038EB RID: 14571
		public Sketch sketch = new Sketch();

		// Token: 0x040038EC RID: 14572
		public int ticksSinceDisallowedBuilding;

		// Token: 0x040038ED RID: 14573
		public bool complete;

		// Token: 0x040038EE RID: 14574
		private static readonly Texture2D PlaceBlueprintsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);

		// Token: 0x040038EF RID: 14575
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x040038F0 RID: 14576
		public const int DestroyAfterTicksSinceDisallowedBuilding = 60000;

		// Token: 0x040038F1 RID: 14577
		private const int MonumentCompletedCheckIntervalTicks = 177;

		// Token: 0x040038F2 RID: 14578
		private static List<ThingDef> tmpAllowedBuildings = new List<ThingDef>();

		// Token: 0x040038F3 RID: 14579
		private static HashSet<Pair<BuildableDef, ThingDef>> tmpUniqueBuildableDefs = new HashSet<Pair<BuildableDef, ThingDef>>();

		// Token: 0x040038F4 RID: 14580
		private static List<SketchBuildable> tmpBuildables = new List<SketchBuildable>();

		// Token: 0x040038F5 RID: 14581
		private static Dictionary<string, Pair<int, int>> tmpBuiltParts = new Dictionary<string, Pair<int, int>>();

		// Token: 0x040038F6 RID: 14582
		private static List<StuffCategoryDef> tmpStuffCategories = new List<StuffCategoryDef>();
	}
}
