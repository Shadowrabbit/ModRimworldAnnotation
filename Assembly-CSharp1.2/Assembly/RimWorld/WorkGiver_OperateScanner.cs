using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D8F RID: 3471
	public class WorkGiver_OperateScanner : WorkGiver_Scanner
	{
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06004F2C RID: 20268 RVA: 0x00037B9D File Offset: 0x00035D9D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(this.ScannerDef);
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06004F2D RID: 20269 RVA: 0x00037BAA File Offset: 0x00035DAA
		public ThingDef ScannerDef
		{
			get
			{
				return this.def.scannerDef;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06004F2E RID: 20270 RVA: 0x00037420 File Offset: 0x00035620
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x001B434C File Offset: 0x001B254C
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(this.ScannerDef);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == pawn.Faction)
				{
					CompScanner compScanner = list[i].TryGetComp<CompScanner>();
					if (compScanner != null && compScanner.CanUseNow)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x001B43B0 File Offset: 0x001B25B0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && !building.IsForbidden(pawn) && pawn.CanReserve(building, 1, -1, null, forced) && building.TryGetComp<CompScanner>().CanUseNow && !building.IsBurning();
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x00037BB7 File Offset: 0x00035DB7
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateScanner, t, 1500, true);
		}
	}
}
