using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD8 RID: 4056
	public class RoleEffect_GiveThoughtOnTend : RoleEffect
	{
		// Token: 0x06005F84 RID: 24452 RVA: 0x0020AC95 File Offset: 0x00208E95
		public RoleEffect_GiveThoughtOnTend()
		{
			this.labelKey = "RoleEffectGiveThoughtOnTended";
		}

		// Token: 0x06005F85 RID: 24453 RVA: 0x0020ACA8 File Offset: 0x00208EA8
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate() + ": " + this.thoughtDef.LabelCap.Formatted(role.LabelCap.ResolveTags());
		}

		// Token: 0x06005F86 RID: 24454 RVA: 0x0020ACF4 File Offset: 0x00208EF4
		public override void Notify_Tended(Pawn doctor, Pawn patient)
		{
			base.Notify_Tended(doctor, patient);
			if (doctor != patient && doctor.Ideo == patient.Ideo)
			{
				patient.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, doctor, null);
			}
		}

		// Token: 0x040036E3 RID: 14051
		public ThoughtDef thoughtDef;
	}
}
