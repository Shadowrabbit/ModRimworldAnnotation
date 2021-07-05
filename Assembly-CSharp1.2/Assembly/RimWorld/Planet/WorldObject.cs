using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002185 RID: 8581
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x0600B6ED RID: 46829 RVA: 0x00076A51 File Offset: 0x00074C51
		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x0600B6EE RID: 46830 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool ShowRelatedQuests
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x0600B6EF RID: 46831 RVA: 0x00076A59 File Offset: 0x00074C59
		public bool Destroyed
		{
			get
			{
				return this.destroyed;
			}
		}

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x0600B6F0 RID: 46832 RVA: 0x00076A61 File Offset: 0x00074C61
		// (set) Token: 0x0600B6F1 RID: 46833 RVA: 0x00076A69 File Offset: 0x00074C69
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

		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x0600B6F2 RID: 46834 RVA: 0x00076AA5 File Offset: 0x00074CA5
		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x0600B6F3 RID: 46835 RVA: 0x00076AB2 File Offset: 0x00074CB2
		public virtual Vector3 DrawPos
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.Tile);
			}
		}

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x0600B6F4 RID: 46836 RVA: 0x00076AC4 File Offset: 0x00074CC4
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x0600B6F5 RID: 46837 RVA: 0x00076ACC File Offset: 0x00074CCC
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x0600B6F6 RID: 46838 RVA: 0x00076AD9 File Offset: 0x00074CD9
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x0600B6F7 RID: 46839 RVA: 0x00076AEC File Offset: 0x00074CEC
		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x0600B6F8 RID: 46840 RVA: 0x00076AF4 File Offset: 0x00074CF4
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x0600B6F9 RID: 46841 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool HasName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x0600B6FA RID: 46842 RVA: 0x00076B07 File Offset: 0x00074D07
		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x0600B6FB RID: 46843 RVA: 0x00076B14 File Offset: 0x00074D14
		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x0600B6FC RID: 46844 RVA: 0x00076B21 File Offset: 0x00074D21
		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x0600B6FD RID: 46845 RVA: 0x00076B2E File Offset: 0x00074D2E
		public virtual Texture2D ExpandingIcon
		{
			get
			{
				return this.def.ExpandingIconTexture ?? ((Texture2D)this.Material.mainTexture);
			}
		}

		// Token: 0x17001B12 RID: 6930
		// (get) Token: 0x0600B6FE RID: 46846 RVA: 0x0034D7BC File Offset: 0x0034B9BC
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

		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x0600B6FF RID: 46847 RVA: 0x00076B4F File Offset: 0x00074D4F
		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x0600B700 RID: 46848 RVA: 0x00076B5C File Offset: 0x00074D5C
		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x0600B701 RID: 46849 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x0600B702 RID: 46850 RVA: 0x00076B69 File Offset: 0x00074D69
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

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x0600B703 RID: 46851 RVA: 0x00076B7A File Offset: 0x00074D7A
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x0600B704 RID: 46852 RVA: 0x00076B83 File Offset: 0x00074D83
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

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x0600B705 RID: 46853 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float ExpandingIconRotation
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x0600B706 RID: 46854 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ExpandingIconFlipHorizontal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B707 RID: 46855 RVA: 0x00076BA4 File Offset: 0x00074DA4
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

		// Token: 0x0600B708 RID: 46856 RVA: 0x0034D7F4 File Offset: 0x0034B9F4
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

		// Token: 0x0600B709 RID: 46857 RVA: 0x0034D8D4 File Offset: 0x0034BAD4
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
					Log.Error("Could not instantiate or initialize a WorldObjectComp: " + arg, false);
					this.comps.Remove(worldObjectComp);
				}
			}
		}

		// Token: 0x0600B70A RID: 46858 RVA: 0x0034D980 File Offset: 0x0034BB80
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
				}), false);
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x0600B70B RID: 46859 RVA: 0x0034D9D8 File Offset: 0x0034BBD8
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
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612, false);
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

		// Token: 0x0600B70C RID: 46860 RVA: 0x0034DACC File Offset: 0x0034BCCC
		public virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		// Token: 0x0600B70D RID: 46861 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		// Token: 0x0600B70E RID: 46862 RVA: 0x0034DB00 File Offset: 0x0034BD00
		public virtual void DrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		// Token: 0x0600B70F RID: 46863 RVA: 0x00076BB4 File Offset: 0x00074DB4
		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		// Token: 0x0600B710 RID: 46864 RVA: 0x00076BBC File Offset: 0x00074DBC
		public virtual void PostAdd()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
		}

		// Token: 0x0600B711 RID: 46865 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void PositionChanged()
		{
		}

		// Token: 0x0600B712 RID: 46866 RVA: 0x00076BD9 File Offset: 0x00074DD9
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

		// Token: 0x0600B713 RID: 46867 RVA: 0x0034DB34 File Offset: 0x0034BD34
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

		// Token: 0x0600B714 RID: 46868 RVA: 0x0034DBC4 File Offset: 0x0034BDC4
		public virtual void Destroy()
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed world object " + this, false);
				return;
			}
			if (this.Spawned)
			{
				Find.WorldObjects.Remove(this);
			}
			this.destroyed = true;
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDestroy();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
		}

		// Token: 0x0600B715 RID: 46869 RVA: 0x0034DC48 File Offset: 0x0034BE48
		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, subMesh, false, true, true);
		}

		// Token: 0x0600B716 RID: 46870 RVA: 0x0034DC7C File Offset: 0x0034BE7C
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

		// Token: 0x0600B717 RID: 46871 RVA: 0x0034DD38 File Offset: 0x0034BF38
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

		// Token: 0x0600B718 RID: 46872 RVA: 0x0034DD88 File Offset: 0x0034BF88
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

		// Token: 0x0600B719 RID: 46873 RVA: 0x00076C0F File Offset: 0x00074E0F
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			if (this.ShowRelatedQuests)
			{
				List<Quest> quests = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < quests.Count; i = num + 1)
				{
					Quest quest = quests[i];
					if (!quest.Historical && !quest.dismissed && quest.QuestLookTargets.Contains(this))
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

		// Token: 0x0600B71A RID: 46874 RVA: 0x00076C1F File Offset: 0x00074E1F
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

		// Token: 0x0600B71B RID: 46875 RVA: 0x00076C36 File Offset: 0x00074E36
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

		// Token: 0x0600B71C RID: 46876 RVA: 0x00076C4D File Offset: 0x00074E4D
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

		// Token: 0x0600B71D RID: 46877 RVA: 0x00076C6B File Offset: 0x00074E6B
		public virtual IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<int, TransportPodsArrivalAction> launchAction)
		{
			yield break;
		}

		// Token: 0x0600B71E RID: 46878 RVA: 0x00076C74 File Offset: 0x00074E74
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x0600B71F RID: 46879 RVA: 0x00076C81 File Offset: 0x00074E81
		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		// Token: 0x0600B720 RID: 46880 RVA: 0x0034DDD4 File Offset: 0x0034BFD4
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

		// Token: 0x0600B721 RID: 46881 RVA: 0x00076C91 File Offset: 0x00074E91
		public override int GetHashCode()
		{
			return this.ID;
		}

		// Token: 0x0600B722 RID: 46882 RVA: 0x00076C99 File Offset: 0x00074E99
		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID;
		}

		// Token: 0x0600B723 RID: 46883 RVA: 0x0034DE2C File Offset: 0x0034C02C
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

		// Token: 0x04007D48 RID: 32072
		public WorldObjectDef def;

		// Token: 0x04007D49 RID: 32073
		public int ID = -1;

		// Token: 0x04007D4A RID: 32074
		private int tileInt = -1;

		// Token: 0x04007D4B RID: 32075
		private Faction factionInt;

		// Token: 0x04007D4C RID: 32076
		public int creationGameTicks = -1;

		// Token: 0x04007D4D RID: 32077
		public List<string> questTags;

		// Token: 0x04007D4E RID: 32078
		private bool destroyed;

		// Token: 0x04007D4F RID: 32079
		public ThingOwner<Thing> rewards;

		// Token: 0x04007D50 RID: 32080
		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		// Token: 0x04007D51 RID: 32081
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04007D52 RID: 32082
		private const float BaseDrawSize = 0.7f;

		// Token: 0x04007D53 RID: 32083
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}
