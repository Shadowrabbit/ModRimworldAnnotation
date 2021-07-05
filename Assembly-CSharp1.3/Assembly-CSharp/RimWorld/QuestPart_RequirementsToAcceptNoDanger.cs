using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9B RID: 2971
	public class QuestPart_RequirementsToAcceptNoDanger : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x0600456D RID: 17773 RVA: 0x001704F3 File Offset: 0x0016E6F3
		public override IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				IAttackTarget attackTarget;
				if (GenHostility.AnyHostileActiveThreatTo(this.mapParent.Map, this.dangerTo, out attackTarget, true, false))
				{
					yield return (Thing)attackTarget;
				}
				yield break;
			}
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x00170504 File Offset: 0x0016E704
		public override AcceptanceReport CanAccept()
		{
			if (this.mapParent != null && this.mapParent.HasMap && GenHostility.AnyHostileActiveThreatTo(this.mapParent.Map, this.dangerTo, true, false))
			{
				return new AcceptanceReport("QuestRequiresNoDangerOnMap".Translate());
			}
			return true;
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x0017055B File Offset: 0x0016E75B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.dangerTo, "dangerTo", false);
		}

		// Token: 0x04002A4C RID: 10828
		public MapParent mapParent;

		// Token: 0x04002A4D RID: 10829
		public Faction dangerTo;
	}
}
