using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ADA RID: 2778
	public class ThoughtStage
	{
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600416E RID: 16750 RVA: 0x0015F7A0 File Offset: 0x0015D9A0
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

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600416F RID: 16751 RVA: 0x0015F7C1 File Offset: 0x0015D9C1
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

		// Token: 0x06004170 RID: 16752 RVA: 0x0015F7E2 File Offset: 0x0015D9E2
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x0015F7FC File Offset: 0x0015D9FC
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

		// Token: 0x0400275B RID: 10075
		[MustTranslate]
		public string label;

		// Token: 0x0400275C RID: 10076
		[MustTranslate]
		public string labelSocial;

		// Token: 0x0400275D RID: 10077
		[MustTranslate]
		public string description;

		// Token: 0x0400275E RID: 10078
		public float baseMoodEffect;

		// Token: 0x0400275F RID: 10079
		public float baseOpinionOffset;

		// Token: 0x04002760 RID: 10080
		public bool visible = true;

		// Token: 0x04002761 RID: 10081
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x04002762 RID: 10082
		[Unsaved(false)]
		private string cachedLabelSocialCap;

		// Token: 0x04002763 RID: 10083
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x04002764 RID: 10084
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelSocial;
	}
}
