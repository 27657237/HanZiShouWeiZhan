using GameFramework.Event;

/// <summary>
/// ������ݱ���¼�����
/// </summary>
public class PlayerDataChangedEventArgs : GameEventArgs
{
    /// <summary>
    /// �¼���ţ���ȫ��Ψһ��������EventId����ͳһ����
    /// </summary>
    public static readonly int EventId = typeof(PlayerDataChangedEventArgs).GetHashCode();

    /// <summary>
    /// ����������
    /// </summary>
    public PlayerData Data { get; private set; }
    public override int Id { get => throw new System.NotImplementedException(); }

    /// <summary>
    /// ��ʼ���¼�����
    /// </summary>
    /// <param name="data">�����������</param>
    public void Initialize(PlayerData data)
    {
        Data = data;
    }

    /// <summary>
    /// �����¼�����������ػ��ƣ�
    /// </summary>
    public override void Clear()
    {
        Data = null;
    }
}