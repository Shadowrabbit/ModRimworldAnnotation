using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013E0 RID: 5088
	[StaticConstructorOnStartup]
	public class TilePicker
	{
		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x06007BBC RID: 31676 RVA: 0x002BA371 File Offset: 0x002B8571
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x06007BBD RID: 31677 RVA: 0x002BA379 File Offset: 0x002B8579
		public bool AllowEscape
		{
			get
			{
				return this.allowEscape;
			}
		}

		// Token: 0x06007BBE RID: 31678 RVA: 0x002BA381 File Offset: 0x002B8581
		public void StartTargeting(Func<int, bool> validator, Action<int> tileChosen, bool allowEscape = true, Action noTileChosen = null)
		{
			this.validator = validator;
			this.allowEscape = allowEscape;
			this.noTileChosen = noTileChosen;
			this.tileChosen = tileChosen;
			Find.WorldSelector.ClearSelection();
			this.active = true;
		}

		// Token: 0x06007BBF RID: 31679 RVA: 0x002BA3B1 File Offset: 0x002B85B1
		public void StopTargeting()
		{
			if (this.active && this.noTileChosen != null)
			{
				this.noTileChosen();
			}
			this.StopTargetingInt();
		}

		// Token: 0x06007BC0 RID: 31680 RVA: 0x002BA3D4 File Offset: 0x002B85D4
		private void StopTargetingInt()
		{
			this.active = false;
		}

		// Token: 0x06007BC1 RID: 31681 RVA: 0x002BA3E0 File Offset: 0x002B85E0
		public void TileSelectorOnGUI()
		{
			Vector2 buttonSize = TilePicker.ButtonSize;
			int num = 24;
			Rect rect = new Rect((float)UI.screenWidth / 2f - 2f * buttonSize.x / 2f - (float)num / 2f, (float)UI.screenHeight - (buttonSize.y + 8f) + -50f, 2f * buttonSize.x + (float)num, buttonSize.y + 16f);
			Widgets.DrawWindowBackground(rect);
			if (Widgets.ButtonText(new Rect(rect.x + 8f, rect.y + 8f, buttonSize.x, buttonSize.y), "SelectRandomSite".Translate(), true, true, true))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
			}
			if (Widgets.ButtonText(new Rect(rect.x + 16f + buttonSize.x, rect.y + 8f, buttonSize.x, buttonSize.y), "Next".Translate(), true, true, true))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				int selectedTile = Find.WorldInterface.SelectedTile;
				if (selectedTile < 0)
				{
					Messages.Message("MustSelectStartingSite".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else if (this.validator(selectedTile))
				{
					this.StopTargetingInt();
					this.tileChosen(selectedTile);
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.Active && !this.allowEscape)
			{
				Event.current.Use();
			}
		}

		// Token: 0x0400447C RID: 17532
		private static readonly Vector2 ButtonSize = new Vector2(150f, 38f);

		// Token: 0x0400447D RID: 17533
		private const int Padding = 8;

		// Token: 0x0400447E RID: 17534
		private const int BottomPanelYOffset = -50;

		// Token: 0x0400447F RID: 17535
		private Func<int, bool> validator;

		// Token: 0x04004480 RID: 17536
		private bool allowEscape;

		// Token: 0x04004481 RID: 17537
		private bool active;

		// Token: 0x04004482 RID: 17538
		private Action<int> tileChosen;

		// Token: 0x04004483 RID: 17539
		private Action noTileChosen;
	}
}
