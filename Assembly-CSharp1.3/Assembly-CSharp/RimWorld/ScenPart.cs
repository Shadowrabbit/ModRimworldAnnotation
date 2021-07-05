using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001007 RID: 4103
	public abstract class ScenPart : IExposable
	{
		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x060060A1 RID: 24737 RVA: 0x0020E665 File Offset: 0x0020C865
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x060060A2 RID: 24738 RVA: 0x0020E66C File Offset: 0x0020C86C
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x060060A3 RID: 24739 RVA: 0x0020E67E File Offset: 0x0020C87E
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x060060A4 RID: 24740 RVA: 0x0020E690 File Offset: 0x0020C890
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x060060A5 RID: 24741 RVA: 0x0020E6A4 File Offset: 0x0020C8A4
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x060060A6 RID: 24742 RVA: 0x0020E6B1 File Offset: 0x0020C8B1
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x060060A7 RID: 24743 RVA: 0x0020E6C0 File Offset: 0x0020C8C0
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x060060A8 RID: 24744 RVA: 0x0020E6CD File Offset: 0x0020C8CD
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x060060A9 RID: 24745 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Randomize()
		{
		}

		// Token: 0x060060AA RID: 24746 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x060060AB RID: 24747 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x060060AC RID: 24748 RVA: 0x0020E6D6 File Offset: 0x0020C8D6
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x060060AD RID: 24749 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x060060AE RID: 24750 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x060060AF RID: 24751 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x060060B0 RID: 24752 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x060060B1 RID: 24753 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreConfigure()
		{
		}

		// Token: 0x060060B2 RID: 24754 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x060060B3 RID: 24755 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostIdeoChosen()
		{
		}

		// Token: 0x060060B4 RID: 24756 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x060060B5 RID: 24757 RVA: 0x0020E6DF File Offset: 0x0020C8DF
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x060060B6 RID: 24758 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x060060B7 RID: 24759 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostGameStart()
		{
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Tick()
		{
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x0020E6E8 File Offset: 0x0020C8E8
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.def == null)
			{
				yield return base.GetType().ToString() + " has null def.";
			}
			yield break;
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x0020E6F8 File Offset: 0x0020C8F8
		public virtual bool HasNullDefs()
		{
			return this.def == null;
		}

		// Token: 0x0400373F RID: 14143
		[TranslationHandle]
		public ScenPartDef def;

		// Token: 0x04003740 RID: 14144
		public bool visible = true;

		// Token: 0x04003741 RID: 14145
		public bool summarized;
	}
}
