using System;

namespace RimWorld
{
	// Token: 0x02000EF1 RID: 3825
	public class Precept_Animal : Precept_ThingDef
	{
		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06005B06 RID: 23302 RVA: 0x001F7AFC File Offset: 0x001F5CFC
		public override string UIInfoFirstLine
		{
			get
			{
				if (base.ThingDef == null)
				{
					return base.UIInfoFirstLine;
				}
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06005B07 RID: 23303 RVA: 0x001F7B1D File Offset: 0x001F5D1D
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + base.ThingDef.LabelCap;
			}
		}
	}
}
