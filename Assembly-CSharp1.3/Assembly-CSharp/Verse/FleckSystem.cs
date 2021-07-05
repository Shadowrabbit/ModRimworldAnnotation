using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000199 RID: 409
	public abstract class FleckSystem : IExposable, ILoadReferenceable
	{
		// Token: 0x06000B86 RID: 2950
		public abstract void Update();

		// Token: 0x06000B87 RID: 2951
		public abstract void Tick();

		// Token: 0x06000B88 RID: 2952
		public abstract void Draw(DrawBatch drawBatch);

		// Token: 0x06000B89 RID: 2953 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void OnGUI()
		{
		}

		// Token: 0x06000B8A RID: 2954
		public abstract void CreateFleck(FleckCreationData fleckData);

		// Token: 0x06000B8B RID: 2955
		public abstract void ExposeData();

		// Token: 0x06000B8C RID: 2956 RVA: 0x0003E627 File Offset: 0x0003C827
		public string GetUniqueLoadID()
		{
			return this.parent.parent.GetUniqueLoadID() + "_FleckSystem_" + base.GetType().FullName;
		}

		// Token: 0x04000995 RID: 2453
		public List<FleckDef> handledDefs = new List<FleckDef>();

		// Token: 0x04000996 RID: 2454
		public FleckManager parent;
	}
}
