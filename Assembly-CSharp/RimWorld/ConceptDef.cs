using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001009 RID: 4105
	public class ConceptDef : Def
	{
		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x0600598B RID: 22923 RVA: 0x0003E30E File Offset: 0x0003C50E
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x0600598C RID: 22924 RVA: 0x0003E320 File Offset: 0x0003C520
		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys(null, true);
			}
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x0003E32F File Offset: 0x0003C52F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.priority > 9999999f)
			{
				yield return "priority isn't set";
			}
			if (this.helpText.NullOrEmpty())
			{
				yield return "no help text";
			}
			if (this.TriggeredDirect && this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			ConceptDef.tmpParseErrors.Clear();
			this.helpText.AdjustedForKeys(ConceptDef.tmpParseErrors, false);
			int num;
			for (int i = 0; i < ConceptDef.tmpParseErrors.Count; i = num + 1)
			{
				yield return "helpText error: " + ConceptDef.tmpParseErrors[i];
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600598E RID: 22926 RVA: 0x0003E33F File Offset: 0x0003C53F
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x001D29AC File Offset: 0x001D0BAC
		public void HighlightAllTags()
		{
			if (this.highlightTags != null)
			{
				for (int i = 0; i < this.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.highlightTags[i]);
				}
			}
		}

		// Token: 0x04003C2C RID: 15404
		public float priority = float.MaxValue;

		// Token: 0x04003C2D RID: 15405
		public bool noteTeaches;

		// Token: 0x04003C2E RID: 15406
		public bool needsOpportunity;

		// Token: 0x04003C2F RID: 15407
		public bool opportunityDecays = true;

		// Token: 0x04003C30 RID: 15408
		public ProgramState gameMode = ProgramState.Playing;

		// Token: 0x04003C31 RID: 15409
		[MustTranslate]
		private string helpText;

		// Token: 0x04003C32 RID: 15410
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x04003C33 RID: 15411
		private static List<string> tmpParseErrors = new List<string>();
	}
}
