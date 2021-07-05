using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002E3 RID: 739
	public abstract class Name : IExposable
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060013F3 RID: 5107
		public abstract string ToStringFull { get; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060013F4 RID: 5108
		public abstract string ToStringShort { get; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060013F5 RID: 5109
		public abstract bool IsValid { get; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00071924 File Offset: 0x0006FB24
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

		// Token: 0x060013F7 RID: 5111
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x060013F8 RID: 5112
		public abstract void ExposeData();

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060013F9 RID: 5113
		public abstract bool Numerical { get; }
	}
}
