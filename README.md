📡 无人机数据可视化平台

一个基于 Unity3D + C# 开发的无人机传感器数据采集与可视化系统。
项目支持 实时数据接收、UI 展示、自动数据存储，并预留 跨平台扩展能力。

✨ 功能模块
🔗 数据通信

使用 原生 WebSocket 与无人机服务端建立连接

实时接收 JSON 格式传感器数据


🖥️ UI 数据展示

通过 TMP_Text 在面板实时更新高度、电量、速度等信息

通过 Photoshop 制作刻度条资源并导入 Unity，开发数据驱动的指针控制逻辑，实现指针根据数据精准指向对应刻度的功能。

模块化 UI 控制（DataUI_UICtrl），支持扩展不同设备的数据展示

📊 数据存储与导出

每接收一定数量的数据 自动生成 Excel / CSV 文件，支持自定义数量

文件包含 时间戳 + 中文字段名，便于直接用于后续分析

⚙️ 架构设计

基于 Singleton 管理框架，实现全局模块管理

自定义 事件中心 (EventCenter)，解耦通信、UI、存储逻辑

模块划分清晰：

DroneWebSocketClient → 通信模块

DataUI_UICtrl → 数据展示模块

ExcelExporter → 数据存储模块

📱 扩展性

预留 Android 插件接口，支持移动端数据采集与存储

可通过 Node.js 网关 转发数据，实现多客户端共享

🛠 技术栈

引擎：Unity3D

语言：C#

网络：WebSocket (无第三方库)

存储：Excel / CSV 导出

UI：Unity UI, TextMeshPro (TMP_Text)

架构：单例模式、事件驱动、模块化管理

📂 项目结构
├── DroneWebSocketClient/   # WebSocket 通信模块
├── DataUI_UICtrl/          # 数据展示 UI 控制
├── ExcelExporter/          # Excel/CSV 自动导出模块
├── Managers/               # 单例/事件中心

🚀 快速开始

克隆项目

git clone https://github.com/yourname/unity-drone-data-collector.git


在 Unity 打开项目

输入无人机服务器 IP 和端口，点击连接按钮

实时查看 UI 面板中的无人机数据，并在接收满 50 条后自动生成 Excel 文件
