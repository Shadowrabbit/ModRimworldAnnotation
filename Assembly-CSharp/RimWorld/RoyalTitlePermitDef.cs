using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC1 RID: 7617
	public class RoyalTitlePermitDef : Def
	{
		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x0600A595 RID: 42389 RVA: 0x0006DA9D File Offset: 0x0006BC9D
		public int CooldownTicks
		{
			get
			{
				return (int)(this.cooldownDays * 60000f);
			}
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x0600A596 RID: 42390 RVA: 0x0006DAAC File Offset: 0x0006BCAC
		public RoyalTitlePermitWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (RoyalTitlePermitWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x0600A597 RID: 42391 RVA: 0x00300F20 File Offset: 0x002FF120
		public bool AvailableForPawn(Pawn pawn, Faction faction)
		{
			if (pawn.royalty == null)
			{
				return false;
			}
			if (pawn.royalty.HasPermit(this, faction))
			{
				return false;
			}
			if (this.prerequisite != null && !pawn.royalty.HasPermit(this.prerequisite, faction))
			{
				return false;
			}
			if (pawn.royalty.GetPermitPoints(faction) < this.permitPointCost)
			{
				return false;
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
			return (currentTitle == null && this.minTitle == null) || ((currentTitle != null || this.minTitle == null) && currentTitle.seniority >= this.minTitle.seniority);
		}

		// Token: 0x0600A598 RID: 42392 RVA: 0x00300FBC File Offset: 0x002FF1BC
		public bool IsPrerequisiteOfHeldPermit(Pawn pawn, Faction faction)
		{
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].Permit.prerequisite == this && allFactionPermits[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A599 RID: 42393 RVA: 0x0006DADE File Offset: 0x0006BCDE
		public override IEnumerable<string> ConfigErrors()
		{
			if (!typeof(RoyalTitlePermitWorker).IsAssignableFrom(this.workerClass))
			{
				yield return string.Format("RoyalTitlePermitDef {0} has worker class {1}, which is not deriving from {2}", this.defName, this.workerClass, typeof(RoyalTitlePermitWorker).FullName);
			}
			if (this.royalAid != null)
			{
				if (this.royalAid.pawnKindDef != null && this.royalAid.pawnCount <= 0)
				{
					yield return "pawnCount should be greater than 0, if you specify pawnKindDef";
				}
				if (!this.royalAid.itemsToDrop.NullOrEmpty<ThingDefCountClass>())
				{
					int num;
					for (int i = 0; i < this.royalAid.itemsToDrop.Count; i = num + 1)
					{
						if (this.royalAid.itemsToDrop[i].count <= 0)
						{
							yield return "item count should be greater than 0.";
						}
						if (this.royalAid.itemsToDrop[i].thingDef == null)
						{
							yield return "thingDef not defined.";
						}
						num = i;
					}
				}
				if (this.royalAid.favorCost <= 0)
				{
					yield return "favor cost should be greater than 0.";
				}
			}
			yield break;
		}

		// Token: 0x04007044 RID: 28740
		public Type workerClass = typeof(RoyalTitlePermitWorker);

		// Token: 0x04007045 RID: 28741
		public RoyalAid royalAid;

		// Token: 0x04007046 RID: 28742
		public float cooldownDays;

		// Token: 0x04007047 RID: 28743
		public RoyalTitleDef minTitle;

		// Token: 0x04007048 RID: 28744
		public int permitPointCost;

		// Token: 0x04007049 RID: 28745
		public FactionDef faction;

		// Token: 0x0400704A RID: 28746
		public bool usableOnWorldMap;

		// Token: 0x0400704B RID: 28747
		public RoyalTitlePermitDef prerequisite;

		// Token: 0x0400704C RID: 28748
		public Vector2 uiPosition;

		// Token: 0x0400704D RID: 28749
		private RoyalTitlePermitWorker worker;
	}
}
