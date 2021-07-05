using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8F RID: 3727
	public sealed class SituationalThoughtHandler
	{
		// Token: 0x06005780 RID: 22400 RVA: 0x001DC8EC File Offset: 0x001DAAEC
		public SituationalThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x001DC93D File Offset: 0x001DAB3D
		public void SituationalThoughtInterval()
		{
			this.RemoveExpiredThoughtsFromCache();
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x001DC948 File Offset: 0x001DAB48
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

		// Token: 0x06005783 RID: 22403 RVA: 0x001DC990 File Offset: 0x001DAB90
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

		// Token: 0x06005784 RID: 22404 RVA: 0x001DC9E4 File Offset: 0x001DABE4
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
				if (ModsConfig.IdeologyActive && this.pawn.Ideo != null)
				{
					Ideo ideo = this.pawn.Ideo;
					for (int k = 0; k < ideo.PreceptsListForReading.Count; k++)
					{
						this.cachedThoughts.AddRange(ideo.PreceptsListForReading[k].SituationThoughtsToAdd(this.pawn, this.cachedThoughts));
					}
				}
			}
			finally
			{
			}
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x001DCB34 File Offset: 0x001DAD34
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

		// Token: 0x06005786 RID: 22406 RVA: 0x001DCC88 File Offset: 0x001DAE88
		private Thought_Situational TryCreateThought(ThoughtDef def)
		{
			Thought_Situational thought_Situational = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def, false))
				{
					return null;
				}
				if (!def.Worker.CurrentState(this.pawn).ActiveFor(def))
				{
					return null;
				}
				thought_Situational = (Thought_Situational)ThoughtMaker.MakeThought(def);
				thought_Situational.pawn = this.pawn;
				if (def.Worker is ThoughtWorker_Precept)
				{
					thought_Situational.sourcePrecept = this.pawn.Ideo.GetFirstPreceptAllowingSituationalThought(def);
				}
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
				}));
			}
			return thought_Situational;
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x001DCD64 File Offset: 0x001DAF64
		private Thought_SituationalSocial TryCreateSocialThought(ThoughtDef def, Pawn otherPawn)
		{
			Thought_SituationalSocial thought_SituationalSocial = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def, false))
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
				if (def.Worker is ThoughtWorker_Precept_Social)
				{
					thought_SituationalSocial.sourcePrecept = this.pawn.Ideo.GetFirstPreceptAllowingSituationalThought(def);
				}
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
				}));
			}
			return thought_SituationalSocial;
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x001DCE48 File Offset: 0x001DB048
		public void Notify_SituationalThoughtsDirty()
		{
			this.cachedThoughts.Clear();
			this.cachedSocialThoughts.Clear();
			this.lastMoodThoughtsRecalculationTick = -99999;
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x001DCE6B File Offset: 0x001DB06B
		private void RemoveExpiredThoughtsFromCache()
		{
			this.cachedSocialThoughts.RemoveAll((KeyValuePair<Pawn, SituationalThoughtHandler.CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
		}

		// Token: 0x040033B1 RID: 13233
		public Pawn pawn;

		// Token: 0x040033B2 RID: 13234
		private List<Thought_Situational> cachedThoughts = new List<Thought_Situational>();

		// Token: 0x040033B3 RID: 13235
		private int lastMoodThoughtsRecalculationTick = -99999;

		// Token: 0x040033B4 RID: 13236
		private Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts> cachedSocialThoughts = new Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts>();

		// Token: 0x040033B5 RID: 13237
		private const int RecalculateStateEveryTicks = 100;

		// Token: 0x040033B6 RID: 13238
		private HashSet<ThoughtDef> tmpCachedThoughts = new HashSet<ThoughtDef>();

		// Token: 0x040033B7 RID: 13239
		private HashSet<ThoughtDef> tmpCachedSocialThoughts = new HashSet<ThoughtDef>();

		// Token: 0x020022F7 RID: 8951
		private class CachedSocialThoughts
		{
			// Token: 0x17001E31 RID: 7729
			// (get) Token: 0x0600C529 RID: 50473 RVA: 0x003DD31A File Offset: 0x003DB51A
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastQueryTick >= 300;
				}
			}

			// Token: 0x17001E32 RID: 7730
			// (get) Token: 0x0600C52A RID: 50474 RVA: 0x003DD337 File Offset: 0x003DB537
			public bool ShouldRecalculateState
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastRecalculationTick >= 100;
				}
			}

			// Token: 0x04008553 RID: 34131
			public List<Thought_SituationalSocial> thoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04008554 RID: 34132
			public List<Thought_SituationalSocial> activeThoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04008555 RID: 34133
			public int lastRecalculationTick = -99999;

			// Token: 0x04008556 RID: 34134
			public int lastQueryTick = -99999;

			// Token: 0x04008557 RID: 34135
			private const int ExpireAfterTicks = 300;
		}
	}
}
