using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FB RID: 4347
	public class CompArt : ThingComp
	{
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06006832 RID: 26674 RVA: 0x00233EC8 File Offset: 0x002320C8
		public TaggedString AuthorName
		{
			get
			{
				if (this.authorNameInt.NullOrEmpty())
				{
					return "UnknownLower".Translate().CapitalizeFirst();
				}
				return this.authorNameInt.Resolve();
			}
		}

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06006833 RID: 26675 RVA: 0x00233F08 File Offset: 0x00232108
		public string Title
		{
			get
			{
				if (this.parent.StyleSourcePrecept != null)
				{
					return this.parent.StyleSourcePrecept.LabelCap;
				}
				if (this.titleInt.NullOrEmpty())
				{
					Log.Error("CompArt got title but it wasn't configured.");
					this.titleInt = "Error";
				}
				return this.titleInt;
			}
		}

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x06006834 RID: 26676 RVA: 0x00233F65 File Offset: 0x00232165
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x06006835 RID: 26677 RVA: 0x00233F70 File Offset: 0x00232170
		public bool CanShowArt
		{
			get
			{
				if (this.Props.mustBeFullGrave)
				{
					Building_Grave building_Grave = this.parent as Building_Grave;
					if (building_Grave == null || !building_Grave.HasCorpse)
					{
						return false;
					}
				}
				QualityCategory qualityCategory;
				return !this.parent.TryGetQuality(out qualityCategory) || qualityCategory >= this.Props.minQualityForArtistic;
			}
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x06006836 RID: 26678 RVA: 0x00233FC5 File Offset: 0x002321C5
		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06006837 RID: 26679 RVA: 0x00233FD0 File Offset: 0x002321D0
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x00233FDD File Offset: 0x002321DD
		public override string TransformLabel(string label)
		{
			if (this.Active && this.parent.StyleSourcePrecept != null)
			{
				return this.Title;
			}
			return base.TransformLabel(label);
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x00234002 File Offset: 0x00232202
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x0023400C File Offset: 0x0023220C
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x00234018 File Offset: 0x00232218
		private void InitializeArt(Thing relatedThing, ArtGenerationContext source)
		{
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
			if (this.CanShowArt)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (relatedThing != null)
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArtConcerning(relatedThing);
					}
					else
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArt(source);
					}
				}
				else
				{
					this.taleRef = TaleReference.Taleless;
				}
				this.titleInt = this.GenerateTitle(source);
				return;
			}
			this.titleInt = null;
			this.taleRef = null;
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x002340A9 File Offset: 0x002322A9
		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.NameFullColored;
			}
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x002340BF File Offset: 0x002322BF
		public void Clear()
		{
			this.authorNameInt = null;
			this.titleInt = null;
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x002340F4 File Offset: 0x002322F4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<TaggedString>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<TaggedString>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", Array.Empty<object>());
		}

		// Token: 0x0600683F RID: 26687 RVA: 0x0023414C File Offset: 0x0023234C
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "Author".Translate() + ": " + this.AuthorName + ("\n" + "Title".Translate() + ": " + this.Title);
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x002341BA File Offset: 0x002323BA
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06006841 RID: 26689 RVA: 0x002341E0 File Offset: 0x002323E0
		public override string GetDescriptionPart()
		{
			if (!this.Active)
			{
				return null;
			}
			return "" + this.Title + "\n\n" + this.GenerateImageDescription() + "\n\n" + ("Author".Translate() + ": " + this.AuthorName);
		}

		// Token: 0x06006842 RID: 26690 RVA: 0x00234254 File Offset: 0x00232454
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x00234261 File Offset: 0x00232461
		public TaggedString GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x002342A0 File Offset: 0x002324A0
		private string GenerateTitle(ArtGenerationContext context)
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + this.parent);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker));
		}

		// Token: 0x04003A90 RID: 14992
		private TaggedString authorNameInt = null;

		// Token: 0x04003A91 RID: 14993
		private TaggedString titleInt = null;

		// Token: 0x04003A92 RID: 14994
		private TaleReference taleRef;
	}
}
