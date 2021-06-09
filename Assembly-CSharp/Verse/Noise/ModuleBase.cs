using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008D5 RID: 2261
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06003825 RID: 14373 RVA: 0x0002B58E File Offset: 0x0002978E
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

		// Token: 0x06003826 RID: 14374 RVA: 0x0002B5A2 File Offset: 0x000297A2
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x170008DD RID: 2269
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

		// Token: 0x06003829 RID: 14377
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x0600382A RID: 14378 RVA: 0x0002B626 File Offset: 0x00029826
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x0002B646 File Offset: 0x00029846
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x0002B664 File Offset: 0x00029864
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x0600382D RID: 14381 RVA: 0x0002B682 File Offset: 0x00029882
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x0002B68A File Offset: 0x0002988A
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x00162930 File Offset: 0x00160B30
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

		// Token: 0x040026E5 RID: 9957
		protected ModuleBase[] modules;

		// Token: 0x040026E6 RID: 9958
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
