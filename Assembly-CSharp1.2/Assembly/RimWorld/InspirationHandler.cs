using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B26 RID: 2854
	public class InspirationHandler : IExposable
	{
		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060042F1 RID: 17137 RVA: 0x00031B81 File Offset: 0x0002FD81
		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060042F2 RID: 17138 RVA: 0x00031B8C File Offset: 0x0002FD8C
		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060042F3 RID: 17139 RVA: 0x00031B94 File Offset: 0x0002FD94
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

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060042F4 RID: 17140 RVA: 0x0018CE0C File Offset: 0x0018B00C
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

		// Token: 0x060042F5 RID: 17141 RVA: 0x00031BAB File Offset: 0x0002FDAB
		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x00031BBA File Offset: 0x0002FDBA
		public void ExposeData()
		{
			Scribe_Deep.Look<Inspiration>(ref this.curState, "curState", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curState != null)
			{
				this.curState.pawn = this.pawn;
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00031BF2 File Offset: 0x0002FDF2
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

		// Token: 0x060042F8 RID: 17144 RVA: 0x00031C1C File Offset: 0x0002FE1C
		[Obsolete("Will be removed in a future game release and replaced with TryStartInspiration_NewTemp.")]
		public bool TryStartInspiration(InspirationDef def)
		{
			return this.TryStartInspiration_NewTemp(def, null);
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x0018CE70 File Offset: 0x0018B070
		public bool TryStartInspiration_NewTemp(InspirationDef def, string reason = null)
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
			this.curState.PostStart();
			return true;
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x0018CEE8 File Offset: 0x0018B0E8
		public void EndInspiration(Inspiration inspiration)
		{
			if (inspiration == null)
			{
				return;
			}
			if (this.curState != inspiration)
			{
				Log.Error("Tried to end inspiration " + inspiration.ToStringSafe<Inspiration>() + " but current inspiration is " + this.curState.ToStringSafe<Inspiration>(), false);
				return;
			}
			this.curState = null;
			inspiration.PostEnd();
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x00031C26 File Offset: 0x0002FE26
		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00031C4A File Offset: 0x0002FE4A
		public void Reset()
		{
			this.curState = null;
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x0018CF38 File Offset: 0x0018B138
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
					this.TryStartInspiration_NewTemp(randomAvailableInspirationDef, "LetterInspirationBeginThanksToHighMoodPart".Translate());
				}
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00031C53 File Offset: 0x0002FE53
		public InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		// Token: 0x04002DD1 RID: 11729
		public Pawn pawn;

		// Token: 0x04002DD2 RID: 11730
		private Inspiration curState;

		// Token: 0x04002DD3 RID: 11731
		private const int CheckStartInspirationIntervalTicks = 100;

		// Token: 0x04002DD4 RID: 11732
		private const float MinMood = 0.5f;

		// Token: 0x04002DD5 RID: 11733
		private const float StartInspirationMTBDaysAtMaxMood = 10f;
	}
}
