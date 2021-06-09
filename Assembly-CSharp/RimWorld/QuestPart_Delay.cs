using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001043 RID: 4163
	public class QuestPart_Delay : QuestPartActivable
	{
		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06005ABE RID: 23230 RVA: 0x0003EDD3 File Offset: 0x0003CFD3
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

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06005ABF RID: 23231 RVA: 0x0003EDF8 File Offset: 0x0003CFF8
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

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06005AC0 RID: 23232 RVA: 0x0003EE2D File Offset: 0x0003D02D
		public override string ExpiryInfoPartTip
		{
			get
			{
				return this.expiryInfoPartTip.Formatted(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.enableTick + this.delayTicks), QuestUtility.GetLocForDates()));
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06005AC1 RID: 23233 RVA: 0x0003EE61 File Offset: 0x0003D061
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

		// Token: 0x06005AC2 RID: 23234 RVA: 0x0003EE71 File Offset: 0x0003D071
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (Find.TickManager.TicksGame >= this.enableTick + this.delayTicks)
			{
				this.DelayFinished();
			}
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x0003EE98 File Offset: 0x0003D098
		protected virtual void DelayFinished()
		{
			base.Complete();
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x0003EEA0 File Offset: 0x0003D0A0
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.inspectStringTargets != null && this.inspectStringTargets.Contains(target))
			{
				return this.inspectString.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x001D67C8 File Offset: 0x001D49C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPart, "expiryInfoPart", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartTip, "expiryInfoPartTip", null, false);
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Collections.Look<ISelectable>(ref this.inspectStringTargets, "inspectStringTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isBad, "isBad", false, false);
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x001D684C File Offset: 0x001D4A4C
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

		// Token: 0x06005AC7 RID: 23239 RVA: 0x0003EEDE File Offset: 0x0003D0DE
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x0003EEFB File Offset: 0x0003D0FB
		public void DebugForceEnd()
		{
			this.DelayFinished();
		}

		// Token: 0x04003D00 RID: 15616
		public int delayTicks;

		// Token: 0x04003D01 RID: 15617
		public string expiryInfoPart;

		// Token: 0x04003D02 RID: 15618
		public string expiryInfoPartTip;

		// Token: 0x04003D03 RID: 15619
		public string inspectString;

		// Token: 0x04003D04 RID: 15620
		public List<ISelectable> inspectStringTargets;

		// Token: 0x04003D05 RID: 15621
		public bool isBad;
	}
}
