using System;

namespace Framework
{
	public interface ISingleton : IDisposable
	{
		void OnInit();
	}
}