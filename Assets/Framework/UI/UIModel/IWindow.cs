namespace Framework
{
	public interface IWindow
	{
		void Open(params object[] args);

		void Close(bool destory);
	}
}