using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020009B0 RID: 2480
	public sealed class Toil : IJobEndable
	{
		// Token: 0x06003C70 RID: 15472 RVA: 0x00172544 File Offset: 0x00170744
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
						}), false);
					}
				}
			}
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x0002E006 File Offset: 0x0002C206
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x00172624 File Offset: 0x00170824
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

		// Token: 0x06003C73 RID: 15475 RVA: 0x0002E00E File Offset: 0x0002C20E
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x0002E01C File Offset: 0x0002C21C
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x0002E03D File Offset: 0x0002C23D
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x0002E05E File Offset: 0x0002C25E
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}

		// Token: 0x040029D9 RID: 10713
		public Pawn actor;

		// Token: 0x040029DA RID: 10714
		public Action initAction;

		// Token: 0x040029DB RID: 10715
		public Action tickAction;

		// Token: 0x040029DC RID: 10716
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x040029DD RID: 10717
		public List<Action> preInitActions;

		// Token: 0x040029DE RID: 10718
		public List<Action> preTickActions;

		// Token: 0x040029DF RID: 10719
		public List<Action> finishActions;

		// Token: 0x040029E0 RID: 10720
		public bool atomicWithPrevious;

		// Token: 0x040029E1 RID: 10721
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x040029E2 RID: 10722
		public Func<SkillDef> activeSkill;

		// Token: 0x040029E3 RID: 10723
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x040029E4 RID: 10724
		public int defaultDuration;

		// Token: 0x040029E5 RID: 10725
		public bool handlingFacing;
	}
}
