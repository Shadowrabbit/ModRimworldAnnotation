using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000FC RID: 252
	public class RoomRoleDef : Def
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0002105F File Offset: 0x0001F25F
		public RoomRoleWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00021085 File Offset: 0x0001F285
		public string PostProcessedLabel
		{
			get
			{
				return this.Worker.PostProcessedLabel(this.label);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00021098 File Offset: 0x0001F298
		public string PostProcessedLabelCap
		{
			get
			{
				return this.PostProcessedLabel.CapitalizeFirst();
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000210A8 File Offset: 0x0001F2A8
		public bool IsStatRelated(RoomStatDef def)
		{
			if (this.relatedStats == null)
			{
				return false;
			}
			for (int i = 0; i < this.relatedStats.Count; i++)
			{
				if (this.relatedStats[i] == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400060E RID: 1550
		public Type workerClass;

		// Token: 0x0400060F RID: 1551
		private List<RoomStatDef> relatedStats;

		// Token: 0x04000610 RID: 1552
		[Unsaved(false)]
		private RoomRoleWorker workerInt;
	}
}
