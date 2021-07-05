using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABC RID: 2748
	public class ScenarioDef : Def
	{
		// Token: 0x06004109 RID: 16649 RVA: 0x0015E998 File Offset: 0x0015CB98
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = this.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = this.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x0015E9FD File Offset: 0x0015CBFD
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.scenario == null)
			{
				yield return "null scenario";
			}
			foreach (string text in this.scenario.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04002686 RID: 9862
		public Scenario scenario;
	}
}
