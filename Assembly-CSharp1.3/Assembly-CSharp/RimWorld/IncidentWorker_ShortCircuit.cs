using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1A RID: 3098
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x060048BC RID: 18620 RVA: 0x00180C9C File Offset: 0x0017EE9C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).Any<Building>();
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x00180CB4 File Offset: 0x0017EEB4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Building culprit;
			if (!ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).TryRandomElement(out culprit))
			{
				return false;
			}
			ShortCircuitUtility.DoShortCircuit(culprit);
			return true;
		}
	}
}
