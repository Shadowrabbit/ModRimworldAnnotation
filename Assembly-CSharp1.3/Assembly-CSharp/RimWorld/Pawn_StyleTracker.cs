using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E78 RID: 3704
	public class Pawn_StyleTracker : IExposable
	{
		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x0600569C RID: 22172 RVA: 0x001D66FC File Offset: 0x001D48FC
		// (set) Token: 0x0600569D RID: 22173 RVA: 0x001D6704 File Offset: 0x001D4904
		public TattooDef FaceTattoo
		{
			get
			{
				return this.faceTattoo;
			}
			set
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return;
				}
				this.faceTattoo = value;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x0600569E RID: 22174 RVA: 0x001D671A File Offset: 0x001D491A
		// (set) Token: 0x0600569F RID: 22175 RVA: 0x001D6722 File Offset: 0x001D4922
		public TattooDef BodyTattoo
		{
			get
			{
				return this.bodyTattoo;
			}
			set
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return;
				}
				this.bodyTattoo = value;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x060056A0 RID: 22176 RVA: 0x001D6738 File Offset: 0x001D4938
		public bool ShouldSpawnHairFilth
		{
			get
			{
				return (this.nextHairDef != null || this.nextBeardDef != null) && (this.pawn.story.hairDef != this.nextHairDef || this.beardDef != this.nextBeardDef);
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x060056A1 RID: 22177 RVA: 0x001D6777 File Offset: 0x001D4977
		public bool LookChangeDesired
		{
			get
			{
				return this.lookChangeDesired && this.CanDesireLookChange;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x060056A2 RID: 22178 RVA: 0x001D6789 File Offset: 0x001D4989
		public bool CanDesireLookChange
		{
			get
			{
				return ModsConfig.IdeologyActive && this.pawn.IsColonistPlayerControlled && !this.pawn.guest.IsSlave && !this.pawn.IsQuestLodger();
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x001D67C1 File Offset: 0x001D49C1
		public bool HasAnyUnwantedStyleItem
		{
			get
			{
				return this.HasUnwantedHairStyle || this.HasUnwantedBeard || this.HasUnwantedFaceTattoo || this.HasUnwantedBodyTattoo;
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x060056A4 RID: 22180 RVA: 0x001D67E4 File Offset: 0x001D49E4
		public bool HasUnwantedHairStyle
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.pawn.story.hairDef, null);
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x060056A5 RID: 22181 RVA: 0x001D6818 File Offset: 0x001D4A18
		public bool HasUnwantedBeard
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.beardDef, null);
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x060056A6 RID: 22182 RVA: 0x001D6844 File Offset: 0x001D4A44
		public bool HasUnwantedFaceTattoo
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.faceTattoo, null);
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x001D6870 File Offset: 0x001D4A70
		public bool HasUnwantedBodyTattoo
		{
			get
			{
				return !PawnStyleItemChooser.WantsToUseStyle(this.pawn, this.bodyTattoo, null);
			}
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x001D689A File Offset: 0x001D4A9A
		public Pawn_StyleTracker()
		{
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x001D68AD File Offset: 0x001D4AAD
		public Pawn_StyleTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x001D68C8 File Offset: 0x001D4AC8
		public void StyleTrackerTick()
		{
			if (!this.lookChangeDesired && this.CanDesireLookChange && this.pawn.IsHashIntervalTick(2500) && this.HasAnyUnwantedStyleItem && Rand.MTBEventOccurs(20f, 60000f, 2500f))
			{
				this.RequestLookChange();
			}
		}

		// Token: 0x060056AB RID: 22187 RVA: 0x001D691C File Offset: 0x001D4B1C
		public void RequestLookChange()
		{
			Find.LetterStack.ReceiveLetter("LetterWantLookChange".Translate(this.pawn.Named("PAWN")), "LetterWantLookChangeDesc".Translate(this.pawn.Named("PAWN")), LetterDefOf.NeutralEvent, new LookTargets(this.pawn), null, null, new List<ThingDef>
			{
				ThingDefOf.StylingStation
			}, null);
			this.lookChangeDesired = true;
			this.ResetNextStyleChangeAttemptTick();
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x001D6998 File Offset: 0x001D4B98
		public void ResetNextStyleChangeAttemptTick()
		{
			this.nextStyleChangeAttemptTick = Find.TickManager.TicksGame + Pawn_StyleTracker.AutoStyleChangeTicksOffsetRange.RandomInRange;
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x001D69C3 File Offset: 0x001D4BC3
		public void Notify_StyleItemChanged()
		{
			if (!this.HasAnyUnwantedStyleItem)
			{
				this.lookChangeDesired = false;
			}
			this.nextHairDef = null;
			this.nextBeardDef = null;
			this.nextFaceTattooDef = null;
			this.nextBodyTatooDef = null;
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x001D69F0 File Offset: 0x001D4BF0
		public void SetGraphicsDirty()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
				PortraitsCache.SetDirty(this.pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			});
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x001D6A04 File Offset: 0x001D4C04
		public void MakeHairFilth()
		{
			foreach (IntVec3 c in GenRadial.RadialCellsAround(this.pawn.Position, 1f, true))
			{
				if (c.InBounds(this.pawn.Map) && Rand.Value < 0.5f)
				{
					FilthMaker.TryMakeFilth(c, this.pawn.Map, ThingDefOf.Filth_Hair, this.pawn.LabelIndefinite(), Rand.Range(1, 3), FilthSourceFlags.None);
				}
			}
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x001D6AA4 File Offset: 0x001D4CA4
		public void SetupNextLookChangeData(HairDef hair = null, BeardDef beard = null, TattooDef faceTatoo = null, TattooDef bodyTattoo = null)
		{
			this.nextHairDef = hair;
			this.nextBeardDef = beard;
			if (ModsConfig.IdeologyActive)
			{
				this.nextFaceTattooDef = faceTatoo;
				this.nextBodyTatooDef = bodyTattoo;
			}
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x001D6ACA File Offset: 0x001D4CCA
		public void SetupTattoos_NoIdeology()
		{
			this.faceTattoo = TattooDefOf.NoTattoo_Face;
			this.bodyTattoo = TattooDefOf.NoTattoo_Body;
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x001D6AE4 File Offset: 0x001D4CE4
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.lookChangeDesired, "lookChangeDesired", false, false);
			Scribe_Values.Look<int>(ref this.nextStyleChangeAttemptTick, "nextStyleChangeAttemptTick", -99999, false);
			Scribe_Defs.Look<BeardDef>(ref this.beardDef, "beardDef");
			Scribe_Defs.Look<TattooDef>(ref this.faceTattoo, "faceTattoo");
			Scribe_Defs.Look<TattooDef>(ref this.bodyTattoo, "bodyTattoo");
			Scribe_Defs.Look<HairDef>(ref this.nextHairDef, "nextHairDef");
			Scribe_Defs.Look<BeardDef>(ref this.nextBeardDef, "nextBeardDef");
			Scribe_Defs.Look<TattooDef>(ref this.nextFaceTattooDef, "nextFaceTattooDef");
			Scribe_Defs.Look<TattooDef>(ref this.nextBodyTatooDef, "nextBodyTattooDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.beardDef == null)
				{
					this.beardDef = BeardDefOf.NoBeard;
				}
				if (this.faceTattoo == null)
				{
					this.faceTattoo = TattooDefOf.NoTattoo_Face;
				}
				if (this.bodyTattoo == null)
				{
					this.bodyTattoo = TattooDefOf.NoTattoo_Body;
				}
			}
		}

		// Token: 0x04003323 RID: 13091
		public Pawn pawn;

		// Token: 0x04003324 RID: 13092
		public BeardDef beardDef;

		// Token: 0x04003325 RID: 13093
		public int nextStyleChangeAttemptTick = -99999;

		// Token: 0x04003326 RID: 13094
		private TattooDef faceTattoo;

		// Token: 0x04003327 RID: 13095
		private TattooDef bodyTattoo;

		// Token: 0x04003328 RID: 13096
		private bool lookChangeDesired;

		// Token: 0x04003329 RID: 13097
		public HairDef nextHairDef;

		// Token: 0x0400332A RID: 13098
		public BeardDef nextBeardDef;

		// Token: 0x0400332B RID: 13099
		public TattooDef nextFaceTattooDef;

		// Token: 0x0400332C RID: 13100
		public TattooDef nextBodyTatooDef;

		// Token: 0x0400332D RID: 13101
		private static readonly IntRange AutoStyleChangeTicksOffsetRange = new IntRange(15000, 30000);

		// Token: 0x0400332E RID: 13102
		private const float StyleChangeMTBDays = 20f;

		// Token: 0x0400332F RID: 13103
		private const int LookChangeCheckInterval = 2500;
	}
}
