using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8E RID: 3982
	public abstract class RitualRole : IExposable, ILoadReferenceable
	{
		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06005E50 RID: 24144 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool Animal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06005E51 RID: 24145 RVA: 0x00205E36 File Offset: 0x00204036
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06005E52 RID: 24146 RVA: 0x00205E3E File Offset: 0x0020403E
		public string LabelCap
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06005E53 RID: 24147 RVA: 0x00205E4B File Offset: 0x0020404B
		public string CategoryLabel
		{
			get
			{
				return this.categoryLabel;
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06005E54 RID: 24148 RVA: 0x00205E53 File Offset: 0x00204053
		public string CategoryLabelCap
		{
			get
			{
				return this.categoryLabel.CapitalizeFirst();
			}
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x00205E60 File Offset: 0x00204060
		public RitualRole()
		{
			if (Find.UniqueIDsManager != null && this.loadID == -1)
			{
				this.loadID = Find.UniqueIDsManager.GetNextRitualRoleID();
			}
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x00205E9D File Offset: 0x0020409D
		public virtual bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			Ideo ideo = p.Ideo;
			return this.AppliesToRole((ideo != null) ? ideo.GetRole(p) : null, out reason, ((ritual != null) ? ritual.Ritual : null) ?? ((assignments != null) ? assignments.Ritual : null), p);
		}

		// Token: 0x06005E57 RID: 24151 RVA: 0x00205ED8 File Offset: 0x002040D8
		public Precept_Role FindInstance(Ideo ideo)
		{
			foreach (Precept_Role precept_Role in ideo.RolesListForReading)
			{
				if (precept_Role.def == this.precept)
				{
					return precept_Role;
				}
			}
			return null;
		}

		// Token: 0x06005E58 RID: 24152
		public abstract bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn pawn = null);

		// Token: 0x06005E59 RID: 24153 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			return null;
		}

		// Token: 0x06005E5A RID: 24154 RVA: 0x00205F3C File Offset: 0x0020413C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.missingDesc, "missingDesc", null, false);
			Scribe_Values.Look<string>(ref this.id, "id", null, false);
			Scribe_Values.Look<int>(ref this.maxCount, "maxCount", 0, false);
			Scribe_Values.Look<bool>(ref this.required, "required", false, false);
			Scribe_Values.Look<bool>(ref this.substitutable, "substitutable", false, false);
			Scribe_Values.Look<bool>(ref this.countsAsParticipant, "countsAsParticipant", true, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", -1, false);
			Scribe_Values.Look<bool>(ref this.defaultForSelectedColonist, "defaultForSelectedColonist", false, false);
			Scribe_Defs.Look<PreceptDef>(ref this.precept, "precept");
			Scribe_Values.Look<string>(ref this.mergeId, "mergeId", null, false);
			Scribe_Values.Look<bool>(ref this.allowOtherIdeos, "allowOtherIdeos", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.loadID == -1)
			{
				this.loadID = Find.UniqueIDsManager.GetNextRitualRoleID();
			}
		}

		// Token: 0x06005E5B RID: 24155 RVA: 0x00206040 File Offset: 0x00204240
		public string GetUniqueLoadID()
		{
			return "RitualRole_" + this.loadID;
		}

		// Token: 0x04003670 RID: 13936
		[MustTranslate]
		protected string label;

		// Token: 0x04003671 RID: 13937
		[MustTranslate]
		protected string categoryLabel;

		// Token: 0x04003672 RID: 13938
		[MustTranslate]
		public string missingDesc;

		// Token: 0x04003673 RID: 13939
		[NoTranslate]
		public string id;

		// Token: 0x04003674 RID: 13940
		public PreceptDef precept;

		// Token: 0x04003675 RID: 13941
		public int maxCount;

		// Token: 0x04003676 RID: 13942
		[NoTranslate]
		public string mergeId;

		// Token: 0x04003677 RID: 13943
		public bool required;

		// Token: 0x04003678 RID: 13944
		public bool substitutable;

		// Token: 0x04003679 RID: 13945
		public bool ignoreBleeding;

		// Token: 0x0400367A RID: 13946
		public bool countsAsParticipant = true;

		// Token: 0x0400367B RID: 13947
		public bool addToLord = true;

		// Token: 0x0400367C RID: 13948
		public bool allowNonAggroMentalState;

		// Token: 0x0400367D RID: 13949
		public bool defaultForSelectedColonist;

		// Token: 0x0400367E RID: 13950
		public bool allowOtherIdeos;

		// Token: 0x0400367F RID: 13951
		private int loadID = -1;
	}
}
