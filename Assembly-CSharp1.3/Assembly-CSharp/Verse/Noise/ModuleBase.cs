using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000516 RID: 1302
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x000F218C File Offset: 0x000F038C
		public int SourceModuleCount
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules.Length;
				}
				return 0;
			}
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000F21A0 File Offset: 0x000F03A0
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x170007B5 RID: 1973
		public virtual ModuleBase this[int index]
		{
			get
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (this.modules[index] == null)
				{
					throw new ArgumentNullException("Desired element is null");
				}
				return this.modules[index];
			}
			set
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (value == null)
				{
					throw new ArgumentNullException("Value should not be null");
				}
				this.modules[index] = value;
			}
		}

		// Token: 0x06002751 RID: 10065
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x06002752 RID: 10066 RVA: 0x000F2224 File Offset: 0x000F0424
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000F2244 File Offset: 0x000F0444
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000F2262 File Offset: 0x000F0462
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x000F2280 File Offset: 0x000F0480
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000F2288 File Offset: 0x000F0488
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000F22A4 File Offset: 0x000F04A4
		protected virtual bool Disposing()
		{
			if (this.modules != null)
			{
				for (int i = 0; i < this.modules.Length; i++)
				{
					this.modules[i].Dispose();
					this.modules[i] = null;
				}
				this.modules = null;
			}
			return true;
		}

		// Token: 0x0400186E RID: 6254
		protected ModuleBase[] modules;

		// Token: 0x0400186F RID: 6255
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
