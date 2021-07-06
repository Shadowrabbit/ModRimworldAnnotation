using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200178E RID: 6030
	public class CompArt : ThingComp
	{
		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x06008518 RID: 34072 RVA: 0x00275AF4 File Offset: 0x00273CF4
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

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x06008519 RID: 34073 RVA: 0x00059292 File Offset: 0x00057492
		public string Title
		{
			get
			{
				if (this.titleInt.NullOrEmpty())
				{
					Log.Error("CompArt got title but it wasn't configured.", false);
					this.titleInt = "Error";
				}
				return this.titleInt;
			}
		}

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x0600851A RID: 34074 RVA: 0x000592C7 File Offset: 0x000574C7
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x0600851B RID: 34075 RVA: 0x00275B34 File Offset: 0x00273D34
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

		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x0600851C RID: 34076 RVA: 0x000592CF File Offset: 0x000574CF
		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x0600851D RID: 34077 RVA: 0x000592DA File Offset: 0x000574DA
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x0600851E RID: 34078 RVA: 0x000592E7 File Offset: 0x000574E7
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		// Token: 0x0600851F RID: 34079 RVA: 0x000592F1 File Offset: 0x000574F1
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x06008520 RID: 34080 RVA: 0x00275B8C File Offset: 0x00273D8C
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
				this.titleInt = this.GenerateTitle();
				return;
			}
			this.titleInt = null;
			this.taleRef = null;
		}

		// Token: 0x06008521 RID: 34081 RVA: 0x000592FB File Offset: 0x000574FB
		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.NameFullColored;
			}
		}

		// Token: 0x06008522 RID: 34082 RVA: 0x00059311 File Offset: 0x00057511
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

		// Token: 0x06008523 RID: 34083 RVA: 0x00275C1C File Offset: 0x00273E1C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<TaggedString>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<TaggedString>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", Array.Empty<object>());
		}

		// Token: 0x06008524 RID: 34084 RVA: 0x00275C74 File Offset: 0x00273E74
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "Author".Translate() + ": " + this.AuthorName + ("\n" + "Title".Translate() + ": " + this.Title);
		}

		// Token: 0x06008525 RID: 34085 RVA: 0x00059345 File Offset: 0x00057545
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06008526 RID: 34086 RVA: 0x00275CE4 File Offset: 0x00273EE4
		public override string GetDescriptionPart()
		{
			if (!this.Active)
			{
				return null;
			}
			return "" + this.Title + "\n\n" + this.GenerateImageDescription() + "\n\n" + ("Author".Translate() + ": " + this.AuthorName);
		}

		// Token: 0x06008527 RID: 34087 RVA: 0x00059369 File Offset: 0x00057569
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x06008528 RID: 34088 RVA: 0x00059376 File Offset: 0x00057576
		public TaggedString GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

		// Token: 0x06008529 RID: 34089 RVA: 0x00275D58 File Offset: 0x00273F58
		private string GenerateTitle()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker));
		}

		// Token: 0x04005618 RID: 22040
		private TaggedString authorNameInt = null;

		// Token: 0x04005619 RID: 22041
		private TaggedString titleInt = null;

		// Token: 0x0400561A RID: 22042
		private TaleReference taleRef;
	}
}
