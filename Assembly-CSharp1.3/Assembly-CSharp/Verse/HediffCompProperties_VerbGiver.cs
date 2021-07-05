using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020002BB RID: 699
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x0006C2D9 File Offset: 0x0006A4D9
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0006C2F4 File Offset: 0x0006A4F4
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

		// Token: 0x060012EA RID: 4842 RVA: 0x0006C33D File Offset: 0x0006A53D
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

		// Token: 0x04000E3F RID: 3647
		public List<VerbProperties> verbs;

		// Token: 0x04000E40 RID: 3648
		public List<Tool> tools;
	}
}
