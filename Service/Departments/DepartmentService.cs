using Company.Dtos.FilterDto;
using Company.Repository.Departments;
using Company.Repository.Departments.Models;

namespace Company.Service.Departments;

public class DepartmentService:IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public int Create(DepartmentCreateDto dto)
    {
        // Nom bo‘sh bo‘lmasligi kerak
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Bo‘lim nomi bo‘sh bo‘lmasligi kerak");

        // Nom uzunligi 200 dan oshmasligi kerak
        if (dto.Name.Length > 200)
            throw new ArgumentException("Bo‘lim nomi 200 belgidan oshmasligi kerak");

        // CompanyId ijobiy bo‘lishi kerak
        if (dto.CompanyId <= 0)
            throw new ArgumentException("Noto‘g‘ri Company ID");

        // Yaratish
        return _repository.Create(dto);
    }

    public DepartmentDto Update(DepartmentUpdate dto, int id)
    {
        var existing = _repository.GetById(id);
        if (existing == null)
            throw new KeyNotFoundException($"Department ID {id} topilmadi");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Bo‘lim nomi bo‘sh bo‘lmasligi kerak");

        if (dto.Name.Length > 200)
            throw new ArgumentException("Bo‘lim nomi 200 belgidan oshmasligi kerak");

        return _repository.Update(dto, id); // ✅ return full DTO
    }

    public bool Delete(int id)
    {
        var existing = _repository.GetById(id);
        if (existing == null)
            throw new KeyNotFoundException($"Department ID {id} mavjud emas");

        return _repository.Delete(id);
    }

    public DepartmentDto GetById(int id)
    {
        var result = _repository.GetById(id);
        if (result == null)
            throw new KeyNotFoundException($"Department ID {id} topilmadi");

        return result;
    }

    public List<DepartmentDto> GetAll(DepartmentFilterDto filter)
    {
        return _repository.GetAll(filter);
    }

}
