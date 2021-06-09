using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110E RID: 4366
	public static class SendShuttleAwayQuestPartUtility
	{
		// Token: 0x06005F62 RID: 24418 RVA: 0x001E21AC File Offset: 0x001E03AC
		public static void SendAway(Thing shuttle, bool dropEverything)
		{
			CompShuttle compShuttle = shuttle.TryGetComp<CompShuttle>();
			CompTransporter compTransporter = shuttle.TryGetComp<CompTransporter>();
			if (shuttle.Spawned)
			{
				if (dropEverything && compTransporter.LoadingInProgressOrReadyToLaunch)
				{
					compTransporter.CancelLoad();
				}
				if (!compTransporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(compTransporter));
				}
				compShuttle.Send();
				return;
			}
			if (shuttle.ParentHolder is Thing && ((Thing)shuttle.ParentHolder).def == ThingDefOf.ShuttleIncoming)
			{
				compShuttle.leaveASAP = true;
			}
		}
	}
}
