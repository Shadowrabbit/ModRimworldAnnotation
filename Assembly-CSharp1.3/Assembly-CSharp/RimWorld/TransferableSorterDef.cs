using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE4 RID: 2788
	public class TransferableSorterDef : Def
	{
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060041A9 RID: 16809 RVA: 0x001602EC File Offset: 0x0015E4EC
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

		// Token: 0x040027DF RID: 10207
		public Type comparerClass;

		// Token: 0x040027E0 RID: 10208
		[Unsaved(false)]
		private TransferableComparer comparerInt;
	}
}
