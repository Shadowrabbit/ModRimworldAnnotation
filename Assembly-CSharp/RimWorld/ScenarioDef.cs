using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDA RID: 4058
	public class ScenarioDef : Def
	{
		// Token: 0x06005883 RID: 22659 RVA: 0x001D032C File Offset: 0x001CE52C
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

		// Token: 0x06005884 RID: 22660 RVA: 0x0003D84A File Offset: 0x0003BA4A
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

		// Token: 0x04003AB4 RID: 15028
		public Scenario scenario;
	}
}
