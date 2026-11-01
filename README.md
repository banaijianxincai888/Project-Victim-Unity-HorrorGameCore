# 恐怖潜行解谜游戏 (Victim)

> 一款基于 Unity 开发的 3D 第一人称恐怖潜行解谜独立游戏（受 Outlast 启发）。**本项目为浙江工业大学大四学生的个人独立作品，由工商管理专业跨考，出于热爱自学 Unity/C# 1 年独立完成。**（注：本仓库仅包含 C# 源码，不含美术与音频资产）。
> 在浏览器里试试我的游戏！

<div align="center">

[![Play in Browser](https://img.shields.io/badge/▶_Play_in_Browser-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://bainainaizi.itch.io/victim)
[![Unity Version](https://img.shields.io/badge/Unity-6000.4.9f1-black?style=for-the-badge&logo=unity)](https://unity.com/)

</div>

---
## 关于我
-  **教育背景**：**浙江工业大学** · 工商管理专业 · **大四在校生**。
-  **技术背景**：出于对游戏开发的极度热爱，**自学 Unity 与 C# 1年**，具备扎实的工程实践能力与源码级排错能力。
-  **求职意向**：正在积极寻找 **Unity 客户端开发实习生** 岗位（可随时到岗实习，期待在杭州本地发展）。

## 核心技术亮点 (Technical Highlights)

### 1. 架构设计与设计模式
- **反射驱动的有限状态机 (FSM)**：未使用臃肿的 `if-else`，而是基于状态模式构建玩家系统（行走、奔跑、躲藏等）。利用 **C# 反射 (Reflection)** 和命名约定，实现状态与触发器的自动注册，完美符合“开闭原则”。
- **事件中心 (Event Center)**：基于 `Action/Func` 委托，统一分发输入、交互、UI 更新等事件，实现模块间的低耦合。
- **黑板模式 (Blackboard Pattern)**：在敌人 AI 中，通过 `EnemyBlackBoard` 集中管理 AI 共享数据（视野、玩家位置等），配合行为树实现复杂的追击、破门逻辑。
- **数据驱动**：大量使用 `ScriptableObject`（如物品 `ItemSO`、解密 `DecryptionSO`），将数值配置与代码逻辑彻底分离。
- **对象池 (Object Pool)**：自研陷阱对象池，复用捕熊夹对象，避免频繁 `Instantiate/Destroy` 带来的 GC 卡顿。

### 2. 引擎深度应用与性能优化
- **行为树 AI (Behavior Designer)**：自定义行为树节点，实现了包含扇形双视野射线检测、听觉感知、寻路追踪在内的完整 AI 状态机。
- **动画与 IK**：熟练使用 `Animation Rigging` 实现拾取物品时的右臂 IK 贴合，并通过 Animation Events 精准驱动逻辑与音效。
- **Cinemachine**：实现平滑的 FOV 过渡、视角震动及跟随相机的无缝切换。
- **渲染优化**：在战利品闪烁效果中，严格使用 **`MaterialPropertyBlock`** 修改自发光属性，避免了材质实例化导致的内存暴增。

### 3. 第三方中间件应用
- **Wwise 音频定制**：深入使用 Wwise 的 API。通过捕获 `EndOfEvent` 回调并利用 Cookie 校验，实现了**音频与字幕的完美同步**。
- **Easy Save 3 (存档系统)**：实现**分帧异步保存**（每帧仅保存一个对象），解决大存档导致的主线程卡顿。同时剥离纯数据结构，避免序列化 UI 与 Component 引用导致的报错。

### 4. 跨平台适配与工程化能力 (WebGL)
为了让游戏能在 itch.io 上无门槛试玩，本项目完成了 WebGL 平台的深度适配：
- **IL2CPP 代码裁剪防护**：针对反射代码被误裁剪的问题，编写了定制化的 `link.xml` 并调整了代码剥离等级。
- **Wwise 内存流加载**：绕过浏览器沙箱限制，使用 `UnityWebRequest` 读取字节流，通过 `GCHandle` 固定内存指针，调用 `LoadBankMemoryCopy` 解决 WebGL 下音频文件无法直接读取的问题。

## 代码目录结构
```text
Assets/Scripts/
├── Core/          # 核心框架（单例基类、事件中心）
├── Player/        # 玩家系统（FSM、相机、输入、生命）
├── Enemy/         # AI 系统（黑板数据、行为树节点）
├── Interaction/   # 交互系统（门、战利品、搬运、解密机关）
├── UI/            # UI 管理（背包、对话框、任务提示）
├── Save/          # 存档系统（分帧读写）
└── Audio/         # 音频管理（Wwise 事件封装）
```
 **[点击阅读：技术架构与实现详解 (Doc/Technology.md)](./Doc/Technology.md)**
