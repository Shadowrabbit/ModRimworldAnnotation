using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001007 RID: 4103
	public class TransferableSorterDef : Def
	{
		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06005987 RID: 22919 RVA: 0x0003E2E8 File Offset: 0x0003C4E8
		public TransferableComparer Comparer
		{
			get
			{
				if (this.comparerInt == null)
				{
					this.comparerInt = (TransferableComparer)Activator.CreateInstance(this.comparerClass);
				}
				return this.comparerInt;
			}
		}

		// Token: 0x04003C2A RID: 15402
		public Type comparerClass;

		// Token: 0x04003C2B RID: 15403
		[Unsaved(false)]
		private TransferableComparer comparerInt;
	}
}
