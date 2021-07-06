using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4D RID: 7245
	[DefOf]
	public static class WorkTypeDefOf
	{
		// Token: 0x06009F50 RID: 40784 RVA: 0x0006A1BD File Offset: 0x000683BD
		static WorkTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkTypeDefOf));
		}

		// Token: 0x04006772 RID: 26482
		public static WorkTypeDef Mining;

		// Token: 0x04006773 RID: 26483
		public static WorkTypeDef Growing;

		// Token: 0x04006774 RID: 26484
		public static WorkTypeDef Construction;

		// Token: 0x04006775 RID: 26485
		public static WorkTypeDef Warden;

		// Token: 0x04006776 RID: 26486
		public static WorkTypeDef Doctor;

		// Token: 0x04006777 RID: 26487
		public static WorkTypeDef Firefighter;

		// Token: 0x04006778 RID: 26488
		public static WorkTypeDef Hunting;

		// Token: 0x04006779 RID: 26489
		public static WorkTypeDef Handling;

		// Token: 0x0400677A RID: 26490
		public static WorkTypeDef Crafting;

		// Token: 0x0400677B RID: 26491
		public static WorkTypeDef Hauling;

		// Token: 0x0400677C RID: 26492
		public static WorkTypeDef Research;
	}
}
