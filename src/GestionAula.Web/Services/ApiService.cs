using System.Net.Http.Json;
using GestionAula.Shared.DTOs;

namespace GestionAula.Web.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient Client => _httpClientFactory.CreateClient("GestionAulaApi");

    // ========== ALUMNOS ==========
    public async Task<List<AlumnoDto>> GetAlumnosAsync()
    {
        return await Client.GetFromJsonAsync<List<AlumnoDto>>("api/alumnos") ?? new();
    }

    public async Task<LibretaDto?> GetLibretaAsync(int alumnoId)
    {
        return await Client.GetFromJsonAsync<LibretaDto>($"api/alumnos/{alumnoId}/libreta");
    }

    public async Task<List<AlumnoDesaprobadoDto>> GetDesaprobadosAsync()
    {
        return await Client.GetFromJsonAsync<List<AlumnoDesaprobadoDto>>("api/alumnos/desaprobados") ?? new();
    }

    // ========== MATERIAS ==========
    public async Task<List<MateriaResumenDto>> GetMateriasResumenAsync()
    {
        return await Client.GetFromJsonAsync<List<MateriaResumenDto>>("api/materias/resumen") ?? new();
    }

    public async Task<List<NotaDto>> GetAprobadosPorMateriaAsync(int materiaId)
    {
        return await Client.GetFromJsonAsync<List<NotaDto>>($"api/materias/{materiaId}/aprobados") ?? new();
    }

    public async Task<List<NotaDto>> GetDesaprobadosPorMateriaAsync(int materiaId)
    {
        return await Client.GetFromJsonAsync<List<NotaDto>>($"api/materias/{materiaId}/desaprobados") ?? new();
    }
}