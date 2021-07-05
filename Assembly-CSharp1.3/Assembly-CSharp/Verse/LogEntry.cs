using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000136 RID: 310
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00027C8C File Offset: 0x00025E8C
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00027C9F File Offset: 0x00025E9F
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00027CA7 File Offset: 0x00025EA7
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00027C9F File Offset: 0x00025E9F
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00027CAF File Offset: 0x00025EAF
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00027CEC File Offset: 0x00025EEC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00027D24 File Offset: 0x00025F24
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

		// Token: 0x06000881 RID: 2177 RVA: 0x00027DD0 File Offset: 0x00025FD0
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			return GrammarResolver.Resolve("r_logentry", this.GenerateGrammarRequest(), null, forceLog, null, null, null, true);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00027DE8 File Offset: 0x00025FE8
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00027E00 File Offset: 0x00026000
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

		// Token: 0x06000884 RID: 2180 RVA: 0x00027E39 File Offset: 0x00026039
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x06000885 RID: 2181
		public abstract bool Concerns(Thing t);

		// Token: 0x06000886 RID: 2182
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x06000887 RID: 2183 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CanBeClickedFromPOV(Thing pov)
		{
			return false;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00027E60 File Offset: 0x00026060
		public virtual Color? IconColorFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_IdeoRemoved(Ideo ideo)
		{
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00027E78 File Offset: 0x00026078
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true, true)).CapitalizeFirst() + ".";
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00027EBA File Offset: 0x000260BA
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00027EC3 File Offset: 0x000260C3
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		// Token: 0x040007F2 RID: 2034
		protected int logID;

		// Token: 0x040007F3 RID: 2035
		protected int ticksAbs = -1;

		// Token: 0x040007F4 RID: 2036
		public LogEntryDef def;

		// Token: 0x040007F5 RID: 2037
		private WeakReference<Thing> cachedStringPov;

		// Token: 0x040007F6 RID: 2038
		private string cachedString;

		// Token: 0x040007F7 RID: 2039
		private float cachedHeightWidth;

		// Token: 0x040007F8 RID: 2040
		private float cachedHeight;

		// Token: 0x040007F9 RID: 2041
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x040007FA RID: 2042
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x040007FB RID: 2043
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x040007FC RID: 2044
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x040007FD RID: 2045
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x040007FE RID: 2046
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}
