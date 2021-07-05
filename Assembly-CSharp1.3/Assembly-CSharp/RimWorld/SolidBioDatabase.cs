using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E27 RID: 3623
	public class SolidBioDatabase
	{
		// Token: 0x060053C1 RID: 21441 RVA: 0x001C5B4B File Offset: 0x001C3D4B
		public static void Clear()
		{
			SolidBioDatabase.allBios.Clear();
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x001C5B58 File Offset: 0x001C3D58
		public static void LoadAllBios()
		{
			foreach (PawnBio pawnBio in DirectXmlLoader.LoadXmlDataInResourcesFolder<PawnBio>("Backstories/Solid"))
			{
				pawnBio.name.ResolveMissingPieces(null);
				if (pawnBio.childhood == null || pawnBio.adulthood == null)
				{
					PawnNameDatabaseSolid.AddPlayerContentName(pawnBio.name, pawnBio.gender);
				}
				else
				{
					pawnBio.PostLoad();
					pawnBio.ResolveReferences();
					foreach (string text in pawnBio.ConfigErrors())
					{
						Log.Error(text);
					}
					SolidBioDatabase.allBios.Add(pawnBio);
					pawnBio.childhood.shuffleable = false;
					pawnBio.childhood.slot = BackstorySlot.Childhood;
					pawnBio.adulthood.shuffleable = false;
					pawnBio.adulthood.slot = BackstorySlot.Adulthood;
					BackstoryDatabase.AddBackstory(pawnBio.childhood);
					BackstoryDatabase.AddBackstory(pawnBio.adulthood);
				}
			}
		}

		// Token: 0x0400314D RID: 12621
		public static List<PawnBio> allBios = new List<PawnBio>();
	}
}
