using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B61 RID: 7009
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x06009A80 RID: 39552
		protected abstract DesignationDef DesignationType { get; }

		// Token: 0x06009A81 RID: 39553 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		// Token: 0x06009A82 RID: 39554 RVA: 0x00066DD2 File Offset: 0x00064FD2
		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		// Token: 0x06009A83 RID: 39555 RVA: 0x002D7AF4 File Offset: 0x002D5CF4
		protected override void SetValue(Pawn pawn, bool value)
		{
			if (value == this.GetValue(pawn))
			{
				return;
			}
			if (value)
			{
				pawn.MapHeld.designationManager.AddDesignation(new Designation(pawn, this.DesignationType));
				this.Notify_DesignationAdded(pawn);
				return;
			}
			Designation designation = this.GetDesignation(pawn);
			if (designation != null)
			{
				pawn.MapHeld.designationManager.RemoveDesignation(designation);
			}
		}

		// Token: 0x06009A84 RID: 39556 RVA: 0x002D7B54 File Offset: 0x002D5D54
		private Designation GetDesignation(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return null;
			}
			return mapHeld.designationManager.DesignationOn(pawn, this.DesignationType);
		}
	}
}
