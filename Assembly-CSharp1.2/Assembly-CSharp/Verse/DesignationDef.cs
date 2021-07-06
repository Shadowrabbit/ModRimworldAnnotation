using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200012D RID: 301
	public class DesignationDef : Def
	{
		// Token: 0x0600081F RID: 2079 RVA: 0x0000C7FF File Offset: 0x0000A9FF
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		// Token: 0x040005DE RID: 1502
		[NoTranslate]
		public string texturePath;

		// Token: 0x040005DF RID: 1503
		public TargetType targetType;

		// Token: 0x040005E0 RID: 1504
		public bool removeIfBuildingDespawned;

		// Token: 0x040005E1 RID: 1505
		public bool designateCancelable = true;

		// Token: 0x040005E2 RID: 1506
		[Unsaved(false)]
		public Material iconMat;
	}
}
