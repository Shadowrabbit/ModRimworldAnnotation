using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF9 RID: 4089
	public class ThoughtStage
	{
		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x0003DDCE File Offset: 0x0003BFCE
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06005920 RID: 22816 RVA: 0x0003DDEF File Offset: 0x0003BFEF
		public string LabelSocialCap
		{
			get
			{
				if (this.cachedLabelSocialCap == null)
				{
					this.cachedLabelSocialCap = this.labelSocial.CapitalizeFirst();
				}
				return this.cachedLabelSocialCap;
			}
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x0003DE10 File Offset: 0x0003C010
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x0003DE2A File Offset: 0x0003C02A
		public IEnumerable<string> ConfigErrors()
		{
			if (!this.labelSocial.NullOrEmpty() && this.labelSocial == this.label)
			{
				yield return "labelSocial is the same as label. labelSocial is unnecessary in this case";
			}
			if (this.baseMoodEffect != 0f && this.description.NullOrEmpty())
			{
				yield return "affects mood but doesn't have a description";
			}
			yield break;
		}

		// Token: 0x04003BA4 RID: 15268
		[MustTranslate]
		public string label;

		// Token: 0x04003BA5 RID: 15269
		[MustTranslate]
		public string labelSocial;

		// Token: 0x04003BA6 RID: 15270
		[MustTranslate]
		public string description;

		// Token: 0x04003BA7 RID: 15271
		public float baseMoodEffect;

		// Token: 0x04003BA8 RID: 15272
		public float baseOpinionOffset;

		// Token: 0x04003BA9 RID: 15273
		public bool visible = true;

		// Token: 0x04003BAA RID: 15274
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x04003BAB RID: 15275
		[Unsaved(false)]
		private string cachedLabelSocialCap;

		// Token: 0x04003BAC RID: 15276
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x04003BAD RID: 15277
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelSocial;
	}
}
