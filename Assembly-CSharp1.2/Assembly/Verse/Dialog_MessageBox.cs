using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200079E RID: 1950
	public class Dialog_MessageBox : Window
	{
		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06003126 RID: 12582 RVA: 0x00026C74 File Offset: 0x00024E74
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 460f);
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06003127 RID: 12583 RVA: 0x00026C85 File Offset: 0x00024E85
		private float TimeUntilInteractive
		{
			get
			{
				return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06003128 RID: 12584 RVA: 0x00026C9A File Offset: 0x00024E9A
		private bool InteractionDelayExpired
		{
			get
			{
				return this.TimeUntilInteractive <= 0f;
			}
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x001445A0 File Offset: 0x001427A0
		public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, bool destructive = false, string title = null)
		{
			return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive, confirmedAct, delegate()
			{
			});
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x001445F8 File Offset: 0x001427F8
		public Dialog_MessageBox(TaggedString text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null, Action cancelAction = null)
		{
			this.text = text;
			this.buttonAText = buttonAText;
			this.buttonAAction = buttonAAction;
			this.buttonADestructive = buttonADestructive;
			this.buttonBText = buttonBText;
			this.buttonBAction = buttonBAction;
			this.title = title;
			this.acceptAction = acceptAction;
			this.cancelAction = cancelAction;
			if (buttonAText.NullOrEmpty())
			{
				this.buttonAText = "OK".Translate();
			}
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.creationRealTime = RealTime.LastRealTime;
			this.onlyOneOfTypeAllowed = false;
			bool flag = buttonAAction == null && buttonBAction == null && this.buttonCAction == null;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = (acceptAction != null || cancelAction != null || flag);
			this.closeOnAccept = flag;
			this.closeOnCancel = flag;
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x001446E0 File Offset: 0x001428E0
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, num, inRect.width, 42f), this.title);
				num += 42f;
			}
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect.x, num, inRect.width, inRect.height - 35f - 5f - num);
			float width = outRect.width - 16f;
			Rect viewRect = new Rect(0f, 0f, width, Text.CalcHeight(this.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			Widgets.Label(new Rect(0f, 0f, viewRect.width, viewRect.height), this.text);
			Widgets.EndScrollView();
			int num2 = this.buttonCText.NullOrEmpty() ? 2 : 3;
			float num3 = inRect.width / (float)num2;
			float width2 = num3 - 10f;
			if (this.buttonADestructive)
			{
				GUI.color = new Color(1f, 0.3f, 0.35f);
			}
			string label = this.InteractionDelayExpired ? this.buttonAText : (this.buttonAText + "(" + Mathf.Ceil(this.TimeUntilInteractive).ToString("F0") + ")");
			if (Widgets.ButtonText(new Rect(num3 * (float)(num2 - 1) + 10f, inRect.height - 35f, width2, 35f), label, true, true, true) && this.InteractionDelayExpired)
			{
				if (this.buttonAAction != null)
				{
					this.buttonAAction();
				}
				this.Close(true);
			}
			GUI.color = Color.white;
			if (this.buttonBText != null && Widgets.ButtonText(new Rect(0f, inRect.height - 35f, width2, 35f), this.buttonBText, true, true, true))
			{
				if (this.buttonBAction != null)
				{
					this.buttonBAction();
				}
				this.Close(true);
			}
			if (this.buttonCText != null && Widgets.ButtonText(new Rect(num3, inRect.height - 35f, width2, 35f), this.buttonCText, true, true, true))
			{
				if (this.buttonCAction != null)
				{
					this.buttonCAction();
				}
				if (this.buttonCClose)
				{
					this.Close(true);
				}
			}
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x00026CAC File Offset: 0x00024EAC
		public override void OnCancelKeyPressed()
		{
			if (this.cancelAction != null)
			{
				this.cancelAction();
				this.Close(true);
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x00026CCF File Offset: 0x00024ECF
		public override void OnAcceptKeyPressed()
		{
			if (this.acceptAction != null)
			{
				this.acceptAction();
				this.Close(true);
				return;
			}
			base.OnAcceptKeyPressed();
		}

		// Token: 0x040021D6 RID: 8662
		public TaggedString text;

		// Token: 0x040021D7 RID: 8663
		public string title;

		// Token: 0x040021D8 RID: 8664
		public string buttonAText;

		// Token: 0x040021D9 RID: 8665
		public Action buttonAAction;

		// Token: 0x040021DA RID: 8666
		public bool buttonADestructive;

		// Token: 0x040021DB RID: 8667
		public string buttonBText;

		// Token: 0x040021DC RID: 8668
		public Action buttonBAction;

		// Token: 0x040021DD RID: 8669
		public string buttonCText;

		// Token: 0x040021DE RID: 8670
		public Action buttonCAction;

		// Token: 0x040021DF RID: 8671
		public bool buttonCClose = true;

		// Token: 0x040021E0 RID: 8672
		public float interactionDelay;

		// Token: 0x040021E1 RID: 8673
		public Action acceptAction;

		// Token: 0x040021E2 RID: 8674
		public Action cancelAction;

		// Token: 0x040021E3 RID: 8675
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x040021E4 RID: 8676
		private float creationRealTime = -1f;

		// Token: 0x040021E5 RID: 8677
		private const float TitleHeight = 42f;

		// Token: 0x040021E6 RID: 8678
		protected const float ButtonHeight = 35f;
	}
}
