using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001513 RID: 5395
	public class Pawn_NativeVerbs : IVerbOwner, IExposable
	{
		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06007457 RID: 29783 RVA: 0x0004E788 File Offset: 0x0004C988
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

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06007458 RID: 29784 RVA: 0x0004E7AF File Offset: 0x0004C9AF
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

		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x06007459 RID: 29785 RVA: 0x0004E7D6 File Offset: 0x0004C9D6
		VerbTracker IVerbOwner.VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x0600745A RID: 29786 RVA: 0x0004E7DE File Offset: 0x0004C9DE
		List<VerbProperties> IVerbOwner.VerbProperties
		{
			get
			{
				this.CheckCreateVerbProperties();
				return this.cachedVerbProperties;
			}
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x0600745B RID: 29787 RVA: 0x0000C32E File Offset: 0x0000A52E
		List<Tool> IVerbOwner.Tools
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x0600745C RID: 29788 RVA: 0x0004935B File Offset: 0x0004755B
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x0600745D RID: 29789 RVA: 0x0004E7EC File Offset: 0x0004C9EC
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "NativeVerbs_" + this.pawn.ThingID;
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x0004E803 File Offset: 0x0004CA03
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.pawn;
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x0600745F RID: 29791 RVA: 0x0004E80E File Offset: 0x0004CA0E
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06007460 RID: 29792 RVA: 0x0004E816 File Offset: 0x0004CA16
		private Thing ConstantCaster { get; }

		// Token: 0x06007461 RID: 29793 RVA: 0x0004E81E File Offset: 0x0004CA1E
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06007462 RID: 29794 RVA: 0x0004E839 File Offset: 0x0004CA39
		public void NativeVerbsTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06007463 RID: 29795 RVA: 0x00236ED0 File Offset: 0x002350D0
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
				}), 76453432, false);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.IgniteVerb.TryStartCastOn(target, false, true);
		}

		// Token: 0x06007464 RID: 29796 RVA: 0x00236F44 File Offset: 0x00235144
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
				}), 935137531, false);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.BeatFireVerb.TryStartCastOn(targetFire, false, true);
		}

		// Token: 0x06007465 RID: 29797 RVA: 0x0004E846 File Offset: 0x0004CA46
		public void ExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06007466 RID: 29798 RVA: 0x00236FB8 File Offset: 0x002351B8
		private void CheckCreateVerbProperties()
		{
			if (this.cachedVerbProperties != null)
			{
				return;
			}
			if (this.pawn.RaceProps.intelligence >= Intelligence.ToolUser)
			{
				this.cachedVerbProperties = new List<VerbProperties>();
				this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
				if (!this.pawn.RaceProps.IsMechanoid)
				{
					this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite));
				}
			}
		}

		// Token: 0x04004CBF RID: 19647
		private Pawn pawn;

		// Token: 0x04004CC0 RID: 19648
		public VerbTracker verbTracker;

		// Token: 0x04004CC1 RID: 19649
		private Verb_BeatFire cachedBeatFireVerb;

		// Token: 0x04004CC2 RID: 19650
		private Verb_Ignite cachedIgniteVerb;

		// Token: 0x04004CC3 RID: 19651
		private List<VerbProperties> cachedVerbProperties;
	}
}
