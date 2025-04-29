namespace EmbeddingLab2.Ollama
{
    public class OllamaEmbeddingResult
    {
        [System.Text.Json.Serialization.JsonPropertyName("model")]
        public required string Model { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("embeddings")]
        public required List<List<float>> Embeddings { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("total_duration")]
        public long? TotalDuration { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("load_duration")]
        public long? LoadDuration { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("prompt_eval_count")]
        public int? PromptEvalCount { get; set; }
    }
}
