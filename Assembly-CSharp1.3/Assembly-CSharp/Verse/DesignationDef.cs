using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C1 RID: 193
	public class DesignationDef : Def
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x0001E19F File Offset: 0x0001C39F
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		// Token: 0x040003F0 RID: 1008
		[NoTranslate]
		public string texturePath;

		// Token: 0x040003F1 RID: 1009
		public TargetType targetType;

		// Token: 0x040003F2 RID: 1010
		public bool removeIfBuildingDespawned;

		// Token: 0x040003F3 RID: 1011
		public bool designateCancelable = true;

		// Token: 0x040003F4 RID: 1012
		[Unsaved(false)]
		public Material iconMat;
	}
}
