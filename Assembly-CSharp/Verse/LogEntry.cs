using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001D7 RID: 471
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0000F630 File Offset: 0x0000D830
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x0000F643 File Offset: 0x0000D843
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0000F64B File Offset: 0x0000D84B
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0000F643 File Offset: 0x0000D843
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0000F653 File Offset: 0x0000D853
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0000F690 File Offset: 0x0000D890
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000A3910 File Offset: 0x000A1B10
		public string ToGameStringFromPOV(Thing pov, bool forceLog = false)
		{
			if (this.cachedString == null || pov == null != (this.cachedStringPov == null) || (this.cachedStringPov != null && pov != this.cachedStringPov.Target) || DebugViewSettings.logGrammarResolution || forceLog)
			{
				Rand.PushState();
				try
				{
					Rand.Seed = this.logID;
					this.cachedStringPov = ((pov != null) ? new WeakReference<Thing>(pov) : null);
					this.cachedString = this.ToGameStringFromPOV_Worker(pov, forceLog);
					this.cachedHeightWidth = 0f;
					this.cachedHeight = 0f;
				}
				finally
				{
					Rand.PopState();
				}
			}
			return this.cachedString;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0000F6C6 File Offset: 0x0000D8C6
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			return GrammarResolver.Resolve("r_logentry", this.GenerateGrammarRequest(), null, forceLog, null, null, null, true);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000A39BC File Offset: 0x000A1BBC
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000A39D4 File Offset: 0x000A1BD4
		public float GetTextHeight(Thing pov, float width)
		{
			string text = this.ToGameStringFromPOV(pov, false);
			if (this.cachedHeightWidth != width)
			{
				this.cachedHeightWidth = width;
				this.cachedHeight = Text.CalcHeight(text, width);
			}
			return this.cachedHeight;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0000F6DE File Offset: 0x0000D8DE
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x06000C35 RID: 3125
		public abstract bool Concerns(Thing t);

		// Token: 0x06000C36 RID: 3126
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x06000C37 RID: 3127 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool CanBeClickedFromPOV(Thing pov)
		{
			return false;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x000A3A10 File Offset: 0x000A1C10
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true, true)).CapitalizeFirst() + ".";
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0000F704 File Offset: 0x0000D904
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0000F70D File Offset: 0x0000D90D
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		// Token: 0x04000A9C RID: 2716
		protected int logID;

		// Token: 0x04000A9D RID: 2717
		protected int ticksAbs = -1;

		// Token: 0x04000A9E RID: 2718
		public LogEntryDef def;

		// Token: 0x04000A9F RID: 2719
		private WeakReference<Thing> cachedStringPov;

		// Token: 0x04000AA0 RID: 2720
		private string cachedString;

		// Token: 0x04000AA1 RID: 2721
		private float cachedHeightWidth;

		// Token: 0x04000AA2 RID: 2722
		private float cachedHeight;

		// Token: 0x04000AA3 RID: 2723
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x04000AA4 RID: 2724
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x04000AA5 RID: 2725
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x04000AA6 RID: 2726
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x04000AA7 RID: 2727
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x04000AA8 RID: 2728
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}
