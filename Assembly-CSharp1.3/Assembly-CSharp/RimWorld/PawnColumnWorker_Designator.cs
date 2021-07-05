using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001377 RID: 4983
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x06007943 RID: 31043
		protected abstract DesignationDef DesignationType { get; }

		// Token: 0x06007944 RID: 31044 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		// Token: 0x06007945 RID: 31045 RVA: 0x002AF685 File Offset: 0x002AD885
		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		// Token: 0x06007946 RID: 31046 RVA: 0x002AF694 File Offset: 0x002AD894
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			if (value == this.GetValue(pawn))
			{
				return;
			}
			if (table.SortingBy == this.def)
			{
				table.SetDirty();
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

		// Token: 0x06007947 RID: 31047 RVA: 0x002AF708 File Offset: 0x002AD908
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
