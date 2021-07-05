using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017EC RID: 6124
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x06008EA2 RID: 36514 RVA: 0x00333656 File Offset: 0x00331856
		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x06008EA3 RID: 36515 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool ShowRelatedQuests
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x06008EA4 RID: 36516 RVA: 0x0033365E File Offset: 0x0033185E
		public bool Destroyed
		{
			get
			{
				return this.destroyed;
			}
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x06008EA5 RID: 36517 RVA: 0x00333666 File Offset: 0x00331866
		// (set) Token: 0x06008EA6 RID: 36518 RVA: 0x0033366E File Offset: 0x0033186E
		public int Tile
		{
			get
			{
				return this.tileInt;
			}
			set
			{
				if (this.tileInt != value)
				{
					this.tileInt = value;
					if (this.Spawned && !this.def.useDynamicDrawer)
					{
						Find.World.renderer.Notify_StaticWorldObjectPosChanged();
					}
					this.PositionChanged();
				}
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x06008EA7 RID: 36519 RVA: 0x003336AA File Offset: 0x003318AA
		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x06008EA8 RID: 36520 RVA: 0x003336B7 File Offset: 0x003318B7
		public virtual Vector3 DrawPos
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.Tile);
			}
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x06008EA9 RID: 36521 RVA: 0x003336C9 File Offset: 0x003318C9
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x06008EAA RID: 36522 RVA: 0x003336D1 File Offset: 0x003318D1
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x06008EAB RID: 36523 RVA: 0x003336DE File Offset: 0x003318DE
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x06008EAC RID: 36524 RVA: 0x003336F1 File Offset: 0x003318F1
		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x06008EAD RID: 36525 RVA: 0x003336F9 File Offset: 0x003318F9
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x06008EAE RID: 36526 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool HasName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x06008EAF RID: 36527 RVA: 0x0033370C File Offset: 0x0033190C
		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x06008EB0 RID: 36528 RVA: 0x00333719 File Offset: 0x00331919
		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x06008EB1 RID: 36529 RVA: 0x00333726 File Offset: 0x00331926
		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x06008EB2 RID: 36530 RVA: 0x00333733 File Offset: 0x00331933
		public virtual Texture2D ExpandingIcon
		{
			get
			{
				return this.def.ExpandingIconTexture ?? ((Texture2D)this.Material.mainTexture);
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x06008EB3 RID: 36531 RVA: 0x00333754 File Offset: 0x00331954
		public virtual Color ExpandingIconColor
		{
			get
			{
				Color? expandingIconColor = this.def.expandingIconColor;
				if (expandingIconColor == null)
				{
					return this.Material.color;
				}
				return expandingIconColor.GetValueOrDefault();
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06008EB4 RID: 36532 RVA: 0x00333789 File Offset: 0x00331989
		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x06008EB5 RID: 36533 RVA: 0x00333796 File Offset: 0x00331996
		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x06008EB6 RID: 36534 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x06008EB7 RID: 36535 RVA: 0x003337A3 File Offset: 0x003319A3
		public IThingHolder ParentHolder
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.World;
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x06008EB8 RID: 36536 RVA: 0x003337B4 File Offset: 0x003319B4
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x06008EB9 RID: 36537 RVA: 0x003337BD File Offset: 0x003319BD
		public BiomeDef Biome
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.WorldGrid[this.Tile].biome;
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x06008EBA RID: 36538 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float ExpandingIconRotation
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x06008EBB RID: 36539 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ExpandingIconFlipHorizontal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008EBC RID: 36540 RVA: 0x003337DE File Offset: 0x003319DE
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			if (this.def.IncidentTargetTags != null)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef in this.def.IncidentTargetTags)
				{
					yield return incidentTargetTagDef;
				}
				List<IncidentTargetTagDef>.Enumerator enumerator = default(List<IncidentTargetTagDef>.Enumerator);
			}
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (IncidentTargetTagDef incidentTargetTagDef2 in this.comps[i].IncidentTargetTags())
				{
					yield return incidentTargetTagDef2;
				}
				IEnumerator<IncidentTargetTagDef> enumerator2 = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008EBD RID: 36541 RVA: 0x003337F0 File Offset: 0x003319F0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<WorldObjectDef>(ref this.def, "def");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.tileInt, "tile", -1, false);
			Scribe_References.Look<Faction>(ref this.factionInt, "faction", false);
			Scribe_Values.Look<int>(ref this.creationGameTicks, "creationGameTicks", 0, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.destroyed, "destroyed", false, false);
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Deep.Look<ThingOwner<Thing>>(ref this.rewards, "rewards", Array.Empty<object>());
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostExposeData();
			}
		}

		// Token: 0x06008EBE RID: 36542 RVA: 0x003338D0 File Offset: 0x00331AD0
		private void InitializeComps()
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				WorldObjectComp worldObjectComp = null;
				try
				{
					worldObjectComp = (WorldObjectComp)Activator.CreateInstance(this.def.comps[i].compClass);
					worldObjectComp.parent = this;
					this.comps.Add(worldObjectComp);
					worldObjectComp.Initialize(this.def.comps[i]);
				}
				catch (Exception arg)
				{
					Log.Error("Could not instantiate or initialize a WorldObjectComp: " + arg);
					this.comps.Remove(worldObjectComp);
				}
			}
		}

		// Token: 0x06008EBF RID: 36543 RVA: 0x0033397C File Offset: 0x00331B7C
		public virtual void SetFaction(Faction newFaction)
		{
			if (!this.def.canHaveFaction && newFaction != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set faction to ",
					newFaction,
					" but this world object (",
					this,
					") cannot have faction."
				}));
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x06008EC0 RID: 36544 RVA: 0x003339D4 File Offset: 0x00331BD4
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Faction != null && this.AppendFactionToInspectString)
			{
				stringBuilder.Append("Faction".Translate() + ": " + this.Faction.Name);
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x06008EC1 RID: 36545 RVA: 0x00333AC8 File Offset: 0x00331CC8
		public virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		// Token: 0x06008EC2 RID: 36546 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		// Token: 0x06008EC3 RID: 36547 RVA: 0x00333AFC File Offset: 0x00331CFC
		public virtual void DrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		// Token: 0x06008EC4 RID: 36548 RVA: 0x00333B30 File Offset: 0x00331D30
		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		// Token: 0x06008EC5 RID: 36549 RVA: 0x00333B38 File Offset: 0x00331D38
		public virtual void PostAdd()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
		}

		// Token: 0x06008EC6 RID: 36550 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void PositionChanged()
		{
		}

		// Token: 0x06008EC7 RID: 36551 RVA: 0x00333B55 File Offset: 0x00331D55
		public virtual void SpawnSetup()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.RegisterDrawable(this);
			}
		}

		// Token: 0x06008EC8 RID: 36552 RVA: 0x00333B8C File Offset: 0x00331D8C
		public virtual void PostRemove()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.DeRegisterDrawable(this);
			}
			Find.WorldSelector.Deselect(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPostRemove();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
		}

		// Token: 0x06008EC9 RID: 36553 RVA: 0x00333C1C File Offset: 0x00331E1C
		public virtual void Destroy()
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed world object " + this);
				return;
			}
			if (this.Spawned)
			{
				Find.WorldObjects.Remove(this);
			}
			this.destroyed = true;
			Find.FactionManager.Notify_WorldObjectDestroyed(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDestroy();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
		}

		// Token: 0x06008ECA RID: 36554 RVA: 0x00333CAC File Offset: 0x00331EAC
		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, subMesh, false, true, true);
		}

		// Token: 0x06008ECB RID: 36555 RVA: 0x00333CE0 File Offset: 0x00331EE0
		public virtual void Draw()
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			float transitionPct = ExpandableWorldObjectsUtility.TransitionPct;
			if (this.def.expandingIcon && transitionPct > 0f)
			{
				Color color = this.Material.color;
				float num = 1f - transitionPct;
				WorldObject.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num));
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, this.Material, false, false, WorldObject.propertyBlock);
				return;
			}
			WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, this.Material, false, false, null);
		}

		// Token: 0x06008ECC RID: 36556 RVA: 0x00333D9C File Offset: 0x00331F9C
		public T GetComponent<T>() where T : WorldObjectComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06008ECD RID: 36557 RVA: 0x00333DEC File Offset: 0x00331FEC
		public WorldObjectComp GetComponent(Type type)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (type.IsAssignableFrom(this.comps[i].GetType()))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x06008ECE RID: 36558 RVA: 0x00333E36 File Offset: 0x00332036
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			if (this.ShowRelatedQuests)
			{
				List<Quest> quests = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < quests.Count; i = num + 1)
				{
					Quest quest = quests[i];
					if (!quest.hidden && !quest.Historical && !quest.dismissed && quest.QuestLookTargets.Contains(this))
					{
						yield return new Command_Action
						{
							defaultLabel = "CommandViewQuest".Translate(quest.name),
							defaultDesc = "CommandViewQuestDesc".Translate(),
							icon = WorldObject.ViewQuestCommandTex,
							action = delegate()
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
								((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
							}
						};
					}
					num = i;
				}
				quests = null;
			}
			for (int i = 0; i < this.comps.Count; i = num)
			{
				foreach (Gizmo gizmo in this.comps[i].GetGizmos())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008ECF RID: 36559 RVA: 0x00333E46 File Offset: 0x00332046
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (Gizmo gizmo in this.comps[i].GetCaravanGizmos(caravan))
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008ED0 RID: 36560 RVA: 0x00333E5D File Offset: 0x0033205D
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num)
			{
				foreach (FloatMenuOption floatMenuOption in this.comps[i].GetFloatMenuOptions(caravan))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008ED1 RID: 36561 RVA: 0x00333E74 File Offset: 0x00332074
		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (FloatMenuOption floatMenuOption in this.comps[i].GetTransportPodsFloatMenuOptions(pods, representative))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008ED2 RID: 36562 RVA: 0x00333E92 File Offset: 0x00332092
		public virtual IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction)
		{
			yield break;
		}

		// Token: 0x06008ED3 RID: 36563 RVA: 0x00333E9B File Offset: 0x0033209B
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x06008ED4 RID: 36564 RVA: 0x00333EA8 File Offset: 0x003320A8
		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		// Token: 0x06008ED5 RID: 36565 RVA: 0x00333EB8 File Offset: 0x003320B8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" ",
				this.LabelCap,
				" (tile=",
				this.Tile,
				")"
			});
		}

		// Token: 0x06008ED6 RID: 36566 RVA: 0x00333F0D File Offset: 0x0033210D
		public override int GetHashCode()
		{
			return this.ID;
		}

		// Token: 0x06008ED7 RID: 36567 RVA: 0x00333F15 File Offset: 0x00332115
		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID;
		}

		// Token: 0x06008ED8 RID: 36568 RVA: 0x00333F2C File Offset: 0x0033212C
		public virtual string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.def.description);
			for (int i = 0; i < this.comps.Count; i++)
			{
				string descriptionPart = this.comps[i].GetDescriptionPart();
				if (!descriptionPart.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(descriptionPart);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040059F1 RID: 23025
		public WorldObjectDef def;

		// Token: 0x040059F2 RID: 23026
		public int ID = -1;

		// Token: 0x040059F3 RID: 23027
		private int tileInt = -1;

		// Token: 0x040059F4 RID: 23028
		private Faction factionInt;

		// Token: 0x040059F5 RID: 23029
		public int creationGameTicks = -1;

		// Token: 0x040059F6 RID: 23030
		public List<string> questTags;

		// Token: 0x040059F7 RID: 23031
		private bool destroyed;

		// Token: 0x040059F8 RID: 23032
		public ThingOwner<Thing> rewards;

		// Token: 0x040059F9 RID: 23033
		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		// Token: 0x040059FA RID: 23034
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x040059FB RID: 23035
		private const float BaseDrawSize = 0.7f;

		// Token: 0x040059FC RID: 23036
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}
