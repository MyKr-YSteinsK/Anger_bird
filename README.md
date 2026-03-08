# Anger_bird

一个使用 Unity 制作的 2D 愤怒的小鸟风格练习项目。

这个项目是我参考 YouTube 教程 [How To Make Angry Birds In Unity](https://www.youtube.com/watch?v=QplEeEAJxck) 跟着一步步编码完成的，主要制作时间在 2024 年 3 月到 4 月，作为个人兴趣项目，用来探索 Unity 编辑器工作流以及 C# 脚本在 2D 物理玩法中的应用。

## 项目内容

- 弹弓拖拽与发射逻辑
- 小鸟飞行与碰撞朝向处理
- 敌人受击扣血与死亡判定
- 回合/剩余发射次数管理
- 关卡胜负判断与重开
- 相机跟随切换
- 基础音效反馈

当前 Build Settings 中包含 2 个场景：

- `SampleScene`
- `Scene2`

## 技术信息

- 引擎：Unity `2022.3.25f1`
- 语言：C#
- 渲染管线：URP
- 主要包：
  - Cinemachine
  - Input System
  - DOTween

## 项目结构

```text
Anger bird/   Unity 源工程
Game_v2/      本地导出的 Windows 构建
```

核心脚本位于 `Anger bird/Assets/Scripts/`，包括：

- `SlingShotHandler.cs`：弹弓拖拽、发射、生成下一只鸟
- `AngieBird.cs`：小鸟发射、碰撞、朝向更新
- `Baddie.cs`：敌人生命值、受伤与死亡
- `GameManager.cs`：回合数、胜负与重开逻辑
- `CameraManager.cs`：待机镜头/跟随镜头切换

## 如何打开

给了exe，直接运行

## 说明

- 这是一个学习和练习性质的项目，重点在于熟悉 Unity 2D 物理、场景组织和脚本控制。
- 仓库默认更适合保存源工程；导出构建产物更推荐通过 GitHub Releases 单独发布。
