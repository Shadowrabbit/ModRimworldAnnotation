using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B12 RID: 2834
	public class QuestPart_Delay : QuestPartActivable
	{
		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x060042A3 RID: 17059 RVA: 0x00164E22 File Offset: 0x00163022
		public int TicksLeft
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return 0;
				}
				return this.enableTick + this.delayTicks - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x060042A4 RID: 17060 RVA: 0x00164E47 File Offset: 0x00163047
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.quest.Historical)
				{
					return null;
				}
				return this.expiryInfoPart.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x060042A5 RID: 17061 RVA: 0x00164E7C File Offset: 0x0016307C
		public override string ExpiryInfoPartTip
		{
			get
			{
				return this.expiryInfoPartTip.Formatted(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.enableTick + this.delayTicks), QuestUtility.GetLocForDates()));
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060042A6 RID: 17062 RVA: 0x00164EB0 File Offset: 0x001630B0
		public override string AlertLabel
		{
			get
			{
				string text = this.alertLabel;
				TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true))) : null;
				if (taggedString == null)
				{
					return null;
				}
				return taggedString.GetValueOrDefault();
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060042A7 RID: 17063 RVA: 0x00164F08 File Offset: 0x00163108
		public override string AlertExplanation
		{
			get
			{
				string text = this.alertExplanation;
				TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true))) : null;
				if (taggedString == null)
				{
					return null;
				}
				return taggedString.GetValueOrDefault();
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x060042A8 RID: 17064 RVA: 0x00164F60 File Offset: 0x00163160
		public override AlertReport AlertReport
		{
			get
			{
				if (this.alertCulprits.Count <= 0)
				{
					return AlertReport.Inactive;
				}
				return AlertReport.CulpritsAre(this.alertCulprits);
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x060042A9 RID: 17065 RVA: 0x00164F81 File Offset: 0x00163181
		public override bool AlertCritical
		{
			get
			{
				return this.TicksLeft < this.ticksLeftAlertCritical;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x060042AA RID: 17066 RVA: 0x00164F91 File Offset: 0x00163191
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.inspectStringTargets != null)
				{
					int num;
					for (int i = 0; i < this.inspectStringTargets.Count; i = num + 1)
					{
						ISelectable selectable = this.inspectStringTargets[i];
						if (selectable is Thing)
						{
							yield return (Thing)selectable;
						}
						else if (selectable is WorldObject)
						{
							yield return (WorldObject)selectable;
						}
						num = i;
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00164FA1 File Offset: 0x001631A1
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (Find.TickManager.TicksGame >= this.enableTick + this.delayTicks)
			{
				this.DelayFinished();
			}
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00164FC8 File Offset: 0x001631C8
		protected virtual void DelayFinished()
		{
			base.Complete();
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x00164FD0 File Offset: 0x001631D0
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.inspectStringTargets != null && this.inspectStringTargets.Contains(target))
			{
				return this.inspectString.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x00165010 File Offset: 0x00163210
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPart, "expiryInfoPart", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartTip, "expiryInfoPartTip", null, false);
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Collections.Look<ISelectable>(ref this.inspectStringTargets, "inspectStringTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isBad, "isBad", false, false);
			Scribe_Values.Look<string>(ref this.alertLabel, "alertLabel", null, false);
			Scribe_Values.Look<string>(ref this.alertExplanation, "alertExplanation", null, false);
			Scribe_Values.Look<int>(ref this.ticksLeftAlertCritical, "ticksLeftAlertCritical", 0, false);
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.alertCulprits, "alertCulprits", LookMode.GlobalTargetInfo, Array.Empty<object>());
			if (this.alertCulprits == null)
			{
				this.alertCulprits = new List<GlobalTargetInfo>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.alertCulprits.RemoveAll((GlobalTargetInfo x) => !x.IsValid);
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x00165128 File Offset: 0x00163328
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true))
			{
				this.DelayFinished();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x0016518C File Offset: 0x0016338C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x001651A9 File Offset: 0x001633A9
		public void DebugForceEnd()
		{
			this.DelayFinished();
		}

		// Token: 0x04002896 RID: 10390
		public int delayTicks;

		// Token: 0x04002897 RID: 10391
		public string expiryInfoPart;

		// Token: 0x04002898 RID: 10392
		public string expiryInfoPartTip;

		// Token: 0x04002899 RID: 10393
		public string inspectString;

		// Token: 0x0400289A RID: 10394
		public List<ISelectable> inspectStringTargets;

		// Token: 0x0400289B RID: 10395
		public bool isBad;

		// Token: 0x0400289C RID: 10396
		public string alertLabel;

		// Token: 0x0400289D RID: 10397
		public string alertExplanation;

		// Token: 0x0400289E RID: 10398
		public List<GlobalTargetInfo> alertCulprits = new List<GlobalTargetInfo>();

		// Token: 0x0400289F RID: 10399
		public int ticksLeftAlertCritical;
	}
}
