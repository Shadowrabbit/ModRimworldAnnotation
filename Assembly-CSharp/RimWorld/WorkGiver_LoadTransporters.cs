using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D87 RID: 3463
	public class WorkGiver_LoadTransporters : WorkGiver_Scanner
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06004EFC RID: 20220 RVA: 0x000379D0 File Offset: 0x00035BD0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Transporter);
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06004EFD RID: 20221 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x001B3934 File Offset: 0x001B1B34
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter);
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x001B3950 File Offset: 0x001B1B50
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
		}
	}
}
