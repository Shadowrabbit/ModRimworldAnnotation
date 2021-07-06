using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CB9 RID: 3257
	public class JobGiver_EnterTransporter : ThinkNode_JobGiver
	{
		// Token: 0x06004B85 RID: 19333 RVA: 0x001A5D40 File Offset: 0x001A3F40
		protected override Job TryGiveJob(Pawn pawn)
		{
			int transportersGroup = pawn.mindState.duty.transportersGroup;
			if (transportersGroup != -1)
			{
				List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i] != pawn && allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						CompTransporter transporter = ((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter != null && transporter.groupID == transportersGroup)
						{
							return null;
						}
					}
				}
				TransporterUtility.GetTransportersInGroup(transportersGroup, pawn.Map, JobGiver_EnterTransporter.tmpTransporters);
				CompTransporter compTransporter = JobGiver_EnterTransporter.FindMyTransporter(JobGiver_EnterTransporter.tmpTransporters, pawn);
				JobGiver_EnterTransporter.tmpTransporters.Clear();
				if (compTransporter == null || !pawn.CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return null;
				}
				return JobMaker.MakeJob(JobDefOf.EnterTransporter, compTransporter.parent);
			}
			else
			{
				Thing thing = pawn.mindState.duty.focus.Thing;
				if (thing == null || !pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return null;
				}
				Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, pawn.mindState.duty.focus.Thing);
				job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, LocomotionUrgency.Walk);
				return job;
			}
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x001A5E8C File Offset: 0x001A408C
		public static CompTransporter FindMyTransporter(List<CompTransporter> transporters, Pawn me)
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				List<TransferableOneWay> leftToLoad = transporters[i].leftToLoad;
				if (leftToLoad != null)
				{
					for (int j = 0; j < leftToLoad.Count; j++)
					{
						if (leftToLoad[j].AnyThing is Pawn)
						{
							List<Thing> things = leftToLoad[j].things;
							for (int k = 0; k < things.Count; k++)
							{
								if (things[k] == me)
								{
									return transporters[i];
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x040031DA RID: 12762
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}
