using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002194 RID: 8596
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x0600B787 RID: 46983 RVA: 0x00077086 File Offset: 0x00075286
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x0600B788 RID: 46984 RVA: 0x00077093 File Offset: 0x00075293
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x0600B789 RID: 46985 RVA: 0x0007709E File Offset: 0x0007529E
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x0600B78A RID: 46986 RVA: 0x000770B3 File Offset: 0x000752B3
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

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x0600B78B RID: 46987 RVA: 0x000770C5 File Offset: 0x000752C5
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x0600B78C RID: 46988 RVA: 0x0034EE88 File Offset: 0x0034D088
		public void Start(float? durationDays = null)
		{
			float num = durationDays ?? this.Props.durationDays;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x0600B78D RID: 46989 RVA: 0x000770D4 File Offset: 0x000752D4
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		// Token: 0x0600B78E RID: 46990 RVA: 0x000770DD File Offset: 0x000752DD
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x0600B78F RID: 46991 RVA: 0x000770FB File Offset: 0x000752FB
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x0600B790 RID: 46992 RVA: 0x0034EEC8 File Offset: 0x0034D0C8
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x0600B791 RID: 46993 RVA: 0x00077111 File Offset: 0x00075311
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x0600B792 RID: 46994 RVA: 0x0007712B File Offset: 0x0007532B
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

		// Token: 0x04007D94 RID: 32148
		private int ticksLeft;
	}
}
