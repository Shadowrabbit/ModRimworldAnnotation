using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E23 RID: 3619
	public class Pawn_StoryTracker : IExposable
	{
		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x001C55A6 File Offset: 0x001C37A6
		// (set) Token: 0x060053AB RID: 21419 RVA: 0x001C55BD File Offset: 0x001C37BD
		public string Title
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				return this.TitleDefault;
			}
			set
			{
				this.title = null;
				if (value != this.Title && !value.NullOrEmpty())
				{
					this.title = value;
				}
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x001C55E3 File Offset: 0x001C37E3
		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x001C55F0 File Offset: 0x001C37F0
		public string TitleDefault
		{
			get
			{
				if (this.adulthood != null)
				{
					return this.adulthood.TitleFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleFor(this.pawn.gender);
				}
				return "";
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x060053AE RID: 21422 RVA: 0x001C5640 File Offset: 0x001C3840
		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x060053AF RID: 21423 RVA: 0x001C5650 File Offset: 0x001C3850
		public string TitleShort
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				if (this.adulthood != null)
				{
					return this.adulthood.TitleShortFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleShortFor(this.pawn.gender);
				}
				return "";
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x060053B0 RID: 21424 RVA: 0x001C56AF File Offset: 0x001C38AF
		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x060053B1 RID: 21425 RVA: 0x001C56BC File Offset: 0x001C38BC
		public Color SkinColor
		{
			get
			{
				if (this.skinColorOverride == null)
				{
					return PawnSkinColors.GetSkinColor(this.melanin);
				}
				return this.skinColorOverride.Value;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x060053B2 RID: 21426 RVA: 0x001C56E2 File Offset: 0x001C38E2
		public bool SkinColorOverriden
		{
			get
			{
				return this.skinColorOverride != null;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x060053B3 RID: 21427 RVA: 0x001C56EF File Offset: 0x001C38EF
		public IEnumerable<Backstory> AllBackstories
		{
			get
			{
				if (this.childhood != null)
				{
					yield return this.childhood;
				}
				if (this.adulthood != null)
				{
					yield return this.adulthood;
				}
				yield break;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x060053B4 RID: 21428 RVA: 0x001C5700 File Offset: 0x001C3900
		public string HeadGraphicPath
		{
			get
			{
				if (this.headGraphicPath == null)
				{
					this.headGraphicPath = GraphicDatabaseHeadRecords.GetHeadRandom(this.pawn.gender, this.pawn.story.SkinColor, this.pawn.story.crownType, this.pawn.story.SkinColorOverriden).GraphicPath;
				}
				return this.headGraphicPath;
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x060053B5 RID: 21429 RVA: 0x001C5768 File Offset: 0x001C3968
		public WorkTags DisabledWorkTagsBackstoryAndTraits
		{
			get
			{
				WorkTags workTags = WorkTags.None;
				if (this.childhood != null)
				{
					workTags |= this.childhood.workDisables;
				}
				if (this.adulthood != null)
				{
					workTags |= this.adulthood.workDisables;
				}
				for (int i = 0; i < this.traits.allTraits.Count; i++)
				{
					workTags |= this.traits.allTraits[i].def.disabledWorkTags;
				}
				return workTags;
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x060053B6 RID: 21430 RVA: 0x001C57E0 File Offset: 0x001C39E0
		public float VoicePitchFactor
		{
			get
			{
				return Pawn_StoryTracker.VoicePitchFactorRange.RandomInRangeSeeded(this.pawn.thingIDNumber);
			}
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x001C5805 File Offset: 0x001C3A05
		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x001C582C File Offset: 0x001C3A2C
		public void ExposeData()
		{
			string text = (this.childhood != null) ? this.childhood.identifier : null;
			Scribe_Values.Look<string>(ref text, "childhood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text.NullOrEmpty() && !BackstoryDatabase.TryGetWithIdentifier(text, out this.childhood, true))
			{
				Log.Error("Couldn't load child backstory with identifier " + text + ". Giving random.");
				this.childhood = BackstoryDatabase.RandomBackstory(BackstorySlot.Childhood);
			}
			string text2 = (this.adulthood != null) ? this.adulthood.identifier : null;
			Scribe_Values.Look<string>(ref text2, "adulthood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text2.NullOrEmpty() && !BackstoryDatabase.TryGetWithIdentifier(text2, out this.adulthood, true))
			{
				Log.Error("Couldn't load adult backstory with identifier " + text2 + ". Giving random.");
				this.adulthood = BackstoryDatabase.RandomBackstory(BackstorySlot.Adulthood);
			}
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Values.Look<CrownType>(ref this.crownType, "crownType", CrownType.Undefined, false);
			Scribe_Values.Look<string>(ref this.headGraphicPath, "headGraphicPath", null, false);
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<Color?>(ref this.skinColorOverride, "skinColorOverride", null, false);
			Scribe_Values.Look<float>(ref this.melanin, "melanin", 0f, false);
			Scribe_Deep.Look<TraitSet>(ref this.traits, "traits", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.birthLastName, "birthLastName", null, false);
			Scribe_Values.Look<Color?>(ref this.favoriteColor, "favoriteColor", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.birthLastName == null && this.pawn.Name is NameTriple)
				{
					this.birthLastName = ((NameTriple)this.pawn.Name).Last;
				}
				if (this.hairDef == null)
				{
					this.hairDef = DefDatabase<HairDef>.AllDefs.RandomElement<HairDef>();
				}
			}
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x001C5A43 File Offset: 0x001C3C43
		public Backstory GetBackstory(BackstorySlot slot)
		{
			if (slot == BackstorySlot.Childhood)
			{
				return this.childhood;
			}
			return this.adulthood;
		}

		// Token: 0x04003131 RID: 12593
		private Pawn pawn;

		// Token: 0x04003132 RID: 12594
		public Backstory childhood;

		// Token: 0x04003133 RID: 12595
		public Backstory adulthood;

		// Token: 0x04003134 RID: 12596
		public float melanin;

		// Token: 0x04003135 RID: 12597
		public Color hairColor = Color.white;

		// Token: 0x04003136 RID: 12598
		public Color? skinColorOverride;

		// Token: 0x04003137 RID: 12599
		public CrownType crownType;

		// Token: 0x04003138 RID: 12600
		public BodyTypeDef bodyType;

		// Token: 0x04003139 RID: 12601
		private string headGraphicPath;

		// Token: 0x0400313A RID: 12602
		public HairDef hairDef;

		// Token: 0x0400313B RID: 12603
		public TraitSet traits;

		// Token: 0x0400313C RID: 12604
		public string title;

		// Token: 0x0400313D RID: 12605
		public string birthLastName;

		// Token: 0x0400313E RID: 12606
		public Color? favoriteColor;

		// Token: 0x0400313F RID: 12607
		private static readonly FloatRange VoicePitchFactorRange = new FloatRange(0.85f, 1.15f);
	}
}
