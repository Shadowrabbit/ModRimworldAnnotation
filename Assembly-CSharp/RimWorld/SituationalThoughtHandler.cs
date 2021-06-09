using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155D RID: 5469
	public sealed class SituationalThoughtHandler
	{
		// Token: 0x06007691 RID: 30353 RVA: 0x00241D04 File Offset: 0x0023FF04
		public SituationalThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x0004FF45 File Offset: 0x0004E145
		public void SituationalThoughtInterval()
		{
			this.RemoveExpiredThoughtsFromCache();
		}

		// Token: 0x06007693 RID: 30355 RVA: 0x00241D58 File Offset: 0x0023FF58
		public void AppendMoodThoughts(List<Thought> outThoughts)
		{
			this.CheckRecalculateMoodThoughts();
			for (int i = 0; i < this.cachedThoughts.Count; i++)
			{
				Thought_Situational thought_Situational = this.cachedThoughts[i];
				if (thought_Situational.Active)
				{
					outThoughts.Add(thought_Situational);
				}
			}
		}

		// Token: 0x06007694 RID: 30356 RVA: 0x00241DA0 File Offset: 0x0023FFA0
		public void AppendSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.CheckRecalculateSocialThoughts(otherPawn);
			SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts = this.cachedSocialThoughts[otherPawn];
			cachedSocialThoughts.lastQueryTick = Find.TickManager.TicksGame;
			List<Thought_SituationalSocial> activeThoughts = cachedSocialThoughts.activeThoughts;
			for (int i = 0; i < activeThoughts.Count; i++)
			{
				outThoughts.Add(activeThoughts[i]);
			}
		}

		// Token: 0x06007695 RID: 30357 RVA: 0x00241DF4 File Offset: 0x0023FFF4
		private void CheckRecalculateMoodThoughts()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastMoodThoughtsRecalculationTick < 100)
			{
				return;
			}
			this.lastMoodThoughtsRecalculationTick = ticksGame;
			try
			{
				this.tmpCachedThoughts.Clear();
				for (int i = 0; i < this.cachedThoughts.Count; i++)
				{
					this.cachedThoughts[i].RecalculateState();
					this.tmpCachedThoughts.Add(this.cachedThoughts[i].def);
				}
				List<ThoughtDef> situationalNonSocialThoughtDefs = ThoughtUtility.situationalNonSocialThoughtDefs;
				int j = 0;
				int count = situationalNonSocialThoughtDefs.Count;
				while (j < count)
				{
					if (!this.tmpCachedThoughts.Contains(situationalNonSocialThoughtDefs[j]))
					{
						Thought_Situational thought_Situational = this.TryCreateThought(situationalNonSocialThoughtDefs[j]);
						if (thought_Situational != null)
						{
							this.cachedThoughts.Add(thought_Situational);
						}
					}
					j++;
				}
			}
			finally
			{
			}
		}

		// Token: 0x06007696 RID: 30358 RVA: 0x00241ED4 File Offset: 0x002400D4
		private void CheckRecalculateSocialThoughts(Pawn otherPawn)
		{
			try
			{
				SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts;
				if (!this.cachedSocialThoughts.TryGetValue(otherPawn, out cachedSocialThoughts))
				{
					cachedSocialThoughts = new SituationalThoughtHandler.CachedSocialThoughts();
					this.cachedSocialThoughts.Add(otherPawn, cachedSocialThoughts);
				}
				if (cachedSocialThoughts.ShouldRecalculateState)
				{
					cachedSocialThoughts.lastRecalculationTick = Find.TickManager.TicksGame;
					this.tmpCachedSocialThoughts.Clear();
					for (int i = 0; i < cachedSocialThoughts.thoughts.Count; i++)
					{
						Thought_SituationalSocial thought_SituationalSocial = cachedSocialThoughts.thoughts[i];
						thought_SituationalSocial.RecalculateState();
						this.tmpCachedSocialThoughts.Add(thought_SituationalSocial.def);
					}
					List<ThoughtDef> situationalSocialThoughtDefs = ThoughtUtility.situationalSocialThoughtDefs;
					int j = 0;
					int count = situationalSocialThoughtDefs.Count;
					while (j < count)
					{
						if (!this.tmpCachedSocialThoughts.Contains(situationalSocialThoughtDefs[j]))
						{
							Thought_SituationalSocial thought_SituationalSocial2 = this.TryCreateSocialThought(situationalSocialThoughtDefs[j], otherPawn);
							if (thought_SituationalSocial2 != null)
							{
								cachedSocialThoughts.thoughts.Add(thought_SituationalSocial2);
							}
						}
						j++;
					}
					cachedSocialThoughts.activeThoughts.Clear();
					for (int k = 0; k < cachedSocialThoughts.thoughts.Count; k++)
					{
						Thought_SituationalSocial thought_SituationalSocial3 = cachedSocialThoughts.thoughts[k];
						if (thought_SituationalSocial3.Active)
						{
							cachedSocialThoughts.activeThoughts.Add(thought_SituationalSocial3);
						}
					}
				}
			}
			finally
			{
			}
		}

		// Token: 0x06007697 RID: 30359 RVA: 0x00242028 File Offset: 0x00240228
		private Thought_Situational TryCreateThought(ThoughtDef def)
		{
			Thought_Situational thought_Situational = null;
			try
			{
				if (!ThoughtUtility.CanGetThought_NewTemp(this.pawn, def, false))
				{
					return null;
				}
				if (!def.Worker.CurrentState(this.pawn).ActiveFor(def))
				{
					return null;
				}
				thought_Situational = (Thought_Situational)ThoughtMaker.MakeThought(def);
				thought_Situational.pawn = this.pawn;
				thought_Situational.RecalculateState();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while recalculating ",
					def,
					" thought state for pawn ",
					this.pawn,
					": ",
					ex
				}), false);
			}
			return thought_Situational;
		}

		// Token: 0x06007698 RID: 30360 RVA: 0x002420DC File Offset: 0x002402DC
		private Thought_SituationalSocial TryCreateSocialThought(ThoughtDef def, Pawn otherPawn)
		{
			Thought_SituationalSocial thought_SituationalSocial = null;
			try
			{
				if (!ThoughtUtility.CanGetThought_NewTemp(this.pawn, def, false))
				{
					return null;
				}
				if (!def.Worker.CurrentSocialState(this.pawn, otherPawn).ActiveFor(def))
				{
					return null;
				}
				thought_SituationalSocial = (Thought_SituationalSocial)ThoughtMaker.MakeThought(def);
				thought_SituationalSocial.pawn = this.pawn;
				thought_SituationalSocial.otherPawn = otherPawn;
				thought_SituationalSocial.RecalculateState();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while recalculating ",
					def,
					" thought state for pawn ",
					this.pawn,
					": ",
					ex
				}), false);
			}
			return thought_SituationalSocial;
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x0004FF4D File Offset: 0x0004E14D
		public void Notify_SituationalThoughtsDirty()
		{
			this.cachedThoughts.Clear();
			this.cachedSocialThoughts.Clear();
			this.lastMoodThoughtsRecalculationTick = -99999;
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x0004FF70 File Offset: 0x0004E170
		private void RemoveExpiredThoughtsFromCache()
		{
			this.cachedSocialThoughts.RemoveAll((KeyValuePair<Pawn, SituationalThoughtHandler.CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
		}

		// Token: 0x04004E41 RID: 20033
		public Pawn pawn;

		// Token: 0x04004E42 RID: 20034
		private List<Thought_Situational> cachedThoughts = new List<Thought_Situational>();

		// Token: 0x04004E43 RID: 20035
		private int lastMoodThoughtsRecalculationTick = -99999;

		// Token: 0x04004E44 RID: 20036
		private Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts> cachedSocialThoughts = new Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts>();

		// Token: 0x04004E45 RID: 20037
		private const int RecalculateStateEveryTicks = 100;

		// Token: 0x04004E46 RID: 20038
		private HashSet<ThoughtDef> tmpCachedThoughts = new HashSet<ThoughtDef>();

		// Token: 0x04004E47 RID: 20039
		private HashSet<ThoughtDef> tmpCachedSocialThoughts = new HashSet<ThoughtDef>();

		// Token: 0x0200155E RID: 5470
		private class CachedSocialThoughts
		{
			// Token: 0x17001257 RID: 4695
			// (get) Token: 0x0600769B RID: 30363 RVA: 0x0004FF9D File Offset: 0x0004E19D
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastQueryTick >= 300;
				}
			}

			// Token: 0x17001258 RID: 4696
			// (get) Token: 0x0600769C RID: 30364 RVA: 0x0004FFBA File Offset: 0x0004E1BA
			public bool ShouldRecalculateState
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastRecalculationTick >= 100;
				}
			}

			// Token: 0x04004E48 RID: 20040
			public List<Thought_SituationalSocial> thoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04004E49 RID: 20041
			public List<Thought_SituationalSocial> activeThoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04004E4A RID: 20042
			public int lastRecalculationTick = -99999;

			// Token: 0x04004E4B RID: 20043
			public int lastQueryTick = -99999;

			// Token: 0x04004E4C RID: 20044
			private const int ExpireAfterTicks = 300;
		}
	}
}
