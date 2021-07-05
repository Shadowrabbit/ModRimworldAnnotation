using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001812 RID: 6162
	public class WITab_Caravan_Social : WITab
	{
		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x06009052 RID: 36946 RVA: 0x00339EDE File Offset: 0x003380DE
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x06009053 RID: 36947 RVA: 0x0033BF3A File Offset: 0x0033A13A
		private float SpecificSocialTabWidth
		{
			get
			{
				this.EnsureSpecificSocialTabForPawnValid();
				if (this.specificSocialTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 540f;
			}
		}

		// Token: 0x06009054 RID: 36948 RVA: 0x0033BF5A File Offset: 0x0033A15A
		public WITab_Caravan_Social()
		{
			this.labelKey = "TabCaravanSocial";
		}

		// Token: 0x06009055 RID: 36949 RVA: 0x0033BF70 File Offset: 0x0033A170
		protected override void FillTab()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 15f, this.size.x, this.size.y - 15f).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06009056 RID: 36950 RVA: 0x0033C020 File Offset: 0x0033A220
		protected override void UpdateSize()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.UpdateSize();
			this.size.x = 243f;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x06009057 RID: 36951 RVA: 0x0033C060 File Offset: 0x0033A260
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificSocialTabForPawn = this.specificSocialTabForPawn;
			if (localSpecificSocialTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificSocialTabWidth = this.SpecificSocialTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificSocialTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificSocialTabForPawn.DestroyedOrNull())
					{
						return;
					}
					SocialCardUtility.DrawSocialCard(rect.AtZero(), localSpecificSocialTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificSocialTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null);
			}
		}

		// Token: 0x06009058 RID: 36952 RVA: 0x0033C108 File Offset: 0x0033A308
		public override void OnOpen()
		{
			base.OnOpen();
			if ((this.specificSocialTabForPawn == null || !this.Pawns.Contains(this.specificSocialTabForPawn)) && this.Pawns.Any<Pawn>())
			{
				this.specificSocialTabForPawn = this.Pawns[0];
			}
		}

		// Token: 0x06009059 RID: 36953 RVA: 0x0033C158 File Offset: 0x0033A358
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(base.SelCaravan, null, null);
			GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
			Widgets.Label(new Rect(0f, curY, scrollViewRect.width, 24f), string.Format("{0}: {1}", "Negotiator".TranslateSimple(), (pawn != null) ? pawn.LabelShort : "NoneCapable".Translate().ToString()));
			curY += 24f;
			if (this.specificSocialTabForPawn != null && !pawns.Contains(this.specificSocialTabForPawn))
			{
				this.specificSocialTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn2 = pawns[i];
				if (pawn2.RaceProps.IsFlesh && pawn2.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn3 = pawns[j];
				if (pawn3.RaceProps.IsFlesh && !pawn3.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn3);
				}
			}
		}

		// Token: 0x0600905A RID: 36954 RVA: 0x0033C2DC File Offset: 0x0033A4DC
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 34f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 34f), p);
			}
			curY += 34f;
		}

		// Token: 0x0600905B RID: 36955 RVA: 0x0033C344 File Offset: 0x0033A544
		private void DoRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificSocialTabForPawn);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButtonInvisible(rect2, p, ref this.specificSocialTabForPawn);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f, null);
			Rect bgRect = new Rect(rect3.xMax + 4f, 8f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x0600905C RID: 36956 RVA: 0x0033C4BC File Offset: 0x0033A6BC
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificSocialTabForPawn = null;
		}

		// Token: 0x0600905D RID: 36957 RVA: 0x0033C4CB File Offset: 0x0033A6CB
		private void EnsureSpecificSocialTabForPawnValid()
		{
			if (this.specificSocialTabForPawn != null && (this.specificSocialTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificSocialTabForPawn)))
			{
				this.specificSocialTabForPawn = null;
			}
		}

		// Token: 0x04005ACA RID: 23242
		private Vector2 scrollPosition;

		// Token: 0x04005ACB RID: 23243
		private float scrollViewHeight;

		// Token: 0x04005ACC RID: 23244
		private Pawn specificSocialTabForPawn;

		// Token: 0x04005ACD RID: 23245
		private const float RowHeight = 34f;

		// Token: 0x04005ACE RID: 23246
		private const float ScrollViewTopMargin = 15f;

		// Token: 0x04005ACF RID: 23247
		private const float PawnLabelHeight = 18f;

		// Token: 0x04005AD0 RID: 23248
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04005AD1 RID: 23249
		private const float SpaceAroundIcon = 4f;
	}
}
