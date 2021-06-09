using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E4 RID: 5860
	public class Dialog_RewardPrefsConfig : Window
	{
		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x060080B6 RID: 32950 RVA: 0x000566FD File Offset: 0x000548FD
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 440f);
			}
		}

		// Token: 0x060080B7 RID: 32951 RVA: 0x0005670E File Offset: 0x0005490E
		public Dialog_RewardPrefsConfig()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x060080B8 RID: 32952 RVA: 0x00262018 File Offset: 0x00260218
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, this.InitialSize.x / 2f, 40f), "ChooseRewards".Translate());
			Text.Font = GameFont.Small;
			string text = "ChooseRewardsDesc".Translate();
			float height = Text.CalcHeight(text, inRect.width);
			Rect rect = new Rect(0f, 40f, inRect.width, height);
			Widgets.Label(rect, text);
			IEnumerable<Faction> allFactionsVisibleInViewOrder = Find.FactionManager.AllFactionsVisibleInViewOrder;
			Rect outRect = new Rect(inRect);
			outRect.yMax -= this.CloseButSize.y;
			outRect.yMin += 44f + rect.height + 4f;
			float num = 0f;
			Rect rect2 = new Rect(0f, num, outRect.width - 16f, this.viewRectHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			int num2 = 0;
			foreach (Faction faction in allFactionsVisibleInViewOrder)
			{
				if (!faction.IsPlayer)
				{
					float x = 0f;
					if (faction.def.HasRoyalTitles)
					{
						this.DoFactionInfo(rect2, faction, ref x, ref num, ref num2);
						TaggedString label = "AcceptRoyalFavor".Translate(faction.Named("FACTION")).CapitalizeFirst();
						Rect rect3 = new Rect(x, num, label.GetWidthCached(), 45f);
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect3, label);
						Text.Anchor = TextAnchor.UpperLeft;
						if (Mouse.IsOver(rect3))
						{
							TooltipHandler.TipRegion(rect3, "AcceptRoyalFavorDesc".Translate(faction.Named("FACTION")));
							Widgets.DrawHighlight(rect3);
						}
						Widgets.Checkbox(rect2.width - 150f, num + 12f, ref faction.allowRoyalFavorRewards, 24f, false, false, null, null);
						num += 45f;
					}
					if (faction.CanEverGiveGoodwillRewards)
					{
						x = 0f;
						this.DoFactionInfo(rect2, faction, ref x, ref num, ref num2);
						TaggedString label2 = "AcceptGoodwill".Translate().CapitalizeFirst();
						Rect rect4 = new Rect(x, num, label2.GetWidthCached(), 45f);
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect4, label2);
						Text.Anchor = TextAnchor.UpperLeft;
						if (Mouse.IsOver(rect4))
						{
							TooltipHandler.TipRegion(rect4, "AcceptGoodwillDesc".Translate(faction.Named("FACTION")));
							Widgets.DrawHighlight(rect4);
						}
						Widgets.Checkbox(rect2.width - 150f, num + 12f, ref faction.allowGoodwillRewards, 24f, false, false, null, null);
						Widgets.Label(new Rect(rect2.width - 100f, num, 100f, 35f), (faction.PlayerGoodwill.ToStringWithSign() + "\n" + faction.PlayerRelationKind.GetLabel()).Colorize(faction.PlayerRelationKind.GetColor()));
						num += 45f;
					}
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				this.viewRectHeight = num;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x060080B9 RID: 32953 RVA: 0x00262398 File Offset: 0x00260598
		private void DoFactionInfo(Rect rect, Faction faction, ref float curX, ref float curY, ref int index)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(new Rect(curX, curY, rect.width, 45f));
			}
			FactionUIUtility.DrawFactionIconWithTooltip(new Rect(curX, curY + 5f, 35f, 35f), faction);
			curX += 45f;
			Rect rect2 = new Rect(curX, curY, 250f, 45f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, faction.Name);
			Text.Anchor = TextAnchor.UpperLeft;
			curX += 250f;
			if (Mouse.IsOver(rect2))
			{
				TipSignal tip = new TipSignal(() => string.Concat(new string[]
				{
					faction.Name,
					"\n\n",
					faction.def.description,
					"\n\n",
					faction.PlayerRelationKind.GetLabel().Colorize(faction.PlayerRelationKind.GetColor())
				}), faction.loadID ^ 71729271);
				TooltipHandler.TipRegion(rect2, tip);
				Widgets.DrawHighlight(rect2);
			}
			index++;
		}

		// Token: 0x04005358 RID: 21336
		private Vector2 scrollPosition;

		// Token: 0x04005359 RID: 21337
		private float viewRectHeight;

		// Token: 0x0400535A RID: 21338
		private const float TitleHeight = 40f;

		// Token: 0x0400535B RID: 21339
		private const float RowHeight = 45f;

		// Token: 0x0400535C RID: 21340
		private const float IconSize = 35f;

		// Token: 0x0400535D RID: 21341
		private const float GoodwillWidth = 100f;

		// Token: 0x0400535E RID: 21342
		private const float CheckboxOffset = 150f;

		// Token: 0x0400535F RID: 21343
		private const float FactionNameWidth = 250f;
	}
}
