using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012C7 RID: 4807
	public abstract class Designator_Place : Designator
	{
		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x060072F7 RID: 29431
		public abstract BuildableDef PlacingDef { get; }

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x060072F8 RID: 29432
		public abstract ThingStyleDef ThingStyleDefForPreview { get; }

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x060072F9 RID: 29433
		public abstract ThingDef StuffDef { get; }

		// Token: 0x060072FA RID: 29434 RVA: 0x00266259 File Offset: 0x00264459
		public Designator_Place()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragBuilding;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_PlaceBuilding;
		}

		// Token: 0x060072FB RID: 29435 RVA: 0x0026628C File Offset: 0x0026448C
		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			Map currentMap = Find.CurrentMap;
			currentMap.deepResourceGrid.DrawPlacingMouseAttachments(this.PlacingDef);
			Vector2 vector = Event.current.mousePosition + Designator_Place.PlaceMouseAttachmentDrawOffset;
			float x = vector.x;
			float y = vector.y;
			this.DrawPlaceMouseAttachments(x, ref y);
			if (this.PlacingDef.PlaceWorkers != null)
			{
				foreach (PlaceWorker placeWorker in this.PlacingDef.PlaceWorkers)
				{
					placeWorker.DrawPlaceMouseAttachments(x, ref y, this.PlacingDef, UI.MouseCell(), this.placingRot);
				}
				foreach (PlaceWorker placeWorker2 in this.PlacingDef.PlaceWorkers)
				{
					placeWorker2.DrawMouseAttachments(this.PlacingDef);
				}
			}
			ThingDef thingDef;
			if (currentMap == null || (thingDef = (this.PlacingDef as ThingDef)) == null || thingDef.displayNumbersBetweenSameDefDistRange.max <= 0f)
			{
				return;
			}
			IntVec3 intVec = UI.MouseCell();
			Designator_Place.tmpThings.Clear();
			Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsOfDef(thingDef));
			Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint));
			foreach (Thing thing in Designator_Place.tmpThings)
			{
				if ((thing.def == thingDef || thing.def.entityDefToBuild == thingDef) && (thing.Position.x == intVec.x || thing.Position.z == intVec.z) && this.CanDrawNumbersBetween(thing, thingDef, intVec, thing.Position, currentMap))
				{
					IntVec3 intVec2 = thing.Position - intVec;
					intVec2.x = Mathf.Abs(intVec2.x) + 1;
					intVec2.z = Mathf.Abs(intVec2.z) + 1;
					if (intVec2.x >= 3)
					{
						Vector2 screenPos = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
						screenPos.y = thing.Position.ToUIPosition().y;
						Color textColor = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.x) ? Color.white : Color.red;
						Widgets.DrawNumberOnMap(screenPos, intVec2.x, textColor);
					}
					if (intVec2.z >= 3)
					{
						Vector2 screenPos2 = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
						screenPos2.x = thing.Position.ToUIPosition().x;
						Color textColor2 = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.z) ? Color.white : Color.red;
						Widgets.DrawNumberOnMap(screenPos2, intVec2.z, textColor2);
					}
				}
			}
			Designator_Place.tmpThings.Clear();
		}

		// Token: 0x060072FC RID: 29436 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void DrawPlaceMouseAttachments(float curX, ref float curY)
		{
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x00266608 File Offset: 0x00264808
		protected virtual bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return !GenThing.CloserThingBetween(def, a, b, map, null);
		}

		// Token: 0x060072FE RID: 29438 RVA: 0x0026661C File Offset: 0x0026481C
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				Rect winRect = new Rect(leftX, bottomY - 90f, 200f, 90f);
				Find.WindowStack.ImmediateWindow(73095, winRect, WindowLayer.GameUI, delegate
				{
					RotationDirection rotationDirection = RotationDirection.None;
					Text.Anchor = TextAnchor.MiddleCenter;
					Text.Font = GameFont.Medium;
					Rect rect = new Rect(winRect.width / 2f - 64f - 5f, 15f, 64f, 64f);
					if (Widgets.ButtonImage(rect, TexUI.RotLeftTex, true))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						rotationDirection = RotationDirection.Counterclockwise;
						Event.current.Use();
					}
					Widgets.Label(rect, KeyBindingDefOf.Designator_RotateLeft.MainKeyLabel);
					Rect rect2 = new Rect(winRect.width / 2f + 5f, 15f, 64f, 64f);
					if (Widgets.ButtonImage(rect2, TexUI.RotRightTex, true))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						rotationDirection = RotationDirection.Clockwise;
						Event.current.Use();
					}
					Widgets.Label(rect2, KeyBindingDefOf.Designator_RotateRight.MainKeyLabel);
					if (rotationDirection != RotationDirection.None)
					{
						this.placingRot.Rotate(rotationDirection);
					}
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
				}, true, false, 1f, null);
			}
		}

		// Token: 0x060072FF RID: 29439 RVA: 0x00266694 File Offset: 0x00264894
		public override void SelectedProcessInput(Event ev)
		{
			base.SelectedProcessInput(ev);
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				this.HandleRotationShortcuts();
			}
		}

		// Token: 0x06007300 RID: 29440 RVA: 0x002666C8 File Offset: 0x002648C8
		public override void SelectedUpdate()
		{
			GenDraw.DrawNoBuildEdgeLines();
			IntVec3 intVec = UI.MouseCell();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted) && intVec.InBounds(base.Map))
			{
				if (this.PlacingDef is TerrainDef)
				{
					GenUI.RenderMouseoverBracket();
					return;
				}
				this.DrawBeforeGhost();
				Color ghostCol;
				if (this.CanDesignateCell(intVec).Accepted)
				{
					ghostCol = Designator_Place.CanPlaceColor;
				}
				else
				{
					ghostCol = Designator_Place.CannotPlaceColor;
				}
				this.DrawGhost(ghostCol);
				if (this.CanDesignateCell(intVec).Accepted && this.PlacingDef.specialDisplayRadius > 0.01f)
				{
					GenDraw.DrawRadiusRing(intVec, this.PlacingDef.specialDisplayRadius);
				}
				GenDraw.DrawInteractionCell((ThingDef)this.PlacingDef, intVec, this.placingRot);
			}
		}

		// Token: 0x06007301 RID: 29441 RVA: 0x00266794 File Offset: 0x00264994
		protected virtual void DrawBeforeGhost()
		{
			ThingDef def;
			if ((def = (this.PlacingDef as ThingDef)) != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, def, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
			}
		}

		// Token: 0x06007302 RID: 29442 RVA: 0x002667CC File Offset: 0x002649CC
		protected virtual void DrawGhost(Color ghostCol)
		{
			IntVec3 center = UI.MouseCell();
			Rot4 rot = this.placingRot;
			ThingDef thingDef = (ThingDef)this.PlacingDef;
			ThingStyleDef thingStyleDefForPreview = this.ThingStyleDefForPreview;
			GhostDrawer.DrawGhostThing(center, rot, thingDef, (thingStyleDefForPreview != null) ? thingStyleDefForPreview.Graphic : null, ghostCol, AltitudeLayer.Blueprint, null, true, this.StuffDef);
		}

		// Token: 0x06007303 RID: 29443 RVA: 0x00266814 File Offset: 0x00264A14
		private void HandleRotationShortcuts()
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					Designator_Place.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - Designator_Place.middleMouseDownTime < 0.15f)
				{
					rotationDirection = RotationDirection.Clockwise;
				}
			}
			if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			if (rotationDirection == RotationDirection.Clockwise)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				this.placingRot.Rotate(RotationDirection.Clockwise);
			}
			if (rotationDirection == RotationDirection.Counterclockwise)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				this.placingRot.Rotate(RotationDirection.Counterclockwise);
			}
		}

		// Token: 0x06007304 RID: 29444 RVA: 0x002668C3 File Offset: 0x00264AC3
		public override void Selected()
		{
			this.placingRot = this.PlacingDef.defaultPlacingRot;
		}

		// Token: 0x04003ECC RID: 16076
		protected Rot4 placingRot = Rot4.North;

		// Token: 0x04003ECD RID: 16077
		protected static float middleMouseDownTime;

		// Token: 0x04003ECE RID: 16078
		private const float RotButSize = 64f;

		// Token: 0x04003ECF RID: 16079
		private const float RotButSpacing = 10f;

		// Token: 0x04003ED0 RID: 16080
		public static readonly Color CanPlaceColor = new Color(0.5f, 1f, 0.6f, 0.4f);

		// Token: 0x04003ED1 RID: 16081
		public static readonly Color CannotPlaceColor = new Color(1f, 0f, 0f, 0.4f);

		// Token: 0x04003ED2 RID: 16082
		private static readonly Vector2 PlaceMouseAttachmentDrawOffset = new Vector2(19f, 17f);

		// Token: 0x04003ED3 RID: 16083
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
