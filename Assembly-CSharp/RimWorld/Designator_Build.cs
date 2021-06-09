using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C0 RID: 6592
	public class Designator_Build : Designator_Place
	{
		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x060091BB RID: 37307 RVA: 0x000619F2 File Offset: 0x0005FBF2
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.entDef;
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x060091BC RID: 37308 RVA: 0x0029DA20 File Offset: 0x0029BC20
		public override string Label
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null && this.writeStuff)
				{
					return GenLabel.ThingLabel(thingDef, this.stuffDef, 1);
				}
				if (thingDef != null && thingDef.MadeFromStuff)
				{
					return this.entDef.label + "...";
				}
				return this.entDef.label;
			}
		}

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x060091BD RID: 37309 RVA: 0x000619FA File Offset: 0x0005FBFA
		public override string Desc
		{
			get
			{
				return this.entDef.description;
			}
		}

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x060091BE RID: 37310 RVA: 0x00061A07 File Offset: 0x0005FC07
		public override Color IconDrawColor
		{
			get
			{
				if (this.stuffDef != null)
				{
					return this.entDef.GetColorForStuff(this.stuffDef);
				}
				return this.entDef.uiIconColor;
			}
		}

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x060091BF RID: 37311 RVA: 0x0029DA80 File Offset: 0x0029BC80
		public override bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (this.entDef.minTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel < this.entDef.minTechLevelToBuild)
				{
					return false;
				}
				if (this.entDef.maxTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel > this.entDef.maxTechLevelToBuild)
				{
					return false;
				}
				if (!this.entDef.IsResearchFinished)
				{
					return false;
				}
				if (!Find.Storyteller.difficultyValues.AllowedToBuild(this.entDef))
				{
					return false;
				}
				if (this.entDef.PlaceWorkers != null)
				{
					using (List<PlaceWorker>.Enumerator enumerator = this.entDef.PlaceWorkers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.IsBuildDesignatorVisible(this.entDef))
							{
								return false;
							}
						}
					}
				}
				if (this.entDef.buildingPrerequisites != null)
				{
					for (int i = 0; i < this.entDef.buildingPrerequisites.Count; i++)
					{
						if (!base.Map.listerBuildings.ColonistsHaveBuilding(this.entDef.buildingPrerequisites[i]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x060091C0 RID: 37312 RVA: 0x00061A2E File Offset: 0x0005FC2E
		public override int DraggableDimensions
		{
			get
			{
				return this.entDef.placingDraggableDimensions;
			}
		}

		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x060091C1 RID: 37313 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x060091C2 RID: 37314 RVA: 0x00061A3B File Offset: 0x0005FC3B
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x060091C3 RID: 37315 RVA: 0x00061A42 File Offset: 0x0005FC42
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-Build-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x060091C4 RID: 37316 RVA: 0x0029DBC8 File Offset: 0x0029BDC8
		public Designator_Build(BuildableDef entDef)
		{
			this.entDef = entDef;
			this.icon = entDef.uiIcon;
			this.iconAngle = entDef.uiIconAngle;
			this.iconOffset = entDef.uiIconOffset;
			this.hotKey = entDef.designationHotKey;
			this.tutorTag = entDef.defName;
			this.order = 20f;
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null)
			{
				this.iconProportions = thingDef.graphicData.drawSize.RotatedBy(thingDef.defaultPlacingRot);
				this.iconDrawScale = GenUI.IconDrawScale(thingDef);
			}
			else
			{
				this.iconProportions = new Vector2(1f, 1f);
				this.iconDrawScale = 1f;
			}
			if (entDef is TerrainDef)
			{
				this.iconTexCoords = Widgets.CroppedTerrainTextureRect(this.icon);
			}
			this.ResetStuffToDefault();
		}

		// Token: 0x060091C5 RID: 37317 RVA: 0x0029DC9C File Offset: 0x0029BE9C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			}
			return result;
		}

		// Token: 0x060091C6 RID: 37318 RVA: 0x00061A70 File Offset: 0x0005FC70
		protected override void DrawIcon(Rect rect, Material buttonMat = null)
		{
			Widgets.DefIcon(rect, this.PlacingDef, this.stuffDef, 0.85f, false);
		}

		// Token: 0x060091C7 RID: 37319 RVA: 0x0029DCD8 File Offset: 0x0029BED8
		public Texture2D ResolvedIcon()
		{
			Graphic_Appearances graphic_Appearances;
			if (this.stuffDef != null && (graphic_Appearances = (this.entDef.graphic as Graphic_Appearances)) != null)
			{
				return (Texture2D)graphic_Appearances.SubGraphicFor(this.stuffDef).MatAt(this.entDef.defaultPlacingRot, null).mainTexture;
			}
			return this.icon;
		}

		// Token: 0x060091C8 RID: 37320 RVA: 0x0029DD30 File Offset: 0x0029BF30
		public void ResetStuffToDefault()
		{
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				this.stuffDef = GenStuff.DefaultStuffFor(thingDef);
			}
		}

		// Token: 0x060091C9 RID: 37321 RVA: 0x0029DD60 File Offset: 0x0029BF60
		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted))
			{
				DesignationDragger dragger = Find.DesignatorManager.Dragger;
				int num;
				if (dragger.Dragging)
				{
					num = dragger.DragCells.Count<IntVec3>();
				}
				else
				{
					num = 1;
				}
				float num2 = 0f;
				Vector2 vector = Event.current.mousePosition + Designator_Build.DragPriceDrawOffset;
				List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = list[i];
					float y = vector.y + num2;
					Widgets.ThingIcon(new Rect(vector.x, y, 27f, 27f), thingDefCountClass.thingDef, null, 1f);
					Rect rect = new Rect(vector.x + 29f, y, 999f, 29f);
					int num3 = num * thingDefCountClass.count;
					string text = num3.ToString();
					if (base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < num3)
					{
						GUI.color = Color.red;
						text += " (" + "NotEnoughStoredLower".Translate() + ")";
					}
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, text);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					num2 += 29f;
				}
			}
		}

		// Token: 0x060091CA RID: 37322 RVA: 0x0029DEE8 File Offset: 0x0029C0E8
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef == null || !thingDef.MadeFromStuff)
			{
				base.ProcessInput(ev);
				return;
			}
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (ThingDef thingDef2 in base.Map.resourceCounter.AllCountedAmounts.Keys)
			{
				if (thingDef2.IsStuff && thingDef2.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || base.Map.listerThings.ThingsOfDef(thingDef2).Count > 0))
				{
					ThingDef localStuffDef = thingDef2;
					list.Add(new FloatMenuOption(GenLabel.ThingLabel(this.entDef, localStuffDef, 1).CapitalizeFirst(), delegate()
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(this);
						this.stuffDef = localStuffDef;
						this.writeStuff = true;
					}, thingDef2, MenuOptionPriority.Default, null, null, 0f, null, null)
					{
						tutorTag = "SelectStuff-" + thingDef.defName + "-" + localStuffDef.defName
					});
				}
			}
			if (list.Count == 0)
			{
				Messages.Message("NoStuffsToBuildWith".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			FloatMenu floatMenu = new FloatMenu(list);
			floatMenu.vanishIfMouseDistant = true;
			floatMenu.onCloseCallback = delegate()
			{
				this.writeStuff = true;
			};
			Find.WindowStack.Add(floatMenu);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x060091CB RID: 37323 RVA: 0x00061A8A File Offset: 0x0005FC8A
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return GenConstruct.CanPlaceBlueprintAt(this.entDef, c, this.placingRot, base.Map, DebugSettings.godMode, null, null, this.stuffDef);
		}

		// Token: 0x060091CC RID: 37324 RVA: 0x0029E0B0 File Offset: 0x0029C2B0
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, c)))
			{
				return;
			}
			if (DebugSettings.godMode || this.entDef.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffDef) == 0f)
			{
				if (this.entDef is TerrainDef)
				{
					base.Map.terrainGrid.SetTerrain(c, (TerrainDef)this.entDef);
				}
				else
				{
					Thing thing = ThingMaker.MakeThing((ThingDef)this.entDef, this.stuffDef);
					thing.SetFactionDirect(Faction.OfPlayer);
					GenSpawn.Spawn(thing, c, base.Map, this.placingRot, WipeMode.Vanish, false);
				}
			}
			else
			{
				GenSpawn.WipeExistingThings(c, this.placingRot, this.entDef.blueprintDef, base.Map, DestroyMode.Deconstruct);
				GenConstruct.PlaceBlueprintForBuild(this.entDef, c, base.Map, this.placingRot, Faction.OfPlayer, this.stuffDef);
			}
			MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.entDef.Size), base.Map);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.IsOrbitalTradeBeacon)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BuildOrbitalTradeBeacon, KnowledgeAmount.Total);
			}
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, c));
			}
			if (this.entDef.PlaceWorkers != null)
			{
				for (int i = 0; i < this.entDef.PlaceWorkers.Count; i++)
				{
					this.entDef.PlaceWorkers[i].PostPlace(base.Map, this.entDef, c, this.placingRot);
				}
			}
		}

		// Token: 0x060091CD RID: 37325 RVA: 0x00061AB1 File Offset: 0x0005FCB1
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.entDef, this.placingRot);
		}

		// Token: 0x060091CE RID: 37326 RVA: 0x0029E250 File Offset: 0x0029C450
		public override void DrawPanelReadout(ref float curY, float width)
		{
			if (this.entDef.costStuffCount <= 0 && this.stuffDef != null)
			{
				this.stuffDef = null;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, thingDef, this.stuffDef);
			}
			else
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, this.entDef);
			}
			Text.Font = GameFont.Small;
			List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, false);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				Color color = GUI.color;
				Widgets.ThingIcon(new Rect(0f, curY, 20f, 20f), thingDefCountClass.thingDef, null, 1f);
				GUI.color = color;
				if (thingDefCountClass.thingDef != null && thingDefCountClass.thingDef.resourceReadoutPriority != ResourceCountPriority.Uncounted && base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < thingDefCountClass.count)
				{
					GUI.color = Color.red;
				}
				Widgets.Label(new Rect(26f, curY + 2f, 50f, 100f), thingDefCountClass.count.ToString());
				GUI.color = Color.white;
				string text;
				if (thingDefCountClass.thingDef == null)
				{
					text = "(" + "UnchosenStuff".Translate() + ")";
				}
				else
				{
					text = thingDefCountClass.thingDef.LabelCap;
				}
				float width2 = width - 60f;
				float num = Text.CalcHeight(text, width2) - 5f;
				Widgets.Label(new Rect(60f, curY + 2f, width2, num + 5f), text);
				curY += num;
			}
			if (this.entDef.constructionSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Construction, this.entDef.constructionSkillPrerequisite, width, ref curY);
			}
			if (this.entDef.artisticSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Artistic, this.entDef.artisticSkillPrerequisite, width, ref curY);
			}
			bool flag = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(SkillDefOf.Construction).Level >= this.entDef.constructionSkillPrerequisite && pawn.skills.GetSkill(SkillDefOf.Artistic).Level >= this.entDef.artisticSkillPrerequisite)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TaggedString taggedString = "NoColonistWithAllSkillsForConstructing".Translate(Faction.OfPlayer.def.pawnsPlural);
				Rect rect = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString, width));
				GUI.color = Color.red;
				Widgets.Label(rect, taggedString);
				GUI.color = Color.white;
				curY += rect.height;
			}
			curY += 4f;
		}

		// Token: 0x060091CF RID: 37327 RVA: 0x0029E58C File Offset: 0x0029C78C
		private bool AnyColonistWithSkill(int skill, SkillDef skillDef, bool careIfDisabled)
		{
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(skillDef).Level >= skill && (!careIfDisabled || pawn.workSettings.WorkIsActive(WorkTypeDefOf.Construction)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060091D0 RID: 37328 RVA: 0x0029E614 File Offset: 0x0029C814
		private void DrawSkillRequirement(SkillDef skillDef, int requirement, float width, ref float curY)
		{
			Rect rect = new Rect(0f, curY + 2f, width, 24f);
			if (!this.AnyColonistWithSkill(requirement, skillDef, false))
			{
				GUI.color = Color.red;
				TooltipHandler.TipRegionByKey(rect, "NoColonistWithSkillTip", Faction.OfPlayer.def.pawnsPlural);
			}
			else if (!this.AnyColonistWithSkill(requirement, skillDef, true))
			{
				GUI.color = Color.yellow;
				TooltipHandler.TipRegionByKey(rect, "AllColonistsWithSkillHaveDisabledConstructingTip", Faction.OfPlayer.def.pawnsPlural, WorkTypeDefOf.Construction.gerundLabel);
			}
			else
			{
				GUI.color = new Color(0.72f, 0.87f, 0.72f);
			}
			Widgets.Label(rect, string.Format("{0}: {1}", "SkillNeededForConstructing".Translate(skillDef.LabelCap), requirement));
			GUI.color = Color.white;
			curY += 18f;
		}

		// Token: 0x060091D1 RID: 37329 RVA: 0x00061ACA File Offset: 0x0005FCCA
		public void SetStuffDef(ThingDef stuffDef)
		{
			this.stuffDef = stuffDef;
		}

		// Token: 0x060091D2 RID: 37330 RVA: 0x00060FB9 File Offset: 0x0005F1B9
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x04005C51 RID: 23633
		protected BuildableDef entDef;

		// Token: 0x04005C52 RID: 23634
		private ThingDef stuffDef;

		// Token: 0x04005C53 RID: 23635
		private bool writeStuff;

		// Token: 0x04005C54 RID: 23636
		private static readonly Vector2 DragPriceDrawOffset = new Vector2(19f, 17f);

		// Token: 0x04005C55 RID: 23637
		private const float DragPriceDrawNumberX = 29f;
	}
}
