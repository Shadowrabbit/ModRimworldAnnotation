using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BC RID: 1724
	public class InspirationHandler : IExposable
	{
		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x0011CBC9 File Offset: 0x0011ADC9
		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x0011CBD4 File Offset: 0x0011ADD4
		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x0011CBDC File Offset: 0x0011ADDC
		public InspirationDef CurStateDef
		{
			get
			{
				if (this.curState == null)
				{
					return null;
				}
				return this.curState.def;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x0011CBF4 File Offset: 0x0011ADF4
		private float StartInspirationMTBDays
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return -1f;
				}
				float curLevel = this.pawn.needs.mood.CurLevel;
				if (curLevel < 0.5f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.5f, 1f, 210f, 10f, curLevel);
			}
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x0011CC57 File Offset: 0x0011AE57
		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x0011CC66 File Offset: 0x0011AE66
		public void ExposeData()
		{
			Scribe_Deep.Look<Inspiration>(ref this.curState, "curState", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curState != null)
			{
				this.curState.pawn = this.pawn;
			}
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x0011CC9E File Offset: 0x0011AE9E
		public void InspirationHandlerTick()
		{
			if (this.curState != null)
			{
				this.curState.InspirationTick();
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				this.CheckStartRandomInspiration();
			}
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x0011CCC8 File Offset: 0x0011AEC8
		public bool TryStartInspiration(InspirationDef def, string reason = null, bool sendLetter = true)
		{
			if (this.Inspired)
			{
				return false;
			}
			if (!def.Worker.InspirationCanOccur(this.pawn))
			{
				return false;
			}
			this.curState = (Inspiration)Activator.CreateInstance(def.inspirationClass);
			this.curState.def = def;
			this.curState.pawn = this.pawn;
			this.curState.reason = reason;
			this.curState.PostStart(sendLetter);
			return true;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x0011CD40 File Offset: 0x0011AF40
		public void EndInspiration(Inspiration inspiration)
		{
			if (inspiration == null)
			{
				return;
			}
			if (this.curState != inspiration)
			{
				Log.Error("Tried to end inspiration " + inspiration.ToStringSafe<Inspiration>() + " but current inspiration is " + this.curState.ToStringSafe<Inspiration>());
				return;
			}
			this.curState = null;
			inspiration.PostEnd();
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x0011CD8D File Offset: 0x0011AF8D
		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x0011CDB1 File Offset: 0x0011AFB1
		public void Reset()
		{
			this.curState = null;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x0011CDBC File Offset: 0x0011AFBC
		private void CheckStartRandomInspiration()
		{
			if (this.Inspired)
			{
				return;
			}
			if (!this.pawn.health.capacities.CanBeAwake)
			{
				return;
			}
			float startInspirationMTBDays = this.StartInspirationMTBDays;
			if (startInspirationMTBDays < 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(startInspirationMTBDays, 60000f, 100f))
			{
				InspirationDef randomAvailableInspirationDef = this.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					this.TryStartInspiration(randomAvailableInspirationDef, "LetterInspirationBeginThanksToHighMoodPart".Translate(), true);
				}
			}
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x0011CE2E File Offset: 0x0011B02E
		public InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		// Token: 0x04001D24 RID: 7460
		public Pawn pawn;

		// Token: 0x04001D25 RID: 7461
		private Inspiration curState;

		// Token: 0x04001D26 RID: 7462
		private const int CheckStartInspirationIntervalTicks = 100;

		// Token: 0x04001D27 RID: 7463
		private const float MinMood = 0.5f;

		// Token: 0x04001D28 RID: 7464
		private const float StartInspirationMTBDaysAtMaxMood = 10f;
	}
}
