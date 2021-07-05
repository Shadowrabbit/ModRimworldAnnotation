using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6E RID: 3694
	public class Pawn_NativeVerbs : IVerbOwner, IExposable
	{
		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x060055CF RID: 21967 RVA: 0x001D1234 File Offset: 0x001CF434
		public Verb_BeatFire BeatFireVerb
		{
			get
			{
				if (this.cachedBeatFireVerb == null)
				{
					this.cachedBeatFireVerb = (Verb_BeatFire)this.verbTracker.GetVerb(VerbCategory.BeatFire);
				}
				return this.cachedBeatFireVerb;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x060055D0 RID: 21968 RVA: 0x001D125B File Offset: 0x001CF45B
		public Verb_Ignite IgniteVerb
		{
			get
			{
				if (this.cachedIgniteVerb == null)
				{
					this.cachedIgniteVerb = (Verb_Ignite)this.verbTracker.GetVerb(VerbCategory.Ignite);
				}
				return this.cachedIgniteVerb;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x060055D1 RID: 21969 RVA: 0x001D1282 File Offset: 0x001CF482
		VerbTracker IVerbOwner.VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x060055D2 RID: 21970 RVA: 0x001D128A File Offset: 0x001CF48A
		List<VerbProperties> IVerbOwner.VerbProperties
		{
			get
			{
				this.CheckCreateVerbProperties();
				return this.cachedVerbProperties;
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x060055D3 RID: 21971 RVA: 0x00002688 File Offset: 0x00000888
		List<Tool> IVerbOwner.Tools
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x060055D4 RID: 21972 RVA: 0x001A4583 File Offset: 0x001A2783
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x001D1298 File Offset: 0x001CF498
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "NativeVerbs_" + this.pawn.ThingID;
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x001D12AF File Offset: 0x001CF4AF
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.pawn;
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x060055D7 RID: 21975 RVA: 0x001D12BA File Offset: 0x001CF4BA
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x060055D8 RID: 21976 RVA: 0x001D12C2 File Offset: 0x001CF4C2
		private Thing ConstantCaster { get; }

		// Token: 0x060055D9 RID: 21977 RVA: 0x001D12CA File Offset: 0x001CF4CA
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x001D12E5 File Offset: 0x001CF4E5
		public void NativeVerbsTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x001D12F4 File Offset: 0x001CF4F4
		public bool TryStartIgnite(Thing target)
		{
			if (this.IgniteVerb == null)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					this.pawn,
					" tried to ignite ",
					target,
					" but has no ignite verb."
				}), 76453432);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.IgniteVerb.TryStartCastOn(target, false, true, false);
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x001D1368 File Offset: 0x001CF568
		public bool TryBeatFire(Fire targetFire)
		{
			if (this.BeatFireVerb == null)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					this.pawn,
					" tried to beat fire ",
					targetFire,
					" but has no beat fire verb."
				}), 935137531);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.BeatFireVerb.TryStartCastOn(targetFire, false, true, false);
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x001D13D9 File Offset: 0x001CF5D9
		public void ExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x001D13FC File Offset: 0x001CF5FC
		private void CheckCreateVerbProperties()
		{
			if (this.cachedVerbProperties != null)
			{
				return;
			}
			if (this.pawn.RaceProps.intelligence >= Intelligence.ToolUser || this.pawn.RaceProps.giveNonToolUserBeatFireVerb)
			{
				this.cachedVerbProperties = new List<VerbProperties>();
				this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
				if (!this.pawn.RaceProps.IsMechanoid && this.pawn.RaceProps.intelligence >= Intelligence.ToolUser)
				{
					this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite));
				}
			}
		}

		// Token: 0x040032BA RID: 12986
		private Pawn pawn;

		// Token: 0x040032BB RID: 12987
		public VerbTracker verbTracker;

		// Token: 0x040032BC RID: 12988
		private Verb_BeatFire cachedBeatFireVerb;

		// Token: 0x040032BD RID: 12989
		private Verb_Ignite cachedIgniteVerb;

		// Token: 0x040032BE RID: 12990
		private List<VerbProperties> cachedVerbProperties;
	}
}
