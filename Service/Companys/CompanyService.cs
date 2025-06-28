using Company.Dtos.FilterDto;
using Company.Repository.Company;
using Company.Repository.Company.Models;
using System.Text.RegularExpressions;

namespace Company.Service.Companys;

public class CompanyService : ICompanyService
{
    private readonly ICopanyRepositoriy _repository;

    public CompanyService(ICopanyRepositoriy repository)
    {
        _repository = repository;
    }

    public int Create(CompanyCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("Company nomi bo‘sh bo‘lmasligi kerak");
        }
        if (string.IsNullOrWhiteSpace(dto.Location))
            throw new ArgumentException("Location bo‘sh bo‘lmasligi kerak");

        if (!Regex.IsMatch(dto.PhoneNumber ?? "", @"^\+?[0-9]{7,14}$"))
            throw new ArgumentException("Telefon raqami noto‘g‘ri formatda");

        if (dto.FoundedYear > DateTime.Today)
            throw new ArgumentException("Tashkil etilgan yil kelajakda bo‘lmasligi kerak");


        return _repository.Create(dto);
    }
    public bool Update(CompanyUpdateDto dto, int id)
    {
       
        var existing = _repository.GetById(id);
        if (existing == null)
            throw new KeyNotFoundException($"Company ID {id} topilmadi");

       
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Company nomi bo‘sh bo‘lmasligi kerak");

        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            if (!Regex.IsMatch(dto.PhoneNumber, @"^\+?[0-9]{7,14}$"))
                throw new ArgumentException("Telefon raqami noto‘g‘ri formatda");
        }

        if (dto.FoundedYear > DateTime.Today)
            throw new ArgumentException("Tashkil topgan sana kelajakda bo‘lishi mumkin emas");

        // 7. Dublikat nom tekshiruvi (agar kerak bo‘lsa)
        //var allCompanies = _repository.GetAll(new CompanyFilterDto());
        //if (allCompanies.Any(c => c.CompanyId != id && c.Name.ToLower() == dto.Name.ToLower()))
        //    throw new ArgumentException("Boshqa kompaniyada shu nom allaqachon mavjud");

        // 8. Update qilish
        return _repository.Update(dto, id);
    }
    public bool Delete(int id)
    {
        // 1. ID bo‘yicha kompaniya mavjudligini tekshirish
        var existing = _repository.GetById(id);
        if (existing == null)
            throw new KeyNotFoundException($"Company ID {id} mavjud emas");

        // 2. Repository orqali o‘chirish
        return _repository.Delete(id);
    }

    public CompanyDto GetById(int id)
    {
        var result = _repository.GetById(id);

     
        if (result == null)
            throw new KeyNotFoundException($"Company ID {id} topilmadi");

        // 3. Topilgan kompaniyani qaytarish
        return result;
    }

    public async Task<List<CompanyDto>> GetAll(CompanyFilterDto filter)
    {
        if (filter == null)
            filter = new CompanyFilterDto();

        var result = await _repository.GetAllAsync(filter);
        return result;
    }

}
