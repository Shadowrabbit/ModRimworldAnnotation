using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E32 RID: 3634
	[StaticConstructorOnStartup]
	public static class GuestUtility
	{
		// Token: 0x06005410 RID: 21520 RVA: 0x001C726A File Offset: 0x001C546A
		public static Texture2D GetGuestIcon(GuestStatus guestStatus)
		{
			if (guestStatus == GuestStatus.Slave)
			{
				return GuestUtility.SlaveIcon;
			}
			return null;
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x001C7277 File Offset: 0x001C5477
		public static IEnumerable<WorkTypeDef> GetDisabledWorkTypes(this Pawn_GuestTracker guest)
		{
			if (guest.IsSlave)
			{
				foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
				{
					if (workTypeDef.disabledForSlaves)
					{
						yield return workTypeDef;
					}
				}
				IEnumerator<WorkTypeDef> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x001C7287 File Offset: 0x001C5487
		public static bool IsSellingToSlavery(Pawn slave, Faction slaveOwner)
		{
			return slave.IsSlave && slave.HomeFaction != slaveOwner;
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x001C729F File Offset: 0x001C549F
		public static void GetExtraFactionsFromGuestStatus(Pawn pawn, List<ExtraFaction> outExtraFactions)
		{
			if (pawn.IsSlave && pawn.SlaveFaction != null)
			{
				outExtraFactions.Add(new ExtraFaction(pawn.SlaveFaction, ExtraFactionType.HomeFaction));
			}
		}

		// Token: 0x04003171 RID: 12657
		public static Texture2D SlaveSuppressionFillTex = SolidColorMaterials.NewSolidColorTexture(new Color32(245, 209, 66, byte.MaxValue));

		// Token: 0x04003172 RID: 12658
		public static readonly Texture2D SlaveIcon = ContentFinder<Texture2D>.Get("UI/Icons/Slavery", true);

		// Token: 0x04003173 RID: 12659
		public static readonly Texture2D RansomIcon = ContentFinder<Texture2D>.Get("UI/Icons/Ransom", true);
	}
}
