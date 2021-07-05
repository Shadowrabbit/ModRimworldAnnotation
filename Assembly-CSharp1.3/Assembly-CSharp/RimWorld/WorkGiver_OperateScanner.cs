using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200084F RID: 2127
	public class WorkGiver_OperateScanner : WorkGiver_Scanner
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x0013CAA3 File Offset: 0x0013ACA3
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(this.ScannerDef);
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003844 RID: 14404 RVA: 0x0013CAB0 File Offset: 0x0013ACB0
		public ThingDef ScannerDef
		{
			get
			{
				return this.def.scannerDef;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06003845 RID: 14405 RVA: 0x001398A1 File Offset: 0x00137AA1
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x0013CAC0 File Offset: 0x0013ACC0
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

		// Token: 0x06003848 RID: 14408 RVA: 0x0013CB24 File Offset: 0x0013AD24
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && !building.IsForbidden(pawn) && pawn.CanReserve(building, 1, -1, null, forced) && building.TryGetComp<CompScanner>().CanUseNow && !building.IsBurning();
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x0013CB86 File Offset: 0x0013AD86
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateScanner, t, 1500, true);
		}
	}
}
