using System;

namespace Verse
{
	// Token: 0x02000136 RID: 310
	public struct GenStepWithParams
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x0000C930 File Offset: 0x0000AB30
		public GenStepWithParams(GenStepDef def, GenStepParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x04000617 RID: 1559
		public GenStepDef def;

		// Token: 0x04000618 RID: 1560
		public GenStepParams parms;
	}
}
