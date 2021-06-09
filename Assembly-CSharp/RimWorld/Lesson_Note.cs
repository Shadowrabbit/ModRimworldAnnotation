using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001BE3 RID: 7139
	public class Lesson_Note : Lesson
	{
		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x06009D24 RID: 40228 RVA: 0x00068940 File Offset: 0x00066B40
		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x06009D25 RID: 40229 RVA: 0x002DF780 File Offset: 0x002DD980
		public Rect MainRect
		{
			get
			{
				float height = Text.CalcHeight(this.def.HelpTextAdjusted, 432f) + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x06009D26 RID: 40230 RVA: 0x002DF7C4 File Offset: 0x002DD9C4
		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		// Token: 0x06009D27 RID: 40231 RVA: 0x0006894F File Offset: 0x00066B4F
		public Lesson_Note()
		{
		}

		// Token: 0x06009D28 RID: 40232 RVA: 0x00068969 File Offset: 0x00066B69
		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		// Token: 0x06009D29 RID: 40233 RVA: 0x0006898A File Offset: 0x00066B8A
		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		// Token: 0x06009D2A RID: 40234 RVA: 0x0006899C File Offset: 0x00066B9C
		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		// Token: 0x06009D2B RID: 40235 RVA: 0x002DF7E0 File Offset: 0x002DD9E0
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
			}, false, false, alpha);
		}

		// Token: 0x06009D2C RID: 40236 RVA: 0x002DF884 File Offset: 0x002DDA84
		private void CloseButtonClicked()
		{
			KnowledgeAmount know = this.def.noteTeaches ? KnowledgeAmount.NoteTaught : KnowledgeAmount.NoteClosed;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		// Token: 0x06009D2D RID: 40237 RVA: 0x000689AF File Offset: 0x00066BAF
		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.2f && !this.Expiring)
			{
				this.expiryTime = Time.timeSinceLevelLoad + 2.1f;
			}
		}

		// Token: 0x040063F7 RID: 25591
		public ConceptDef def;

		// Token: 0x040063F8 RID: 25592
		public bool doFadeIn = true;

		// Token: 0x040063F9 RID: 25593
		private float expiryTime = float.MaxValue;

		// Token: 0x040063FA RID: 25594
		private const float RectWidth = 500f;

		// Token: 0x040063FB RID: 25595
		private const float TextWidth = 432f;

		// Token: 0x040063FC RID: 25596
		private const float FadeInDuration = 0.4f;

		// Token: 0x040063FD RID: 25597
		private const float DoneButPad = 8f;

		// Token: 0x040063FE RID: 25598
		private const float DoneButSize = 32f;

		// Token: 0x040063FF RID: 25599
		private const float ExpiryDuration = 2.1f;

		// Token: 0x04006400 RID: 25600
		private const float ExpiryFadeTime = 1.1f;
	}
}
