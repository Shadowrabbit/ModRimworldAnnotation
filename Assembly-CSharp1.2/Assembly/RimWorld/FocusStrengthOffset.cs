using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E5 RID: 6117
	public abstract class FocusStrengthOffset
	{
		// Token: 0x06008764 RID: 34660 RVA: 0x0000A713 File Offset: 0x00008913
		public virtual string GetExplanation(Thing parent)
		{
			return "";
		}

		// Token: 0x06008765 RID: 34661 RVA: 0x0000A713 File Offset: 0x00008913
		public virtual string GetExplanationAbstract(ThingDef def = null)
		{
			return "";
		}

		// Token: 0x06008766 RID: 34662 RVA: 0x0000A713 File Offset: 0x00008913
		public virtual string InspectStringExtra(Thing parent, Pawn user = null)
		{
			return "";
		}

		// Token: 0x06008767 RID: 34663 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float GetOffset(Thing parent, Pawn user = null)
		{
			return 0f;
		}

		// Token: 0x06008768 RID: 34664 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanApply(Thing parent, Pawn user = null)
		{
			return true;
		}

		// Token: 0x06008769 RID: 34665 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
		}

		// Token: 0x0600876A RID: 34666 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float MinOffset(Thing parent = null)
		{
			return 0f;
		}

		// Token: 0x0600876B RID: 34667 RVA: 0x0005AE5B File Offset: 0x0005905B
		public virtual float MaxOffset(Thing parent = null)
		{
			return this.offset;
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x0600876C RID: 34668 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool DependsOnPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x0600876D RID: 34669 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool NeedsToBeSpawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600876E RID: 34670 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x040056FD RID: 22269
		public float offset;
	}
}
