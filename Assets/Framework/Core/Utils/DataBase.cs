/**************************************************
* 创建作者：	咕咕咕
* 作用描述：	#
***************************************************/

namespace Framework
{
	public abstract class DataBase<TKey> 
	{
        #region ----字段----
        protected TKey key;
        #endregion

        #region ----属性----
        public TKey Key => key;
        #endregion
    }
}