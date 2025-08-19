# 📡 无人机数据可视化平台

一个基于 **Unity3D + C#** 开发的无人机传感器数据采集与可视化系统。
项目支持 **实时数据接收、UI 展示、自动数据存储**，并预留 **跨平台扩展能力**。

---

## ✨ 功能模块

### 🔗 数据通信

* 使用 **原生 WebSocket** 与无人机服务端建立连接
* 实时接收 JSON 格式传感器数据

### 🖥️ UI 数据展示

* 通过 `TMP_Text` 在面板实时更新高度、电量、速度等信息
* 使用PS设计并实现 UI 数据展示模块：采用文本、图片动态变化、折线图动态更新传感器信息，显示无人机数据，支持不同场景下的 UI 面板切换。
* 模块化 UI 控制（`DataUI_UICtrl`），支持扩展不同设备的数据展示

### 📊 数据存储与导出

* 每接收 **一定量数据** 自动生成 **Excel / CSV 文件**
* 文件包含 **时间戳 + 中文字段名**，便于直接用于后续分析
* 用户可 **自定义自动保存多少条数据**

### ⚙️ 架构设计

* 基于 **Singleton 管理框架**，实现全局模块管理
* 自定义 **事件中心 (EventCenter)**，解耦通信、UI、存储逻辑
* 模块划分清晰：

  * `DroneWebSocketClient` → 通信模块
  * `DataUI_UICtrl` → 数据展示模块
  * `ExcelExporter` → 数据存储模块

### 📱 扩展性

* 预留 **Android 插件接口**，支持移动端数据采集与存储
* 可通过 **Node.js 网关** 转发数据，实现多客户端共享

---

## 🛠 技术栈

* **引擎**：Unity3D
* **语言**：C#
* **网络**：WebSocket (无第三方库)
* **存储**：Excel / CSV 导出
* **UI**：Unity UI, TextMeshPro (TMP\_Text)
* **架构**：单例模式、事件驱动、模块化管理

---

## 📂 项目结构

```
├── DroneWebSocketClient/   # WebSocket 通信模块
├── DataUI_UICtrl/          # 数据展示 UI 控制
├── ExcelExporter/          # Excel/CSV 自动导出模块
├── Managers/               # 单例/事件中心
```

---

## 🚀 快速开始

1. 克隆项目

   ```bash
   git clone https://github.com/15070917659/Drone-Data-Visualization-Platform.git
   ```

## 🙋 关于我

* 🎮 热爱 **游戏开发 & 系统架构设计**
* 🔧 擅长 **Unity3D / C# / 游戏系统设计 / 数据驱动架构**
* 📬 如果你对我的项目感兴趣，欢迎联系我：**\[2529116790@qq.com]**

