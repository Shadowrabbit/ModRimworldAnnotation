using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001563 RID: 5475
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x060076B4 RID: 30388
		public abstract int CurStageIndex { get; }

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x060076B5 RID: 30389 RVA: 0x0005012A File Offset: 0x0004E32A
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x060076B6 RID: 30390 RVA: 0x00050142 File Offset: 0x0004E342
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x060076B7 RID: 30391 RVA: 0x00242608 File Offset: 0x00240808
		public virtual string LabelCap
		{
			get
			{
				if (this.def.Worker == null)
				{
					return this.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN"));
				}
				return this.def.Worker.PostProcessLabel(this.pawn, this.CurStage.LabelCap);
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x060076B8 RID: 30392 RVA: 0x0005014F File Offset: 0x0004E34F
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x060076B9 RID: 30393 RVA: 0x0005015C File Offset: 0x0004E35C
		public virtual string LabelCapSocial
		{
			get
			{
				if (this.CurStage.labelSocial != null)
				{
					return this.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"));
				}
				return this.LabelCap;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x060076BA RID: 30394 RVA: 0x0024266C File Offset: 0x0024086C
		public virtual string Description
		{
			get
			{
				string text = this.CurStage.description;
				if (text == null)
				{
					text = this.def.description;
				}
				Thought_Memory thought_Memory;
				ISocialThought socialThought;
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessDescription(this.pawn, text);
				}
				else if ((thought_Memory = (this as Thought_Memory)) != null && thought_Memory.otherPawn != null)
				{
					text = text.Formatted(this.pawn.Named("PAWN"), thought_Memory.otherPawn.Named("OTHERPAWN"));
				}
				else if ((socialThought = (this as ISocialThought)) != null && socialThought.OtherPawn() != null)
				{
					text = text.Formatted(this.pawn.Named("PAWN"), socialThought.OtherPawn().Named("OTHERPAWN"));
				}
				else
				{
					text = text.Formatted(this.pawn.Named("PAWN"));
				}
				string text2 = ThoughtUtility.ThoughtNullifiedMessage(this.pawn, this.def);
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + "\n\n(" + text2 + ")";
				}
				return text;
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x060076BB RID: 30395 RVA: 0x00050197 File Offset: 0x0004E397
		public Texture2D Icon
		{
			get
			{
				if (this.def.Icon != null)
				{
					return this.def.Icon;
				}
				if (this.MoodOffset() > 0f)
				{
					return Thought.DefaultGoodIcon;
				}
				return Thought.DefaultBadIcon;
			}
		}

		// Token: 0x060076BC RID: 30396 RVA: 0x000501D0 File Offset: 0x0004E3D0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
		}

		// Token: 0x060076BD RID: 30397 RVA: 0x00242788 File Offset: 0x00240988
		public virtual float MoodOffset()
		{
			if (this.CurStage == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"CurStage is null while ShouldDiscard is false on ",
					this.def.defName,
					" for ",
					this.pawn
				}), false);
				return 0f;
			}
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = this.BaseMoodOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true);
			}
			if (this.def.Worker != null)
			{
				num *= this.def.Worker.MoodMultiplier(this.pawn);
			}
			return num;
		}

		// Token: 0x060076BE RID: 30398 RVA: 0x000501E2 File Offset: 0x0004E3E2
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def;
		}

		// Token: 0x060076BF RID: 30399 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Init()
		{
		}

		// Token: 0x060076C0 RID: 30400 RVA: 0x000501F2 File Offset: 0x0004E3F2
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}

		// Token: 0x04004E58 RID: 20056
		public Pawn pawn;

		// Token: 0x04004E59 RID: 20057
		public ThoughtDef def;

		// Token: 0x04004E5A RID: 20058
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x04004E5B RID: 20059
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);
	}
}
