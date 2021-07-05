using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A8A RID: 2698
	public abstract class MainButtonWorker
	{
		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600406B RID: 16491 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float ButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600406C RID: 16492 RVA: 0x0015C748 File Offset: 0x0015A948
		public virtual bool Disabled
		{
			get
			{
				return (Find.CurrentMap == null && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World)) || (Find.WorldRoutePlanner.Active && Find.WorldRoutePlanner.FormingCaravan && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World));
			}
		}

		// Token: 0x0600406D RID: 16493
		public abstract void Activate();

		// Token: 0x0600406E RID: 16494 RVA: 0x0015C7B0 File Offset: 0x0015A9B0
		public virtual void InterfaceTryActivate()
		{
			if (TutorSystem.TutorialMode && this.def.canBeTutorDenied && Find.MainTabsRoot.OpenTab != this.def && !TutorSystem.AllowAction("MainTab-" + this.def.defName + "-Open"))
			{
				return;
			}
			if (this.def.closesWorldView && Find.TilePicker.Active && !Find.TilePicker.AllowEscape)
			{
				Messages.Message("MessagePlayerMustSelectTile".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			this.Activate();
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0015C850 File Offset: 0x0015AA50
		public virtual void DoButton(Rect rect)
		{
			Text.Font = GameFont.Small;
			string text = this.def.LabelCap;
			float num = this.def.LabelCapWidth;
			if (num > rect.width - 2f)
			{
				text = this.def.ShortenedLabelCap;
				num = this.def.ShortenedLabelCapWidth;
			}
			if (this.Disabled)
			{
				Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
				if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
				{
					Event.current.Use();
					return;
				}
			}
			else
			{
				bool flag = num > 0.85f * rect.width - 1f;
				Rect rect2 = rect;
				string label = (this.def.Icon == null) ? text : "";
				float textLeftMargin = flag ? 2f : -1f;
				if (Widgets.ButtonTextSubtle(rect2, label, this.ButtonBarPercent, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2), null, false))
				{
					this.InterfaceTryActivate();
				}
				if (this.def.Icon != null)
				{
					Vector2 vector = rect.center;
					float num2 = 16f;
					if (Mouse.IsOver(rect))
					{
						vector += new Vector2(2f, -2f);
					}
					GUI.DrawTexture(new Rect(vector.x - num2, vector.y - num2, 32f, 32f), this.def.Icon);
				}
				if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
				{
					UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
				}
				if (Mouse.IsOver(rect) && !this.def.description.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, this.def.LabelCap + "\n\n" + this.def.description);
				}
			}
		}

		// Token: 0x0400250E RID: 9486
		public MainButtonDef def;

		// Token: 0x0400250F RID: 9487
		private const float CompactModeMargin = 2f;

		// Token: 0x04002510 RID: 9488
		private const float IconSize = 32f;
	}
}
