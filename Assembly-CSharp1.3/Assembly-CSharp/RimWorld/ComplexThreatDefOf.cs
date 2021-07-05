using System;

namespace RimWorld
{
	// Token: 0x0200147A RID: 5242
	[DefOf]
	public static class ComplexThreatDefOf
	{
		// Token: 0x06007D6C RID: 32108 RVA: 0x002C4D48 File Offset: 0x002C2F48
		static ComplexThreatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ComplexThreatDefOf));
		}

		// Token: 0x04004E33 RID: 20019
		[MayRequireIdeology]
		public static ComplexThreatDef SleepingInsects;

		// Token: 0x04004E34 RID: 20020
		[MayRequireIdeology]
		public static ComplexThreatDef Infestation;

		// Token: 0x04004E35 RID: 20021
		[MayRequireIdeology]
		public static ComplexThreatDef SleepingMechanoids;

		// Token: 0x04004E36 RID: 20022
		[MayRequireIdeology]
		public static ComplexThreatDef CryptosleepPods;

		// Token: 0x04004E37 RID: 20023
		[MayRequireIdeology]
		public static ComplexThreatDef MechDrop;

		// Token: 0x04004E38 RID: 20024
		[MayRequireIdeology]
		public static ComplexThreatDef FuelNode;
	}
}
