using ElibraryParserWeb.Models;
using HtmlAgilityPack;
using System.Net.Http;

namespace ElibraryParserWeb.Services
{
    public class SimpleParser
    {
        private readonly HttpClient _httpClient;

        public SimpleParser()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<Publication> ParseElibraryArticleAsync(string url)
        {
            try
            {
                var html = await _httpClient.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                return new Publication
                {
                    Link = url,
                    Title = ExtractTitle(doc),
                    Authors = ExtractAuthors(doc)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка парсинга: {ex.Message}");
            }
        }

        private string ExtractTitle(HtmlDocument doc)
        {
            // Самый простой способ - из заголовка страницы
            var titleNode = doc.DocumentNode.SelectSingleNode("//title");
            if (titleNode != null)
            {
                var title = titleNode.InnerText.Trim();

                // Упрощенная очистка
                title = System.Net.WebUtility.HtmlDecode(title);
                title = title.Replace("&nbsp;", " ").Trim();

                return title.Length > 200 ? title.Substring(0, 200) + "..." : title;
            }

            // Альтернативный поиск
            var h1 = doc.DocumentNode.SelectSingleNode("//h1");
            if (h1 != null)
            {
                return CleanText(h1.InnerText);
            }

            return "Название не найдено";
        }

        private string ExtractAuthors(HtmlDocument doc)
        {
            // Ищем авторов в мета-тегах
            var authorMeta = doc.DocumentNode.SelectSingleNode("//meta[@name='author']");
            if (authorMeta != null)
            {
                var authors = authorMeta.GetAttributeValue("content", "");
                if (!string.IsNullOrEmpty(authors))
                    return CleanText(authors);
            }

            // Ищем авторов по классам/стилям
            var potentialAuthors = doc.DocumentNode.SelectNodes("//span[contains(@class, 'author')] | //div[contains(@class, 'author')]");
            if (potentialAuthors != null)
            {
                foreach (var node in potentialAuthors)
                {
                    var text = CleanText(node.InnerText);
                    if (text.Length > 5 && text.Contains(",") && !text.Contains("©") && !text.Contains("http"))
                    {
                        return text;
                    }
                }
            }

            // Простой поиск по тексту "Авторы:"
            var nodes = doc.DocumentNode.SelectNodes("//*[contains(text(), 'Авторы:') or contains(text(), 'Автор:')]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var parentText = node.ParentNode?.InnerText ?? "";
                    if (parentText.Contains("Авторы:"))
                    {
                        var parts = parentText.Split(new[] { "Авторы:", "Автор:" }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 1)
                        {
                            return CleanText(parts[1].Split('<')[0].Trim());
                        }
                    }
                }
            }

            return "Авторы не найдены";
        }

        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = System.Net.WebUtility.HtmlDecode(text);
            text = text.Replace("&nbsp;", " ")
                      .Replace("\n", " ")
                      .Replace("\r", " ")
                      .Replace("\t", " ")
                      .Trim();

            // Удаляем лишние пробелы
            while (text.Contains("  "))
                text = text.Replace("  ", " ");

            return text;
        }
    }
}
