using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200212A RID: 8490
	public class DestroyedSettlement : MapParent
	{
		// Token: 0x0600B45A RID: 46170 RVA: 0x000751E2 File Offset: 0x000733E2
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = true;
				return true;
			}
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x0600B45B RID: 46171 RVA: 0x00075236 File Offset: 0x00073436
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, false);
			}
			yield break;
			yield break;
		}
	}
}
