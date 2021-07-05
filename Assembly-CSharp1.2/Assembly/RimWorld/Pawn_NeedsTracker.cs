using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F6 RID: 5366
	public class Pawn_NeedsTracker : IExposable
	{
		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x0600738F RID: 29583 RVA: 0x0004DD39 File Offset: 0x0004BF39
		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		// Token: 0x06007390 RID: 29584 RVA: 0x0004DD41 File Offset: 0x0004BF41
		public Pawn_NeedsTracker()
		{
		}

		// Token: 0x06007391 RID: 29585 RVA: 0x0004DD54 File Offset: 0x0004BF54
		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x06007392 RID: 29586 RVA: 0x00234660 File Offset: 0x00232860
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
					Log.Error("Pawn " + this.pawn.ToStringSafe<Pawn>() + " had some null needs after loading.", false);
				}
				this.BindDirectNeedFields();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06007393 RID: 29587 RVA: 0x002346F8 File Offset: 0x002328F8
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
			this.drugsDesire = this.TryGetNeed<Need_Chemical_Any>();
			this.authority = null;
		}

		// Token: 0x06007394 RID: 29588 RVA: 0x00234778 File Offset: 0x00232978
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

		// Token: 0x06007395 RID: 29589 RVA: 0x002347C0 File Offset: 0x002329C0
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

		// Token: 0x06007396 RID: 29590 RVA: 0x00234820 File Offset: 0x00232A20
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

		// Token: 0x06007397 RID: 29591 RVA: 0x00234868 File Offset: 0x00232A68
		public void SetInitialLevels()
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		// Token: 0x06007398 RID: 29592 RVA: 0x0023489C File Offset: 0x00232A9C
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
					}), false);
				}
			}
		}

		// Token: 0x06007399 RID: 29593 RVA: 0x0023495C File Offset: 0x00232B5C
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
			if (this.pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.disablesNeed == nd))
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
			if (nd.hediffRequiredAny != null)
			{
				bool flag2 = false;
				foreach (HediffDef def in nd.hediffRequiredAny)
				{
					if (this.pawn.health.hediffSet.HasHediff(def, false))
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
			if (nd.defName == "Authority")
			{
				return false;
			}
			if (nd == NeedDefOf.Food)
			{
				return this.pawn.RaceProps.EatsFood;
			}
			return nd != NeedDefOf.Rest || this.pawn.RaceProps.needsRest;
		}

		// Token: 0x0600739A RID: 29594 RVA: 0x00234BE8 File Offset: 0x00232DE8
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

		// Token: 0x0600739B RID: 29595 RVA: 0x00234C34 File Offset: 0x00232E34
		private void RemoveNeed(NeedDef nd)
		{
			Need item = this.TryGetNeed(nd);
			this.needs.Remove(item);
			this.BindDirectNeedFields();
		}

		// Token: 0x04004C59 RID: 19545
		private Pawn pawn;

		// Token: 0x04004C5A RID: 19546
		private List<Need> needs = new List<Need>();

		// Token: 0x04004C5B RID: 19547
		public Need_Mood mood;

		// Token: 0x04004C5C RID: 19548
		public Need_Food food;

		// Token: 0x04004C5D RID: 19549
		public Need_Rest rest;

		// Token: 0x04004C5E RID: 19550
		public Need_Joy joy;

		// Token: 0x04004C5F RID: 19551
		public Need_Beauty beauty;

		// Token: 0x04004C60 RID: 19552
		public Need_RoomSize roomsize;

		// Token: 0x04004C61 RID: 19553
		public Need_Outdoors outdoors;

		// Token: 0x04004C62 RID: 19554
		public Need_Chemical_Any drugsDesire;

		// Token: 0x04004C63 RID: 19555
		public Need_Comfort comfort;

		// Token: 0x04004C64 RID: 19556
		public Need_Authority authority;
	}
}
