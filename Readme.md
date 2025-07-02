# 部落格文章範例 
使用生成式 AI 結合 RAG 技術實作屬於自己的 LLM 知識庫，詳細請參閱部落格。


---
## 本系列文章連結
1. [前言及流程規劃](https://lawrencetech.blogspot.com/2025/03/ai-rag-llm.html)。
2. [建置本地端 Ollama 服務及 LLM 知識庫所需的環境設置](https://lawrencetech.blogspot.com/2025/04/ollama-llm.html)。
3. [蝦咪系 Word embeddings？詞嵌入模型概念及實作](https://lawrencetech.blogspot.com/2025/04/word-embedding.html)。
4. [Hello Gemini，串接第一個 Gemini 生成式 AI](https://lawrencetech.blogspot.com/2025/06/hello-gemini-gemini-ai.html)。
5. [做個有記憶力的 AI 機器人，實作對話記憶](https://lawrencetech.blogspot.com/2025/06/ai.html)。
6. [來跟 AI 玩玩角色扮演吧，提示工程（Prompt engineering）實作](https://lawrencetech.blogspot.com/2025/06/ai-prompt-engineering.html)。
7. 解決 AI 幻覺，讓 RAG 幫你吧。
8. AI 也能認識你是誰喔，實作自定義 Function。
9. 來吧，開始建立基於生成式 AI 的 KM 系統了。
10. 番外篇 - 我想換個生成模型呢。


---
## 專案路徑 
```
Root
|__ OllamaEnvBuild
|   |__ docker-compose.yaml，詞嵌入模型環境
|__ EmbeddingLab
|   |__ 詞嵌入模型概念及實作，使用 Ollama 套件
|__ EmbeddingLab2
|   |__ 詞嵌入模型概念及實作，自行實作 ITextEmbeddingGenerationService
|__ HelloGemini
|   |__ 串接第一個 Gemini 生成式 AI
|__ HelloGeminiMemory
|   |__ 做個有記憶力的 AI 機器人，實作對話記憶
|__ PromptInline
|   |__ 來跟 AI 玩玩角色扮演吧，提示工程（Prompt engineering）實作，第一階段：程式內寫死的 Hot Code Prompt
|__ PromptFunction
|   |__ 來跟 AI 玩玩角色扮演吧，提示工程（Prompt engineering）實作，第二階段：使用 Prompt Plugin（Semantic Plugin）
```

---
### License
The MIT license
