using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001534 RID: 5428
	public class RoyalTitlePermitDef : Def
	{
		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06008117 RID: 33047 RVA: 0x002DA901 File Offset: 0x002D8B01
		public int CooldownTicks
		{
			get
			{
				return (int)(this.cooldownDays * 60000f);
			}
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x06008118 RID: 33048 RVA: 0x002DA910 File Offset: 0x002D8B10
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

		// Token: 0x06008119 RID: 33049 RVA: 0x002DA944 File Offset: 0x002D8B44
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

		// Token: 0x0600811A RID: 33050 RVA: 0x002DA9E0 File Offset: 0x002D8BE0
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

		// Token: 0x0600811B RID: 33051 RVA: 0x002DAA30 File Offset: 0x002D8C30
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

		// Token: 0x04005068 RID: 20584
		public Type workerClass = typeof(RoyalTitlePermitWorker);

		// Token: 0x04005069 RID: 20585
		public RoyalAid royalAid;

		// Token: 0x0400506A RID: 20586
		public float cooldownDays;

		// Token: 0x0400506B RID: 20587
		public RoyalTitleDef minTitle;

		// Token: 0x0400506C RID: 20588
		public int permitPointCost;

		// Token: 0x0400506D RID: 20589
		public FactionDef faction;

		// Token: 0x0400506E RID: 20590
		public bool usableOnWorldMap;

		// Token: 0x0400506F RID: 20591
		public RoyalTitlePermitDef prerequisite;

		// Token: 0x04005070 RID: 20592
		public Vector2 uiPosition;

		// Token: 0x04005071 RID: 20593
		private RoyalTitlePermitWorker worker;
	}
}
