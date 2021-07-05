using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A58 RID: 2648
	public class ComplexDef : Def
	{
		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06003FBC RID: 16316 RVA: 0x00159F34 File Offset: 0x00158134
		public ComplexWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (ComplexWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04002365 RID: 9061
		public List<ComplexRoomDef> roomDefs;

		// Token: 0x04002366 RID: 9062
		public List<ComplexThreat> threats;

		// Token: 0x04002367 RID: 9063
		public ThingSetMakerDef rewardThingSetMakerDef;

		// Token: 0x04002368 RID: 9064
		public Type workerClass = typeof(ComplexWorker);

		// Token: 0x04002369 RID: 9065
		[Unsaved(false)]
		private ComplexWorker workerInt;
	}
}
