using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB7 RID: 2999
	public class QuestPart_TransporterPawns_TendWithMedicine : QuestPart_TransporterPawns_Tend
	{
		// Token: 0x060045F5 RID: 17909 RVA: 0x00172604 File Offset: 0x00170804
		protected override void DoTend(Pawn pawn)
		{
			Pawn doctor = null;
			if (this.allowSelfTend && pawn.playerSettings != null && pawn.playerSettings.selfTend && pawn.GetStatValue(StatDefOf.MedicalTendQuality, true) > 0.75f)
			{
				doctor = pawn;
			}
			Medicine medicine = (Medicine)ThingMaker.MakeThing(this.medicineDef, null);
			TendUtility.DoTend(doctor, pawn, medicine);
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x0017265F File Offset: 0x0017085F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.medicineDef, "medicineDef");
			Scribe_Values.Look<bool>(ref this.allowSelfTend, "allowSelfTend", false, false);
		}

		// Token: 0x04002AA1 RID: 10913
		public ThingDef medicineDef;

		// Token: 0x04002AA2 RID: 10914
		public bool allowSelfTend;
	}
}
