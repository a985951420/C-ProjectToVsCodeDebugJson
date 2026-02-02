using VsCodeDebugGen.Desktop.Models;

namespace VsCodeDebugGen.Desktop.Services.Interfaces;

/// <summary>
/// 历史服务接口
/// </summary>
public interface IHistoryService
{
    /// <summary>
    /// 添加历史记录
    /// </summary>
    /// <param name="entry">历史条目</param>
    Task AddHistoryEntryAsync(HistoryEntry entry);

    /// <summary>
    /// 加载所有历史记录
    /// </summary>
    /// <returns>历史记录列表</returns>
    Task<List<HistoryEntry>> LoadHistoryAsync();

    /// <summary>
    /// 清空历史记录
    /// </summary>
    Task ClearHistoryAsync();

    /// <summary>
    /// 删除指定的历史记录
    /// </summary>
    /// <param name="id">历史记录ID</param>
    Task DeleteHistoryEntryAsync(Guid id);

    /// <summary>
    /// 搜索历史记录
    /// </summary>
    /// <param name="keyword">搜索关键字</param>
    /// <returns>匹配的历史记录列表</returns>
    Task<List<HistoryEntry>> SearchHistoryAsync(string keyword);
}
