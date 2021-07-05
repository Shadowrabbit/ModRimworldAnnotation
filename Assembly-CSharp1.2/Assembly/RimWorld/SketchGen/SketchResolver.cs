using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E05 RID: 7685
	public abstract class SketchResolver
	{
		// Token: 0x0600A67B RID: 42619 RVA: 0x00304760 File Offset: 0x00302960
		public void Resolve(ResolveParams parms)
		{
			try
			{
				this.ResolveInt(parms);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception resolving ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nParms:\n",
					parms.ToString()
				}), false);
			}
		}

		// Token: 0x0600A67C RID: 42620 RVA: 0x003047D4 File Offset: 0x003029D4
		public bool CanResolve(ResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception test running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nParms:\n",
					parms.ToString()
				}), false);
				result = false;
			}
			return result;
		}

		// Token: 0x0600A67D RID: 42621
		protected abstract void ResolveInt(ResolveParams parms);

		// Token: 0x0600A67E RID: 42622
		protected abstract bool CanResolveInt(ResolveParams parms);
	}
}
