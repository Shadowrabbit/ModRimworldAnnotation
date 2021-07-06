using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200089C RID: 2204
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060036A1 RID: 13985 RVA: 0x0002A674 File Offset: 0x00028874
		public SpecialThingFilterWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SpecialThingFilterWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x0002A69A File Offset: 0x0002889A
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "SpecialThingFilterDef " + this.defName + " has no worker class.";
			}
			yield break;
			yield break;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0002A6AA File Offset: 0x000288AA
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x04002626 RID: 9766
		public ThingCategoryDef parentCategory;

		// Token: 0x04002627 RID: 9767
		public string saveKey;

		// Token: 0x04002628 RID: 9768
		public bool allowedByDefault;

		// Token: 0x04002629 RID: 9769
		public bool configurable = true;

		// Token: 0x0400262A RID: 9770
		public Type workerClass;

		// Token: 0x0400262B RID: 9771
		[Unsaved(false)]
		private SpecialThingFilterWorker workerInt;
	}
}
