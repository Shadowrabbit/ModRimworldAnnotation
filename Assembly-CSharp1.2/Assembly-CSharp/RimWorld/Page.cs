using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A8E RID: 6798
	public abstract class Page : Window
	{
		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x0600961F RID: 38431 RVA: 0x00064291 File Offset: 0x00062491
		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x06009620 RID: 38432 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06009621 RID: 38433 RVA: 0x00064298 File Offset: 0x00062498
		public Page()
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
		}

		// Token: 0x06009622 RID: 38434 RVA: 0x000642C3 File Offset: 0x000624C3
		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		// Token: 0x06009623 RID: 38435 RVA: 0x002B9E20 File Offset: 0x002B8020
		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = 45f + extraTopSpace;
			}
			return new Rect(0f, num, rect.width, rect.height - 38f - num - 17f);
		}

		// Token: 0x06009624 RID: 38436 RVA: 0x002B9E68 File Offset: 0x002B8068
		protected void DoBottomButtons(Rect rect, string nextLabel = null, string midLabel = null, Action midAct = null, bool showNext = true, bool doNextOnKeypress = true)
		{
			float y = rect.y + rect.height - 38f;
			Text.Font = GameFont.Small;
			string label = "Back".Translate();
			if ((Widgets.ButtonText(new Rect(rect.x, y, Page.BottomButSize.x, Page.BottomButSize.y), label, true, true, true) || KeyBindingDefOf.Cancel.KeyDownEvent) && this.CanDoBack())
			{
				this.DoBack();
			}
			if (showNext)
			{
				if (nextLabel.NullOrEmpty())
				{
					nextLabel = "Next".Translate();
				}
				Rect rect2 = new Rect(rect.x + rect.width - Page.BottomButSize.x, y, Page.BottomButSize.x, Page.BottomButSize.y);
				if ((Widgets.ButtonText(rect2, nextLabel, true, true, true) || (doNextOnKeypress && KeyBindingDefOf.Accept.KeyDownEvent)) && this.CanDoNext())
				{
					this.DoNext();
				}
				UIHighlighter.HighlightOpportunity(rect2, "NextPage");
			}
			if (midAct != null && Widgets.ButtonText(new Rect(rect.x + rect.width / 2f - Page.BottomButSize.x / 2f, y, Page.BottomButSize.x, Page.BottomButSize.y), midLabel, true, true, true))
			{
				midAct();
			}
		}

		// Token: 0x06009625 RID: 38437 RVA: 0x000642F7 File Offset: 0x000624F7
		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		// Token: 0x06009626 RID: 38438 RVA: 0x00064311 File Offset: 0x00062511
		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

		// Token: 0x06009627 RID: 38439 RVA: 0x002B9FC8 File Offset: 0x002B81C8
		protected virtual void DoNext()
		{
			if (this.next != null)
			{
				Find.WindowStack.Add(this.next);
			}
			if (this.nextAct != null)
			{
				this.nextAct();
			}
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToNextPage");
			this.Close(true);
		}

		// Token: 0x06009628 RID: 38440 RVA: 0x0006432B File Offset: 0x0006252B
		protected virtual void DoBack()
		{
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToPrevPage");
			if (this.prev != null)
			{
				Find.WindowStack.Add(this.prev);
			}
			this.Close(true);
		}

		// Token: 0x06009629 RID: 38441 RVA: 0x002BA028 File Offset: 0x002B8228
		public override void OnCancelKeyPressed()
		{
			if (!this.closeOnCancel)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoBack())
			{
				this.DoBack();
			}
			else
			{
				this.Close(true);
			}
			Event.current.Use();
			base.OnCancelKeyPressed();
		}

		// Token: 0x0600962A RID: 38442 RVA: 0x0006436A File Offset: 0x0006256A
		public override void OnAcceptKeyPressed()
		{
			if (!this.closeOnAccept)
			{
				return;
			}
			if (Find.World != null && Find.WorldRoutePlanner.Active)
			{
				return;
			}
			if (this.CanDoNext())
			{
				this.DoNext();
			}
			Event.current.Use();
		}

		// Token: 0x04005FB2 RID: 24498
		public Page prev;

		// Token: 0x04005FB3 RID: 24499
		public Page next;

		// Token: 0x04005FB4 RID: 24500
		public Action nextAct;

		// Token: 0x04005FB5 RID: 24501
		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		// Token: 0x04005FB6 RID: 24502
		public const float TitleAreaHeight = 45f;

		// Token: 0x04005FB7 RID: 24503
		public const float BottomButHeight = 38f;

		// Token: 0x04005FB8 RID: 24504
		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);
	}
}
