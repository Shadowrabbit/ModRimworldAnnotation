using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001276 RID: 4726
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x0600710C RID: 28940 RVA: 0x0025AAC4 File Offset: 0x00258CC4
		private List<Pawn> AwaitingMedicalOperation
		{
			get
			{
				this.awaitingMedicalOperationResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(list[i]))
					{
						this.awaitingMedicalOperationResult.Add(list[i]);
					}
				}
				List<Pawn> allMaps_PrisonersOfColonySpawned = PawnsFinder.AllMaps_PrisonersOfColonySpawned;
				for (int j = 0; j < allMaps_PrisonersOfColonySpawned.Count; j++)
				{
					if (this.<get_AwaitingMedicalOperation>g__IsAwaiting|3_0(allMaps_PrisonersOfColonySpawned[j]))
					{
						this.awaitingMedicalOperationResult.Add(allMaps_PrisonersOfColonySpawned[j]);
					}
				}
				return this.awaitingMedicalOperationResult;
			}
		}

		// Token: 0x0600710D RID: 28941 RVA: 0x0025AB57 File Offset: 0x00258D57
		public override string GetLabel()
		{
			return "PatientsAwaitingMedicalOperation".Translate(this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}

		// Token: 0x0600710E RID: 28942 RVA: 0x0025AB80 File Offset: 0x00258D80
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AwaitingMedicalOperation)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "PatientsAwaitingMedicalOperationDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x0600710F RID: 28943 RVA: 0x0025AC08 File Offset: 0x00258E08
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}

		// Token: 0x06007110 RID: 28944 RVA: 0x0025AC15 File Offset: 0x00258E15
		[CompilerGenerated]
		private bool <get_AwaitingMedicalOperation>g__IsAwaiting|3_0(Pawn p)
		{
			return HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed() && !this.awaitingMedicalOperationResult.Contains(p);
		}

		// Token: 0x04003E34 RID: 15924
		private List<Pawn> awaitingMedicalOperationResult = new List<Pawn>();
	}
}
