using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001087 RID: 4231
	public class Building_Throne : Building
	{
		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x060064AF RID: 25775 RVA: 0x0021E9C8 File Offset: 0x0021CBC8
		public static IEnumerable<RoyalTitleDef> AllTitlesForThroneStature
		{
			get
			{
				return from title in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where !title.throneRoomRequirements.NullOrEmpty<RoomRequirement>()
				orderby title.seniority
				select title;
			}
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x060064B0 RID: 25776 RVA: 0x0021EA22 File Offset: 0x0021CC22
		public Pawn AssignedPawn
		{
			get
			{
				if (!ModLister.CheckRoyalty("Throne"))
				{
					return null;
				}
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060064B1 RID: 25777 RVA: 0x0021EA5F File Offset: 0x0021CC5F
		public CompAssignableToPawn_Throne CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Throne>();
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060064B2 RID: 25778 RVA: 0x0021EA68 File Offset: 0x0021CC68
		public RoyalTitleDef TitleStature
		{
			get
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room == null || room.OutdoorsForWork)
				{
					return null;
				}
				RoyalTitleDef result = null;
				foreach (RoyalTitleDef royalTitleDef in Building_Throne.AllTitlesForThroneStature)
				{
					bool flag = true;
					for (int i = 0; i < royalTitleDef.throneRoomRequirements.Count; i++)
					{
						if (!(royalTitleDef.throneRoomRequirements[i] is RoomRequirement_HasAssignedThroneAnyOf) && !royalTitleDef.throneRoomRequirements[i].MetOrDisabled(room, null))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					result = royalTitleDef;
				}
				return result;
			}
		}

		// Token: 0x060064B3 RID: 25779 RVA: 0x0021EB18 File Offset: 0x0021CD18
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			Room room = this.GetRoom(RegionType.Set_All);
			Pawn p = (this.CompAssignableToPawn.AssignedPawnsForReading.Count == 1) ? this.CompAssignableToPawn.AssignedPawnsForReading[0] : null;
			RoyalTitleDef titleStature = this.TitleStature;
			text += "\n" + "ThroneMaxSatisfiedTitle".Translate() + ": " + ((titleStature == null) ? "None".Translate() : titleStature.GetLabelCapFor(p));
			string text2 = RoomRoleWorker_ThroneRoom.Validate(room);
			if (text2 != null)
			{
				return text + "\n" + text2;
			}
			return text;
		}

		// Token: 0x060064B4 RID: 25780 RVA: 0x0021EBC4 File Offset: 0x0021CDC4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.CheckRoyalty("Throne"))
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}
	}
}
