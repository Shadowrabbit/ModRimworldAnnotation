using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013D7 RID: 5079
	public class Lesson_Note : Lesson
	{
		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x06007B85 RID: 31621 RVA: 0x002B8B7D File Offset: 0x002B6D7D
		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x06007B86 RID: 31622 RVA: 0x002B8B8C File Offset: 0x002B6D8C
		public Rect MainRect
		{
			get
			{
				float height = Text.CalcHeight(this.def.HelpTextAdjusted, 432f) + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x06007B87 RID: 31623 RVA: 0x002B8BD0 File Offset: 0x002B6DD0
		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		// Token: 0x06007B88 RID: 31624 RVA: 0x002B8BEB File Offset: 0x002B6DEB
		public Lesson_Note()
		{
		}

		// Token: 0x06007B89 RID: 31625 RVA: 0x002B8C05 File Offset: 0x002B6E05
		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		// Token: 0x06007B8A RID: 31626 RVA: 0x002B8C26 File Offset: 0x002B6E26
		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		// Token: 0x06007B8B RID: 31627 RVA: 0x002B8C38 File Offset: 0x002B6E38
		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		// Token: 0x06007B8C RID: 31628 RVA: 0x002B8C4C File Offset: 0x002B6E4C
		public override void LessonOnGUI()
		{
			Rect mainRect = this.MainRect;
			float alpha = 1f;
			if (this.doFadeIn)
			{
				alpha = Mathf.Clamp01(base.AgeSeconds / 0.4f);
			}
			if (this.Expiring)
			{
				float num = this.expiryTime - Time.timeSinceLevelLoad;
				if (num < 1.1f)
				{
					alpha = num / 1.1f;
				}
			}
			Find.WindowStack.ImmediateWindow(134706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.Expiring)
				{
					this.def.HighlightAllTags();
				}
				if (this.doFadeIn || this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				rect2.width = 432f;
				Widgets.Label(rect2, this.def.HelpTextAdjusted);
				Rect butRect = new Rect(rect.xMax - 32f - 8f, rect.y + 8f, 32f, 32f);
				Texture2D tex;
				if (this.Expiring)
				{
					tex = Widgets.CheckboxOnTex;
				}
				else
				{
					tex = TexButton.CloseXBig;
				}
				if (Widgets.ButtonImage(butRect, tex, new Color(0.95f, 0.95f, 0.95f), new Color(0.8352941f, 0.6666667f, 0.27450982f), true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.CloseButtonClicked();
				}
				if (Time.timeSinceLevelLoad > this.expiryTime)
				{
					this.CloseButtonClicked();
				}
				GUI.color = Color.white;
			}, false, false, alpha, null);
		}

		// Token: 0x06007B8D RID: 31629 RVA: 0x002B8CF4 File Offset: 0x002B6EF4
		private void CloseButtonClicked()
		{
			KnowledgeAmount know = this.def.noteTeaches ? KnowledgeAmount.NoteTaught : KnowledgeAmount.NoteClosed;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		// Token: 0x06007B8E RID: 31630 RVA: 0x002B8D29 File Offset: 0x002B6F29
		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.2f && !this.Expiring)
			{
				this.expiryTime = Time.timeSinceLevelLoad + 2.1f;
			}
		}

		// Token: 0x0400444E RID: 17486
		public ConceptDef def;

		// Token: 0x0400444F RID: 17487
		public bool doFadeIn = true;

		// Token: 0x04004450 RID: 17488
		private float expiryTime = float.MaxValue;

		// Token: 0x04004451 RID: 17489
		private const float RectWidth = 500f;

		// Token: 0x04004452 RID: 17490
		private const float TextWidth = 432f;

		// Token: 0x04004453 RID: 17491
		private const float FadeInDuration = 0.4f;

		// Token: 0x04004454 RID: 17492
		private const float DoneButPad = 8f;

		// Token: 0x04004455 RID: 17493
		private const float DoneButSize = 32f;

		// Token: 0x04004456 RID: 17494
		private const float ExpiryDuration = 2.1f;

		// Token: 0x04004457 RID: 17495
		private const float ExpiryFadeTime = 1.1f;
	}
}
