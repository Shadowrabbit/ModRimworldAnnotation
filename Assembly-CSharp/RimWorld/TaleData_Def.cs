using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001633 RID: 5683
	public class TaleData_Def : TaleData
	{
		// Token: 0x06007B8B RID: 31627 RVA: 0x0025136C File Offset: 0x0024F56C
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpDefName = ((this.def != null) ? this.def.defName : null);
				this.tmpDefType = ((this.def != null) ? this.def.GetType() : null);
			}
			Scribe_Values.Look<string>(ref this.tmpDefName, "defName", null, false);
			Scribe_Values.Look<Type>(ref this.tmpDefType, "defType", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.tmpDefName != null)
			{
				this.def = GenDefDatabase.GetDef(this.tmpDefType, BackCompatibility.BackCompatibleDefName(this.tmpDefType, this.tmpDefName, false, null), true);
			}
		}

		// Token: 0x06007B8C RID: 31628 RVA: 0x000530A0 File Offset: 0x000512A0
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			if (this.def != null)
			{
				yield return new Rule_String(prefix + "_label", this.def.label);
				yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.def.label, false, false));
				yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.def.label, false, false));
			}
			yield break;
		}

		// Token: 0x06007B8D RID: 31629 RVA: 0x000530B7 File Offset: 0x000512B7
		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}

		// Token: 0x040050E5 RID: 20709
		public Def def;

		// Token: 0x040050E6 RID: 20710
		private string tmpDefName;

		// Token: 0x040050E7 RID: 20711
		private Type tmpDefType;
	}
}
