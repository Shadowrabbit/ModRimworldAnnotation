using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000432 RID: 1074
	public abstract class Name : IExposable
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060019FA RID: 6650
		public abstract string ToStringFull { get; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060019FB RID: 6651
		public abstract string ToStringShort { get; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060019FC RID: 6652
		public abstract bool IsValid { get; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x000E4644 File Offset: 0x000E2844
		public bool UsedThisGame
		{
			get
			{
				using (IEnumerator<Name> enumerator = NameUseChecker.AllPawnsNamesEverUsed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ConfusinglySimilarTo(this))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060019FE RID: 6654
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x060019FF RID: 6655
		public abstract void ExposeData();

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001A00 RID: 6656
		public abstract bool Numerical { get; }
	}
}
