using System;

namespace Verse
{
	// Token: 0x020000C9 RID: 201
	public struct GenStepWithParams
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x0001E46C File Offset: 0x0001C66C
		public GenStepWithParams(GenStepDef def, GenStepParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x04000429 RID: 1065
		public GenStepDef def;

		// Token: 0x0400042A RID: 1066
		public GenStepParams parms;
	}
}
