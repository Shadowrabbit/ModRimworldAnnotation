using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001024 RID: 4132
	public class TaleData_Def : TaleData
	{
		// Token: 0x06006193 RID: 24979 RVA: 0x002125E4 File Offset: 0x002107E4
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

		// Token: 0x06006194 RID: 24980 RVA: 0x0021268A File Offset: 0x0021088A
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			if (this.def != null)
			{
				yield return new Rule_String(prefix + "_label", this.def.label);
				yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.def.label, false, false));
				yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.def.label, false, false));
			}
			yield break;
		}

		// Token: 0x06006195 RID: 24981 RVA: 0x002126A1 File Offset: 0x002108A1
		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}

		// Token: 0x04003798 RID: 14232
		public Def def;

		// Token: 0x04003799 RID: 14233
		private string tmpDefName;

		// Token: 0x0400379A RID: 14234
		private Type tmpDefType;
	}
}
