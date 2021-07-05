using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004ED RID: 1261
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002608 RID: 9736 RVA: 0x000EBF70 File Offset: 0x000EA170
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

		// Token: 0x06002609 RID: 9737 RVA: 0x000EBF96 File Offset: 0x000EA196
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

		// Token: 0x0600260A RID: 9738 RVA: 0x000EBFA6 File Offset: 0x000EA1A6
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x040017E5 RID: 6117
		public ThingCategoryDef parentCategory;

		// Token: 0x040017E6 RID: 6118
		public string saveKey;

		// Token: 0x040017E7 RID: 6119
		public bool allowedByDefault;

		// Token: 0x040017E8 RID: 6120
		public bool configurable = true;

		// Token: 0x040017E9 RID: 6121
		public Type workerClass;

		// Token: 0x040017EA RID: 6122
		[Unsaved(false)]
		private SpecialThingFilterWorker workerInt;
	}
}
