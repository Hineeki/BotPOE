using System.Net;

namespace RequestPoeNinjaData
{
    public class GetRequest
    {
        private HttpWebRequest _request;
        private string _url;
        public string Response { get; set; }
        public GetRequest(string url)
        {
            _url = url;
        }
        public void Run()
        {
            _request = (HttpWebRequest)WebRequest.Create(_url);
            _request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    Response = new StreamReader(stream).ReadToEnd();
                }
                else
                {
                    Console.WriteLine("Неудалось получить ответ от сервера.");
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Возникли проблемы с вводом/выводом данных в процессе отправки запроса или получения ответа.");
            }
            catch (WebException we)
            {
                Console.WriteLine("Нет подключения к интернету");
            }
            catch (NotSupportedException nse)
            {
                Console.WriteLine("Удостоверьтесь, что URL использует поддерживаемый протокол (например, http или https).");
            }
            catch (ObjectDisposedException ode)
            {
                Console.WriteLine("Запрос закрыт или удалён.");
            }
        }
    }
}