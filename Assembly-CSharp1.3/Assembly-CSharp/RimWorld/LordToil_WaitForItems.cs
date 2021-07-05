using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C7 RID: 2247
	public class LordToil_WaitForItems : LordToil
	{
		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003B33 RID: 15155 RVA: 0x0014AC86 File Offset: 0x00148E86
		public bool HasAllRequestedItems
		{
			get
			{
				return this.CountRemaining <= 0;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003B34 RID: 15156 RVA: 0x0014AC94 File Offset: 0x00148E94
		public int CountRemaining
		{
			get
			{
				int num = 0;
				ThingOwner<Thing> innerContainer = this.target.inventory.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					if (innerContainer[i].def == this.requestedThingDef)
					{
						num += innerContainer[i].stackCount;
					}
				}
				return this.requestedThingCount - num;
			}
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x0014ACF0 File Offset: 0x00148EF0
		public LordToil_WaitForItems(Pawn target, ThingDef thingDef, int amount, IntVec3 waitSpot)
		{
			this.target = target;
			this.requestedThingDef = thingDef;
			this.requestedThingCount = amount;
			this.waitSpot = waitSpot;
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x0014AD18 File Offset: 0x00148F18
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.WanderClose, this.waitSpot, 10f);
			}
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0014AD75 File Offset: 0x00148F75
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			if (pawn == this.target)
			{
				pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0014AD93 File Offset: 0x00148F93
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
		{
			if (this.target == requester)
			{
				Thing reachableThing = GiveItemsToPawnUtility.FindItemToGive(current, this.requestedThingDef);
				string value = string.Format("x{0} {1}", this.CountRemaining, this.requestedThingDef.label);
				if (reachableThing != null)
				{
					TaggedString taggedString = "GiveItemsTo".Translate(value, requester);
					yield return new FloatMenuOption(taggedString, delegate()
					{
						Job job = JobMaker.MakeJob(JobDefOf.GiveToPawn, reachableThing, requester);
						job.haulMode = HaulMode.ToContainer;
						job.lord = requester.GetLord();
						current.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.High, null, requester, 0f, null, null, true, 0);
				}
				else
				{
					TaggedString taggedString2 = "CannotGiveItemsTo".Translate(value, requester) + ": " + "NoItemFound".Translate();
					yield return new FloatMenuOption(taggedString2, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			yield break;
		}

		// Token: 0x0400203D RID: 8253
		public Pawn target;

		// Token: 0x0400203E RID: 8254
		public ThingDef requestedThingDef;

		// Token: 0x0400203F RID: 8255
		public int requestedThingCount;

		// Token: 0x04002040 RID: 8256
		public IntVec3 waitSpot;

		// Token: 0x04002041 RID: 8257
		private const float WanderRadius = 10f;
	}
}
