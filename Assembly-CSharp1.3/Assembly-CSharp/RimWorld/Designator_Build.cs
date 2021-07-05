using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C4 RID: 4804
	public class Designator_Build : Designator_Place
	{
		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x060072C8 RID: 29384 RVA: 0x00264C9F File Offset: 0x00262E9F
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.entDef;
			}
		}

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x060072C9 RID: 29385 RVA: 0x00264CA8 File Offset: 0x00262EA8
		public override ThingDef StuffDef
		{
			get
			{
				if (this.stuffDef != null)
				{
					return this.stuffDef;
				}
				ThingDef thingDef = null;
				ThingDef thingDef2 = this.entDef as ThingDef;
				if (thingDef2 != null && thingDef2.MadeFromStuff)
				{
					thingDef = GenStuff.DefaultStuffFor(thingDef2);
					if (Find.CurrentMap != null && Find.CurrentMap.resourceCounter.GetCount(thingDef) < thingDef2.CostStuffCount)
					{
						ThingDef thingDef3 = null;
						foreach (KeyValuePair<ThingDef, int> keyValuePair in Find.CurrentMap.resourceCounter.AllCountedAmounts)
						{
							if (keyValuePair.Key.IsStuff && keyValuePair.Key.stuffProps.canSuggestUseDefaultStuff && keyValuePair.Key.stuffProps.CanMake(thingDef2) && keyValuePair.Value >= thingDef2.CostStuffCount)
							{
								thingDef3 = keyValuePair.Key;
								break;
							}
						}
						if (thingDef3 != null)
						{
							thingDef = thingDef3;
						}
					}
				}
				return thingDef;
			}
		}

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x060072CA RID: 29386 RVA: 0x00264DB0 File Offset: 0x00262FB0
		public override string Label
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				string text;
				if (thingDef != null && this.writeStuff)
				{
					text = GenLabel.ThingLabel(thingDef, this.StuffDef, 1);
				}
				else
				{
					text = this.entDef.label;
				}
				if (this.sourcePrecept != null)
				{
					text = this.sourcePrecept.TransformThingLabel(text);
				}
				if (thingDef != null && !this.writeStuff && thingDef.MadeFromStuff)
				{
					text += "...";
				}
				return text;
			}
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x060072CB RID: 29387 RVA: 0x00264E2B File Offset: 0x0026302B
		public override string Desc
		{
			get
			{
				return this.entDef.description;
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x060072CC RID: 29388 RVA: 0x00264E38 File Offset: 0x00263038
		public override Color IconDrawColor
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					Color? ideoColorForBuilding = IdeoUtility.GetIdeoColorForBuilding(thingDef, Faction.OfPlayer);
					if (ideoColorForBuilding != null)
					{
						return ideoColorForBuilding.Value;
					}
				}
				if (this.StuffDef != null)
				{
					return this.entDef.GetColorForStuff(this.StuffDef);
				}
				return this.entDef.uiIconColor;
			}
		}

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x060072CD RID: 29389 RVA: 0x00264E98 File Offset: 0x00263098
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
				if (!Find.Storyteller.difficulty.AllowedToBuild(this.entDef))
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

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x060072CE RID: 29390 RVA: 0x00264FE0 File Offset: 0x002631E0
		public override int DraggableDimensions
		{
			get
			{
				return this.entDef.placingDraggableDimensions;
			}
		}

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x060072CF RID: 29391 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x060072D0 RID: 29392 RVA: 0x00264FED File Offset: 0x002631ED
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x060072D1 RID: 29393 RVA: 0x00264FF4 File Offset: 0x002631F4
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

		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x060072D2 RID: 29394 RVA: 0x00265024 File Offset: 0x00263224
		public override ThingStyleDef ThingStyleDefForPreview
		{
			get
			{
				if (this.sourcePrecept == null)
				{
					return this.ThingStyleDefNonPreceptSource;
				}
				ThingDef thingDef;
				if ((thingDef = (this.entDef as ThingDef)) != null)
				{
					return this.sourcePrecept.ideo.GetStyleFor(thingDef);
				}
				return null;
			}
		}

		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x060072D3 RID: 29395 RVA: 0x00265064 File Offset: 0x00263264
		public ThingStyleDef ThingStyleDefNonPreceptSource
		{
			get
			{
				ThingDef thingDef;
				if ((thingDef = (this.PlacingDef as ThingDef)) == null)
				{
					return null;
				}
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				if (ideos == null)
				{
					return null;
				}
				Ideo primaryIdeo = ideos.PrimaryIdeo;
				if (primaryIdeo == null)
				{
					return null;
				}
				return primaryIdeo.GetStyleFor(thingDef);
			}
		}

		// Token: 0x060072D4 RID: 29396 RVA: 0x002650A4 File Offset: 0x002632A4
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

		// Token: 0x060072D5 RID: 29397 RVA: 0x00265178 File Offset: 0x00263378
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			}
			return result;
		}

		// Token: 0x060072D6 RID: 29398 RVA: 0x002651B4 File Offset: 0x002633B4
		protected override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			ThingStyleDef thingStyleDef = (this.PlacingDef is ThingDef) ? this.ThingStyleDefForPreview : null;
			Color? color = parms.lowLight ? new Color?(Command.LowLightIconColor) : null;
			color = new Color?(color ?? this.IconDrawColor);
			Widgets.DefIcon(rect, this.PlacingDef, this.StuffDef, 0.85f, thingStyleDef, false, color);
		}

		// Token: 0x060072D7 RID: 29399 RVA: 0x00265234 File Offset: 0x00263434
		public Texture2D ResolvedIcon()
		{
			ThingDef thingDef;
			if ((thingDef = (this.entDef as ThingDef)) != null)
			{
				ThingStyleDef thingStyleDefNonPreceptSource = this.ThingStyleDefNonPreceptSource;
				return Widgets.GetIconFor(thingDef, this.StuffDef, thingStyleDefNonPreceptSource);
			}
			Graphic_Appearances graphic_Appearances;
			if (this.StuffDef != null && (graphic_Appearances = (this.entDef.graphic as Graphic_Appearances)) != null)
			{
				return (Texture2D)graphic_Appearances.SubGraphicFor(this.StuffDef).MatAt(this.entDef.defaultPlacingRot, null).mainTexture;
			}
			return this.icon;
		}

		// Token: 0x060072D8 RID: 29400 RVA: 0x002652AF File Offset: 0x002634AF
		public void ResetStuffToDefault()
		{
			this.stuffDef = null;
		}

		// Token: 0x060072D9 RID: 29401 RVA: 0x002652B8 File Offset: 0x002634B8
		protected override void DrawPlaceMouseAttachments(float curX, ref float curY)
		{
			base.DrawPlaceMouseAttachments(curX, ref curY);
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
				List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.StuffDef, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = list[i];
					float y = curY;
					Widgets.ThingIcon(new Rect(curX, y, 27f, 27f), thingDefCountClass.thingDef, null, null, 1f, null);
					Rect rect = new Rect(curX + 29f, y, 999f, 29f);
					int num2 = num * thingDefCountClass.count;
					string text = num2.ToString();
					if (base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < num2)
					{
						GUI.color = Color.red;
						text += " (" + "NotEnoughStoredLower".Translate() + ")";
					}
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, text);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					curY += 29f;
				}
			}
		}

		// Token: 0x060072DA RID: 29402 RVA: 0x0026541C File Offset: 0x0026361C
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
			foreach (ThingDef thingDef2 in base.Map.resourceCounter.AllCountedAmounts.Keys.OrderByDescending(delegate(ThingDef d)
			{
				StuffProperties stuffProps = d.stuffProps;
				if (stuffProps == null)
				{
					return float.PositiveInfinity;
				}
				return stuffProps.commonality;
			}).ThenBy((ThingDef d) => d.BaseMarketValue))
			{
				if (thingDef2.IsStuff && thingDef2.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || base.Map.listerThings.ThingsOfDef(thingDef2).Count > 0))
				{
					ThingDef localStuffDef = thingDef2;
					string text;
					if (this.sourcePrecept != null)
					{
						text = "ThingMadeOfStuffLabel".Translate(localStuffDef.LabelAsStuff, this.sourcePrecept.Label);
					}
					else
					{
						text = GenLabel.ThingLabel(this.entDef, localStuffDef, 1);
					}
					text = text.CapitalizeFirst();
					list.Add(new FloatMenuOption(text, delegate()
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(this);
						this.stuffDef = localStuffDef;
						this.writeStuff = true;
					}, thingDef2, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
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

		// Token: 0x060072DB RID: 29403 RVA: 0x00265680 File Offset: 0x00263880
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return GenConstruct.CanPlaceBlueprintAt(this.entDef, c, this.placingRot, base.Map, DebugSettings.godMode, null, null, this.StuffDef);
		}

		// Token: 0x060072DC RID: 29404 RVA: 0x002656A8 File Offset: 0x002638A8
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, c)))
			{
				return;
			}
			if (DebugSettings.godMode || this.entDef.GetStatValueAbstract(StatDefOf.WorkToBuild, this.StuffDef) == 0f)
			{
				if (this.entDef is TerrainDef)
				{
					base.Map.terrainGrid.SetTerrain(c, (TerrainDef)this.entDef);
				}
				else
				{
					Thing thing = ThingMaker.MakeThing((ThingDef)this.entDef, this.StuffDef);
					thing.SetFactionDirect(Faction.OfPlayer);
					Thing thing2 = GenSpawn.Spawn(thing, c, base.Map, this.placingRot, WipeMode.Vanish, false);
					Building building;
					if (this.sourcePrecept != null && (building = (thing2 as Building)) != null)
					{
						building.StyleSourcePrecept = this.sourcePrecept;
					}
					else if (this.sourcePrecept == null)
					{
						thing2.StyleDef = this.ThingStyleDefNonPreceptSource;
					}
				}
			}
			else
			{
				GenSpawn.WipeExistingThings(c, this.placingRot, this.entDef.blueprintDef, base.Map, DestroyMode.Deconstruct);
				Blueprint_Build blueprint_Build = GenConstruct.PlaceBlueprintForBuild(this.entDef, c, base.Map, this.placingRot, Faction.OfPlayer, this.StuffDef);
				blueprint_Build.StyleSourcePrecept = this.sourcePrecept;
				ThingDef thingDef;
				if (blueprint_Build.StyleSourcePrecept == null && (thingDef = (this.entDef as ThingDef)) != null)
				{
					Thing thing3 = blueprint_Build;
					FactionIdeosTracker ideos = Find.FactionManager.OfPlayer.ideos;
					ThingStyleDef styleDef;
					if (ideos == null)
					{
						styleDef = null;
					}
					else
					{
						Ideo primaryIdeo = ideos.PrimaryIdeo;
						styleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(thingDef) : null);
					}
					thing3.StyleDef = styleDef;
				}
			}
			FleckMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.entDef.Size), base.Map);
			ThingDef thingDef2 = this.entDef as ThingDef;
			if (thingDef2 != null && thingDef2.IsOrbitalTradeBeacon)
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

		// Token: 0x060072DD RID: 29405 RVA: 0x002658E1 File Offset: 0x00263AE1
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.entDef, this.placingRot);
		}

		// Token: 0x060072DE RID: 29406 RVA: 0x002658FC File Offset: 0x00263AFC
		public override void DrawPanelReadout(ref float curY, float width)
		{
			if (this.entDef.CostStuffCount <= 0 && this.stuffDef != null)
			{
				this.stuffDef = null;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, thingDef, this.StuffDef);
			}
			else
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, this.entDef);
			}
			Text.Font = GameFont.Small;
			List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.StuffDef, false);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				Color color = GUI.color;
				Widgets.ThingIcon(new Rect(0f, curY, 20f, 20f), thingDefCountClass.thingDef, null, null, 1f, null);
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
			StyleCategoryDef styleCategoryDef = this.entDef.dominantStyleCategory;
			if (styleCategoryDef == null && thingDef != null)
			{
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				StyleCategoryDef styleCategoryDef2;
				if (ideos == null)
				{
					styleCategoryDef2 = null;
				}
				else
				{
					Ideo primaryIdeo = ideos.PrimaryIdeo;
					styleCategoryDef2 = ((primaryIdeo != null) ? primaryIdeo.GetStyleCategoryFor(thingDef) : null);
				}
				styleCategoryDef = styleCategoryDef2;
			}
			if (styleCategoryDef != null)
			{
				TaggedString taggedString = "DominantStyle".Translate().CapitalizeFirst() + ": " + styleCategoryDef.LabelCap;
				Rect rect = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString, width));
				Widgets.Label(rect, taggedString);
				curY += rect.height;
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
				TaggedString taggedString2 = "NoColonistWithAllSkillsForConstructing".Translate(Faction.OfPlayer.def.pawnsPlural);
				Rect rect2 = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString2, width));
				GUI.color = Color.red;
				Widgets.Label(rect2, taggedString2);
				GUI.color = Color.white;
				curY += rect2.height;
			}
			curY += 4f;
		}

		// Token: 0x060072DF RID: 29407 RVA: 0x00265CE4 File Offset: 0x00263EE4
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

		// Token: 0x060072E0 RID: 29408 RVA: 0x00265D6C File Offset: 0x00263F6C
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

		// Token: 0x060072E1 RID: 29409 RVA: 0x00265E6E File Offset: 0x0026406E
		public void SetStuffDef(ThingDef stuffDef)
		{
			this.stuffDef = stuffDef;
		}

		// Token: 0x060072E2 RID: 29410 RVA: 0x00260D19 File Offset: 0x0025EF19
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x04003EC7 RID: 16071
		protected BuildableDef entDef;

		// Token: 0x04003EC8 RID: 16072
		public Precept_Building sourcePrecept;

		// Token: 0x04003EC9 RID: 16073
		private ThingDef stuffDef;

		// Token: 0x04003ECA RID: 16074
		private bool writeStuff;

		// Token: 0x04003ECB RID: 16075
		private const float DragPriceDrawNumberX = 29f;
	}
}
