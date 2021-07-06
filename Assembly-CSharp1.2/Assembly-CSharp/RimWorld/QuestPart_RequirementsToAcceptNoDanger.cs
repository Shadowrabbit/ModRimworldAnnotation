using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001101 RID: 4353
	public class QuestPart_RequirementsToAcceptNoDanger : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005F20 RID: 24352 RVA: 0x00041CFB File Offset: 0x0003FEFB
		public override IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				IAttackTarget attackTarget;
				if (GenHostility.AnyHostileActiveThreatTo(this.map, this.dangerTo, out attackTarget, true))
				{
					yield return (Thing)attackTarget;
				}
				yield break;
			}
		}

		// Token: 0x06005F21 RID: 24353 RVA: 0x00041D0B File Offset: 0x0003FF0B
		public override AcceptanceReport CanAccept()
		{
			if (this.map != null && GenHostility.AnyHostileActiveThreatTo(this.map, this.dangerTo, true))
			{
				return new AcceptanceReport("QuestRequiresNoDangerOnMap".Translate());
			}
			return true;
		}

		// Token: 0x06005F22 RID: 24354 RVA: 0x00041D44 File Offset: 0x0003FF44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.dangerTo, "dangerTo", false);
		}

		// Token: 0x04003FA9 RID: 16297
		public Map map;

		// Token: 0x04003FAA RID: 16298
		public Faction dangerTo;
	}
}
