using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020003F6 RID: 1014
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x060018AD RID: 6317 RVA: 0x00017784 File Offset: 0x00015984
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x000E0008 File Offset: 0x000DE208
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0001779C File Offset: 0x0001599C
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.tools != null)
			{
				Tool tool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (tool != null)
				{
					yield return string.Format("duplicate hediff tool id {0}", tool.id);
				}
				foreach (Tool tool2 in this.tools)
				{
					foreach (string text2 in tool2.ConfigErrors())
					{
						yield return text2;
					}
					enumerator = null;
				}
				List<Tool>.Enumerator enumerator2 = default(List<Tool>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x040012A6 RID: 4774
		public List<VerbProperties> verbs;

		// Token: 0x040012A7 RID: 4775
		public List<Tool> tools;
	}
}
