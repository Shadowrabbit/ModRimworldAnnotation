using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000252 RID: 594
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x060010F7 RID: 4343 RVA: 0x00060250 File Offset: 0x0005E450
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x000602B4 File Offset: 0x0005E4B4
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x000602C4 File Offset: 0x0005E4C4
		public override string ToString()
		{
			int num = (this.operations != null) ? this.operations.Count : 0;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				text = text + ", lastFailedOperation=" + this.lastFailedOperation;
			}
			return text + ")";
		}

		// Token: 0x04000CE4 RID: 3300
		private List<PatchOperation> operations;

		// Token: 0x04000CE5 RID: 3301
		private PatchOperation lastFailedOperation;
	}
}
