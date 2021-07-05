using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000120 RID: 288
	public class WorkTypeDef : Def
	{
		// Token: 0x060007B2 RID: 1970 RVA: 0x00023BD2 File Offset: 0x00021DD2
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.naturalPriority < 0 || this.naturalPriority > 10000)
			{
				yield return "naturalPriority is " + this.naturalPriority + ", but it must be between 0 and 10000";
			}
			yield break;
			yield break;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00023BE4 File Offset: 0x00021DE4
		public override void ResolveReferences()
		{
			foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
			{
				this.workGiversByPriority.Add(item);
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00023C6C File Offset: 0x00021E6C
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		// Token: 0x04000775 RID: 1909
		public WorkTags workTags;

		// Token: 0x04000776 RID: 1910
		[MustTranslate]
		public string labelShort;

		// Token: 0x04000777 RID: 1911
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x04000778 RID: 1912
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x04000779 RID: 1913
		[MustTranslate]
		public string verb;

		// Token: 0x0400077A RID: 1914
		public bool visible = true;

		// Token: 0x0400077B RID: 1915
		public int naturalPriority;

		// Token: 0x0400077C RID: 1916
		public bool alwaysStartActive;

		// Token: 0x0400077D RID: 1917
		public bool requireCapableColonist;

		// Token: 0x0400077E RID: 1918
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x0400077F RID: 1919
		public bool disabledForSlaves;

		// Token: 0x04000780 RID: 1920
		[Unsaved(false)]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();
	}
}
