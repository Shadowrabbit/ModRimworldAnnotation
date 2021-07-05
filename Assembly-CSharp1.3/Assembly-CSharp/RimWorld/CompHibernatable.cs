using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CC9 RID: 3273
	public class CompHibernatable : ThingComp
	{
		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06004C37 RID: 19511 RVA: 0x00196620 File Offset: 0x00194820
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06004C38 RID: 19512 RVA: 0x0019662D File Offset: 0x0019482D
		// (set) Token: 0x06004C39 RID: 19513 RVA: 0x00196635 File Offset: 0x00194835
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

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x00196662 File Offset: 0x00194862
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x00196671 File Offset: 0x00194871
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x00196697 File Offset: 0x00194897
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.info.parent.Notify_HibernatableChanged();
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x001966D0 File Offset: 0x001948D0
		public void Startup()
		{
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				Log.ErrorOnce("Attempted to start a non-hibernating object", 34361223);
				return;
			}
			this.State = HibernatableStateDefOf.Starting;
			this.endStartupTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + this.Props.startupDays * 60000f);
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x00196730 File Offset: 0x00194930
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

		// Token: 0x06004C3F RID: 19519 RVA: 0x0019679C File Offset: 0x0019499C
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

		// Token: 0x06004C40 RID: 19520 RVA: 0x001968BE File Offset: 0x00194ABE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}

		// Token: 0x04002E19 RID: 11801
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x04002E1A RID: 11802
		private int endStartupTick;

		// Token: 0x04002E1B RID: 11803
		private Sustainer sustainer;
	}
}
