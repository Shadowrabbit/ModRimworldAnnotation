using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5E RID: 3678
	public class Pawn_NeedsTracker : IExposable
	{
		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005514 RID: 21780 RVA: 0x001CCECB File Offset: 0x001CB0CB
		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06005515 RID: 21781 RVA: 0x001CCED3 File Offset: 0x001CB0D3
		public List<Need> MiscNeeds
		{
			get
			{
				return this.needsMisc;
			}
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x001CCEDB File Offset: 0x001CB0DB
		public Pawn_NeedsTracker()
		{
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x001CCEFA File Offset: 0x001CB0FA
		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x001CCF28 File Offset: 0x001CB128
		public void ExposeData()
		{
			Scribe_Collections.Look<Need>(ref this.needs, "needs", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.needs.RemoveAll((Need x) => x == null || x.def == null) != 0)
				{
					Log.Error("Pawn " + this.pawn.ToStringSafe<Pawn>() + " had some null needs after loading.");
				}
				this.BindDirectNeedFields();
				this.CacheMiscNeeds();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x001CCFC4 File Offset: 0x001CB1C4
		private void BindDirectNeedFields()
		{
			this.mood = this.TryGetNeed<Need_Mood>();
			this.food = this.TryGetNeed<Need_Food>();
			this.rest = this.TryGetNeed<Need_Rest>();
			this.joy = this.TryGetNeed<Need_Joy>();
			this.beauty = this.TryGetNeed<Need_Beauty>();
			this.comfort = this.TryGetNeed<Need_Comfort>();
			this.roomsize = this.TryGetNeed<Need_RoomSize>();
			this.outdoors = this.TryGetNeed<Need_Outdoors>();
			this.indoors = this.TryGetNeed<Need_Indoors>();
			this.drugsDesire = this.TryGetNeed<Need_Chemical_Any>();
			this.authority = null;
		}

		// Token: 0x0600551A RID: 21786 RVA: 0x001CD050 File Offset: 0x001CB250
		private void CacheMiscNeeds()
		{
			this.needsMisc.Clear();
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x001CD060 File Offset: 0x001CB260
		public void NeedsTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(150))
			{
				for (int i = 0; i < this.needs.Count; i++)
				{
					this.needs[i].NeedInterval();
				}
			}
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x001CD0A8 File Offset: 0x001CB2A8
		public T TryGetNeed<T>() where T : Need
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].GetType() == typeof(T))
				{
					return (T)((object)this.needs[i]);
				}
			}
			return default(T);
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x001CD108 File Offset: 0x001CB308
		public Need TryGetNeed(NeedDef def)
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].def == def)
				{
					return this.needs[i];
				}
			}
			return null;
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x001CD150 File Offset: 0x001CB350
		public void SetInitialLevels()
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x001CD184 File Offset: 0x001CB384
		public void AddOrRemoveNeedsAsAppropriate()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				try
				{
					NeedDef needDef = allDefsListForReading[i];
					if (this.ShouldHaveNeed(needDef))
					{
						if (this.TryGetNeed(needDef) == null)
						{
							this.AddNeed(needDef);
						}
					}
					else if (this.TryGetNeed(needDef) != null)
					{
						this.RemoveNeed(needDef);
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while determining if ",
						this.pawn.ToStringSafe<Pawn>(),
						" should have Need ",
						allDefsListForReading[i].ToStringSafe<NeedDef>(),
						": ",
						ex
					}));
				}
			}
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x001CD240 File Offset: 0x001CB440
		private bool ShouldHaveNeed(NeedDef nd)
		{
			if (this.pawn.RaceProps.intelligence < nd.minIntelligence)
			{
				return false;
			}
			if (nd.colonistsOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer))
			{
				return false;
			}
			if (nd.colonistAndPrisonersOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || this.pawn.HostFaction != Faction.OfPlayer))
			{
				return false;
			}
			if (this.pawn.health.hediffSet.hediffs.Any((Hediff x) => !x.def.disablesNeeds.NullOrEmpty<NeedDef>() && x.def.disablesNeeds.Contains(nd)))
			{
				return false;
			}
			if (nd.onlyIfCausedByHediff && !this.pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.causesNeed == nd))
			{
				return false;
			}
			if (nd.neverOnPrisoner && this.pawn.IsPrisoner)
			{
				return false;
			}
			if (nd.neverOnSlave && this.pawn.IsSlave)
			{
				return false;
			}
			if (nd.titleRequiredAny != null)
			{
				if (this.pawn.royalty == null)
				{
					return false;
				}
				bool flag = false;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
					if (nd.titleRequiredAny.Contains(royalTitle.def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (nd.nullifyingPrecepts != null && this.pawn.Ideo != null)
			{
				bool flag2 = false;
				foreach (PreceptDef preceptDef in nd.nullifyingPrecepts)
				{
					if (this.pawn.Ideo.HasPrecept(preceptDef))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					return false;
				}
			}
			if (nd.hediffRequiredAny != null)
			{
				bool flag3 = false;
				foreach (HediffDef def in nd.hediffRequiredAny)
				{
					if (this.pawn.health.hediffSet.HasHediff(def, false))
					{
						flag3 = true;
					}
				}
				if (!flag3)
				{
					return false;
				}
			}
			if (nd.defName == "Authority")
			{
				return false;
			}
			if (nd.onlyIfCausedByTrait)
			{
				Pawn_StoryTracker story = this.pawn.story;
				List<Trait> list;
				if (story == null)
				{
					list = null;
				}
				else
				{
					TraitSet traits = story.traits;
					list = ((traits != null) ? traits.allTraits : null);
				}
				List<Trait> list2 = list;
				if (list2.NullOrEmpty<Trait>())
				{
					return false;
				}
				bool flag4 = false;
				for (int i = 0; i < list2.Count; i++)
				{
					Trait trait = list2[i];
					if (!trait.CurrentData.needs.NullOrEmpty<NeedDef>() && trait.CurrentData.needs.Contains(nd))
					{
						flag4 = true;
					}
				}
				if (!flag4)
				{
					return false;
				}
			}
			if (nd.slavesOnly && !this.pawn.IsSlave)
			{
				return false;
			}
			if (nd == NeedDefOf.Food)
			{
				return this.pawn.RaceProps.EatsFood;
			}
			return nd != NeedDefOf.Rest || this.pawn.RaceProps.needsRest;
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x001CD61C File Offset: 0x001CB81C
		private void AddNeed(NeedDef nd)
		{
			Need need = (Need)Activator.CreateInstance(nd.needClass, new object[]
			{
				this.pawn
			});
			need.def = nd;
			this.needs.Add(need);
			need.SetInitialLevel();
			this.BindDirectNeedFields();
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x001CD668 File Offset: 0x001CB868
		private void RemoveNeed(NeedDef nd)
		{
			Need item = this.TryGetNeed(nd);
			this.needs.Remove(item);
			this.BindDirectNeedFields();
		}

		// Token: 0x04003268 RID: 12904
		private Pawn pawn;

		// Token: 0x04003269 RID: 12905
		private List<Need> needs = new List<Need>();

		// Token: 0x0400326A RID: 12906
		public Need_Mood mood;

		// Token: 0x0400326B RID: 12907
		public Need_Food food;

		// Token: 0x0400326C RID: 12908
		public Need_Rest rest;

		// Token: 0x0400326D RID: 12909
		public Need_Joy joy;

		// Token: 0x0400326E RID: 12910
		public Need_Beauty beauty;

		// Token: 0x0400326F RID: 12911
		public Need_RoomSize roomsize;

		// Token: 0x04003270 RID: 12912
		public Need_Outdoors outdoors;

		// Token: 0x04003271 RID: 12913
		public Need_Indoors indoors;

		// Token: 0x04003272 RID: 12914
		public Need_Chemical_Any drugsDesire;

		// Token: 0x04003273 RID: 12915
		public Need_Comfort comfort;

		// Token: 0x04003274 RID: 12916
		public Need_Authority authority;

		// Token: 0x04003275 RID: 12917
		private List<Need> needsMisc = new List<Need>(0);
	}
}
