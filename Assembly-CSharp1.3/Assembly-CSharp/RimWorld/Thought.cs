using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E92 RID: 3730
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x0600579B RID: 22427
		public abstract int CurStageIndex { get; }

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x0600579C RID: 22428 RVA: 0x001DD3F9 File Offset: 0x001DB5F9
		public virtual int DurationTicks
		{
			get
			{
				return this.def.DurationTicks;
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x0600579D RID: 22429 RVA: 0x001DD406 File Offset: 0x001DB606
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x0600579E RID: 22430 RVA: 0x001DD41E File Offset: 0x001DB61E
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x0600579F RID: 22431 RVA: 0x001DD42C File Offset: 0x001DB62C
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

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x060057A0 RID: 22432 RVA: 0x001DD48D File Offset: 0x001DB68D
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x001DD49A File Offset: 0x001DB69A
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

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x060057A2 RID: 22434 RVA: 0x001DD4D8 File Offset: 0x001DB6D8
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
				if (ModsConfig.IdeologyActive && this.sourcePrecept != null)
				{
					text += this.CausedByBeliefInPrecept;
				}
				string text2 = ThoughtUtility.ThoughtNullifiedMessage(this.pawn, this.def);
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + "\n\n(" + text2 + ")";
				}
				return text;
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x060057A3 RID: 22435 RVA: 0x001DD610 File Offset: 0x001DB810
		public string CausedByBeliefInPrecept
		{
			get
			{
				return "\n\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + this.sourcePrecept.ideo.name + " (" + this.sourcePrecept.def.issue.LabelCap + ": " + this.sourcePrecept.def.LabelCap + ")";
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x060057A4 RID: 22436 RVA: 0x001DD6BB File Offset: 0x001DB8BB
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

		// Token: 0x060057A5 RID: 22437 RVA: 0x001DD6F4 File Offset: 0x001DB8F4
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Precept>(ref this.sourcePrecept, "sourcePrecept", false);
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x001DD718 File Offset: 0x001DB918
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
				}));
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

		// Token: 0x060057A7 RID: 22439 RVA: 0x001DD7D8 File Offset: 0x001DB9D8
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def && this.CurStageIndex == other.CurStageIndex;
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Init()
		{
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x001DD7F8 File Offset: 0x001DB9F8
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}

		// Token: 0x040033BF RID: 13247
		public Pawn pawn;

		// Token: 0x040033C0 RID: 13248
		public ThoughtDef def;

		// Token: 0x040033C1 RID: 13249
		public Precept sourcePrecept;

		// Token: 0x040033C2 RID: 13250
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x040033C3 RID: 13251
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);
	}
}
