using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200084A RID: 2122
	public class WorkGiver_LoadTransporters : WorkGiver_Scanner
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06003827 RID: 14375 RVA: 0x0013C24F File Offset: 0x0013A44F
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Transporter);
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06003828 RID: 14376 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x0013C258 File Offset: 0x0013A458
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter);
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x0013C274 File Offset: 0x0013A474
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
		}
	}
}
