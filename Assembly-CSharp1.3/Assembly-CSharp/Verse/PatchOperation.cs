using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000246 RID: 582
	public class PatchOperation
	{
		// Token: 0x060010DC RID: 4316 RVA: 0x0005FA44 File Offset: 0x0005DC44
		public bool Apply(XmlDocument xml)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(base.GetType().FullName + " Worker");
			}
			bool flag = this.ApplyWorker(xml);
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
			if (this.success == PatchOperation.Success.Always)
			{
				flag = true;
			}
			else if (this.success == PatchOperation.Success.Never)
			{
				flag = false;
			}
			else if (this.success == PatchOperation.Success.Invert)
			{
				flag = !flag;
			}
			if (flag)
			{
				this.neverSucceeded = false;
			}
			return flag;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0005FABD File Offset: 0x0005DCBD
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail");
			return false;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0005FACC File Offset: 0x0005DCCC
		public virtual void Complete(string modIdentifier)
		{
			if (this.neverSucceeded)
			{
				string text = string.Format("[{0}] Patch operation {1} failed", modIdentifier, this);
				if (!string.IsNullOrEmpty(this.sourceFile))
				{
					text = text + "\nfile: " + this.sourceFile;
				}
				Log.Error(text);
			}
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0005FB13 File Offset: 0x0005DD13
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04000CD6 RID: 3286
		public string sourceFile;

		// Token: 0x04000CD7 RID: 3287
		private bool neverSucceeded = true;

		// Token: 0x04000CD8 RID: 3288
		private PatchOperation.Success success;

		// Token: 0x020019C8 RID: 6600
		private enum Success
		{
			// Token: 0x040062EF RID: 25327
			Normal,
			// Token: 0x040062F0 RID: 25328
			Invert,
			// Token: 0x040062F1 RID: 25329
			Always,
			// Token: 0x040062F2 RID: 25330
			Never
		}
	}
}
