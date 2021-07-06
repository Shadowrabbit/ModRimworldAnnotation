using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015F4 RID: 5620
	public abstract class ScenPart : IExposable
	{
		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06007A19 RID: 31257 RVA: 0x000522BA File Offset: 0x000504BA
		public static float RowHeight
		{
			get
			{
				return Text.LineHeight;
			}
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06007A1A RID: 31258 RVA: 0x000522C1 File Offset: 0x000504C1
		public virtual string Label
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x06007A1B RID: 31259 RVA: 0x000522D3 File Offset: 0x000504D3
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ScenPartDef>(ref this.def, "def");
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x000522E5 File Offset: 0x000504E5
		public ScenPart CopyForEditing()
		{
			ScenPart scenPart = this.CopyForEditingInner();
			scenPart.def = this.def;
			return scenPart;
		}

		// Token: 0x06007A1D RID: 31261 RVA: 0x000522F9 File Offset: 0x000504F9
		protected virtual ScenPart CopyForEditingInner()
		{
			return (ScenPart)base.MemberwiseClone();
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x00052306 File Offset: 0x00050506
		public virtual void DoEditInterface(Listing_ScenEdit listing)
		{
			listing.GetScenPartRect(this, ScenPart.RowHeight);
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x00052315 File Offset: 0x00050515
		public virtual string Summary(Scenario scen)
		{
			return this.def.description;
		}

		// Token: 0x06007A20 RID: 31264 RVA: 0x00052322 File Offset: 0x00050522
		public virtual IEnumerable<string> GetSummaryListEntries(string tag)
		{
			yield break;
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Randomize()
		{
		}

		// Token: 0x06007A22 RID: 31266 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanCoexistWith(ScenPart other)
		{
			return true;
		}

		// Token: 0x06007A24 RID: 31268 RVA: 0x0005232B File Offset: 0x0005052B
		public virtual IEnumerable<Page> GetConfigPages()
		{
			yield break;
		}

		// Token: 0x06007A25 RID: 31269 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return true;
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
		}

		// Token: 0x06007A27 RID: 31271 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
		}

		// Token: 0x06007A28 RID: 31272 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnDied(Corpse corpse)
		{
		}

		// Token: 0x06007A29 RID: 31273 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreConfigure()
		{
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostWorldGenerate()
		{
		}

		// Token: 0x06007A2B RID: 31275 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreMapGenerate()
		{
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x00052334 File Offset: 0x00050534
		public virtual IEnumerable<Thing> PlayerStartingThings()
		{
			yield break;
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GenerateIntoMap(Map map)
		{
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06007A2F RID: 31279 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostGameStart()
		{
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Tick()
		{
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x0005233D File Offset: 0x0005053D
		public virtual IEnumerable<string> ConfigErrors()
		{
			if (this.def == null)
			{
				yield return base.GetType().ToString() + " has null def.";
			}
			yield break;
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x0005234D File Offset: 0x0005054D
		public virtual bool HasNullDefs()
		{
			return this.def == null;
		}

		// Token: 0x04005029 RID: 20521
		[TranslationHandle]
		public ScenPartDef def;

		// Token: 0x0400502A RID: 20522
		public bool visible = true;

		// Token: 0x0400502B RID: 20523
		public bool summarized;
	}
}
