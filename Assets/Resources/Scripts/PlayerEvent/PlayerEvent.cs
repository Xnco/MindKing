using UnityEngine;
using System.Collections;

public enum PlayerEvent
{
    BeginReply,  // Main 通知 玩家 和 AI 开始答题

    AI_Reply, // AI 回答问题

    GameOver, // 游戏结束
}
