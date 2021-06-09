using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D2 RID: 5842
	public class Building_Throne : Building
	{
		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x06008042 RID: 32834 RVA: 0x0025F670 File Offset: 0x0025D870
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

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06008043 RID: 32835 RVA: 0x0025F6CC File Offset: 0x0025D8CC
		public Pawn AssignedPawn
		{
			get
			{
				if (!ModLister.RoyaltyInstalled)
				{
					Log.ErrorOnce("Thrones are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 1222185, false);
					return null;
				}
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x06008044 RID: 32836 RVA: 0x000561A6 File Offset: 0x000543A6
		public CompAssignableToPawn_Throne CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Throne>();
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06008045 RID: 32837 RVA: 0x0025F720 File Offset: 0x0025D920
		public RoyalTitleDef TitleStature
		{
			get
			{
				Room room = this.GetRoom(RegionType.Set_Passable);
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
						if (!(royalTitleDef.throneRoomRequirements[i] is RoomRequirement_HasAssignedThroneAnyOf) && !royalTitleDef.throneRoomRequirements[i].Met(room, null))
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

		// Token: 0x06008046 RID: 32838 RVA: 0x0025F7D0 File Offset: 0x0025D9D0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			Room room = this.GetRoom(RegionType.Set_Passable);
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

		// Token: 0x06008047 RID: 32839 RVA: 0x000561AE File Offset: 0x000543AE
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Thrones are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.  See rules on the Ludeon forum for more info.", 1222185, false);
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
