using System;
using UnityEngine;

namespace Verse
{
    // Token: 0x0200044E RID: 1102
    public class Dialog_MessageBox : Window
    {
        // Token: 0x1700063E RID: 1598
        // (get) Token: 0x06002171 RID: 8561 RVA: 0x000D0F4B File Offset: 0x000CF14B
        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(640f, 460f);
            }
        }

        // Token: 0x1700063F RID: 1599
        // (get) Token: 0x06002172 RID: 8562 RVA: 0x000D0F5C File Offset: 0x000CF15C
        private float TimeUntilInteractive
        {
            get
            {
                return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
            }
        }

        // Token: 0x17000640 RID: 1600
        // (get) Token: 0x06002173 RID: 8563 RVA: 0x000D0F71 File Offset: 0x000CF171
        private bool InteractionDelayExpired
        {
            get
            {
                return this.TimeUntilInteractive <= 0f;
            }
        }

        // Token: 0x06002174 RID: 8564 RVA: 0x000D0F84 File Offset: 0x000CF184
        public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, bool destructive = false,
            string title = null)
        {
            return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive,
                confirmedAct, delegate()
                {
                });
        }

        // Token: 0x06002175 RID: 8565 RVA: 0x000D0FDC File Offset: 0x000CF1DC
        public Dialog_MessageBox(TaggedString text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null,
            Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null,
            Action cancelAction = null)
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

        // Token: 0x06002176 RID: 8566 RVA: 0x000D10C4 File Offset: 0x000CF2C4
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
            string label = this.InteractionDelayExpired
                ? this.buttonAText
                : (this.buttonAText + "(" + Mathf.Ceil(this.TimeUntilInteractive).ToString("F0") + ")");
            if (Widgets.ButtonText(new Rect(num3 * (float)(num2 - 1) + 10f, inRect.height - 35f, width2, 35f), label, true, true, true) &&
                this.InteractionDelayExpired)
            {
                if (this.buttonAAction != null)
                {
                    this.buttonAAction();
                }
                this.Close(true);
            }
            GUI.color = Color.white;
            if (this.buttonBText != null &&
                Widgets.ButtonText(new Rect(0f, inRect.height - 35f, width2, 35f), this.buttonBText, true, true, true))
            {
                if (this.buttonBAction != null)
                {
                    this.buttonBAction();
                }
                this.Close(true);
            }
            if (this.buttonCText != null &&
                Widgets.ButtonText(new Rect(num3, inRect.height - 35f, width2, 35f), this.buttonCText, true, true, true))
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

        // Token: 0x06002177 RID: 8567 RVA: 0x000D1342 File Offset: 0x000CF542
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

        // Token: 0x06002178 RID: 8568 RVA: 0x000D1365 File Offset: 0x000CF565
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

        // Token: 0x040014BF RID: 5311
        public TaggedString text;

        // Token: 0x040014C0 RID: 5312
        public string title;

        // Token: 0x040014C1 RID: 5313
        public string buttonAText;

        // Token: 0x040014C2 RID: 5314
        public Action buttonAAction;

        // Token: 0x040014C3 RID: 5315
        public bool buttonADestructive;

        // Token: 0x040014C4 RID: 5316
        public string buttonBText;

        // Token: 0x040014C5 RID: 5317
        public Action buttonBAction;

        // Token: 0x040014C6 RID: 5318
        public string buttonCText;

        // Token: 0x040014C7 RID: 5319
        public Action buttonCAction;

        // Token: 0x040014C8 RID: 5320
        public bool buttonCClose = true;

        // Token: 0x040014C9 RID: 5321
        public float interactionDelay;

        // Token: 0x040014CA RID: 5322
        public Action acceptAction;

        // Token: 0x040014CB RID: 5323
        public Action cancelAction;

        // Token: 0x040014CC RID: 5324
        private Vector2 scrollPosition = Vector2.zero;

        // Token: 0x040014CD RID: 5325
        private float creationRealTime = -1f;

        // Token: 0x040014CE RID: 5326
        private const float TitleHeight = 42f;

        // Token: 0x040014CF RID: 5327
        protected const float ButtonHeight = 35f;
    }
}
