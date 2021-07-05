using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABA RID: 2746
	public class ScenPartDef : Def
	{
		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x0015E93B File Offset: 0x0015CB3B
		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed;
			}
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x0015E949 File Offset: 0x0015CB49
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.scenPartClass == null)
			{
				yield return "scenPartClass is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002677 RID: 9847
		public ScenPartCategory category;

		// Token: 0x04002678 RID: 9848
		public Type scenPartClass;

		// Token: 0x04002679 RID: 9849
		public float summaryPriority = -1f;

		// Token: 0x0400267A RID: 9850
		public float selectionWeight = 1f;

		// Token: 0x0400267B RID: 9851
		public int maxUses = 999999;

		// Token: 0x0400267C RID: 9852
		public Type pageClass;

		// Token: 0x0400267D RID: 9853
		public GameConditionDef gameCondition;

		// Token: 0x0400267E RID: 9854
		public bool gameConditionTargetsWorld;

		// Token: 0x0400267F RID: 9855
		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		// Token: 0x04002680 RID: 9856
		public Type designatorType;
	}
}
