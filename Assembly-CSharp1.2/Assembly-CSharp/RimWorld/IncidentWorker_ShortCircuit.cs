using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E0 RID: 4576
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x06006442 RID: 25666 RVA: 0x00044C36 File Offset: 0x00042E36
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).Any<Building>();
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x001F2304 File Offset: 0x001F0504
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
