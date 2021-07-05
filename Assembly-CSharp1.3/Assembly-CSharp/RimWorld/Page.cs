using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200131D RID: 4893
	public abstract class Page : Window
	{
		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x0600761A RID: 30234 RVA: 0x0028D245 File Offset: 0x0028B445
		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x0600761B RID: 30235 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600761C RID: 30236 RVA: 0x0028D24C File Offset: 0x0028B44C
		public Page()
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
		}

		// Token: 0x0600761D RID: 30237 RVA: 0x0028D277 File Offset: 0x0028B477
		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600761E RID: 30238 RVA: 0x0028D2AC File Offset: 0x0028B4AC
		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = 45f + extraTopSpace;
			}
			return new Rect(0f, num, rect.width, rect.height - 38f - num - 17f);
		}

		// Token: 0x0600761F RID: 30239 RVA: 0x0028D2F4 File Offset: 0x0028B4F4
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

		// Token: 0x06007620 RID: 30240 RVA: 0x0028D451 File Offset: 0x0028B651
		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		// Token: 0x06007621 RID: 30241 RVA: 0x0028D46B File Offset: 0x0028B66B
		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

		// Token: 0x06007622 RID: 30242 RVA: 0x0028D488 File Offset: 0x0028B688
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

		// Token: 0x06007623 RID: 30243 RVA: 0x0028D4E5 File Offset: 0x0028B6E5
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

		// Token: 0x06007624 RID: 30244 RVA: 0x0028D524 File Offset: 0x0028B724
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

		// Token: 0x06007625 RID: 30245 RVA: 0x0028D575 File Offset: 0x0028B775
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

		// Token: 0x04004181 RID: 16769
		public Page prev;

		// Token: 0x04004182 RID: 16770
		public Page next;

		// Token: 0x04004183 RID: 16771
		public Action nextAct;

		// Token: 0x04004184 RID: 16772
		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		// Token: 0x04004185 RID: 16773
		public const float TitleAreaHeight = 45f;

		// Token: 0x04004186 RID: 16774
		public const float BottomButHeight = 38f;

		// Token: 0x04004187 RID: 16775
		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);
	}
}
