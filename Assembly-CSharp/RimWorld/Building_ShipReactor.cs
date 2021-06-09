using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001688 RID: 5768
	public class Building_ShipReactor : Building
	{
		// Token: 0x06007E0D RID: 32269 RVA: 0x00054C09 File Offset: 0x00052E09
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.charlonsReactor)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "ReactorDestroyed");
			}
			base.Destroy(mode);
		}

		// Token: 0x06007E0E RID: 32270 RVA: 0x00054C34 File Offset: 0x00052E34
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

		// Token: 0x06007E0F RID: 32271 RVA: 0x00054C44 File Offset: 0x00052E44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.charlonsReactor, "charlonsReactor", false, false);
		}

		// Token: 0x0400521F RID: 21023
		public bool charlonsReactor;
	}
}
