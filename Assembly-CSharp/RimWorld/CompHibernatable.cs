using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012E3 RID: 4835
	public class CompHibernatable : ThingComp
	{
		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x060068AE RID: 26798 RVA: 0x000474B5 File Offset: 0x000456B5
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x060068AF RID: 26799 RVA: 0x000474C2 File Offset: 0x000456C2
		// (set) Token: 0x060068B0 RID: 26800 RVA: 0x000474CA File Offset: 0x000456CA
		public HibernatableStateDef State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state == value)
				{
					return;
				}
				this.state = value;
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x060068B1 RID: 26801 RVA: 0x000474F7 File Offset: 0x000456F7
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x00047506 File Offset: 0x00045706
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x060068B3 RID: 26803 RVA: 0x0004752C File Offset: 0x0004572C
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.info.parent.Notify_HibernatableChanged();
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x060068B4 RID: 26804 RVA: 0x00204164 File Offset: 0x00202364
		public void Startup()
		{
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				Log.ErrorOnce("Attempted to start a non-hibernating object", 34361223, false);
				return;
			}
			this.State = HibernatableStateDefOf.Starting;
			this.endStartupTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + this.Props.startupDays * 60000f);
		}

		// Token: 0x060068B5 RID: 26805 RVA: 0x002041C4 File Offset: 0x002023C4
		public override string CompInspectStringExtra()
		{
			if (this.State == HibernatableStateDefOf.Hibernating)
			{
				return "HibernatableHibernating".Translate();
			}
			if (this.State == HibernatableStateDefOf.Starting)
			{
				return string.Format("{0}: {1}", "HibernatableStartingUp".Translate(), (this.endStartupTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x060068B6 RID: 26806 RVA: 0x00204230 File Offset: 0x00202430
		public override void CompTick()
		{
			if (this.State == HibernatableStateDefOf.Starting && Find.TickManager.TicksGame > this.endStartupTick)
			{
				this.State = HibernatableStateDefOf.Running;
				this.endStartupTick = 0;
				string str;
				if (this.parent.Map.Parent.GetComponent<EscapeShipComp>() != null)
				{
					str = "LetterHibernateComplete".Translate();
				}
				else
				{
					str = "LetterHibernateCompleteStandalone".Translate();
				}
				Find.LetterStack.ReceiveLetter("LetterLabelHibernateComplete".Translate(), str, LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.parent), null, null, null, null);
			}
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.sustainerActive.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
				return;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x060068B7 RID: 26807 RVA: 0x00047565 File Offset: 0x00045765
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}

		// Token: 0x040045A8 RID: 17832
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x040045A9 RID: 17833
		private int endStartupTick;

		// Token: 0x040045AA RID: 17834
		private Sustainer sustainer;
	}
}
