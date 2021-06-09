using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDD RID: 4061
	public class ScenPartDef : Def
	{
		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x0600588F RID: 22671 RVA: 0x0003D8A0 File Offset: 0x0003BAA0
		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed;
			}
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x0003D8AE File Offset: 0x0003BAAE
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

		// Token: 0x04003AC5 RID: 15045
		public ScenPartCategory category;

		// Token: 0x04003AC6 RID: 15046
		public Type scenPartClass;

		// Token: 0x04003AC7 RID: 15047
		public float summaryPriority = -1f;

		// Token: 0x04003AC8 RID: 15048
		public float selectionWeight = 1f;

		// Token: 0x04003AC9 RID: 15049
		public int maxUses = 999999;

		// Token: 0x04003ACA RID: 15050
		public Type pageClass;

		// Token: 0x04003ACB RID: 15051
		public GameConditionDef gameCondition;

		// Token: 0x04003ACC RID: 15052
		public bool gameConditionTargetsWorld;

		// Token: 0x04003ACD RID: 15053
		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		// Token: 0x04003ACE RID: 15054
		public Type designatorType;
	}
}
