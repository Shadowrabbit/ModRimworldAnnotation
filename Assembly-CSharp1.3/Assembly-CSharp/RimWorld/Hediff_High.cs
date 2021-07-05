using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D90 RID: 3472
	public class Hediff_High : HediffWithComps
	{
		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06005093 RID: 20627 RVA: 0x001AF867 File Offset: 0x001ADA67
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
