using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C7 RID: 6087
	public class DestroyedSettlement : MapParent
	{
		// Token: 0x06008D56 RID: 36182 RVA: 0x0032D9A0 File Offset: 0x0032BBA0
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

		// Token: 0x06008D57 RID: 36183 RVA: 0x0032DA8F File Offset: 0x0032BC8F
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
