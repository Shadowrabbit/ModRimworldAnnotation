using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B3 RID: 435
	public class WorkTypeDef : Def
	{
		// Token: 0x06000B08 RID: 2824 RVA: 0x0000E966 File Offset: 0x0000CB66
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

		// Token: 0x06000B09 RID: 2825 RVA: 0x0009FADC File Offset: 0x0009DCDC
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

		// Token: 0x06000B0A RID: 2826 RVA: 0x0000E976 File Offset: 0x0000CB76
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		// Token: 0x040009EE RID: 2542
		public WorkTags workTags;

		// Token: 0x040009EF RID: 2543
		[MustTranslate]
		public string labelShort;

		// Token: 0x040009F0 RID: 2544
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x040009F1 RID: 2545
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x040009F2 RID: 2546
		[MustTranslate]
		public string verb;

		// Token: 0x040009F3 RID: 2547
		public bool visible = true;

		// Token: 0x040009F4 RID: 2548
		public int naturalPriority;

		// Token: 0x040009F5 RID: 2549
		public bool alwaysStartActive;

		// Token: 0x040009F6 RID: 2550
		public bool requireCapableColonist;

		// Token: 0x040009F7 RID: 2551
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x040009F8 RID: 2552
		[Unsaved(false)]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();
	}
}
