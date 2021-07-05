using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE7 RID: 2791
	public class ConceptDef : Def
	{
		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x00160312 File Offset: 0x0015E512
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x00160324 File Offset: 0x0015E524
		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys(null, true);
			}
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x00160333 File Offset: 0x0015E533
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

		// Token: 0x060041B1 RID: 16817 RVA: 0x00160343 File Offset: 0x0015E543
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x0016034C File Offset: 0x0015E54C
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

		// Token: 0x040027E6 RID: 10214
		public float priority = float.MaxValue;

		// Token: 0x040027E7 RID: 10215
		public bool noteTeaches;

		// Token: 0x040027E8 RID: 10216
		public bool needsOpportunity;

		// Token: 0x040027E9 RID: 10217
		public bool opportunityDecays = true;

		// Token: 0x040027EA RID: 10218
		public ProgramState gameMode = ProgramState.Playing;

		// Token: 0x040027EB RID: 10219
		[MustTranslate]
		private string helpText;

		// Token: 0x040027EC RID: 10220
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x040027ED RID: 10221
		private static List<string> tmpParseErrors = new List<string>();
	}
}
