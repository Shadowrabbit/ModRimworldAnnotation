using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020021DE RID: 8670
	public class WITab_Caravan_Social : WITab
	{
		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x0600B99B RID: 47515 RVA: 0x00077FF1 File Offset: 0x000761F1
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x0600B99C RID: 47516 RVA: 0x000782DE File Offset: 0x000764DE
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

		// Token: 0x0600B99D RID: 47517 RVA: 0x000782FE File Offset: 0x000764FE
		public WITab_Caravan_Social()
		{
			this.labelKey = "TabCaravanSocial";
		}

		// Token: 0x0600B99E RID: 47518 RVA: 0x003566F0 File Offset: 0x003548F0
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

		// Token: 0x0600B99F RID: 47519 RVA: 0x00078311 File Offset: 0x00076511
		protected override void UpdateSize()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.UpdateSize();
			this.size.x = 243f;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x0600B9A0 RID: 47520 RVA: 0x003567A0 File Offset: 0x003549A0
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
				}, true, false, 1f);
			}
		}

		// Token: 0x0600B9A1 RID: 47521 RVA: 0x00356848 File Offset: 0x00354A48
		public override void OnOpen()
		{
			base.OnOpen();
			if ((this.specificSocialTabForPawn == null || !this.Pawns.Contains(this.specificSocialTabForPawn)) && this.Pawns.Any<Pawn>())
			{
				this.specificSocialTabForPawn = this.Pawns[0];
			}
		}

		// Token: 0x0600B9A2 RID: 47522 RVA: 0x00356898 File Offset: 0x00354A98
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

		// Token: 0x0600B9A3 RID: 47523 RVA: 0x00356A1C File Offset: 0x00354C1C
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

		// Token: 0x0600B9A4 RID: 47524 RVA: 0x00356A84 File Offset: 0x00354C84
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
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
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

		// Token: 0x0600B9A5 RID: 47525 RVA: 0x00078350 File Offset: 0x00076550
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificSocialTabForPawn = null;
		}

		// Token: 0x0600B9A6 RID: 47526 RVA: 0x0007835F File Offset: 0x0007655F
		private void EnsureSpecificSocialTabForPawnValid()
		{
			if (this.specificSocialTabForPawn != null && (this.specificSocialTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificSocialTabForPawn)))
			{
				this.specificSocialTabForPawn = null;
			}
		}

		// Token: 0x04007ED6 RID: 32470
		private Vector2 scrollPosition;

		// Token: 0x04007ED7 RID: 32471
		private float scrollViewHeight;

		// Token: 0x04007ED8 RID: 32472
		private Pawn specificSocialTabForPawn;

		// Token: 0x04007ED9 RID: 32473
		private const float RowHeight = 34f;

		// Token: 0x04007EDA RID: 32474
		private const float ScrollViewTopMargin = 15f;

		// Token: 0x04007EDB RID: 32475
		private const float PawnLabelHeight = 18f;

		// Token: 0x04007EDC RID: 32476
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04007EDD RID: 32477
		private const float SpaceAroundIcon = 4f;
	}
}
