using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B8 RID: 5048
	public class Hediff_High : HediffWithComps
	{
		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06006D82 RID: 28034 RVA: 0x0004A70C File Offset: 0x0004890C
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity <= 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent("F0");
			}
		}
	}
}
