using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB4 RID: 4020
	public abstract class MainButtonWorker
	{
		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x060057F3 RID: 22515 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float ButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x060057F4 RID: 22516 RVA: 0x001CEBF0 File Offset: 0x001CCDF0
		public virtual bool Disabled
		{
			get
			{
				return (Find.CurrentMap == null && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World)) || (Find.WorldRoutePlanner.Active && Find.WorldRoutePlanner.FormingCaravan && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World));
			}
		}

		// Token: 0x060057F5 RID: 22517
		public abstract void Activate();

		// Token: 0x060057F6 RID: 22518 RVA: 0x001CEC58 File Offset: 0x001CCE58
		public virtual void InterfaceTryActivate()
		{
			if (TutorSystem.TutorialMode && this.def.canBeTutorDenied && Find.MainTabsRoot.OpenTab != this.def && !TutorSystem.AllowAction("MainTab-" + this.def.defName + "-Open"))
			{
				return;
			}
			this.Activate();
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x001CECB8 File Offset: 0x001CCEB8
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
				if (Widgets.ButtonTextSubtle(rect2, label, this.ButtonBarPercent, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2)))
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

		// Token: 0x040039F3 RID: 14835
		public MainButtonDef def;

		// Token: 0x040039F4 RID: 14836
		private const float CompactModeMargin = 2f;

		// Token: 0x040039F5 RID: 14837
		private const float IconSize = 32f;
	}
}
