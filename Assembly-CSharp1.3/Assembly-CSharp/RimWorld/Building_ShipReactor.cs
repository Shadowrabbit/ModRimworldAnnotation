using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001058 RID: 4184
	public class Building_ShipReactor : Building
	{
		// Token: 0x06006325 RID: 25381 RVA: 0x00219129 File Offset: 0x00217329
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.charlonsReactor)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "ReactorDestroyed");
			}
			base.Destroy(mode);
		}

		// Token: 0x06006326 RID: 25382 RVA: 0x00219154 File Offset: 0x00217354
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x00219164 File Offset: 0x00217364
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.charlonsReactor, "charlonsReactor", false, false);
		}

		// Token: 0x04003840 RID: 14400
		public bool charlonsReactor;
	}
}
