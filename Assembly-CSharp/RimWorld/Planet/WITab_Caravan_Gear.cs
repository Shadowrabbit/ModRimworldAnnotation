using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020021D1 RID: 8657
	public class WITab_Caravan_Gear : WITab
	{
		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x0600B94E RID: 47438 RVA: 0x00077FF1 File Offset: 0x000761F1
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x0600B94F RID: 47439 RVA: 0x00077FFE File Offset: 0x000761FE
		public WITab_Caravan_Gear()
		{
			this.labelKey = "TabCaravanGear";
		}

		// Token: 0x0600B950 RID: 47440 RVA: 0x00354780 File Offset: 0x00352980
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.leftPaneWidth = 469f;
			this.rightPaneWidth = 345f;
			this.size.x = this.leftPaneWidth + this.rightPaneWidth;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x0600B951 RID: 47441 RVA: 0x00078011 File Offset: 0x00076211
		public override void OnOpen()
		{
			base.OnOpen();
			this.draggedItem = null;
		}

		// Token: 0x0600B952 RID: 47442 RVA: 0x003547E4 File Offset: 0x003529E4
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			this.CheckDraggedItemStillValid();
			this.CheckDropDraggedItem();
			Rect position = new Rect(0f, 0f, this.leftPaneWidth, this.size.y);
			GUI.BeginGroup(position);
			this.DoLeftPane();
			GUI.EndGroup();
			GUI.BeginGroup(new Rect(position.xMax, 0f, this.rightPaneWidth, this.size.y));
			this.DoRightPane();
			GUI.EndGroup();
			if (this.draggedItem != null && this.droppedDraggedItem)
			{
				this.droppedDraggedItem = false;
				this.draggedItem = null;
			}
		}

		// Token: 0x0600B953 RID: 47443 RVA: 0x00354888 File Offset: 0x00352A88
		private void DoLeftPane()
		{
			Rect rect = new Rect(0f, 0f, this.leftPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.leftPaneScrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.leftPaneScrollPosition, rect2, true);
			this.DoPawnRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.leftPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600B954 RID: 47444 RVA: 0x00354924 File Offset: 0x00352B24
		private void DoRightPane()
		{
			Rect rect = new Rect(0f, 0f, this.rightPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.rightPaneScrollViewHeight);
			if (this.draggedItem != null && rect.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != null)
			{
				Widgets.DrawHighlight(rect);
				if (this.droppedDraggedItem)
				{
					this.MoveDraggedItemToInventory();
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
			}
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.rightPaneScrollPosition, rect2, true);
			this.DoInventoryRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.rightPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600B955 RID: 47445 RVA: 0x00354A0C File Offset: 0x00352C0C
		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.draggedItem != null)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Rect rect = new Rect(mousePosition.x - this.draggedItemPosOffset.x, mousePosition.y - this.draggedItemPosOffset.y, 32f, 32f);
				Find.WindowStack.ImmediateWindow(1283641090, rect, WindowLayer.Super, delegate
				{
					if (this.draggedItem == null)
					{
						return;
					}
					Widgets.ThingIcon(rect.AtZero(), this.draggedItem, 1f);
				}, false, false, 0f);
			}
			this.CheckDropDraggedItem();
		}

		// Token: 0x0600B956 RID: 47446 RVA: 0x00354AA8 File Offset: 0x00352CA8
		private void DoPawnRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(135f, curY + 6f, 200f, 100f), "DragToRearrange".Translate());
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (pawn2.IsPrisoner)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisoners".Translate());
						flag = true;
					}
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x0600B957 RID: 47447 RVA: 0x00354BA4 File Offset: 0x00352DA4
		private void DoPawnRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.leftPaneScrollPosition.y - 50f;
			float num2 = this.leftPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoPawnRow(new Rect(0f, curY, viewRect.width, 50f), p);
			}
			curY += 50f;
		}

		// Token: 0x0600B958 RID: 47448 RVA: 0x00354C0C File Offset: 0x00352E0C
		private void DoPawnRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			bool flag = this.draggedItem != null && rect2.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != p;
			if ((Mouse.IsOver(rect2) && this.draggedItem == null) || flag)
			{
				Widgets.DrawHighlight(rect2);
			}
			if (flag && this.droppedDraggedItem)
			{
				this.TryEquipDraggedItem(p);
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
			Rect bgRect = new Rect(rect3.xMax + 4f, 16f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float xMax = bgRect.xMax;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					this.DoEquippedGear(allEquipmentListForReading[i], p, ref xMax);
				}
			}
			if (p.apparel != null)
			{
				WITab_Caravan_Gear.tmpApparel.Clear();
				WITab_Caravan_Gear.tmpApparel.AddRange(p.apparel.WornApparel);
				WITab_Caravan_Gear.tmpApparel.SortBy((Apparel x) => x.def.apparel.LastLayer.drawOrder, (Apparel x) => -x.def.apparel.HumanBodyCoverage);
				for (int j = 0; j < WITab_Caravan_Gear.tmpApparel.Count; j++)
				{
					this.DoEquippedGear(WITab_Caravan_Gear.tmpApparel[j], p, ref xMax);
				}
			}
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x0600B959 RID: 47449 RVA: 0x00354E94 File Offset: 0x00353094
		private void DoInventoryRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanWeaponsAndApparel".Translate());
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (this.IsVisibleWeapon(thing.def))
				{
					if (!flag)
					{
						flag = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < list.Count; j++)
			{
				Thing thing2 = list[j];
				if (thing2.def.IsApparel)
				{
					if (!flag2)
					{
						flag2 = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing2);
				}
			}
			if (!flag && !flag2)
			{
				Widgets.NoneLabel(ref curY, scrollViewRect.width, null);
			}
		}

		// Token: 0x0600B95A RID: 47450 RVA: 0x00354F58 File Offset: 0x00353158
		private void DoInventoryRow(ref float curY, Rect viewRect, Rect scrollOutRect, Thing t)
		{
			float num = this.rightPaneScrollPosition.y - 30f;
			float num2 = this.rightPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoInventoryRow(new Rect(0f, curY, viewRect.width, 30f), t);
			}
			curY += 30f;
		}

		// Token: 0x0600B95B RID: 47451 RVA: 0x00354FC0 File Offset: 0x003531C0
		private void DoInventoryRow(Rect rect, Thing t)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, t);
			rect2.width -= 24f;
			if (this.draggedItem == null && Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			float num = (t == this.draggedItem) ? 0.5f : 1f;
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, t, num);
			GUI.color = new Color(1f, 1f, 1f, num);
			Rect rect4 = new Rect(rect3.xMax + 4f, 0f, 250f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect4, t.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect2))
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = new Vector2(16f, 16f);
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			GUI.EndGroup();
		}

		// Token: 0x0600B95C RID: 47452 RVA: 0x00355138 File Offset: 0x00353338
		private void DoEquippedGear(Thing t, Pawn p, ref float curX)
		{
			Rect rect = new Rect(curX, 9f, 32f, 32f);
			bool flag = Mouse.IsOver(rect);
			float alpha;
			if (t == this.draggedItem)
			{
				alpha = 0.2f;
			}
			else if (flag && this.draggedItem == null)
			{
				alpha = 0.75f;
			}
			else
			{
				alpha = 1f;
			}
			Widgets.ThingIcon(rect, t, alpha);
			curX += 32f;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, t.LabelCap);
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && flag)
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = Event.current.mousePosition - rect.position;
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600B95D RID: 47453 RVA: 0x00355218 File Offset: 0x00353418
		private void CheckDraggedItemStillValid()
		{
			if (this.draggedItem == null)
			{
				return;
			}
			if (this.draggedItem.Destroyed)
			{
				this.draggedItem = null;
				return;
			}
			if (this.CurrentWearerOf(this.draggedItem) != null)
			{
				return;
			}
			if (CaravanInventoryUtility.AllInventoryItems(base.SelCaravan).Contains(this.draggedItem))
			{
				return;
			}
			this.draggedItem = null;
		}

		// Token: 0x0600B95E RID: 47454 RVA: 0x00078020 File Offset: 0x00076220
		private void CheckDropDraggedItem()
		{
			if (this.draggedItem == null)
			{
				return;
			}
			if (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseUp)
			{
				this.droppedDraggedItem = true;
			}
		}

		// Token: 0x0600B95F RID: 47455 RVA: 0x0007804C File Offset: 0x0007624C
		private bool IsVisibleWeapon(ThingDef t)
		{
			return t.IsWeapon && t != ThingDefOf.WoodLog && t != ThingDefOf.Beer;
		}

		// Token: 0x0600B960 RID: 47456 RVA: 0x00355274 File Offset: 0x00353474
		private Pawn CurrentWearerOf(Thing t)
		{
			IThingHolder parentHolder = t.ParentHolder;
			if (parentHolder is Pawn_EquipmentTracker || parentHolder is Pawn_ApparelTracker)
			{
				return (Pawn)parentHolder.ParentHolder;
			}
			return null;
		}

		// Token: 0x0600B961 RID: 47457 RVA: 0x003552A8 File Offset: 0x003534A8
		private void MoveDraggedItemToInventory()
		{
			this.droppedDraggedItem = false;
			Apparel apparel;
			if ((apparel = (this.draggedItem as Apparel)) != null && this.CurrentWearerOf(apparel) != null && this.CurrentWearerOf(apparel).apparel.IsLocked(apparel))
			{
				Messages.Message("MessageCantUnequipLockedApparel".Translate(), this.CurrentWearerOf(apparel), MessageTypeDefOf.RejectInput, false);
				this.draggedItem = null;
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(this.draggedItem, this.Pawns, null, null);
			if (pawn != null)
			{
				this.draggedItem.holdingOwner.TryTransferToContainer(this.draggedItem, pawn.inventory.innerContainer, 1, true);
			}
			else
			{
				Log.Warning("Could not find any pawn to move " + this.draggedItem + " to.", false);
			}
			this.draggedItem = null;
		}

		// Token: 0x0600B962 RID: 47458 RVA: 0x00355378 File Offset: 0x00353578
		private void TryEquipDraggedItem(Pawn p)
		{
			WITab_Caravan_Gear.<>c__DisplayClass41_0 CS$<>8__locals1 = new WITab_Caravan_Gear.<>c__DisplayClass41_0();
			CS$<>8__locals1.p = p;
			CS$<>8__locals1.<>4__this = this;
			this.droppedDraggedItem = false;
			string str;
			if (!EquipmentUtility.CanEquip_NewTmp(this.draggedItem, CS$<>8__locals1.p, out str, true))
			{
				Messages.Message("MessageCantEquipCustom".Translate(str.CapitalizeFirst()), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
				this.draggedItem = null;
				return;
			}
			if (this.draggedItem.def.IsWeapon)
			{
				if (CS$<>8__locals1.p.guest.IsPrisoner)
				{
					Messages.Message("MessageCantEquipCustom".Translate("MessagePrisonerCannotEquipWeapon".Translate(CS$<>8__locals1.p.Named("PAWN"))), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (CS$<>8__locals1.p.WorkTagIsDisabled(WorkTags.Violent))
				{
					Messages.Message("MessageCantEquipIncapableOfViolence".Translate(CS$<>8__locals1.p.LabelShort, CS$<>8__locals1.p), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (!CS$<>8__locals1.p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					Messages.Message("MessageCantEquipIncapableOfManipulation".Translate(), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
			}
			Apparel apparel = this.draggedItem as Apparel;
			CS$<>8__locals1.thingWithComps = (this.draggedItem as ThingWithComps);
			if (apparel != null && CS$<>8__locals1.p.apparel != null)
			{
				if (!ApparelUtility.HasPartsToWear(CS$<>8__locals1.p, apparel.def))
				{
					Messages.Message("MessageCantWearApparelMissingBodyParts".Translate(CS$<>8__locals1.p.LabelShort, CS$<>8__locals1.p), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (this.CurrentWearerOf(apparel) != null && this.CurrentWearerOf(apparel).apparel.IsLocked(apparel))
				{
					Messages.Message("MessageCantUnequipLockedApparel".Translate(), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (CS$<>8__locals1.p.apparel.WouldReplaceLockedApparel(apparel))
				{
					Messages.Message("MessageWouldReplaceLockedApparel".Translate(CS$<>8__locals1.p.LabelShort, CS$<>8__locals1.p), CS$<>8__locals1.p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				WITab_Caravan_Gear.tmpExistingApparel.Clear();
				WITab_Caravan_Gear.tmpExistingApparel.AddRange(CS$<>8__locals1.p.apparel.WornApparel);
				for (int i = 0; i < WITab_Caravan_Gear.tmpExistingApparel.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(apparel.def, WITab_Caravan_Gear.tmpExistingApparel[i].def, CS$<>8__locals1.p.RaceProps.body))
					{
						CS$<>8__locals1.p.apparel.Remove(WITab_Caravan_Gear.tmpExistingApparel[i]);
						Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(WITab_Caravan_Gear.tmpExistingApparel[i], this.Pawns, null, null);
						if (pawn != null)
						{
							pawn.inventory.innerContainer.TryAdd(WITab_Caravan_Gear.tmpExistingApparel[i], true);
						}
						else
						{
							Log.Warning("Could not find any pawn to move " + WITab_Caravan_Gear.tmpExistingApparel[i] + " to.", false);
							WITab_Caravan_Gear.tmpExistingApparel[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				CS$<>8__locals1.p.apparel.Wear((Apparel)apparel.SplitOff(1), false, false);
				if (CS$<>8__locals1.p.outfits != null)
				{
					CS$<>8__locals1.p.outfits.forcedHandler.SetForced(apparel, true);
				}
			}
			else if (CS$<>8__locals1.thingWithComps != null && CS$<>8__locals1.p.equipment != null)
			{
				string personaWeaponConfirmationText = EquipmentUtility.GetPersonaWeaponConfirmationText(this.draggedItem, CS$<>8__locals1.p);
				if (!personaWeaponConfirmationText.NullOrEmpty())
				{
					Thing thing = this.draggedItem;
					Find.WindowStack.Add(new Dialog_MessageBox(personaWeaponConfirmationText, "Yes".Translate(), delegate()
					{
						base.<TryEquipDraggedItem>g__AddEquipment|0();
					}, "No".Translate(), null, null, false, null, null));
					this.draggedItem = null;
					return;
				}
				CS$<>8__locals1.<TryEquipDraggedItem>g__AddEquipment|0();
			}
			else
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not make ",
					CS$<>8__locals1.p,
					" equip or wear ",
					this.draggedItem
				}), false);
			}
			this.draggedItem = null;
		}

		// Token: 0x04007E91 RID: 32401
		private Vector2 leftPaneScrollPosition;

		// Token: 0x04007E92 RID: 32402
		private float leftPaneScrollViewHeight;

		// Token: 0x04007E93 RID: 32403
		private Vector2 rightPaneScrollPosition;

		// Token: 0x04007E94 RID: 32404
		private float rightPaneScrollViewHeight;

		// Token: 0x04007E95 RID: 32405
		private Thing draggedItem;

		// Token: 0x04007E96 RID: 32406
		private Vector2 draggedItemPosOffset;

		// Token: 0x04007E97 RID: 32407
		private bool droppedDraggedItem;

		// Token: 0x04007E98 RID: 32408
		private float leftPaneWidth;

		// Token: 0x04007E99 RID: 32409
		private float rightPaneWidth;

		// Token: 0x04007E9A RID: 32410
		private const float PawnRowHeight = 50f;

		// Token: 0x04007E9B RID: 32411
		private const float ItemRowHeight = 30f;

		// Token: 0x04007E9C RID: 32412
		private const float PawnLabelHeight = 18f;

		// Token: 0x04007E9D RID: 32413
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04007E9E RID: 32414
		private const float GearLabelColumnWidth = 250f;

		// Token: 0x04007E9F RID: 32415
		private const float SpaceAroundIcon = 4f;

		// Token: 0x04007EA0 RID: 32416
		private const float EquippedGearColumnWidth = 250f;

		// Token: 0x04007EA1 RID: 32417
		private const float EquippedGearIconSize = 32f;

		// Token: 0x04007EA2 RID: 32418
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x04007EA3 RID: 32419
		private static List<ThingWithComps> tmpExistingEquipment = new List<ThingWithComps>();

		// Token: 0x04007EA4 RID: 32420
		private static List<Apparel> tmpExistingApparel = new List<Apparel>();
	}
}
