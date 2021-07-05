using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFB RID: 3835
	public abstract class PreceptRequirement : IExposable
	{
		// Token: 0x06005B6E RID: 23406
		public abstract bool Met(List<Precept> precepts);

		// Token: 0x06005B6F RID: 23407
		public abstract Precept MakePrecept(Ideo ideo);

		// Token: 0x06005B70 RID: 23408 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}
	}
}
