using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BB RID: 5307
	public class Pawn_StoryTracker : IExposable
	{
		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x06007244 RID: 29252 RVA: 0x0004CD70 File Offset: 0x0004AF70
		// (set) Token: 0x06007245 RID: 29253 RVA: 0x0004CD87 File Offset: 0x0004AF87
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

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x06007246 RID: 29254 RVA: 0x0004CDAD File Offset: 0x0004AFAD
		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x06007247 RID: 29255 RVA: 0x0022F2BC File Offset: 0x0022D4BC
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

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x06007248 RID: 29256 RVA: 0x0004CDBA File Offset: 0x0004AFBA
		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06007249 RID: 29257 RVA: 0x0022F30C File Offset: 0x0022D50C
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

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x0600724A RID: 29258 RVA: 0x0004CDC7 File Offset: 0x0004AFC7
		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x0600724B RID: 29259 RVA: 0x0004CDD4 File Offset: 0x0004AFD4
		public Color SkinColor
		{
			get
			{
				return PawnSkinColors.GetSkinColor(this.melanin);
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x0600724C RID: 29260 RVA: 0x0004CDE1 File Offset: 0x0004AFE1
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

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x0600724D RID: 29261 RVA: 0x0022F36C File Offset: 0x0022D56C
		public string HeadGraphicPath
		{
			get
			{
				if (this.headGraphicPath == null)
				{
					this.headGraphicPath = GraphicDatabaseHeadRecords.GetHeadRandom(this.pawn.gender, this.pawn.story.SkinColor, this.pawn.story.crownType).GraphicPath;
				}
				return this.headGraphicPath;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x0600724E RID: 29262 RVA: 0x0022F3C4 File Offset: 0x0022D5C4
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

		// Token: 0x0600724F RID: 29263 RVA: 0x0004CDF1 File Offset: 0x0004AFF1
		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

		// Token: 0x06007250 RID: 29264 RVA: 0x0022F43C File Offset: 0x0022D63C
		public void ExposeData()
		{
			string text = (this.childhood != null) ? this.childhood.identifier : null;
			Scribe_Values.Look<string>(ref text, "childhood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text.NullOrEmpty() && !BackstoryDatabase.TryGetWithIdentifier(text, out this.childhood, true))
			{
				Log.Error("Couldn't load child backstory with identifier " + text + ". Giving random.", false);
				this.childhood = BackstoryDatabase.RandomBackstory(BackstorySlot.Childhood);
			}
			string text2 = (this.adulthood != null) ? this.adulthood.identifier : null;
			Scribe_Values.Look<string>(ref text2, "adulthood", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && !text2.NullOrEmpty() && !BackstoryDatabase.TryGetWithIdentifier(text2, out this.adulthood, true))
			{
				Log.Error("Couldn't load adult backstory with identifier " + text2 + ". Giving random.", false);
				this.adulthood = BackstoryDatabase.RandomBackstory(BackstorySlot.Adulthood);
			}
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Values.Look<CrownType>(ref this.crownType, "crownType", CrownType.Undefined, false);
			Scribe_Values.Look<string>(ref this.headGraphicPath, "headGraphicPath", null, false);
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<float>(ref this.melanin, "melanin", 0f, false);
			Scribe_Deep.Look<TraitSet>(ref this.traits, "traits", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.birthLastName, "birthLastName", null, false);
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

		// Token: 0x06007251 RID: 29265 RVA: 0x0004CE17 File Offset: 0x0004B017
		public Backstory GetBackstory(BackstorySlot slot)
		{
			if (slot == BackstorySlot.Childhood)
			{
				return this.childhood;
			}
			return this.adulthood;
		}

		// Token: 0x04004B3E RID: 19262
		private Pawn pawn;

		// Token: 0x04004B3F RID: 19263
		public Backstory childhood;

		// Token: 0x04004B40 RID: 19264
		public Backstory adulthood;

		// Token: 0x04004B41 RID: 19265
		public float melanin;

		// Token: 0x04004B42 RID: 19266
		public Color hairColor = Color.white;

		// Token: 0x04004B43 RID: 19267
		public CrownType crownType;

		// Token: 0x04004B44 RID: 19268
		public BodyTypeDef bodyType;

		// Token: 0x04004B45 RID: 19269
		private string headGraphicPath;

		// Token: 0x04004B46 RID: 19270
		public HairDef hairDef;

		// Token: 0x04004B47 RID: 19271
		public TraitSet traits;

		// Token: 0x04004B48 RID: 19272
		public string title;

		// Token: 0x04004B49 RID: 19273
		public string birthLastName;
	}
}
