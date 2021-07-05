using System;

namespace Verse
{
	// Token: 0x0200032B RID: 811
	public enum LookMode : byte
	{
		// Token: 0x04001004 RID: 4100
		Undefined,
		// Token: 0x04001005 RID: 4101
		Value,
		// Token: 0x04001006 RID: 4102
		Deep,
		// Token: 0x04001007 RID: 4103
		Reference,
		// Token: 0x04001008 RID: 4104
		Def,
		// Token: 0x04001009 RID: 4105
		LocalTargetInfo,
		// Token: 0x0400100A RID: 4106
		TargetInfo,
		// Token: 0x0400100B RID: 4107
		GlobalTargetInfo,
		// Token: 0x0400100C RID: 4108
		BodyPart
	}
}
