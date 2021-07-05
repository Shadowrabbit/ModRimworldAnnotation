using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C1 RID: 5313
	public class SolidBioDatabase
	{
		// Token: 0x0600726A RID: 29290 RVA: 0x0004CEFF File Offset: 0x0004B0FF
		public static void Clear()
		{
			SolidBioDatabase.allBios.Clear();
		}

		// Token: 0x0600726B RID: 29291 RVA: 0x0022F9BC File Offset: 0x0022DBBC
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
						Log.Error(text, false);
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

		// Token: 0x04004B60 RID: 19296
		public static List<PawnBio> allBios = new List<PawnBio>();
	}
}
