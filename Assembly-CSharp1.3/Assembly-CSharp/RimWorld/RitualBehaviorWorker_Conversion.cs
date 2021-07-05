using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2C RID: 3884
	public class RitualBehaviorWorker_Conversion : RitualBehaviorWorker
	{
		// Token: 0x06005C60 RID: 23648 RVA: 0x001FD352 File Offset: 0x001FB552
		public RitualBehaviorWorker_Conversion()
		{
		}

		// Token: 0x06005C61 RID: 23649 RVA: 0x001FD35A File Offset: 0x001FB55A
		public RitualBehaviorWorker_Conversion(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x001FDCE4 File Offset: 0x001FBEE4
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			Precept_Role precept_Role = ritual.ideo.RolesListForReading.First((Precept_Role r) => r.def == PreceptDefOf.IdeoRole_Moralist);
			if (precept_Role.ChosenPawnSingle() == null)
			{
				return "CantStartRitualRoleNotAssigned".Translate(precept_Role.LabelCap);
			}
			bool flag = false;
			using (List<Pawn>.Enumerator enumerator = target.Map.mapPawns.FreeColonistsAndPrisonersSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (RitualBehaviorWorker_Conversion.ValidateConvertee(enumerator.Current, precept_Role.ChosenPawnSingle(), false))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return "CantStartRitualNoConvertee".Translate(precept_Role.ChosenPawnSingle().Ideo.name);
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x001FDDD4 File Offset: 0x001FBFD4
		public static bool ValidateConvertee(Pawn convertee, Pawn leader, bool throwMessages)
		{
			return convertee != null && AbilityUtility.ValidateNoMentalState(convertee, throwMessages) && AbilityUtility.ValidateCanWalk(convertee, throwMessages) && AbilityUtility.ValidateNotSameIdeo(leader, convertee, throwMessages);
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x001FDE00 File Offset: 0x001FC000
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("moralist");
			Pawn pawn = ritual.PawnWithRole("convertee");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}
	}
}
