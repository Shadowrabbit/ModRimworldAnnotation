using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200047B RID: 1147
	public class DefMap<D, V> : IExposable, IEnumerable<KeyValuePair<D, V>>, IEnumerable where D : Def, new() where V : new()
	{
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x000DB288 File Offset: 0x000D9488
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x17000677 RID: 1655
		public V this[D def]
		{
			get
			{
				return this.values[(int)def.index];
			}
			set
			{
				this.values[(int)def.index] = value;
			}
		}

		// Token: 0x17000678 RID: 1656
		public V this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000DB2E4 File Offset: 0x000D94E4
		public DefMap()
		{
			int defCount = DefDatabase<D>.DefCount;
			if (defCount == 0)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Constructed DefMap<",
					typeof(D),
					", ",
					typeof(V),
					"> without defs being initialized. Try constructing it in ResolveReferences instead of the constructor."
				}));
			}
			this.values = new List<V>(defCount);
			for (int i = 0; i < defCount; i++)
			{
				this.values.Add(Activator.CreateInstance<V>());
			}
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000DB36C File Offset: 0x000D956C
		public void ExposeData()
		{
			Scribe_Collections.Look<V>(ref this.values, "vals", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int defCount = DefDatabase<D>.DefCount;
				for (int i = this.values.Count; i < defCount; i++)
				{
					this.values.Add(Activator.CreateInstance<V>());
				}
				while (this.values.Count > defCount)
				{
					this.values.RemoveLast<V>();
				}
			}
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000DB3E0 File Offset: 0x000D95E0
		public void SetAll(V val)
		{
			for (int i = 0; i < this.values.Count; i++)
			{
				this.values[i] = val;
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000DB410 File Offset: 0x000D9610
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000DB418 File Offset: 0x000D9618
		public IEnumerator<KeyValuePair<D, V>> GetEnumerator()
		{
			return (from d in DefDatabase<D>.AllDefsListForReading
			select new KeyValuePair<D, V>(d, this[d])).GetEnumerator();
		}

		// Token: 0x040015D7 RID: 5591
		private List<V> values;
	}
}
