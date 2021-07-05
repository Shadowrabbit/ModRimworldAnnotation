using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001966 RID: 6502
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x06008FDA RID: 36826 RVA: 0x00296908 File Offset: 0x00294B08
		private List<Pawn> AwaitingMedicalOperation
		{
			get
			{
				this.awaitingMedicalOperationResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (Alert_AwaitingMedicalOperation.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(list[i]))
					{
						this.awaitingMedicalOperationResult.Add(list[i]);
					}
				}
				List<Pawn> allMaps_PrisonersOfColonySpawned = PawnsFinder.AllMaps_PrisonersOfColonySpawned;
				for (int j = 0; j < allMaps_PrisonersOfColonySpawned.Count; j++)
				{
					if (Alert_AwaitingMedicalOperation.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(allMaps_PrisonersOfColonySpawned[j]))
					{
						this.awaitingMedicalOperationResult.Add(allMaps_PrisonersOfColonySpawned[j]);
					}
				}
				return this.awaitingMedicalOperationResult;
			}
		}

		// Token: 0x06008FDB RID: 36827 RVA: 0x00060709 File Offset: 0x0005E909
		public override string GetLabel()
		{
			return "PatientsAwaitingMedicalOperation".Translate(this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06008FDC RID: 36828 RVA: 0x0029699C File Offset: 0x00294B9C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "PatientsAwaitingMedicalOperationDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008FDD RID: 36829 RVA: 0x0006072F File Offset: 0x0005E92F
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}

		// Token: 0x06008FDE RID: 36830 RVA: 0x0006073C File Offset: 0x0005E93C
		[CompilerGenerated]
		internal static bool <get_AwaitingMedicalOperation>g__IsAwaiting|3_0(Pawn p)
		{
			return HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed();
		}

		// Token: 0x04005B87 RID: 23431
		private List<Pawn> awaitingMedicalOperationResult = new List<Pawn>();
	}
}
