using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200017C RID: 380
	public class RoomRoleDef : Def
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0000D7F9 File Offset: 0x0000B9F9
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

		// Token: 0x06000997 RID: 2455 RVA: 0x00099B24 File Offset: 0x00097D24
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

		// Token: 0x04000835 RID: 2101
		public Type workerClass;

		// Token: 0x04000836 RID: 2102
		private List<RoomStatDef> relatedStats;

		// Token: 0x04000837 RID: 2103
		[Unsaved(false)]
		private RoomRoleWorker workerInt;
	}
}
