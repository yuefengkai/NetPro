using System.Threading.Tasks;

namespace StackExchange.Redis.Extensions.Core.Abstractions
{
    /// <summary>
    /// The Redis Database
    /// </summary>
    public partial interface IRedisDatabase
    {
        /// <summary>
        /// ���item���������б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redsi key</param>
        /// <param name="item">Ҫ��ӵ���</param>
        /// <param name="when">����������,Ĭ�ϲ�����Ƿ����ʼ�����</param>
        /// <param name="flag">��Ϊ���,Ĭ��None=PreferMaster:��������������ִ��</param>
        /// <returns></returns>
        Task<long> ListAddToLeftAsync<T>(string key, T item, When when = When.Always, CommandFlags flag = CommandFlags.None)
            where T : class;

        /// <summary>
        /// ���item���������б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redsi key</param>
        /// <param name="item">Ҫ��ӵ���</param>
        /// <param name="when">����������,Ĭ�ϲ�����Ƿ����ʼ�����</param>
        /// <param name="flag">��Ϊ���,Ĭ��None=PreferMaster:��������������ִ��</param>
        /// <returns></returns>
        long ListAddToLeft<T>(string key, T item, When when = When.Always, CommandFlags flag = CommandFlags.None)
            where T : class;

        /// <summary>
        /// ���items ���鼯�ϵ��б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redsi key</param>
        /// <param name="items">items ���鼯��</param>
        /// <param name="flag">��Ϊ���,Ĭ��None=PreferMaster:��������������ִ��</param>
        /// <returns></returns>
        Task<long> ListAddToLeftAsync<T>(string key, T[] items, CommandFlags flag = CommandFlags.None)
            where T : class;

        /// <summary>
        /// ���items ���鼯�ϵ��б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redsi key</param>
        /// <param name="items">items ���鼯��</param>
        /// <param name="flag">��Ϊ���,Ĭ��None=PreferMaster:��������������ִ��</param>
        /// <returns></returns>
        long ListAddToLeft<T>(string key, T[] items, CommandFlags flag = CommandFlags.None)
            where T : class;

        /// <summary>
        /// ɾ�������ش洢�ڼ������б�����һ��Ԫ�ء�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redsi key</param>
        /// <param name="flag">��Ϊ���,Ĭ��None=PreferMaster:��������������ִ��</param>
        /// <returns></returns>
        Task<T> ListGetFromRightAsync<T>(string key, CommandFlags flag = CommandFlags.None)
            where T : class;

        /// <summary>
        /// ɾ�������ش洢�ڼ������б�����һ��Ԫ�ء�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redsi key</param>
        /// <param name="flag">��Ϊ���,Ĭ��None=PreferMaster:��������������ִ��</param>
        /// <returns></returns>
        T ListGetFromRight<T>(string key, CommandFlags flag = CommandFlags.None)
            where T : class;
    }
}
