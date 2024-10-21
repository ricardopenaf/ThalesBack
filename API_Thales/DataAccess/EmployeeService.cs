using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using static System.Net.WebRequestMethods;

namespace DataAccess
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://hub.dummyapis.com/";

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            //Configurar el HttpClient para ignorar errores de certificado
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
        /// <summary>
        /// All Employees
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Employees>> GetEmployeesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("employee");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Employees>>(content);
            }
            catch (HttpRequestException e)
            {
                // Loguear el error
                Console.WriteLine($"Error al obtener empleados: {e.Message}");
                throw new Exception("Failed to retrieve employees", e);
            }
        }

        public async Task<Employees?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync("employee");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var employees = JsonConvert.DeserializeObject<List<Employees>>(content);

                return employees?.FirstOrDefault(e => e.id == id);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error de red al obtener los empleados: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Error al deserializar la respuesta: {e.Message}");
                return null;
            }
        }
    }

}

