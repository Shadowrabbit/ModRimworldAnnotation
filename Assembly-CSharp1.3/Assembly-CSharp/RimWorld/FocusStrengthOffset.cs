using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001151 RID: 4433
	public abstract class FocusStrengthOffset
	{
		// Token: 0x06006A87 RID: 27271 RVA: 0x00014F75 File Offset: 0x00013175
		public virtual string GetExplanation(Thing parent)
		{
			return "";
		}

		// Token: 0x06006A88 RID: 27272 RVA: 0x00014F75 File Offset: 0x00013175
		public virtual string GetExplanationAbstract(ThingDef def = null)
		{
			return "";
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x00014F75 File Offset: 0x00013175
		public virtual string InspectStringExtra(Thing parent, Pawn user = null)
		{
			return "";
		}

		// Token: 0x06006A8A RID: 27274 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float GetOffset(Thing parent, Pawn user = null)
		{
			return 0f;
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanApply(Thing parent, Pawn user = null)
		{
			return true;
		}

		// Token: 0x06006A8C RID: 27276 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
		}

		// Token: 0x06006A8D RID: 27277 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float MinOffset(Thing parent = null)
		{
			return 0f;
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x0023D32B File Offset: 0x0023B52B
		public virtual float MaxOffset(Thing parent = null)
		{
			return this.offset;
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06006A8F RID: 27279 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool DependsOnPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06006A90 RID: 27280 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool NeedsToBeSpawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x04003B57 RID: 15191
		public float offset;
	}
}
