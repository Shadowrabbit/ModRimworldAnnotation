using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B3 RID: 4275
	public class StunHandler : IExposable
	{
		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x0600661A RID: 26138 RVA: 0x002278B6 File Offset: 0x00225AB6
		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0;
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x0600661B RID: 26139 RVA: 0x002278C4 File Offset: 0x00225AC4
		private int EMPAdaptationTicksDuration
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return 2200;
				}
				return 0;
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x0600661C RID: 26140 RVA: 0x002278F4 File Offset: 0x00225AF4
		private bool AffectedByEMP
		{
			get
			{
				Pawn pawn;
				return (pawn = (this.parent as Pawn)) == null || !pawn.RaceProps.IsFlesh;
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x0600661D RID: 26141 RVA: 0x00227920 File Offset: 0x00225B20
		public int StunTicksLeft
		{
			get
			{
				return this.stunTicksLeft;
			}
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x00227928 File Offset: 0x00225B28
		public StunHandler(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600661F RID: 26143 RVA: 0x00227940 File Offset: 0x00225B40
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.showStunMote, "showStunMote", false, false);
			Scribe_Values.Look<int>(ref this.EMPAdaptedTicksLeft, "EMPAdaptedTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.stunFromEMP, "stunFromEMP", false, false);
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x00227998 File Offset: 0x00225B98
		public void StunHandlerTick()
		{
			if (this.EMPAdaptedTicksLeft > 0)
			{
				this.EMPAdaptedTicksLeft--;
			}
			if (this.stunTicksLeft > 0)
			{
				this.stunTicksLeft--;
				if (this.showStunMote && (this.moteStun == null || this.moteStun.Destroyed))
				{
					this.moteStun = MoteMaker.MakeStunOverlay(this.parent);
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.Downed)
				{
					this.stunTicksLeft = 0;
				}
				if (this.moteStun != null)
				{
					this.moteStun.Maintain();
				}
				if (this.AffectedByEMP && this.stunFromEMP)
				{
					if (this.empEffecter == null)
					{
						this.empEffecter = EffecterDefOf.DisabledByEMP.Spawn();
					}
					this.empEffecter.EffectTick(this.parent, this.parent);
					return;
				}
			}
			else if (this.empEffecter != null)
			{
				this.empEffecter.Cleanup();
				this.empEffecter = null;
				this.stunFromEMP = false;
			}
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x00227AA0 File Offset: 0x00225CA0
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			Pawn pawn = this.parent as Pawn;
			if (pawn != null && (pawn.Downed || pawn.Dead))
			{
				return;
			}
			if (dinfo.Def == DamageDefOf.Stun)
			{
				this.StunFor(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator, true, true);
				return;
			}
			if (dinfo.Def == DamageDefOf.EMP && this.AffectedByEMP)
			{
				if (this.EMPAdaptedTicksLeft <= 0)
				{
					this.StunFor(Mathf.RoundToInt(dinfo.Amount * 30f), dinfo.Instigator, true, true);
					this.EMPAdaptedTicksLeft = this.EMPAdaptationTicksDuration;
					this.stunFromEMP = true;
					return;
				}
				MoteMaker.ThrowText(new Vector3((float)this.parent.Position.x + 1f, (float)this.parent.Position.y, (float)this.parent.Position.z + 1f), this.parent.Map, "Adapted".Translate(), Color.white, -1f);
			}
		}

		// Token: 0x06006622 RID: 26146 RVA: 0x00227BC4 File Offset: 0x00225DC4
		public void StunFor(int ticks, Thing instigator, bool addBattleLog = true, bool showMote = true)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
			this.showStunMote = showMote;
			if (addBattleLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_Event(this.parent, RulePackDefOf.Event_Stun, instigator));
			}
		}

		// Token: 0x040039A7 RID: 14759
		public Thing parent;

		// Token: 0x040039A8 RID: 14760
		private int stunTicksLeft;

		// Token: 0x040039A9 RID: 14761
		private Mote moteStun;

		// Token: 0x040039AA RID: 14762
		private bool showStunMote = true;

		// Token: 0x040039AB RID: 14763
		private int EMPAdaptedTicksLeft;

		// Token: 0x040039AC RID: 14764
		private Effecter empEffecter;

		// Token: 0x040039AD RID: 14765
		private bool stunFromEMP;

		// Token: 0x040039AE RID: 14766
		public const float StunDurationTicksPerDamage = 30f;
	}
}
