using GameFramework.Event;

/// <summary>
/// 玩家数据变更事件参数
/// </summary>
public class PlayerDataChangedEventArgs : GameEventArgs
{
    /// <summary>
    /// 事件编号（需全局唯一，建议在EventId类中统一管理）
    /// </summary>
    public static readonly int EventId = typeof(PlayerDataChangedEventArgs).GetHashCode();

    /// <summary>
    /// 变更后的数据
    /// </summary>
    public PlayerData Data { get; private set; }
    public override int Id { get => throw new System.NotImplementedException(); }

    /// <summary>
    /// 初始化事件参数
    /// </summary>
    /// <param name="data">最新玩家数据</param>
    public void Initialize(PlayerData data)
    {
        Data = data;
    }

    /// <summary>
    /// 回收事件参数（对象池机制）
    /// </summary>
    public override void Clear()
    {
        Data = null;
    }
}