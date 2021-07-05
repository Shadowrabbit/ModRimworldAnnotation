using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F1 RID: 6129
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06008EF0 RID: 36592 RVA: 0x00334657 File Offset: 0x00332857
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x06008EF1 RID: 36593 RVA: 0x00334664 File Offset: 0x00332864
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x06008EF2 RID: 36594 RVA: 0x0033466F File Offset: 0x0033286F
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x06008EF3 RID: 36595 RVA: 0x00334684 File Offset: 0x00332884
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.ticksLeft;
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x06008EF4 RID: 36596 RVA: 0x00334696 File Offset: 0x00332896
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x06008EF5 RID: 36597 RVA: 0x003346A8 File Offset: 0x003328A8
		public void Start(float? durationDays = null)
		{
			float num = durationDays ?? this.Props.durationDays;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x06008EF6 RID: 36598 RVA: 0x003346E7 File Offset: 0x003328E7
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		// Token: 0x06008EF7 RID: 36599 RVA: 0x003346F0 File Offset: 0x003328F0
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x06008EF8 RID: 36600 RVA: 0x0033470E File Offset: 0x0033290E
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x06008EF9 RID: 36601 RVA: 0x00334724 File Offset: 0x00332924
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x06008EFA RID: 36602 RVA: 0x00334753 File Offset: 0x00332953
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06008EFB RID: 36603 RVA: 0x0033476D File Offset: 0x0033296D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Set enter cooldown to 1 hour",
					action = delegate()
					{
						this.ticksLeft = 2500;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Dev: Reset enter cooldown",
					action = delegate()
					{
						this.ticksLeft = 0;
					}
				};
			}
			yield break;
		}

		// Token: 0x04005A07 RID: 23047
		private int ticksLeft;
	}
}
