using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012EC RID: 4844
	public class Dialog_KeyBindings : Window
	{
		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x0600743C RID: 29756 RVA: 0x00276382 File Offset: 0x00274582
		public override Vector2 InitialSize
		{
			get
			{
				return this.WindowSize;
			}
		}

		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x0600743D RID: 29757 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600743E RID: 29758 RVA: 0x0027638C File Offset: 0x0027458C
		public Dialog_KeyBindings()
		{
			this.forcePause = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
			this.scrollPosition = new Vector2(0f, 0f);
			this.keyPrefsData = KeyPrefs.KeyPrefsData.Clone();
			this.contentHeight = 0f;
			KeyBindingCategoryDef keyBindingCategoryDef = null;
			foreach (KeyBindingDef keyBindingDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (keyBindingCategoryDef != keyBindingDef.category)
				{
					keyBindingCategoryDef = keyBindingDef.category;
					this.contentHeight += 44f;
				}
				this.contentHeight += 34f;
			}
		}

		// Token: 0x0600743F RID: 29759 RVA: 0x00276468 File Offset: 0x00274668
		public override void DoWindowContents(Rect inRect)
		{
			Vector2 vector = new Vector2(120f, 40f);
			float y = vector.y;
			float num = 600f;
			Rect position = new Rect((inRect.width - num) / 2f + inRect.x, inRect.y, num, inRect.height - (y + 10f)).ContractedBy(10f);
			Rect position2 = new Rect(position.x, position.y + position.height + 10f, position.width, y);
			GUI.BeginGroup(position);
			Rect rect = new Rect(0f, 0f, position.width, 40f);
			Text.Font = GameFont.Medium;
			GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
			Widgets.Label(rect, "KeyboardConfig".Translate());
			GenUI.ResetLabelAlign();
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(0f, rect.height, position.width, position.height - rect.height);
			Rect rect2 = new Rect(0f, 0f, outRect.width - 16f, this.contentHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			float num2 = 0f;
			KeyBindingCategoryDef keyBindingCategoryDef = null;
			Dialog_KeyBindings.keyBindingsWorkingList.Clear();
			Dialog_KeyBindings.keyBindingsWorkingList.AddRange(DefDatabase<KeyBindingDef>.AllDefs);
			Dialog_KeyBindings.keyBindingsWorkingList.SortBy((KeyBindingDef x) => x.category.index, (KeyBindingDef x) => x.index);
			for (int i = 0; i < Dialog_KeyBindings.keyBindingsWorkingList.Count; i++)
			{
				KeyBindingDef keyBindingDef = Dialog_KeyBindings.keyBindingsWorkingList[i];
				if (keyBindingCategoryDef != keyBindingDef.category)
				{
					bool skipDrawing = num2 - this.scrollPosition.y + 40f < 0f || num2 - this.scrollPosition.y > outRect.height;
					keyBindingCategoryDef = keyBindingDef.category;
					this.DrawCategoryEntry(keyBindingCategoryDef, rect2.width, ref num2, skipDrawing);
				}
				bool skipDrawing2 = num2 - this.scrollPosition.y + 34f < 0f || num2 - this.scrollPosition.y > outRect.height;
				this.DrawKeyEntry(keyBindingDef, rect2, ref num2, skipDrawing2);
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.BeginGroup(position2);
			int num3 = 3;
			float num4 = vector.x * (float)num3 + 10f * (float)(num3 - 1);
			float num5 = (position2.width - num4) / 2f;
			float num6 = vector.x + 10f;
			Rect rect3 = new Rect(num5, 0f, vector.x, vector.y);
			Rect rect4 = new Rect(num5 + num6, 0f, vector.x, vector.y);
			Rect rect5 = new Rect(num5 + num6 * 2f, 0f, vector.x, vector.y);
			if (Widgets.ButtonText(rect3, "ResetButton".Translate(), true, true, true))
			{
				this.keyPrefsData.ResetToDefaults();
				this.keyPrefsData.ErrorCheck();
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				Event.current.Use();
			}
			if (Widgets.ButtonText(rect4, "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
				Event.current.Use();
			}
			if (Widgets.ButtonText(rect5, "OK".Translate(), true, true, true))
			{
				KeyPrefs.KeyPrefsData = this.keyPrefsData;
				KeyPrefs.Save();
				this.Close(true);
				this.keyPrefsData.ErrorCheck();
				Event.current.Use();
			}
			GUI.EndGroup();
		}

		// Token: 0x06007440 RID: 29760 RVA: 0x00276840 File Offset: 0x00274A40
		private void DrawCategoryEntry(KeyBindingCategoryDef category, float width, ref float curY, bool skipDrawing)
		{
			if (!skipDrawing)
			{
				Rect rect = new Rect(0f, curY, width, 40f).ContractedBy(4f);
				Text.Font = GameFont.Medium;
				Widgets.Label(rect, category.LabelCap);
				Text.Font = GameFont.Small;
				if (Mouse.IsOver(rect) && !category.description.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, new TipSignal(category.description));
				}
			}
			curY += 40f;
			if (!skipDrawing)
			{
				Color color = GUI.color;
				GUI.color = new Color(0.3f, 0.3f, 0.3f);
				Widgets.DrawLineHorizontal(0f, curY, width);
				GUI.color = color;
			}
			curY += 4f;
		}

		// Token: 0x06007441 RID: 29761 RVA: 0x002768F4 File Offset: 0x00274AF4
		private void DrawKeyEntry(KeyBindingDef keyDef, Rect parentRect, ref float curY, bool skipDrawing)
		{
			if (!skipDrawing)
			{
				Rect rect = new Rect(parentRect.x, parentRect.y + curY, parentRect.width, 34f).ContractedBy(3f);
				GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
				Widgets.Label(rect, keyDef.LabelCap);
				GenUI.ResetLabelAlign();
				float num = 4f;
				Vector2 vector = new Vector2(140f, 28f);
				Rect rect2 = new Rect(rect.x + rect.width - vector.x * 2f - num, rect.y, vector.x, vector.y);
				Rect rect3 = new Rect(rect.x + rect.width - vector.x, rect.y, vector.x, vector.y);
				TooltipHandler.TipRegionByKey(rect2, "BindingButtonToolTip");
				TooltipHandler.TipRegionByKey(rect3, "BindingButtonToolTip");
				if (Widgets.ButtonText(rect2, this.keyPrefsData.GetBoundKeyCode(keyDef, KeyPrefs.BindingSlot.A).ToStringReadable(), true, true, true))
				{
					this.SettingButtonClicked(keyDef, KeyPrefs.BindingSlot.A);
				}
				if (Widgets.ButtonText(rect3, this.keyPrefsData.GetBoundKeyCode(keyDef, KeyPrefs.BindingSlot.B).ToStringReadable(), true, true, true))
				{
					this.SettingButtonClicked(keyDef, KeyPrefs.BindingSlot.B);
				}
			}
			curY += 34f;
		}

		// Token: 0x06007442 RID: 29762 RVA: 0x00276A34 File Offset: 0x00274C34
		private void SettingButtonClicked(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			if (Event.current.button == 0)
			{
				Find.WindowStack.Add(new Dialog_DefineBinding(this.keyPrefsData, keyDef, slot));
				Event.current.Use();
				return;
			}
			if (Event.current.button == 1)
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("ResetBinding".Translate(), delegate()
				{
					KeyCode keyCode = (slot == KeyPrefs.BindingSlot.A) ? keyDef.defaultKeyCodeA : keyDef.defaultKeyCodeB;
					this.keyPrefsData.SetBinding(keyDef, slot, keyCode);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list.Add(new FloatMenuOption("ClearBinding".Translate(), delegate()
				{
					this.keyPrefsData.SetBinding(keyDef, slot, KeyCode.None);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x04003FFD RID: 16381
		protected Vector2 scrollPosition;

		// Token: 0x04003FFE RID: 16382
		protected float contentHeight;

		// Token: 0x04003FFF RID: 16383
		protected KeyPrefsData keyPrefsData;

		// Token: 0x04004000 RID: 16384
		protected Vector2 WindowSize = new Vector2(900f, 760f);

		// Token: 0x04004001 RID: 16385
		protected const float EntryHeight = 34f;

		// Token: 0x04004002 RID: 16386
		protected const float CategoryHeadingHeight = 40f;

		// Token: 0x04004003 RID: 16387
		private static List<KeyBindingDef> keyBindingsWorkingList = new List<KeyBindingDef>();
	}
}
