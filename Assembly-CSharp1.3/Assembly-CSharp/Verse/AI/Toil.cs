using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005AB RID: 1451
	public sealed class Toil : IJobEndable
	{
		// Token: 0x06002A60 RID: 10848 RVA: 0x000FF094 File Offset: 0x000FD294
		public void Cleanup(int myIndex, JobDriver jobDriver)
		{
			if (this.finishActions != null)
			{
				for (int i = 0; i < this.finishActions.Count; i++)
				{
					try
					{
						this.finishActions[i]();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn ",
							this.actor.ToStringSafe<Pawn>(),
							" threw exception while executing toil's finish action (",
							i,
							"), jobDriver=",
							jobDriver.ToStringSafe<JobDriver>(),
							", job=",
							jobDriver.job.ToStringSafe<Job>(),
							", toilIndex=",
							myIndex,
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000FF174 File Offset: 0x000FD374
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000FF17C File Offset: 0x000FD37C
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.endConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000FF1AD File Offset: 0x000FD3AD
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000FF1BB File Offset: 0x000FD3BB
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000FF1DC File Offset: 0x000FD3DC
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000FF1FD File Offset: 0x000FD3FD
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}

		// Token: 0x04001A41 RID: 6721
		public Pawn actor;

		// Token: 0x04001A42 RID: 6722
		public Action initAction;

		// Token: 0x04001A43 RID: 6723
		public Action tickAction;

		// Token: 0x04001A44 RID: 6724
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x04001A45 RID: 6725
		public List<Action> preInitActions;

		// Token: 0x04001A46 RID: 6726
		public List<Action> preTickActions;

		// Token: 0x04001A47 RID: 6727
		public List<Action> finishActions;

		// Token: 0x04001A48 RID: 6728
		public bool atomicWithPrevious;

		// Token: 0x04001A49 RID: 6729
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x04001A4A RID: 6730
		public Func<SkillDef> activeSkill;

		// Token: 0x04001A4B RID: 6731
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x04001A4C RID: 6732
		public int defaultDuration;

		// Token: 0x04001A4D RID: 6733
		public bool handlingFacing;
	}
}
