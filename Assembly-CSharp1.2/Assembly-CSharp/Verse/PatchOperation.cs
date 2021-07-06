using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200035E RID: 862
	public class PatchOperation
	{
		// Token: 0x06001616 RID: 5654 RVA: 0x000D4EC0 File Offset: 0x000D30C0
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

		// Token: 0x06001617 RID: 5655 RVA: 0x00015B60 File Offset: 0x00013D60
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail", false);
			return false;
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000D4F3C File Offset: 0x000D313C
		public virtual void Complete(string modIdentifier)
		{
			if (this.neverSucceeded)
			{
				string text = string.Format("[{0}] Patch operation {1} failed", modIdentifier, this);
				if (!string.IsNullOrEmpty(this.sourceFile))
				{
					text = text + "\nfile: " + this.sourceFile;
				}
				Log.Error(text, false);
			}
		}

		// Token: 0x040010EB RID: 4331
		public string sourceFile;

		// Token: 0x040010EC RID: 4332
		private bool neverSucceeded = true;

		// Token: 0x040010ED RID: 4333
		private PatchOperation.Success success;

		// Token: 0x0200035F RID: 863
		private enum Success
		{
			// Token: 0x040010EF RID: 4335
			Normal,
			// Token: 0x040010F0 RID: 4336
			Invert,
			// Token: 0x040010F1 RID: 4337
			Always,
			// Token: 0x040010F2 RID: 4338
			Never
		}
	}
}
